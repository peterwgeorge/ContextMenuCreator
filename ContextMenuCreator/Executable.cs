using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenuCreator
{
    public class Executable
    {
        private string[] args;
        private string path;

        public Executable(string pathToExe, string[] arguments)
        {
            args = arguments;
            path = pathToExe;
        }

        public (string filepath, string[] arguments) GetInfo()
        {
            return (path, args);
        }
    }
}
