using System;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tridion.ContentManager.CoreService.Client;
using System.ServiceModel.Channels;

namespace CheckIfModifiedAfterPublishTest
{
    [TestClass]
    public class UnitTest1
    {
        public Binding CreateBinding(int maxSize, bool isHttps)
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
            return String.Format("{0}{1}{2}{3}",
                hostname.StartsWith("http") ? "" : "http://",
                hostname,
                hostname.EndsWith("/") ? "" : "/",
                serviceAndBindingPath
            );
        }

        /// <summary>
        /// Creates an EndpointAddress object from the given "unclean" hostname and "clean" service and binding path
        /// </summary>
        /// <param name="hostname">Hostname with or without protocol and trailing slash</param>
        /// <param name="serviceAndBindingPath">Path to service without leading slash</param>
        /// <returns>EndpointAddress object for the service endpoint</returns>
        public static EndpointAddress CreateEndpoint(string hostname, string serviceAndBindingPath)
        {
            return new EndpointAddress(GetEndpointAddress(hostname, serviceAndBindingPath));
        }

        private const string SERVICE_HOST = "http://azeroth.local:81/";

        private ISessionAwareCoreService GetCoreServiceClient()
        {
            var x = new SessionAwareCoreServiceClient(CreateBinding(2 * 1024 * 1024, false), CreateEndpoint(SERVICE_HOST, "webservices/CoreService2013.svc/wsHttp"));
            x.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            x.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("AZEROTH\\Administrator", "Tridion2013SP1");

            return x;
        }

        private CheckIfModifiedAfterPublish.ModifiedAfterPublishReportGenerator generator;

        public UnitTest1()
        {
            ISessionAwareCoreService client = this.GetCoreServiceClient();
            this.generator = new CheckIfModifiedAfterPublish.ModifiedAfterPublishReportGenerator(client);
        }

        [TestMethod]
        public void TestApiVersion()
        {
            string apiVersion = generator.GetApiVersion();
            Assert.AreEqual("7.1.0", apiVersion);
        }

        [TestMethod]
        public void IsModifiedAfter2002()
        {
            DateTime when = DateTime.Parse("2002/01/01");
            string itemUri = "tcm:6-59-64";

            bool testData = generator.IsModifiedAfter(itemUri, when);

            Assert.AreEqual(true, testData);
        }

        [TestMethod]
        public void IsModifiedAfter2020()
        {
            DateTime when = DateTime.Parse("2020/01/01");
            string itemUri = "tcm:6-59-64";

            bool testData = generator.IsModifiedAfter(itemUri, when);

            Assert.AreEqual(false, testData);
        }

        [TestMethod]
        public void GetLastPublishDate()
        {
            string itemUri = "tcm:6-59-64";
            DateTime testData = generator.GetLastPublishDate(itemUri);
            Assert.AreNotEqual(DateTime.Now, testData);
        }

        [TestMethod]
        public void IsModifiedAfterPublish()
        {
            string itemUri = "tcm:6-59-64";
            bool testData = generator.IsModifiedAfterPublish(itemUri);
            Assert.AreEqual(false, testData);            
        }
    }
}
