using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public class WinFileService : IFileService
    {
        public bool CheckPath(string path, bool endsWithFile)
        {
            return endsWithFile == true ? File.Exists(path) : Directory.Exists(path);
        }
    }
}
