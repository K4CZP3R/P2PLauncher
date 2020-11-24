using P2PLauncher.Exceptions;
using P2PLauncher.Models;
using P2PLauncher.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Modules
{
    class FreeLanModule
    {
        public static string FreeLanExecutableLocation = "bin\\freelan.exe";
        public static string ProgramRootDir = "FreeLAN";

        private ConfigurationModule ConfigurationModule;

        public FreeLanModule()
        {
            ConfigurationModule = new ConfigurationModule();
        }

        /// <summary>
        /// Gets status of freelan installation
        /// 
        /// (REWORK): Use simple bool and not enum.
        /// </summary>
        /// <returns>Current status of freelan installation</returns>
        public FreeLanStatus GetStatus()
        {
            try
            {
                ValidateFreeLanInstallation();
            }
            catch (FreeLanInstallationNotFoundException)
            {
                return FreeLanStatus.EXECUTABLE_UNKNOWN;
            }

            return FreeLanStatus.READY_TO_START;
        }

        /// <summary>
        /// Validate freelan path
        /// </summary>
        /// <exception cref="FreeLanExecutableLocation">Thrown when path was never there or is no more valid.</exception>
        private void ValidateFreeLanInstallation()
        {
            ConfigurationModel config = ConfigurationModule.Load();

            if (config.FreeLanExecutablePath == null)
            {
                throw new FreeLanInstallationNotFoundException("FreeLanExecutablePath is not set!");
            }

            if(!File.Exists(config.FreeLanExecutablePath))
            {
                throw new FreeLanInstallationNotFoundException("FreeLanExecutablePath is invalid!");
            }

        }

        /// <summary>
        /// Try to run through known paths for FreeLan
        /// </summary>
        /// <exception cref="FreeLanInstallationNotFoundException">Throws when known paths are not containing FreeLan (User does not have FreeLan / installed is somewhere else)</exception>
        public void TryFindFreeLanInstallation()
        {
            List<string> possiblePaths = new List<string>();

            string programFilesLocation = EnvironmentUtils.GetProgramFilesPath();
            string programFilesX86Location = EnvironmentUtils.GetProgramFilesX86Path();

            possiblePaths.Add($"{programFilesLocation}\\{ProgramRootDir}\\{FreeLanExecutableLocation}");
            possiblePaths.Add($"{programFilesX86Location}\\{ProgramRootDir}\\{FreeLanExecutableLocation}");

            foreach (string path in possiblePaths)
            {
                if (EnvironmentUtils.FileExists(path))
                {
                    ConfigurationModel config = ConfigurationModule.Load();
                    config.FreeLanExecutablePath = path;
                    ConfigurationModule.Save(config);
                    return;
                }

            }

            throw new FreeLanInstallationNotFoundException("Could not find freelan in the defined paths...");
        }

    }
}
