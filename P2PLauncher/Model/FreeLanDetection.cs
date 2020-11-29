using P2PLauncher.Services;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Model
{
    public class FreeLanDetection
    {

        private readonly string FreeLanExecutableLocation = "bin\\freelan.exe";
        private readonly string ProgramRootDir = "FreeLAN";
        private readonly string DownloadUrl = "https://www.freelan.org/download.html#windows";
        private readonly IFileService _fileService;

        public FreeLanDetection(IFileService fileService)
        {
            this._fileService = fileService;

        }


        public string GetDownloadUrl()
        {
            return DownloadUrl;
        }

        private bool IsConfigValid()
        {
            return !(Properties.Settings.Default.FreeLanExecutableLocation == null || Properties.Settings.Default.FreeLanExecutableLocation.Length == 0);
        }
        private bool IsPathValid()
        {
            return _fileService.CheckPath(Properties.Settings.Default.FreeLanExecutableLocation, true);
        }


        public FreeLanInstallationStatus GetInstallationStatus()
        {
            if (!IsConfigValid()) return FreeLanInstallationStatus.CONFIG_NOT_SET;
            else if (!IsPathValid()) return FreeLanInstallationStatus.INVALID_PATH;
            return FreeLanInstallationStatus.OK;
        }

        public void SetFreelanPath(string path)
        {
            Properties.Settings.Default.FreeLanExecutableLocation = path;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }


        public bool FindFreeLan()
        {
            List<string> possiblePaths = new List<string>();

            string pfLocation = EnvHelper.GetProgramFilesPath();
            string pfx86Location = EnvHelper.GetProgramFilesX86Path();

            possiblePaths.Add($"{pfLocation}\\{ProgramRootDir}\\{FreeLanExecutableLocation}");
            possiblePaths.Add($"{pfx86Location}\\{ProgramRootDir}\\{FreeLanExecutableLocation}");

            foreach (string path in possiblePaths)
            {
                if (_fileService.CheckPath(path, true))
                {
                    SetFreelanPath(path);
                    return true;
                }
            }
            return false;

        }



    }
}
