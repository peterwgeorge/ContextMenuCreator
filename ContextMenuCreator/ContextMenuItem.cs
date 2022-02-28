using System;
using System.Collections.Generic;
using Microsoft.Win32;
using ContextMenuCreator.Common;

namespace ContextMenuCreator
{
    public class ContextMenuItem : ContextMenuRegistryMapping
    {
        const string quote = "\"";
        private Executable exe;
        private bool exeMapped;
        
        public string MySubkey; //name in registry

        public ContextMenuItem(string keyName, Menu owner = null)
        {
            Parent = owner;
            MySubkey = keyName;
            keysVerified = false;
            exeMapped = false;
            displayAssigned = false;
            rootPathSet = false;
        }

        public void MapExe(Executable underlyingExe)
        {
            exe = underlyingExe;
            exeMapped = true;
        }

        public void CreateMenuItem()
        {
            if (keysVerified && exeMapped && displayAssigned && rootPathSet)
            {
                string menuItemSubKey = rootPath + "\\" + MySubkey;
                string menuItemCommandSubKey = menuItemSubKey + "\\" + GetNameString(Names.COMMAND);
                (string textDisplayedInMenu, string icon) = display.GetInfo();
                (string pathToExeOnDisk, string[] args) = exe.GetInfo();
                Registry.SetValue(menuItemSubKey, GetNameString(Names.DEFAULT), textDisplayedInMenu);
                if (icon == "")
                {
                    Registry.SetValue(menuItemSubKey, GetNameString(Names.ICON), pathToExeOnDisk);
                }
                else
                {
                    Registry.SetValue(menuItemSubKey, GetNameString(Names.ICON), icon);
                }

                string command = CreateExeCommand();
                Registry.SetValue(menuItemCommandSubKey, GetNameString(Names.DEFAULT), command);
            }
            else
            {
                throw new Exception("Cannot create context menu item. Ensure subkeys exist, and that AssignDisplay, MapExe, and SetRoot have been called prior.");
            }
        }

        private string CreateExeCommand()
        {
            (string pathToExeOnDisk, string[] argumentsToExe) = exe.GetInfo();
            string command = quote + pathToExeOnDisk + quote + " ";
            foreach (string arg in argumentsToExe)
            {
                command += quote + arg + quote + " ";
            }
            command.TrimEnd();
            return command;
        }

    }
}
