// Copyright 2019 Google LLC
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

// Generated code. DO NOT EDIT!

#pragma warning disable CS8981
using gax = Google.Api.Gax;
using gaxgrpc = Google.Api.Gax.Grpc;
using grpccore = Grpc.Core;
using grpcinter = Grpc.Core.Interceptors;
using mel = Microsoft.Extensions.Logging;
using proto = Google.Protobuf;
using scg = System.Collections.Generic;
using sco = System.Collections.ObjectModel;
using st = System.Threading;
using stt = System.Threading.Tasks;
using sys = System;

namespace Testing.PublishingSettings
{
    /// <summary>Settings for <see cref="ServiceWithSelectiveGenerationClient"/> instances.</summary>
    public sealed partial class ServiceWithSelectiveGenerationSettings : gaxgrpc::ServiceSettingsBase
    {
        /// <summary>Get a new instance of the default <see cref="ServiceWithSelectiveGenerationSettings"/>.</summary>
        /// <returns>A new instance of the default <see cref="ServiceWithSelectiveGenerationSettings"/>.</returns>
        public static ServiceWithSelectiveGenerationSettings GetDefault() => new ServiceWithSelectiveGenerationSettings();

        /// <summary>
        /// Constructs a new <see cref="ServiceWithSelectiveGenerationSettings"/> object with default settings.
        /// </summary>
        public ServiceWithSelectiveGenerationSettings()
        {
        }

        private ServiceWithSelectiveGenerationSettings(ServiceWithSelectiveGenerationSettings existing) : base(existing)
        {
            gax::GaxPreconditions.CheckNotNull(existing, nameof(existing));
            PublicMethodSettings = existing.PublicMethodSettings;
            SelectiveMethodSettings = existing.SelectiveMethodSettings;
            OnCopy(existing);
        }

        partial void OnCopy(ServiceWithSelectiveGenerationSettings existing);

        /// <summary>
        /// <see cref="gaxgrpc::CallSettings"/> for synchronous and asynchronous calls to
        /// <c>ServiceWithSelectiveGenerationClient.PublicMethod</c> and
        /// <c>ServiceWithSelectiveGenerationClient.PublicMethodAsync</c>.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>This call will not be retried.</description></item>
        /// <item><description>No timeout is applied.</description></item>
        /// </list>
        /// </remarks>
        public gaxgrpc::CallSettings PublicMethodSettings { get; set; } = gaxgrpc::CallSettings.FromExpiration(gax::Expiration.None);

        /// <summary>
        /// <see cref="gaxgrpc::CallSettings"/> for synchronous and asynchronous calls to
        /// <c>ServiceWithSelectiveGenerationClient.SelectiveMethod</c> and
        /// <c>ServiceWithSelectiveGenerationClient.SelectiveMethodAsync</c>.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><description>This call will not be retried.</description></item>
        /// <item><description>No timeout is applied.</description></item>
        /// </list>
        /// </remarks>
        public gaxgrpc::CallSettings SelectiveMethodSettings { get; set; } = gaxgrpc::CallSettings.FromExpiration(gax::Expiration.None);

        /// <summary>Creates a deep clone of this object, with all the same property values.</summary>
        /// <returns>A deep clone of this <see cref="ServiceWithSelectiveGenerationSettings"/> object.</returns>
        public ServiceWithSelectiveGenerationSettings Clone() => new ServiceWithSelectiveGenerationSettings(this);
    }

    /// <summary>
    /// Builder class for <see cref="ServiceWithSelectiveGenerationClient"/> to provide simple configuration of
    /// credentials, endpoint etc.
    /// </summary>
    public sealed partial class ServiceWithSelectiveGenerationClientBuilder : gaxgrpc::ClientBuilderBase<ServiceWithSelectiveGenerationClient>
    {
        /// <summary>The settings to use for RPCs, or <c>null</c> for the default settings.</summary>
        public ServiceWithSelectiveGenerationSettings Settings { get; set; }

        /// <summary>Creates a new builder with default settings.</summary>
        public ServiceWithSelectiveGenerationClientBuilder() : base(ServiceWithSelectiveGenerationClient.ServiceMetadata)
        {
        }

        partial void InterceptBuild(ref ServiceWithSelectiveGenerationClient client);

