using CsvHelper;
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

namespace TfsAdminDashboardConsole
{
    class ExtractBuildMachineListCommand
    {
        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

        public ExtractBuildMachineListCommand() { }

        public void Execute(string[] args)
        {
            ICollection<BuildServiceHostDefinition> buildServiceHostList = BuildServerHelper.GetAllBuildServiceHosts(configurationServer);

            ArrayList records = new ArrayList();
            foreach(BuildServiceHostDefinition buildServiceHostDefinition in buildServiceHostList)
            {
                foreach(BuildControllerDefinition controller in buildServiceHostDefinition.BuildControllers)
                {
                    records.Add(
                        new { 
                            HostName = buildServiceHostDefinition.Name,
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
                            CollectionName = buildServiceHostDefinition.CollectionName,
                            ServiceType = "Agent",
                            ServiceName = agent.Name,
                            Status = agent.Status
                        });
                }
            }

            CsvWriter csv = new CsvWriter(new StreamWriter(Path.Combine(Environment.GetEnvironmentVariable("TfsExtractPath", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsExtractMachineList", EnvironmentVariableTarget.User))));
            csv.WriteRecords(records);
        }
    }
}
