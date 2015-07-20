//-----------------------------------------------------------------------
// <copyright file="TridionItemProcessor.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::Tridion.ContentManager.CoreService.Client;

    /// <summary>
    /// Implements the business logic for determining if a page is modified after it has been last published.
    /// </summary>
    public class TridionItemProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TridionItemProcessor"/> class.
        /// </summary>
        /// <param name="client">The core service client to use.</param>
        public TridionItemProcessor(ISessionAwareCoreService client)
        {
            this.Client = client;
        }

        /// <summary>
        /// Gets or sets the core service client to use.
        /// </summary>
        protected ISessionAwareCoreService Client { get; set; }

        /// <summary>
        /// Determines if the given page or component (or any of its used components) 
        ///   is modified after the last time it was published.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the page or component as string</param>
        /// <returns>True if the page or component or any of its used components are modified after the last publish date of the item; false otherwise</returns>
        public bool IsModifiedAfterPublish(string tcmUri)
        {
            return this.IsModifiedAfter(tcmUri, this.GetLastPublishDate(tcmUri));
        }

        /// <summary>
        /// Determines if the given page or component (or any of its used components) 
        ///   is modified after the given date.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the page or component as string</param>
        /// <param name="afterWhen">Date and time to check against</param>
        /// <returns>True if the page or component or any of its used components are modified after the given date; false otherwise</returns>
        public bool IsModifiedAfter(string tcmUri, DateTime afterWhen)
        {
            return this.GetLastModificationDate(tcmUri) > afterWhen;
        }

        /// <summary>
        /// Determines the last time this item was published.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the page or component as string</param>
        /// <returns>Date and time the item was last published or lowest possible date and time value if the item has never been published</returns>
        public DateTime GetLastPublishDate(string tcmUri)
        {
            var publishInfo = this.Client.GetListPublishInfo(tcmUri);
            return publishInfo.Count() == 0 ? DateTime.MinValue : publishInfo.Max(pi => pi.PublishedAt);
        }

        /// <summary>
        /// Determines the date a page or component was last modified recursively by
        ///   walking the tree of dependencies (using items).
        /// Note that only components as using items are taken into account
        ///   to optimize performance...
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the page or component as string</param>
        /// <returns>DateTime when the item or any of its dependencies were last modified</returns>
        public DateTime GetLastModificationDate(string tcmUri)
        {
            var item = this.Client.Read(tcmUri, new ReadOptions());
            if (item == null)
            {
                throw new ItemNotFoundException(tcmUri);
            }
            else
            {
                return this.GetLastModificationDate(item);
            }
        }

        /// <summary>
        /// Determines the date a page or component was last modified recursively by
        ///   walking the tree of dependencies (using items).
        /// Note that only components as using items are taken into account
        ///   to optimize performance...
        /// </summary>
        /// <param name="item">The page or component</param>
        /// <returns>DateTime when the item or any of its dependencies were last modified</returns>
        public DateTime GetLastModificationDate(IdentifiableObjectData item)
        {
            return this.Latest(
                item.VersionInfo.RevisionDate ?? DateTime.MinValue, 
                this.GetLastChildModificationDate(item.Id));
        }

        /// <summary>
        /// Returns the latest date time value in a sequence.
        /// </summary>
        /// <param name="dateTimeValues">Sequence of date time values</param>
        /// <returns>Latest date time value</returns>
        /// <example>Latest(DateTime.Parse("2012/03/03"), DateTime.Parse("2012/01/01"), DateTime.Parse("2012/02/02")) => 2012/03/03</example>
        private DateTime Latest(params DateTime[] dateTimeValues)
        {
            return dateTimeValues.Count() == 0 ? DateTime.MinValue : dateTimeValues.Max();
        }

        /// <summary>
        /// Determines the last modification date of the using components of the given Tridion item.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the page or component as string</param>
        /// <returns>Latest date time any of the used components of the given page or component were modified</returns>
        private DateTime GetLastChildModificationDate(string tcmUri)
        {
            var children = this.GetUsedComponents(tcmUri);
            
            // Return highest value of last modification date if any; otherwise return lowest possible DateTime value
            return children.Count() == 0 ? DateTime.MinValue : children.Max(c => this.GetLastModificationDate(c.Id));
        }

        /// <summary>
        /// Gets the list of using items.
        /// </summary>
        /// <param name="tcmUri">Tcm URI of the Tridion item</param>
        /// <returns>Collection of IdentifiableObject items that are used components</returns>
        private IEnumerable<IdentifiableObjectData> GetUsedComponents(string tcmUri)
        {
            return this.Client.GetList(
                tcmUri,
                new UsedItemsFilterData()
                {
                    ItemTypes = new[] 
                    { 
                        ItemType.Component 
                    }
                });
        }
    }
}
