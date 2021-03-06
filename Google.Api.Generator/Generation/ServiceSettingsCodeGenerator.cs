﻿// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Api.Generator.RoslynUtils;
using Google.LongRunning;
using Grpc.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static Google.Api.Generator.RoslynUtils.Modifier;
using static Google.Api.Generator.RoslynUtils.RoslynBuilder;

namespace Google.Api.Generator.Generation
{
    /// <summary>
    /// Generate all code for the `Settings` class of a proto-defined service.
    /// </summary>
    internal class ServiceSettingsCodeGenerator
    {
        private static readonly PollSettings s_lroDefaultPollSettings = new PollSettings(
            expiration: Expiration.FromTimeout(TimeSpan.FromHours(24)),
            delay: TimeSpan.FromSeconds(20),
            delayMultiplier: 1.5,
            maxDelay: TimeSpan.FromSeconds(45));

        private static readonly SyntaxAnnotation s_cloneSetting = new SyntaxAnnotation("cloneSetting");

        public static ClassDeclarationSyntax Generate(SourceFileContext ctx, ServiceDetails svc) =>
            new ServiceSettingsCodeGenerator(ctx, svc).Generate();

        private ServiceSettingsCodeGenerator(SourceFileContext ctx, ServiceDetails svc) =>
            (_ctx, _svc) = (ctx, svc);

        private readonly SourceFileContext _ctx;
        private readonly ServiceDetails _svc;

        private ClassDeclarationSyntax Generate()
        {
            var cls = Class(Public | Sealed | Partial, _svc.SettingsTyp, baseTypes: _ctx.Type<ServiceSettingsBase>())
                .WithXmlDoc(XmlDoc.Summary("Settings for ", _ctx.Type(_svc.ClientAbstractTyp), " instances."));
            using (_ctx.InClass(cls))
            {
                cls = cls.AddMembers(GetDefault());
                cls = cls.AddMembers(ParameterlessCtor());
                cls = cls.AddMembers(CopyCtor());
                cls = cls.AddMembers(OnCopyPartial());
                cls = cls.AddMembers(SettingsProperties().ToArray());
                cls = cls.AddMembers(Clone());
            }
            return cls;
        }

        // Generates the GetDefault() static method in the Settings class.
        private MemberDeclarationSyntax GetDefault() =>
            Method(Public | Static, _ctx.CurrentType, "GetDefault")()
                .WithBody(New(_ctx.CurrentType)())
                .WithXmlDoc(
                    XmlDoc.Summary("Get a new instance of the default ", _ctx.CurrentType, "."),
                    XmlDoc.Returns("A new instance of the default ", _ctx.CurrentType, "."));

        // Generates the parameterless constructor in the Settings class.
        private MemberDeclarationSyntax ParameterlessCtor() =>
            Ctor(Public, _ctx.CurrentTyp)()
                .WithBody()
                .WithXmlDoc(XmlDoc.Summary("Constructs a new ", _ctx.CurrentType, " object with default settings."));

        // Generates the copy ctor in the Settings class.
        private MemberDeclarationSyntax CopyCtor()
        {
            var existing = Parameter(_ctx.CurrentType, "existing");
            return Ctor(Private, _ctx.CurrentTyp, initializer: BaseInitializer(existing))(existing)
                .WithBody(
                    // Check `existing` parameter value is not null.
                    _ctx.Type(typeof(GaxPreconditions)).Call(nameof(GaxPreconditions.CheckNotNull))(existing, Nameof(existing)),
                    // Copy all the per-method settings.
                    SettingsProperties().Select(CopySetting),
                    // Call the OnCopy() partial method.
                    This.Call("OnCopy")(existing)
                );

            object CopySetting(PropertyDeclarationSyntax property)
            {
                var assign = property.Assign(existing.Access(property));
                return property.HasAnnotation(s_cloneSetting) ? assign.Call("Clone")() : (object)assign;
            }
        }

