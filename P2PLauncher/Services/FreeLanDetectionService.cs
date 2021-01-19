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
        private readonly string DownloadUrl = "https://github.com/freelan-developers/freelan/releases";
        private readonly IFileService _fileService;
        private readonly IDialogService dialogService;

        public FreeLanDetectionService(IFileService fileService, IDialogService dialogService)
        {
            this._fileService = fileService;
            this.dialogService = dialogService;
        }

        public string GetFreeLanExecutableLocation()
        {
            return Properties.Settings.Default.FreeLanExecutableLocation;

        }

        /// <summary>
        /// Returns FreeLan download URL.
        /// </summary>
        /// <returns>Url pointing to the Download page of FreeLan.</returns>
        public string GetDownloadUrl()
        {
            return DownloadUrl;
        }

        /// <summary>
        /// Checks if config contains FreeLan path.
        /// </summary>
        /// <returns>True if config contains path.</returns>
        private bool IsConfigValid()
        {
            return !(Properties.Settings.Default.FreeLanExecutableLocation == null || Properties.Settings.Default.FreeLanExecutableLocation.Length == 0);
        }
        
        /// <summary>
        /// Checks if path from config is valid
        /// </summary>
        /// <returns>True if path specified in config is a real path.</returns>
        private bool IsPathValid()
        {
            return _fileService.CheckPath(Properties.Settings.Default.FreeLanExecutableLocation, true);
        }

        /// <summary>
        /// Opens Windows's File Dialog and saves selected path.
        /// </summary>
        /// <returns>True if File Dialog was not canceled.</returns>
        public bool SelectPath()
        {
            if(dialogService.OpenFileDialog())
            {
                SetFreelanPath(dialogService.FilePath);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Determines FreeLan installation status.
        /// </summary>
        /// <returns>Enum representing current installation status.</returns>
        public FreeLanInstallationStatus GetInstallationStatus()
        {
            if (!IsConfigValid()) return FreeLanInstallationStatus.CONFIG_NOT_SET;
            else if (!IsPathValid()) return FreeLanInstallationStatus.INVALID_PATH;
            return FreeLanInstallationStatus.OK;
        }

        /// <summary>
        /// Saves FreeLan path to the config file.
        /// </summary>
        /// <param name="path">Path of FreeLan executable.</param>
        public void SetFreelanPath(string path)
        {
            Properties.Settings.Default.FreeLanExecutableLocation = path;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Reload();
        }


        
        /// <summary>
        /// Tries to find FreeLan automatically.
        /// Searches through Program Files.
        /// </summary>
        /// <returns>True if FreeLan is found.</returns>
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
