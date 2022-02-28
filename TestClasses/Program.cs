using System;
using ContextMenuCreator;
using ContextMenuCreator.Common;

namespace TestClasses
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Create Top Level
            Menu top = new Menu("Utilities");
            top.AssignDisplay(new ItemDisplay("Show Utilities"));
            top.SetRoot(Roots.DIRECTORY_BACKGROUND);
            #endregion

            #region Create entries
            ContextMenuItem cmd = new ContextMenuItem("cmd");
            cmd.AssignDisplay(new ItemDisplay("cmd.exe"));
            cmd.MapExe(new Executable("cmd.exe", new string[] { "%1" }));

            Menu subMenu = new Menu("editors");
            subMenu.AssignDisplay(new ItemDisplay("Text Editors"));

            ContextMenuItem notepad = new ContextMenuItem("notepad");
            notepad.AssignDisplay(new ItemDisplay("notepad"));
            notepad.MapExe(new Executable(@"C:\Windows\System32\notepad.exe", new string[] { }));

            ContextMenuItem notepadpp = new ContextMenuItem("notepadpp");
            notepadpp.AssignDisplay(new ItemDisplay("notepad++"));
            notepadpp.MapExe(new Executable(@"C:\Program Files\Notepad++\notepad++.exe", new string[] { }));
            #endregion

            #region Add entries and create
            top.AddContextMenuItem(cmd);
            subMenu.AddContextMenuItem(notepad);
            subMenu.AddContextMenuItem(notepadpp);
            top.AddSubMenu(subMenu);
            top.CreateAll();
            #endregion

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