        private IEnumerable<PropertyDeclarationSyntax> SettingsProperties()
        {
            return _svc.Methods.SelectMany(PerMethod);
            IEnumerable<PropertyDeclarationSyntax> PerMethod(MethodDetails method)
            {
                var cSync = XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.SyncMethodName}");
                var cAsync = XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.AsyncMethodName}");
                // Add the general per-method settings property.
                var property = AutoProperty(Public, _ctx.Type<CallSettings>(), method.SettingsName, hasSetter: true);
                var xmlSummary = XmlDoc.Summary(_ctx.Type<CallSettings>(), " for synchronous and asynchronous calls to ", cSync, " and ", cAsync, ".");
                if (method.MethodRetry != null)
                {
                    var rb = method.MethodRetry.RetryBackoff;
                    var tb = method.MethodRetry.TimeoutBackoff;
                    property = property.WithInitializer(
                        _ctx.Type<CallSettings>().Call(nameof(CallSettings.FromCallTiming))(
                            _ctx.Type<CallTiming>().Call(nameof(CallTiming.FromRetry))(
                                New(_ctx.Type<RetrySettings>())(
                                    ("retryBackoff", New(_ctx.Type<BackoffSettings>())(
                                        ("delay", _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromMilliseconds))((int)rb.Delay.TotalMilliseconds)),
                                        ("maxDelay", _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromMilliseconds))((int)rb.MaxDelay.TotalMilliseconds)),
                                        ("delayMultiplier", rb.DelayMultiplier))),
                                    ("timeoutBackoff", New(_ctx.Type<BackoffSettings>())(
                                        ("delay", _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromMilliseconds))((int)tb.Delay.TotalMilliseconds)),
                                        ("maxDelay", _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromMilliseconds))((int)tb.MaxDelay.TotalMilliseconds)),
                                        ("delayMultiplier", tb.DelayMultiplier))),
                                    ("totalExpiration", _ctx.Type<Expiration>().Call(nameof(Expiration.FromTimeout))(
                                        _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromMilliseconds))((int)method.MethodRetry.TotalExpiration.Timeout.Value.TotalMilliseconds))),
                                    ("retryFilter", _ctx.Type<RetrySettings>().Call(nameof(RetrySettings.FilterForStatusCodes))(
                                        method.MethodRetryStatusCodes.Select(x => _ctx.Type<StatusCode>().Access(x))))))))
                        .WithXmlDoc(xmlSummary,
                            XmlDoc.Remarks(
                                XmlDoc.UL(
                                    $"Initial retry delay: {(int)rb.Delay.TotalMilliseconds} milliseconds.",
                                    $"Retry delay multiplier: {rb.DelayMultiplier}",
                                    $"Retry maximum delay: {(int)rb.MaxDelay.TotalMilliseconds} milliseconds.",
                                    $"Initial timeout: {(int)tb.Delay.TotalMilliseconds} milliseconds.",
                                    $"Timeout multiplier: {tb.DelayMultiplier}",
                                    $"Timeout maximum delay: {(int)tb.MaxDelay.TotalMilliseconds} milliseconds.",
                                    $"Total timeout: {(int)method.MethodRetry.TotalExpiration.Timeout.Value.TotalSeconds} seconds.")));
                }
                else
                {
                    property = property.WithXmlDoc(xmlSummary, XmlDoc.Remarks("By default, retry will not be attempted."));
                }
                yield return property;
                // Add extra properties as required for special call types.
                switch (method)
                {
                    case MethodDetails.Lro lro:
                        yield return LroSettingsProperty(lro);
                        break;
                    case MethodDetails.BidiStreaming bidi:
                        yield return BidiSettingsProperty(bidi);
                        break;
                }
            }
        }

        private PropertyDeclarationSyntax LroSettingsProperty(MethodDetails.Lro method) =>
            AutoProperty(Public, _ctx.Type<OperationsSettings>(), method.LroSettingsName, hasSetter: true)
                .WithInitializer(New(_ctx.Type<OperationsSettings>())().WithInitializer(
                    new ObjectInitExpr(nameof(OperationsSettings.DefaultPollSettings), New(_ctx.Type<PollSettings>())(
                        _ctx.Type<Expiration>().Call(nameof(Expiration.FromTimeout))(_ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromHours))((int)s_lroDefaultPollSettings.Expiration.Timeout.Value.TotalHours)),
                        _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromSeconds))((int)s_lroDefaultPollSettings.Delay.TotalSeconds),
                        s_lroDefaultPollSettings.DelayMultiplier,
                        _ctx.Type<TimeSpan>().Call(nameof(TimeSpan.FromSeconds))((int)s_lroDefaultPollSettings.MaxDelay.TotalSeconds)))))
                .WithXmlDoc(
                    XmlDoc.Summary("Long Running Operation settings for calls to ",
                        XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.SyncMethodName}"), " and ",
                        XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.AsyncMethodName}"), "."),
                    XmlDoc.Remarks("Uses default ", _ctx.Type<PollSettings>(), " of:", XmlDoc.UL(
                        $"Initial delay: {(int)s_lroDefaultPollSettings.Delay.TotalSeconds} seconds.",
                        $"Delay multiplier: {s_lroDefaultPollSettings.DelayMultiplier}",
                        $"Maximum delay: {(int)s_lroDefaultPollSettings.MaxDelay.TotalSeconds} seconds.",
                        $"Total timeout: {(int)s_lroDefaultPollSettings.Expiration.Timeout.Value.TotalHours} hours.")))
                .WithAdditionalAnnotations(s_cloneSetting);

        private PropertyDeclarationSyntax BidiSettingsProperty(MethodDetails.BidiStreaming method) =>
            AutoProperty(Public, _ctx.Type<BidirectionalStreamingSettings>(), method.StreamingSettingsName, hasSetter: true)
                .WithInitializer(New(_ctx.Type<BidirectionalStreamingSettings>())(100))
                .WithXmlDoc(
                    XmlDoc.Summary(_ctx.Type<BidirectionalStreamingSettings>(), " for calls to ",
                        XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.SyncMethodName}"), " and ",
                        XmlDoc.C($"{_svc.ClientAbstractTyp.Name}.{method.AsyncMethodName}"), "."),
                    XmlDoc.Remarks("The default local send queue size is 100."));

        private MemberDeclarationSyntax OnCopyPartial() => PartialMethod("OnCopy")(Parameter(_ctx.CurrentType, "existing"));

        private MemberDeclarationSyntax Clone() =>
            Method(Public, _ctx.CurrentType, "Clone")()
                .WithBody(New(_ctx.CurrentType)(This))
                .WithXmlDoc(
                    XmlDoc.Summary("Creates a deep clone of this object, with all the same property values."),
                    XmlDoc.Returns("A deep clone of this ", _ctx.CurrentType, " object.")
                );
    }
}