        partial void InterceptBuildAsync(st::CancellationToken cancellationToken, ref stt::Task<ServiceWithSelectiveGenerationClient> task);

        /// <summary>Builds the resulting client.</summary>
        public override ServiceWithSelectiveGenerationClient Build()
        {
            ServiceWithSelectiveGenerationClient client = null;
            InterceptBuild(ref client);
            return client ?? BuildImpl();
        }

        /// <summary>Builds the resulting client asynchronously.</summary>
        public override stt::Task<ServiceWithSelectiveGenerationClient> BuildAsync(st::CancellationToken cancellationToken = default)
        {
            stt::Task<ServiceWithSelectiveGenerationClient> task = null;
            InterceptBuildAsync(cancellationToken, ref task);
            return task ?? BuildAsyncImpl(cancellationToken);
        }

        private ServiceWithSelectiveGenerationClient BuildImpl()
        {
            Validate();
            grpccore::CallInvoker callInvoker = CreateCallInvoker();
            return ServiceWithSelectiveGenerationClient.Create(callInvoker, GetEffectiveSettings(Settings?.Clone()), Logger);
        }

        private async stt::Task<ServiceWithSelectiveGenerationClient> BuildAsyncImpl(st::CancellationToken cancellationToken)
        {
            Validate();
            grpccore::CallInvoker callInvoker = await CreateCallInvokerAsync(cancellationToken).ConfigureAwait(false);
            return ServiceWithSelectiveGenerationClient.Create(callInvoker, GetEffectiveSettings(Settings?.Clone()), Logger);
        }

        /// <summary>Returns the channel pool to use when no other options are specified.</summary>
        protected override gaxgrpc::ChannelPool GetChannelPool() => ServiceWithSelectiveGenerationClient.ChannelPool;
    }

    /// <summary>ServiceWithSelectiveGeneration client wrapper, for convenient use.</summary>
    /// <remarks>
    /// This is a service with one normal method and one method which is generated selectively.
    /// Prototype testing: two different commits, one with this hidden and one with it internal.
    /// Our config doesn't allow a mixture of hidden and internal in the same protobuf package.
    /// </remarks>
    public abstract partial class ServiceWithSelectiveGenerationClient
    {
        /// <summary>
        /// The default endpoint for the ServiceWithSelectiveGeneration service, which is a host of "" and a port of
        /// 443.
        /// </summary>
        public static string DefaultEndpoint { get; } = ":443";

        /// <summary>The default ServiceWithSelectiveGeneration scopes.</summary>
        /// <remarks>The default ServiceWithSelectiveGeneration scopes are:<list type="bullet"></list></remarks>
        public static scg::IReadOnlyList<string> DefaultScopes { get; } = new sco::ReadOnlyCollection<string>(new string[] { });

        /// <summary>The service metadata associated with this client type.</summary>
        public static gaxgrpc::ServiceMetadata ServiceMetadata { get; } = new gaxgrpc::ServiceMetadata(ServiceWithSelectiveGeneration.Descriptor, DefaultEndpoint, DefaultScopes, true, gax::ApiTransports.Grpc, PackageApiMetadata.ApiMetadata);

        internal static gaxgrpc::ChannelPool ChannelPool { get; } = new gaxgrpc::ChannelPool(ServiceMetadata);

        /// <summary>
        /// Asynchronously creates a <see cref="ServiceWithSelectiveGenerationClient"/> using the default credentials,
        /// endpoint and settings. To specify custom credentials or other settings, use
        /// <see cref="ServiceWithSelectiveGenerationClientBuilder"/>.
        /// </summary>
        /// <param name="cancellationToken">
        /// The <see cref="st::CancellationToken"/> to use while creating the client.
        /// </param>
        /// <returns>The task representing the created <see cref="ServiceWithSelectiveGenerationClient"/>.</returns>
        public static stt::Task<ServiceWithSelectiveGenerationClient> CreateAsync(st::CancellationToken cancellationToken = default) =>
            new ServiceWithSelectiveGenerationClientBuilder().BuildAsync(cancellationToken);

