//-----------------------------------------------------------------------
// <copyright file="CheckCommandSet.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish.Configs
{
    using Alchemy4Tridion.Plugins.GUI.Configuration;

    /// <summary>
    /// Command set for the extension.
    /// </summary>
    public class CheckCommandSet : CommandSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckCommandSet"/> class.
        /// </summary>
        public CheckCommandSet()
        {
            this.AddCommand("Check", "Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check");
        }
    }
}
