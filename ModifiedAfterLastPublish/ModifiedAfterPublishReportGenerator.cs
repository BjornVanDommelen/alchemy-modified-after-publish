using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tridion.ContentManager.CoreService.Client;

namespace CheckIfModifiedAfterPublish
{
    public class ModifiedAfterPublishReportGenerator
    {
        protected ISessionAwareCoreService Client { get; set; }

        public ModifiedAfterPublishReportGenerator(ISessionAwareCoreService client)
        {
            this.Client = client;
        }

        public string GetApiVersion()
        {
            return this.Client.GetApiVersion();
        }

        public bool IsModifiedAfterPublish(string tcmUri)
        {
            return IsModifiedAfter(tcmUri, GetLastPublishDate(tcmUri));
        }

        public DateTime GetLastPublishDate(string tcmUri)
        {
            var publishInfo = Client.GetListPublishInfo(tcmUri);
            return publishInfo.Max(pi => pi.PublishedAt);
        }

        public DateTime GetLastModificationDate(string tcmUri)
        {
            var item = this.Client.Read(tcmUri, new ReadOptions());
            return item.VersionInfo.RevisionDate ?? DateTime.MinValue;
        }

        public bool IsModifiedAfter(string tcmUri, DateTime afterWhen)
        {
            return GetLastModificationDate(tcmUri) > afterWhen;
        }
    }
}