        /// <summary>
        /// Synchronously creates a <see cref="ServiceWithSelectiveGenerationClient"/> using the default credentials,
        /// endpoint and settings. To specify custom credentials or other settings, use
        /// <see cref="ServiceWithSelectiveGenerationClientBuilder"/>.
        /// </summary>
        /// <returns>The created <see cref="ServiceWithSelectiveGenerationClient"/>.</returns>
        public static ServiceWithSelectiveGenerationClient Create() =>
            new ServiceWithSelectiveGenerationClientBuilder().Build();

        /// <summary>
        /// Creates a <see cref="ServiceWithSelectiveGenerationClient"/> which uses the specified call invoker for
        /// remote operations.
        /// </summary>
        /// <param name="callInvoker">
        /// The <see cref="grpccore::CallInvoker"/> for remote operations. Must not be null.
        /// </param>
        /// <param name="settings">Optional <see cref="ServiceWithSelectiveGenerationSettings"/>.</param>
        /// <param name="logger">Optional <see cref="mel::ILogger"/>.</param>
        /// <returns>The created <see cref="ServiceWithSelectiveGenerationClient"/>.</returns>
        internal static ServiceWithSelectiveGenerationClient Create(grpccore::CallInvoker callInvoker, ServiceWithSelectiveGenerationSettings settings = null, mel::ILogger logger = null)
        {
            gax::GaxPreconditions.CheckNotNull(callInvoker, nameof(callInvoker));
            grpcinter::Interceptor interceptor = settings?.Interceptor;
            if (interceptor != null)
            {
                callInvoker = grpcinter::CallInvokerExtensions.Intercept(callInvoker, interceptor);
            }
            ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient grpcClient = new ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient(callInvoker);
            return new ServiceWithSelectiveGenerationClientImpl(grpcClient, settings, logger);
        }

        /// <summary>
        /// Shuts down any channels automatically created by <see cref="Create()"/> and
        /// <see cref="CreateAsync(st::CancellationToken)"/>. Channels which weren't automatically created are not
        /// affected.
        /// </summary>
        /// <remarks>
        /// After calling this method, further calls to <see cref="Create()"/> and
        /// <see cref="CreateAsync(st::CancellationToken)"/> will create new channels, which could in turn be shut down
        /// by another call to this method.
        /// </remarks>
        /// <returns>A task representing the asynchronous shutdown operation.</returns>
        public static stt::Task ShutdownDefaultChannelsAsync() => ChannelPool.ShutdownChannelsAsync();

        /// <summary>The underlying gRPC ServiceWithSelectiveGeneration client</summary>
        public virtual ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient GrpcClient => throw new sys::NotImplementedException();

        /// <summary>
        /// Public RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response PublicMethod(Request request, gaxgrpc::CallSettings callSettings = null) =>
            throw new sys::NotImplementedException();

        /// <summary>
        /// Public RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> PublicMethodAsync(Request request, gaxgrpc::CallSettings callSettings = null) =>
            throw new sys::NotImplementedException();

        /// <summary>
        /// Public RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> PublicMethodAsync(Request request, st::CancellationToken cancellationToken) =>
            PublicMethodAsync(request, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));

        /// <summary>
        /// Selectively-generated RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public virtual Response SelectiveMethod(Request request, gaxgrpc::CallSettings callSettings = null) =>
            throw new sys::NotImplementedException();

        /// <summary>
        /// Selectively-generated RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SelectiveMethodAsync(Request request, gaxgrpc::CallSettings callSettings = null) =>
            throw new sys::NotImplementedException();

