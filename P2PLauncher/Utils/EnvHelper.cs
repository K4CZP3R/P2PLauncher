using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P2PLauncher.Utils
{
    public static class EnvHelper
    {
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

        public static string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public static bool Is64Bit()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"));
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string GetPublicAddress()
        {
            try
            {
               return  new WebClient().DownloadString("http://icanhazip.com");
            }
            catch(Exception ex)
            {
                return "127.0.0.1";
            }
        }

    }
}
