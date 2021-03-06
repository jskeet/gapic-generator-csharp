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
using Google.Api.Generator.Utils;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Google.Api.Generator.Generation
{
    /// <summary>
    /// The context tracking the state for a single C# source file.
    /// </summary>
    internal abstract class SourceFileContext
    {
        private sealed class FullyAliased : SourceFileContext
        {
            private static readonly IReadOnlyDictionary<string, string> s_wellknownNamespaceAliases = new Dictionary<string, string>
            {
                { typeof(System.Int32).Namespace, "sys" }, // Don't use "s"; one-letter aliases cause a compilation error!
                { typeof(System.Net.WebUtility).Namespace, "sysnet" },
                { typeof(System.Collections.Generic.IEnumerable<>).Namespace, "scg" },
                { typeof(System.Collections.ObjectModel.Collection<>).Namespace, "sco" },
                { typeof(System.Linq.Enumerable).Namespace, "linq" },
                { typeof(Google.Api.Gax.Expiration).Namespace, "gax" },
                { typeof(Google.Api.Gax.Grpc.CallSettings).Namespace, "gaxgrpc" },
                { typeof(Grpc.Core.CallCredentials).Namespace, "grpccore" },
                { typeof(Grpc.Core.Interceptors.Interceptor).Namespace, "grpcinter" },
                { typeof(Google.Protobuf.WellKnownTypes.Any).Namespace, "wkt" },
                { typeof(Google.LongRunning.Operation).Namespace, "lro" },
                { typeof(Google.Protobuf.ByteString).Namespace, "proto" },
                { typeof(Moq.Mock).Namespace, "moq" },
                { typeof(Xunit.Assert).Namespace, "xunit" },
            };

            public FullyAliased(IClock clock) : base(clock) { }

            // Namespace -> alias
            private readonly Dictionary<string, string> _namespaceAliases = new Dictionary<string, string>();
            private readonly HashSet<string> _namespaceAliasesOnly = new HashSet<string>();

            public override TypeSyntax Type(Typ typ, bool forceFullyQualified)
            {
                var type = base.Type(typ, forceFullyQualified);
                if (type != null)
                {
                    return type;
                }
                string namespaceAlias;
                if (!forceFullyQualified && $"{Namespace}.".StartsWith($"{typ.Namespace}."))
                {
                    // Within the current namespace, no import required.
                    namespaceAlias = null;
                }
                else if (!_namespaceAliases.TryGetValue(typ.Namespace, out namespaceAlias))
                {
                    // A namespace that hasn't been seen before. Create an alias for it.
                    if (!s_wellknownNamespaceAliases.TryGetValue(typ.Namespace, out var rawAlias))
                    {
                        // If it's not a well-known namespace (e.g. "System"), then create an alias 
                        // using the first character of each namespace part.
                        rawAlias = typ.Namespace
                            .Split('.', StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => char.ToLowerInvariant(x[0]))
                            .Aggregate("", (a, c) => a + c);
                    }
                    // Make sure all aliases are unique by adding a numeric suffix if necessary.
                    int index = 0;
                    namespaceAlias = rawAlias;
                    while (_namespaceAliasesOnly.Contains(namespaceAlias))
                    {
                        namespaceAlias = $"{rawAlias}{++index}";
                    }
                    // Record the alias for later.
                    _namespaceAliases.Add(typ.Namespace, namespaceAlias);
                    _namespaceAliasesOnly.Add(namespaceAlias);
                }
                SimpleNameSyntax result = IdentifierName(typ.Name);
                if (typ.GenericArgTyps != null)
                {
                    // Generic typ, so return a generic name by recursively calling this method on all type args.
                    result = GenericName(result.Identifier, TypeArgumentList(SeparatedList(typ.GenericArgTyps.Select(Type))));
                }
                // Return the final TypeSyntax, aliased or not as required.
                return namespaceAlias == null ? (TypeSyntax)result : AliasQualifiedName(namespaceAlias, result);
            }

            public override CompilationUnitSyntax CreateCompilationUnit(NamespaceDeclarationSyntax ns)
            {
                var usings = _namespaceAliases
                    .OrderBy(x => x.Key)
                    .Select(x => UsingDirective(NameEquals(x.Value), IdentifierName(x.Key)));
                return AddLicense(CompilationUnit().AddUsings(usings.ToArray()).AddMembers(ns));
            }
        }

        private sealed class Unaliased : SourceFileContext
        {
            // TODO: Handle duplicate names.
            // TODO: Handle nested types.

            public Unaliased(IClock clock) : base(clock) { }

            private readonly HashSet<string> _imports = new HashSet<string>();

            public override TypeSyntax Type(Typ typ, bool forceFullyQualified)
            {
                var type = base.Type(typ, forceFullyQualified);
                if (type != null)
                {
                    return type;
                }
                if (!$"{Namespace}.".StartsWith($"{typ.Namespace}."))
                {
                    _imports.Add(typ.Namespace);
                }
                SimpleNameSyntax result = IdentifierName(typ.Name);
                if (typ.GenericArgTyps != null)
                {
                    // Generic typ, so return a generic name by recursively calling this method on all type args.
                    result = GenericName(result.Identifier, TypeArgumentList(SeparatedList(typ.GenericArgTyps.Select(Type))));
                }
                return result;
            }

            public override T Import<T>(System.Type t, T o)
            {
                _imports.Add(Typ.Of(t).Namespace);
                return o;
            }

            public override CompilationUnitSyntax CreateCompilationUnit(NamespaceDeclarationSyntax ns)
            {
                var usings = _imports.OrderBy(x => x).Select(x => UsingDirective(IdentifierName(x)));
                return AddLicense(CompilationUnit().AddMembers(ns.AddUsings(usings.ToArray())));
            }
        }

        public enum ImportStyle
        {
            FullyAliased,
            Unaliased,
        }

        private static readonly IReadOnlyDictionary<string, TypeSyntax> s_predefinedTypes = new Dictionary<string, TypeSyntax>
        {
            { typeof(bool).FullName, PredefinedType(Token(SyntaxKind.BoolKeyword)) },
            { typeof(byte).FullName, PredefinedType(Token(SyntaxKind.ByteKeyword)) },
            { typeof(sbyte).FullName, PredefinedType(Token(SyntaxKind.SByteKeyword)) },
            { typeof(short).FullName, PredefinedType(Token(SyntaxKind.ShortKeyword)) },
            { typeof(ushort).FullName, PredefinedType(Token(SyntaxKind.UShortKeyword)) },
            { typeof(int).FullName, PredefinedType(Token(SyntaxKind.IntKeyword)) },
            { typeof(uint).FullName, PredefinedType(Token(SyntaxKind.UIntKeyword)) },
            { typeof(long).FullName, PredefinedType(Token(SyntaxKind.LongKeyword)) },
            { typeof(ulong).FullName, PredefinedType(Token(SyntaxKind.ULongKeyword)) },
            { typeof(char).FullName, PredefinedType(Token(SyntaxKind.CharKeyword)) },
            { typeof(decimal).FullName, PredefinedType(Token(SyntaxKind.DecimalKeyword)) },
            { typeof(double).FullName, PredefinedType(Token(SyntaxKind.DoubleKeyword)) },
            { typeof(float).FullName, PredefinedType(Token(SyntaxKind.FloatKeyword)) },
            { typeof(string).FullName, PredefinedType(Token(SyntaxKind.StringKeyword)) },
            { typeof(object).FullName, PredefinedType(Token(SyntaxKind.ObjectKeyword)) },
        };

        public static SourceFileContext Create(ImportStyle importStyle, IClock clock)
        {
            switch (importStyle)
            {
                case ImportStyle.FullyAliased:
                    return new FullyAliased(clock);
                case ImportStyle.Unaliased:
                    return new Unaliased(clock);
                default:
                    throw new NotImplementedException($"Unrecognised import style: {importStyle}");
            }
        }

        protected SourceFileContext(IClock clock) => _clock = clock;

        private readonly IClock _clock;

        /// <summary>
        /// The current namespace. This will change depending on code location.
        /// </summary>
        public string Namespace { get; private set; } = "";

        /// <summary>
        /// The stack of typ nesting. Element 0 is the outermost typ.
        /// </summary>
        public IReadOnlyList<Typ> Typs { get; private set; } = new Typ[0];

        /// <summary>
        /// The current typ. This will change depending on code location.
        /// </summary>
        public Typ CurrentTyp => Typs.Last();

        /// <summary>
        /// The current TypeSyntax. This will change depending on code location.
        /// </summary>
        public TypeSyntax CurrentType => Type(CurrentTyp);

        /// <summary>
        /// Move the current context into a namespace.
        /// This is relevant for namespacing of type references.
        /// Disposing of the return value moves the context back out of the namespace.
        /// </summary>
        /// <param name="ns">The namespace into which to move.</param>
        /// <returns>An <c>IDisposable</c> value that must be disposed of to move out of this namespace.</returns>
        public IDisposable InNamespace(NamespaceDeclarationSyntax ns)
        {
            var restoreNamespace = Namespace;
            Namespace = $"{Namespace}.{ns.Name}".Trim('.');
            return new Disposable(() => Namespace = restoreNamespace);
        }

        /// <summary>
        /// Move the current context into a class.
        /// This is relevant for type references to nested classes.
        /// Disposing of the return value moves the context back out of the class.
        /// </summary>
        /// <param name="cls">The class into which to move.</param>
        /// <returns>An <c>IDisposable</c> value that must be disposed of to move out of this class.</returns>
        public IDisposable InClass(ClassDeclarationSyntax cls)
        {
            var restoreTypes = Typs;
            Typs = Typs.Append(Typ.Manual(Namespace, cls)).ToList();
            return new Disposable(() => Typs = restoreTypes);
        }

        public TypeSyntax TypeDontCare => Type<int>();

        public TypeSyntax Type<T>() => Type(Typ.Of<T>());

        public TypeSyntax Type(System.Type type) => Type(Typ.Of(type));

        public TypeSyntax Type(Typ typ) => Type(typ, false);

        public virtual TypeSyntax Type(Typ typ, bool forceFullyQualified)
        {
            if (typ is Typ.Special special)
            {
                // Handle special typ; e.g. `class` generic constraint.
                return IdentifierName($"{Typ.Special.NamePrefix}{special.SpecialType}");
            }
            if (typ is Typ.GenericParameter)
            {
                // Handle generic parameters.
                return IdentifierName(typ.Name);
            }
            if (typ.ElementTyp != null)
            {
                // Handle array typs.
                return SyntaxFactory.ArrayType(Type(typ.ElementTyp));
            }
            if (typ.GenericArgTyps != null && $"{typ.FullName}`1" == typeof(Nullable<>).FullName)
            {
                // Handle nullable types
                return NullableType(Type(typ.GenericArgTyps.First()));
            }
            if (typ.DeclaringTyp != null)
            {
                // Handle nested typs.
                if (!forceFullyQualified && typ.DeclaringTyp == CurrentTyp)
                {
                    return IdentifierName(typ.Name);
                }
                var outerType = Type(typ.DeclaringTyp);
                return QualifiedName((NameSyntax)outerType, IdentifierName(typ.Name));
            }
            if (s_predefinedTypes.TryGetValue(typ.FullName, out var predefinedType))
            {
                // Handle typs representing predefined C# types (e.g. `string`).
                return predefinedType;
            }
            return null;
        }

        public ArrayTypeSyntax ArrayType(Typ arrayTyp, int? size = null)
        {
            if (arrayTyp.ElementTyp is null)
            {
                throw new ArgumentException("Type argument must be an array.", nameof(arrayTyp));
            }
            if (size != null)
            {
                throw new ArgumentException("Array size specification not yet supported", nameof(size));
            }
            return ((ArrayTypeSyntax)Type(arrayTyp)).WithRankSpecifiers(SingletonList(ArrayRankSpecifier()));
        }

        public ArrayTypeSyntax ArrayType<T>(int? size = null) => ArrayType(Typ.Of<T>(), size);

        /// <summary>
        /// Force an import of referenced type.
        /// </summary>
        public virtual T Import<T>(System.Type t, T o) => o;

        public abstract CompilationUnitSyntax CreateCompilationUnit(NamespaceDeclarationSyntax ns);

        protected CompilationUnitSyntax AddLicense(CompilationUnitSyntax cu)
        {
            // TODO: Possibly allow customization of license? Is this required?
            string licenseText = $@"
// Copyright {_clock.GetCurrentDateTimeUtc().Year} Google LLC
//
// Licensed under the Apache License, Version 2.0 (the ""License"");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an ""AS IS"" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// Generated code. DO NOT EDIT!
".TrimStart();
            var licenseTrivia = TriviaList(licenseText.Replace("\r\n", "\n").Replace('\r', '\n')
                .Split('\n')
                .Select(x => x.StartsWith("//") || x == "" ? Comment(x) : throw new InvalidOperationException("Invalid text in license.")));
            if (cu.Usings.Any())
            {
                // Using directives present, so attach license to the first using directive.
                var u0 = cu.Usings[0];
                var u0Licensed = u0.WithUsingKeyword(u0.UsingKeyword.WithLeadingTrivia(licenseTrivia));
                return cu.WithUsings(List(cu.Usings.Skip(1).Prepend(u0Licensed)));
            }
            else
            {
                // No using directives, attach license to first member (probably a namespace).
                switch (cu.Members[0])
                {
                    case NamespaceDeclarationSyntax ns0:
                        var ns0Licensed = ns0.WithNamespaceKeyword(ns0.NamespaceKeyword.WithLeadingTrivia(licenseTrivia));
                        return cu.WithMembers(List(cu.Members.Skip(1).Prepend(ns0Licensed)));
                    default:
                        throw new InvalidOperationException("Cannot find an item to attach license to.");
                }
            }
        }
    }
}
