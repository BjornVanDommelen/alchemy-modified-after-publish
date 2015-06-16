using Alchemy4Tridion.Plugins;
using System;
using System.Web;
using System.Web.Http;
using Tridion.ContentManager.CoreService.Client;

namespace CheckIfModifiedAfterPublish.Controllers
{
    [AlchemyRoutePrefix(typeof(AlchemyPlugin), "Service")]
    public class ServiceController : AlchemyApiController
    {
        // GET /Alchemy/Plugins/CheckIfModifiedAfterPublish/api/Service/GetReport/tcmUri
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
                var reportGenerator = new ModifiedAfterPublishReportGenerator(client);
                DateTime lastModificationDate = reportGenerator.GetLastModificationDate(tcmUri);
                DateTime lastPublishDate = reportGenerator.GetLastPublishDate(tcmUri);
                bool isModified = lastModificationDate > lastPublishDate;
                string report = "";
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

                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