        /// <summary>
        /// Selectively-generated RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="cancellationToken">A <see cref="st::CancellationToken"/> to use for this RPC.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public virtual stt::Task<Response> SelectiveMethodAsync(Request request, st::CancellationToken cancellationToken) =>
            SelectiveMethodAsync(request, gaxgrpc::CallSettings.FromCancellationToken(cancellationToken));
    }

    /// <summary>ServiceWithSelectiveGeneration client wrapper implementation, for convenient use.</summary>
    /// <remarks>
    /// This is a service with one normal method and one method which is generated selectively.
    /// Prototype testing: two different commits, one with this hidden and one with it internal.
    /// Our config doesn't allow a mixture of hidden and internal in the same protobuf package.
    /// </remarks>
    public sealed partial class ServiceWithSelectiveGenerationClientImpl : ServiceWithSelectiveGenerationClient
    {
        private readonly gaxgrpc::ApiCall<Request, Response> _callPublicMethod;

        private readonly gaxgrpc::ApiCall<Request, Response> _callSelectiveMethod;

        /// <summary>
        /// Constructs a client wrapper for the ServiceWithSelectiveGeneration service, with the specified gRPC client
        /// and settings.
        /// </summary>
        /// <param name="grpcClient">The underlying gRPC client.</param>
        /// <param name="settings">
        /// The base <see cref="ServiceWithSelectiveGenerationSettings"/> used within this client.
        /// </param>
        /// <param name="logger">Optional <see cref="mel::ILogger"/> to use within this client.</param>
        public ServiceWithSelectiveGenerationClientImpl(ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient grpcClient, ServiceWithSelectiveGenerationSettings settings, mel::ILogger logger)
        {
            GrpcClient = grpcClient;
            ServiceWithSelectiveGenerationSettings effectiveSettings = settings ?? ServiceWithSelectiveGenerationSettings.GetDefault();
            gaxgrpc::ClientHelper clientHelper = new gaxgrpc::ClientHelper(new gaxgrpc::ClientHelper.Options
            {
                Settings = effectiveSettings,
                Logger = logger,
            });
            _callPublicMethod = clientHelper.BuildApiCall<Request, Response>("PublicMethod", grpcClient.PublicMethodAsync, grpcClient.PublicMethod, effectiveSettings.PublicMethodSettings);
            Modify_ApiCall(ref _callPublicMethod);
            Modify_PublicMethodApiCall(ref _callPublicMethod);
            _callSelectiveMethod = clientHelper.BuildApiCall<Request, Response>("SelectiveMethod", grpcClient.SelectiveMethodAsync, grpcClient.SelectiveMethod, effectiveSettings.SelectiveMethodSettings);
            Modify_ApiCall(ref _callSelectiveMethod);
            Modify_SelectiveMethodApiCall(ref _callSelectiveMethod);
            OnConstruction(grpcClient, effectiveSettings, clientHelper);
        }

        partial void Modify_ApiCall<TRequest, TResponse>(ref gaxgrpc::ApiCall<TRequest, TResponse> call) where TRequest : class, proto::IMessage<TRequest> where TResponse : class, proto::IMessage<TResponse>;

        partial void Modify_PublicMethodApiCall(ref gaxgrpc::ApiCall<Request, Response> call);

        partial void Modify_SelectiveMethodApiCall(ref gaxgrpc::ApiCall<Request, Response> call);

        partial void OnConstruction(ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient grpcClient, ServiceWithSelectiveGenerationSettings effectiveSettings, gaxgrpc::ClientHelper clientHelper);

        /// <summary>The underlying gRPC ServiceWithSelectiveGeneration client</summary>
        public override ServiceWithSelectiveGeneration.ServiceWithSelectiveGenerationClient GrpcClient { get; }

        partial void Modify_Request(ref Request request, ref gaxgrpc::CallSettings settings);

        /// <summary>
        /// Public RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public override Response PublicMethod(Request request, gaxgrpc::CallSettings callSettings = null)
        {
            Modify_Request(ref request, ref callSettings);
            return _callPublicMethod.Sync(request, callSettings);
        }

        /// <summary>
        /// Public RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public override stt::Task<Response> PublicMethodAsync(Request request, gaxgrpc::CallSettings callSettings = null)
        {
            Modify_Request(ref request, ref callSettings);
            return _callPublicMethod.Async(request, callSettings);
        }

        /// <summary>
        /// Selectively-generated RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>The RPC response.</returns>
        public override Response SelectiveMethod(Request request, gaxgrpc::CallSettings callSettings = null)
        {
            Modify_Request(ref request, ref callSettings);
            return _callSelectiveMethod.Sync(request, callSettings);
        }

        /// <summary>
        /// Selectively-generated RPC.
        /// </summary>
        /// <param name="request">The request object containing all of the parameters for the API call.</param>
        /// <param name="callSettings">If not null, applies overrides to this RPC call.</param>
        /// <returns>A Task containing the RPC response.</returns>
        public override stt::Task<Response> SelectiveMethodAsync(Request request, gaxgrpc::CallSettings callSettings = null)
        {
            Modify_Request(ref request, ref callSettings);
            return _callSelectiveMethod.Async(request, callSettings);
        }
    }
}
