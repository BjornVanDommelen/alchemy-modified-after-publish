//-----------------------------------------------------------------------
// <copyright file="CoreServiceClientFactory.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublishTest
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using global::Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// Static factory class for initializing CoreService clients (session aware).
    /// </summary>
    public static class CoreServiceClientFactory
    {
        /// <summary>
        /// Creates a core service client for the given host/user/password.
        /// </summary>
        /// <param name="hostName">Hostname to connect to</param>
        /// <param name="userName">Username to connect with</param>
        /// <param name="password">Password to connect with</param>
        /// <returns>Session aware core service client using wshttp binding</returns>
        public static ISessionAwareCoreService CreateClient(string hostName, string userName, string password)
        {
            var client = new SessionAwareCoreServiceClient(
                CreateBinding(2 * 1024 * 1024, false), 
                CreateEndpoint(hostName, "webservices/CoreService2013.svc/wsHttp"));

            // Set impersonation level and username/password
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            client.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential(userName, password);

            return client;
        }

        /// <summary>
        /// Creates the binding for the client.
        /// </summary>
        /// <param name="maxSize">Maximum size in bytes for strings, arrays, etc.</param>
        /// <param name="isHttps">True if this is a connection over HTTPS false otherwise</param>
        /// <returns>The binding to use for the client</returns>
        private static Binding CreateBinding(int maxSize, bool isHttps)
        {
            return new WSHttpBinding()
            {
                MaxBufferPoolSize = maxSize,
                MaxReceivedMessageSize = maxSize,
                ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas()
                {
                    MaxStringContentLength = maxSize,
                    MaxArrayLength = maxSize,
                },
                Security = new WSHttpSecurity()
                {
                    Mode = isHttps ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message,
                    Message = new NonDualMessageSecurityOverHttp()
                    {
                        ClientCredentialType = MessageCredentialType.Windows
                    }
                }
            };
        }

        /// <summary>
        /// Generates the endpoint address string from the given "unclean" hostname and "clean" service and binding path
        /// </summary>
        /// <param name="hostname">Hostname with or without protocol and trailing slash</param>
        /// <param name="serviceAndBindingPath">Path to service without leading slash</param>
        /// <returns>Fully qualified URL of service endpoint</returns>
        private static string GetEndpointAddress(string hostname, string serviceAndBindingPath)
        {
            return string.Format(
                "{0}{1}{2}{3}",
                hostname.StartsWith("http") ? string.Empty : "http://",
                hostname,
                hostname.EndsWith("/") ? string.Empty : "/",
                serviceAndBindingPath);
        }

        /// <summary>
        /// Creates an EndpointAddress object from the given "unclean" hostname and "clean" service and binding path
        /// </summary>
        /// <param name="hostname">Hostname with or without protocol and trailing slash</param>
        /// <param name="serviceAndBindingPath">Path to service without leading slash</param>
        /// <returns>EndpointAddress object for the service endpoint</returns>
        private static EndpointAddress CreateEndpoint(string hostname, string serviceAndBindingPath)
        {
            return new EndpointAddress(GetEndpointAddress(hostname, serviceAndBindingPath));
        }
    }
}
