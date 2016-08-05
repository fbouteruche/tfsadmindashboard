using System;

namespace TFSDataService
{
    static class DataServiceBase
    {
        private static bool alreadyChecked = false;

        public static void CheckVariables()
        {
            if (!alreadyChecked)
            {
                CheckEnvVar("TfsUrl");
                CheckEnvVar("TfsLoginName");
                CheckEnvVar("TfsPassword");
                alreadyChecked = true;
            }
        }

        private static void CheckEnvVar(string variable)
        {
            if (Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.User) == null)
            {
                throw new Exception(string.Format("No {0} Environment Variable found, please create it. Environment varables required: TfsUrl (like http://mytfs:8080/tfs), TfsLoginName (with AD prefix if necessary), and TfsPassword", variable));
            }
        }
    }
}