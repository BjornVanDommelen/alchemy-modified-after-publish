using Alchemy4Tridion.Plugins.GUI.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIfModifiedAfterPublish.Configs
{
    public class CheckCommandSet : CommandSet
    {
        public CheckCommandSet()
        {
            AddCommand("Check", "Alchemy.Plugins.CheckIfModifiedAfterPublish.Commands.Check");
        }
    }
}
