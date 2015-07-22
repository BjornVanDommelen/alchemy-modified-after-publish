//-----------------------------------------------------------------------
// <copyright file="ModifiedAfterPublishReport.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    /// <summary>
    /// Data class for holding the JSON data for the web service result.
    /// </summary>
    public class ModifiedAfterPublishReport
    {
        /// <summary>
        /// Gets or sets a value indicating whether the item was modified after it has been published.
        /// </summary>
        public bool IsModifiedAfterPublish { get; set; }

        /// <summary>
        /// Gets or sets the textual representation of the report.
        /// </summary>
        public string ReportText { get; set; }
    }
}
