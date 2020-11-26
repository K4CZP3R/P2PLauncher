using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public interface IFileService
    {
        bool CheckPath(string path, bool endsWithFile);
        bool FileContainsValue(string path, string value);
    }
}
