//-----------------------------------------------------------------------
// <copyright file="DefaultExtension.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish.Configs
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    /// <summary>
    /// Extension definition. This configures the extension as a context menu extension.
    /// </summary>
    public class DefaultExtension : ContextMenuExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExtension"/> class.
        /// </summary>
        public DefaultExtension()
        {
            // Unique ID of the extension.
            this.AssignId = "CheckIfModifiedAfterPublishMenu";

            // Display name of the extension.
            this.Name = "CheckIfModifiedAfterPublish";

            // Where the items in the context menu are added.
            this.InsertBefore = "cm_refresh";

            // What menu entries to add.
            this.AddItem("CheckIfModifiedAfterPublish_Menu1", "Has it changed", "Check");

            // Dependencies to load. This must include the resource group.
            this.Dependencies.Add<DefaultResourceGroup>();

            // Where to apply the context menu extension.
            Apply.ToView(Constants.Views.DashboardView);
        }
    }
}
