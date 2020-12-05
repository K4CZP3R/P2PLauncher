using P2PLauncher.Model;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Services
{
    public class FreeLanDetectionService
    {
        private readonly string FreeLanExecutableLocation = "bin\\freelan.exe";
        private readonly string ProgramRootDir = "FreeLAN";
        private readonly string DownloadUrl = "https://www.freelan.org/download.html#windows";
        private readonly IFileService _fileService;
        private readonly IDialogService dialogService;

        public FreeLanDetectionService(IFileService fileService, IDialogService dialogService)
        {
            this._fileService = fileService;
            this.dialogService = dialogService;
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

        public bool SelectPath()
        {
            if(dialogService.OpenFileDialog())
            {
                SetFreelanPath(dialogService.FilePath);
                return true;
            }
            return false;
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
