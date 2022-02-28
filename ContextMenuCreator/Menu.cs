using System;
using System.Collections.Generic;
using ContextMenuCreator.Common;
using Microsoft.Win32;

namespace ContextMenuCreator
{
    public class Menu : ContextMenuRegistryMapping
    {
        private List<string> subKeys;
        private List<Menu> submenus;
        private List<ContextMenuItem> items;

        public string MySubkey;

        public Menu(string subKeyName, Menu owner = null)
        {
            Parent = owner;
            MySubkey = subKeyName;
            submenus = new List<Menu>();
            items = new List<ContextMenuItem>();
            rootPathSet = false;
            keysVerified = false;
            displayAssigned = false;
        }

        public void AddSubMenu(Menu subMenu)
        {
            subMenu.Parent = this;
            submenus.Add(subMenu);
        }

        public void AddContextMenuItem(ContextMenuItem item)
        {
            item.Parent = this;
            items.Add(item);
        }

        public void CreateAll()
        {
            if (keysVerified && displayAssigned && rootPathSet)
            {
                this.CreateMyself();
                foreach (ContextMenuItem item in items)
                {
                    item.SetRoot();
                    item.CreateMenuItem();
                }

                foreach (Menu menu in submenus)
                {
                    menu.SetRoot();
                    menu.CreateAll();
                }
            }
            else
            {
                throw new Exception("Cannot create context menu item. Ensure subkeys exist, and that AssignDisplay and SetRoot have been called prior.");
            }
        }

        private void CreateMyself()
        {
            string mySubKeyPath = rootPath + "\\" + MySubkey;
            string shellSubKey = mySubKeyPath + "\\" + GetNameString(Names.SHELL);
            
            (string textDisplayedInMenu, string icon) = display.GetInfo();
            Registry.SetValue(mySubKeyPath, GetNameString(Names.MUIVERB), textDisplayedInMenu);
            Registry.SetValue(mySubKeyPath, GetNameString(Names.SUBCOMMANDS), "");
            if (icon == "")
            {
               
            }
            else
            {
                Registry.SetValue(mySubKeyPath, GetNameString(Names.ICON), icon);
            }
            Registry.SetValue(shellSubKey, GetNameString(Names.DEFAULT), "");
        }
    }
}

