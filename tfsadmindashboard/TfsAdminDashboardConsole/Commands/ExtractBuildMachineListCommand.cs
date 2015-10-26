using CsvHelper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using TfsAdminDashboardConsole.ExtensionsMethods;
using TfsAdminDashboardConsole.Service;

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractBuildMachineListCommand : TFSAccessHelper, iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExtractBuildMachineListCommand() { }

        public void Execute(CommandLineOptions args)
        {
            logger.Info("Extract Build Machines in progress...");
            ICollection<BuildServiceHostDefinition> buildServiceHostList = BuildServerHelper.GetAllBuildServiceHosts(configurationServer);

            ArrayList records = new ArrayList();
            foreach(BuildServiceHostDefinition buildServiceHostDefinition in buildServiceHostList)
            {
                foreach(BuildControllerDefinition controller in buildServiceHostDefinition.BuildControllers)
                {
                    records.Add(
                        new { 
                            HostName = buildServiceHostDefinition.Name,
                            IP = GetIPFromHostName(buildServiceHostDefinition.Name),
                            CollectionName = buildServiceHostDefinition.CollectionName,
                            ServiceType = "Controller",
                            Tags = string.Empty,
                            ServiceName = controller.Name,
                            Status = controller.Status
                            });
                }
                foreach (BuildAgentDefinition agent in buildServiceHostDefinition.BuildAgents)
                {
                    records.Add(
                        new
                        {
                            HostName = buildServiceHostDefinition.Name,
                            IP = GetIPFromHostName(buildServiceHostDefinition.Name),
                            CollectionName = buildServiceHostDefinition.CollectionName,
                            ServiceType = "Agent",
                            Tags = agent.Tags,
                            ServiceName = agent.Name,
                            Status = agent.Status
                        });
                }
            }

            string fileName = FileNameTool.GetFileName("TfsExtractMachineList", args.OutputFormat);

            if (args.OutputFormat == "CSV")
            {
                using (CsvWriter csv = new CsvWriter((new StreamWriter(fileName))))
                {
                    csv.WriteExcelSeparator();
                    csv.WriteRecords(records);
                }
            }
            else
            {
                string json = JsonConvert.SerializeObject(records);
                File.WriteAllText(fileName, json);
            }

            if (args.UploadSFTP)
            {
                logger.Info("STFP Upload");
                ISFTPService sftp = new SFTPService();
                sftp.UploadFile(fileName, args.SFTPAuthentMode.ToEnum<AuthentMethod>());
            }

            logger.Info("Extract Build Machines done");
        }

        /// <summary>
        /// Gets the name of the ip from host.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private string GetIPFromHostName(string host)
        {
            try
            { 
            IPHostEntry hostEntry;

                hostEntry = Dns.GetHostEntry(host + Environment.GetEnvironmentVariable("RDPDomain", EnvironmentVariableTarget.User));

                if (hostEntry.AddressList.Length > 0)
                {
                    var ip = hostEntry.AddressList[0];
                    return ip.ToString();
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                // No IP adress found for this host, no sweat though
                ;
            }

            return string.Empty;
        }
    }
}
