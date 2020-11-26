using P2PLauncher.Exceptions;
using P2PLauncher.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace P2PLauncher.Utils
{
    class EnvironmentUtils
    {

        /// <summary>
        /// Uses HTTP request to detect client's public ip address.
        /// </summary>
        /// <exception cref="EnvironmentErrorException">Website returning client's ip address returned something else.</exception>
        /// <returns>Client's public IP address.</returns>
        public static string GetIPAddress()
        {
            string serverResponse = WebRequestModule.MakeGetRequest("http://checkip.dyndns.org/");
            MatchCollection matchCollection = new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}").Matches(serverResponse);
            if (matchCollection.Count > 0)
            {
                throw new EnvironmentErrorException("IP request returned invalid data.");
            }
            return matchCollection[0].ToString();
        }


        /// <summary>
        /// Get Program Files path (x64)
        /// Variable was introduced in Windows 7 (So it won't work in releases before W7)
        /// </summary>
        /// <returns>Path of Program Files</returns>
        public static string GetProgramFilesPath()
        {
            return Environment.ExpandEnvironmentVariables("%ProgramW6432%");
        }

        /// <summary>
        /// Get Program Files path (x86)
        /// Variable was introduced in Windows 7 (So it won't work in releases before W7)
        /// </summary>
        /// <returns>Path of Program Files x86</returns>
        public static string GetProgramFilesX86Path()
        {
            return Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%");
        }

        /// <summary>
        /// Checks if path exists
        /// </summary>
        /// <param name="filePath">File + Path</param>
        /// <returns>Answers question: "Does this path exist?"</returns>
        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
