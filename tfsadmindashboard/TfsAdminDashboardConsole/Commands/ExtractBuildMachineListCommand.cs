﻿using CsvHelper;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using System.Net.Sockets;
using TfsAdminDashboardConsole.Commands.IO;
using NLog;

namespace TfsAdminDashboardConsole.Commands
{
    class ExtractBuildMachineListCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

        public ExtractBuildMachineListCommand() { }

        public void Execute()
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
                            ServiceName = agent.Name,
                            Status = agent.Status
                        });
                }
            }

            string fileName = FileNameTool.GetFileName("TfsExtractMachineList");

            using (CsvWriter csv = new CsvWriter((new StreamWriter(fileName))))
            {
                csv.WriteExcelSeparator();
                csv.WriteRecords(records);
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
