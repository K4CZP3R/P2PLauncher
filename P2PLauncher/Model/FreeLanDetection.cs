using P2PLauncher.Services;
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

        private readonly string MagicValue = "FreeLan";

        private readonly IFileService _fileService;
        public string InstallationInfo { get; set; }

        public FreeLanInstallationStatus InstallationStatus { get; set; }


        public FreeLanDetection(IFileService fileService)
        {
            this._fileService = fileService;

        }

        private bool IsConfigValid()
        {
            return !(Properties.Settings.Default.FreeLanExecutableLocation == null);
        }
        private bool IsPathValid()
        {
            return _fileService.CheckPath(Properties.Settings.Default.FreeLanExecutableLocation, true);
        }

        private bool IsExecutableValid()
        {
            return _fileService.FileContainsValue(Properties.Settings.Default.FreeLanExecutableLocation, MagicValue);
        }

        public FreeLanInstallationStatus GetInstallationStatus()
        {
            if (!IsConfigValid()) return FreeLanInstallationStatus.CONFIG_NOT_SET;
            if (!IsPathValid()) return FreeLanInstallationStatus.INVALID_PATH;
            if (!IsExecutableValid()) return FreeLanInstallationStatus.INVALID_EXECUTABLE;
            return FreeLanInstallationStatus.OK;
        }

    }
}
