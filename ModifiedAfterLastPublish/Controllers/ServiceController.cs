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
        /// The core service client to use.
        /// </summary>
        private readonly SessionAwareCoreServiceClient client; 

        /// <summary>
        /// The report generator to use.
        /// </summary>
        private readonly ReportGenerator generator; 

        /// <summary>
        /// Flag indicating whether the instance has already been disposed.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceController"/> class.
        /// </summary>
        public ServiceController()
        {
            this.client = new SessionAwareCoreServiceClient("netTcp_2013");
            this.generator = new ReportGenerator(new TridionItemProcessor(this.client));
        }

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
            string tcmUri = DecodeTcmuri(encodedUri);

            try
            {
                // Note: we need to enforce security so impersonate the current user at every request.
                this.ImpersonateCurrentUser();
                return this.Json<ModifiedAfterPublishReport>(this.generator.Generate(tcmUri));
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        #region Overrides of base class (for IDisposable)
        /// <summary>
        /// Cleans up resources held by this instance.
        /// </summary>
        /// <param name="disposing">True when explicitly called from code; false when called from finalizer</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    // Release managed resources
                    if (this.client != null)
                    {
                        this.client.Close();
                    }
                }

                // Cleanup unmanaged resources

                // Register that this instance has been disposed
                this.isDisposed = true;

                // Call base class dispose
                base.Dispose(disposing);
            }
        }
        #endregion

        /// <summary>
        /// Decodes the Tcm URI from the parameter encoded form.
        /// </summary>
        /// <param name="encodedTcmUri">Parameter encoded form of Tcm URI</param>
        /// <returns>Valid Tcm URI as string</returns>
        private static string DecodeTcmuri(string encodedTcmUri)
        {
            return "tcm:" + encodedTcmUri;
        }

        /// <summary>
        /// Determines the windows user name of the current logged on user.
        /// </summary>
        /// <returns>Name of the current logged on user</returns>
        private static string GetCurrentUser()
        {
            return System.Web.HttpContext.Current.User.Identity.Name;
        }

        /// <summary>
        /// Impersonates the current user with the core service client.
        /// </summary>
        private void ImpersonateCurrentUser()
        {
            string user = GetCurrentUser();
            try
            {
                this.client.Impersonate(user);
            }
            catch (Exception ex)
            {
                throw new ImpersonationException(user, ex);
            }
        }
    }
}
