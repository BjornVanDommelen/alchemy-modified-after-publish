//-----------------------------------------------------------------------
// <copyright file="TridionItemProcessorTest.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublishTest
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// Tests for the core service client.
    /// These represent the tests for the actual business logic of the plugin
    ///   which is all executed server side.
    /// </summary>
    [TestClass]
    public class TridionItemProcessorTest
    {
        /// <summary>
        /// Host name to connect to.
        /// </summary>
        private const string SERVICEHOST = "http://azeroth.local:81/";

        /// <summary>
        /// User name to connect with.
        /// </summary>
        private const string SERVICEUSER = "AZEROTH\\Administrator";

        /// <summary>
        /// Password to connect with.
        /// </summary>
        private const string SERVICEPSWD = "Tridion2013SP1";

        /// <summary>
        /// Object to test.
        /// </summary>
        private CheckIfModifiedAfterPublish.TridionItemProcessor generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TridionItemProcessorTest"/> class.
        /// Initializes the core service client and report generator.
        /// </summary>
        public TridionItemProcessorTest()
        {
            ISessionAwareCoreService client = CoreServiceClientFactory.CreateClient(SERVICEHOST, SERVICEUSER, SERVICEPSWD);
            this.generator = new CheckIfModifiedAfterPublish.TridionItemProcessor(client);
        }

        /// <summary>
        /// Tests if the generator correctly asserts a last-modified date to be greater than
        ///   a known date in the past.
        /// </summary>
        [TestMethod]
        public void IsModifiedAfter2002()
        {
            DateTime when = DateTime.Parse("2002/01/01");
            string itemUri = "tcm:6-59-64";

            bool testData = this.generator.IsModifiedAfter(itemUri, when);

            Assert.AreEqual(true, testData);
        }

        /// <summary>
        /// Tests if the generator correctly asserts a last modified date to be smaller than
        ///   a known date in the future.
        /// </summary>
        [TestMethod]
        public void IsModifiedAfter2020()
        {
            DateTime when = DateTime.Parse("2020/01/01");
            string itemUri = "tcm:6-59-64";

            bool testData = this.generator.IsModifiedAfter(itemUri, when);

            Assert.AreEqual(false, testData);
        }

        /// <summary>
        /// Tests if the generator can determine the last publish date of a page.
        /// </summary>
        [TestMethod]
        public void GetLastPublishDate()
        {
            string itemUri = "tcm:6-59-64";
            DateTime testData = this.generator.GetLastPublishDate(itemUri);
            Assert.AreNotEqual(DateTime.Now, testData);
        }

        /// <summary>
        /// Tests is the generator can correctly assess if the last publish date
        ///   of a page is older than the last modification date of all items in the
        ///   dependency graph.
        /// </summary>
        [TestMethod]
        public void IsModifiedAfterPublish()
        {
            string itemUri = "tcm:6-59-64";
            bool testData = this.generator.IsModifiedAfterPublish(itemUri);
            Assert.AreEqual(false, testData);            
        }
    }
}
