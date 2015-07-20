//-----------------------------------------------------------------------
// <copyright file="ServiceController.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish.Controllers
{
    using System;
    using System.Web.Http;
    using Alchemy4Tridion.Plugins;
    using global::Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// Web API controller for the extension. This captures requests made by the client on the server
    ///   and routes them to the appropriate handler.
    /// </summary>
    [AlchemyRoutePrefix(typeof(AlchemyPlugin), "Service")]
    public class ServiceController : AlchemyApiController
    {
        /// <summary>
        /// Gets the modified-after-last-publish report on the given Tridion item.
        /// URL: GET /Alchemy/Plugins/CheckIfModifiedAfterPublish/api/Service/GetReport/tcmUri
        /// </summary>
        /// <param name="encodedUri">TCMURI of the tridion item in some encoded form</param>
        /// <returns>20X status code with JSON object containing report data or 400 result with error message</returns>
        [HttpGet]
        [Route("GetReport/{encodedUri}")]
        public IHttpActionResult GetReport(string encodedUri)
        {
            // Note: custom URI encoding because even encodeURI doesn't solve the problem
            //   that the controller doesn't want to fire.
            // So we just strip tcm: from the uri before passing it around. 
            // Reappend here.
            string tcmUri = "tcm:" + encodedUri;

            try
            {
                SessionAwareCoreServiceClient client = new SessionAwareCoreServiceClient("netTcp_2013");
                client.Impersonate("AZEROTH\\administrator");
                var reportGenerator = new TridionItemProcessor(client);
                DateTime lastModificationDate = reportGenerator.GetLastModificationDate(tcmUri);
                DateTime lastPublishDate = reportGenerator.GetLastPublishDate(tcmUri);
                bool isModified = lastModificationDate > lastPublishDate;
                string report = string.Empty;
                if (isModified)
                {
                    report = string.Format(
                        "NOK|Item {0} has been modified since it was last published.\nPublish date: {1}\nModification date:{2}",
                        tcmUri,
                        lastPublishDate,
                        lastModificationDate);
                }
                else
                {
                    report = string.Format(
                        "OK|Item {0} has not been modified since it was last published.\nPublish date: {1}\nModification date:{2}",
                        tcmUri,
                        lastPublishDate,
                        lastModificationDate);
                }

                client.Close();

                return this.Ok(report);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
