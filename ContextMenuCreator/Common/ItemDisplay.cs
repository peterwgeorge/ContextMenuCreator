using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenuCreator.Common
{
    public class ItemDisplay
    {
        private string icon;
        private string textInMenu;

        public ItemDisplay(string textDisplay, string iconPath="")
        {
            icon = iconPath;
            textInMenu = textDisplay;
        }

        public (string textDisplay, string iconPath) GetInfo()
        {
            return (textInMenu, icon);
        }
    }
}
