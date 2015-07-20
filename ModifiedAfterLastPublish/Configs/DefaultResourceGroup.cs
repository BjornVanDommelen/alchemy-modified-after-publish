//-----------------------------------------------------------------------
// <copyright file="DefaultResourceGroup.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish.Configs
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    /// <summary>
    /// Resource group with the client side files to use for the extension.
    /// </summary>
    public class DefaultResourceGroup : ResourceGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultResourceGroup"/> class.
        /// </summary>
        public DefaultResourceGroup()
        {
            this.AddFile("Check.js");
            this.AddFile<CheckCommandSet>();
            this.AddWebApiProxy();
        }
    }
}
