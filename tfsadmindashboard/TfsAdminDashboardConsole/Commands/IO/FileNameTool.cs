using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Commands.IO
{
    static class FileNameTool
    {
        /// <summary>
        /// Gets a dated file name.
        /// </summary>
        /// <param name="envVFileName">Name of the env variable containing the file name.</param>
        /// <returns></returns>
        public static string GetFileName(string envVFileName)
        {
            return Path.Combine(
                Environment.GetEnvironmentVariable("TfsExtractPath", EnvironmentVariableTarget.User),
                DateTime.Now.ToString("yyyy_MM_dd") + "_" +
                Environment.GetEnvironmentVariable(envVFileName, EnvironmentVariableTarget.User));

        }
    }
}
