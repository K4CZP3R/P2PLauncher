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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <exception cref="FileNotFoundException">File does not exist.</exception>
        /// <returns></returns>
        public bool FileContainsValue(string path, string value)
        {

            if (!CheckPath(path, true))
            {
                throw new FileNotFoundException();
            }
            return File.ReadAllText(path).Contains(value);
        }
    }
}
