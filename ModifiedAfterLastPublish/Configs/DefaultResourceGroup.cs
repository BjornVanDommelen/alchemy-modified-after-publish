using Alchemy4Tridion.Plugins.GUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIfModifiedAfterPublish.Configs
{
    public class DefaultResourceGroup : ResourceGroup
    {
        public DefaultResourceGroup()
        {
            AddFile("Check.js");
            AddFile<CheckCommandSet>();
            AddWebApiProxy();
        }
    }
}
