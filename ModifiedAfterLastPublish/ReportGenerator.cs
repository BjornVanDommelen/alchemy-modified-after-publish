//-----------------------------------------------------------------------
// <copyright file="ReportGenerator.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    using System;

    /// <summary>
    /// Generates reports on item modification state.
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// Tridion item processor to use.
        /// </summary>
        private readonly TridionItemProcessor processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerator"/> class.
        /// </summary>
        /// <param name="processor">Tridion item processor to use</param>
        public ReportGenerator(TridionItemProcessor processor)
        {
            this.processor = processor;
        }

        /// <summary>
        /// Generates the report for a single item.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of tridion item</param>
        /// <returns>Report object for JSON serialization</returns>
        public ModifiedAfterPublishReport Generate(string tcmUri)
        {
            DateTime lastModified = this.processor.GetLastModificationDate(tcmUri);
            DateTime lastPublished = this.processor.GetLastPublishDate(tcmUri);
            return new ModifiedAfterPublishReport()
            {
                IsModifiedAfterPublish = lastModified > lastPublished,
                ReportText = string.Format(
                    "The item {0} was last published at {1} and last modified at {2}",
                    tcmUri,
                    this.FormatDate(lastPublished),
                    this.FormatDate(lastModified))
            };
        }

        /// <summary>
        /// Formats the given date time.
        /// </summary>
        /// <param name="dtm">Date time to format</param>
        /// <returns>Formatted date time as string</returns>
        private string FormatDate(DateTime dtm)
        {
            return dtm.ToString();
        }
    }
}
