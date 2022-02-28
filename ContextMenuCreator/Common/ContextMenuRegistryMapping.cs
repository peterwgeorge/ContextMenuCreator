using System;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenuCreator.Common
{
    public abstract class ContextMenuRegistryMapping
    {
        
        protected ItemDisplay display;
        protected string rootPath;
        protected bool rootPathSet;
        protected bool keysVerified;
        protected bool displayAssigned;

        public Menu Parent;

        protected virtual string DetermineRootPath(Roots root)
        {
            switch (root)
            {
                case Roots.DIRECTORY:
                    return "HKEY_CLASSES_ROOT\\Directory\\shell";
                case Roots.DIRECTORY_BACKGROUND:
                    return "HKEY_CLASSES_ROOT\\Directory\\Background\\shell";
                case Roots.FILES:
                    return "HKEY_CLASSES_ROOT\\*\\shell";
                default:
                    throw new ArgumentException("Unsupported root selected.");
            }
        }

        protected void VerifyRegistryKeys(Roots root)
        {
            if (rootPathSet == false)
            {
                throw new Exception("Rootpath must be set prior to verification of registry keys.");
            }
            RegistryKey HKCR = Registry.ClassesRoot;
            RegistryKey topLevel = null;
            string[] ownersSubKeys = rootPath.Replace("HKEY_CLASSES_ROOT\\", "").Split("\\", StringSplitOptions.RemoveEmptyEntries);
            switch (root)
            {
                case Roots.FILES:
                    topLevel = HKCR.OpenSubKey("*").OpenSubKey("shell");
                    break;
                case Roots.DIRECTORY:
                    topLevel = HKCR.OpenSubKey("Directory").OpenSubKey("shell");
                    break;
                case Roots.DIRECTORY_BACKGROUND:
                    topLevel = HKCR.OpenSubKey("Directory").OpenSubKey("Background").OpenSubKey("shell");
                    break;
                case Roots.SUBMENU:
                    RegistryKey temp = HKCR.OpenSubKey(ownersSubKeys[0]);
                    foreach (string key in ownersSubKeys[1..(ownersSubKeys.Length - 1)])
                    {
                        topLevel = temp.OpenSubKey(key);
                        temp = topLevel;
                    }
                    break;
                default:
                    throw new ArgumentException("Unsupported root selected.");
            }

            if (topLevel == null)
            {
                throw new Exception("Directory is not a subkey of HKCR - cannot proceed.");
            }

            keysVerified = true;
        }

        public virtual void SetRoot(Roots hkcr_software = Roots.SUBMENU)
        {
            if (hkcr_software != Roots.SUBMENU && Parent != null)
            {
                //Attempting to set root path for a submenu member
                throw new ArgumentException("Root for a submenu must be set by parent menu subkeys.");
            }
            else if (hkcr_software != Roots.SUBMENU && Parent == null)
            {
                //Setting root path for the root node.
                rootPath = DetermineRootPath(hkcr_software);
            }
            else if (hkcr_software == Roots.SUBMENU && Parent != null)
            {
                //Setting root path for submenu member
                rootPath = Parent.rootPath + "\\" + Parent.MySubkey + "\\" + "shell";
            }
            if (rootPath != null)
            {
                rootPathSet = true;
            }
            else
            {
                throw new Exception("Attempting to manually create Submenu item without parent. Top Level sets root.");
            }
            
            VerifyRegistryKeys(hkcr_software);
        }

        public virtual void AssignDisplay(ItemDisplay itemDisplay)
        {
            display = itemDisplay;
            displayAssigned = true;
        }

        protected virtual string GetNameString(Names name)
        {
            switch (name)
            {
                case Names.MUIVERB:
                    return "MUIVerb";
                case Names.DEFAULT:
                    return "";
                case Names.ICON:
                    return "Icon";
                case Names.SHELL:
                    return "shell";
                case Names.SUBCOMMANDS:
                    return "subcommands";
                case Names.COMMAND:
                    return "command";
                default:
                    throw new ArgumentException("Unknown name provided.");
            }
        }
    }
}
