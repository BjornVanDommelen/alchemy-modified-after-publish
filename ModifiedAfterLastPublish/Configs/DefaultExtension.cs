using Alchemy4Tridion.Plugins.GUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIfModifiedAfterPublish.Configs
{
    public class DefaultExtension : ContextMenuExtension
    {
        public DefaultExtension()
        {
            AssignId = "CheckIfModifiedAfterPublishMenu";
            Name = "CheckIfModifiedAfterPublish";
            InsertBefore = "cm_refresh";
            AddItem("CheckIfModifiedAfterPublish_Menu1", "Has it changed", "Check");
            this.Dependencies.Add<DefaultResourceGroup>();
            Apply.ToView(Constants.Views.DashboardView);
        }
    }
}
