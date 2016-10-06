using NLog;
using PostMan;
using System;
using TfsAdminDashboardConsole.Commands;

namespace TfsAdminDashboardConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            try
            {
                var options = new CommandLineOptions();
                bool processed = false;

                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();

               
                sw.Start();

                logger.Info("{0} v.{1}", assemblyName.Name, assemblyName.Version);

                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    processed = (options.extractProjects || options.extractUsers || options.extractSimpleProjects) == true;

                    if (options.extractProjects == true)
                    {
                        logger.Info("Extract Projects");
                        iCommand command = new ExtractProjectListCommand();
                        command.Execute(options);
                    }

                    if (options.extractSimpleProjects == true)
                    {
                        logger.Info("Extract Simple Projects");
                        iCommand command = new ExtractSimpleProjectListCommand();
                        command.Execute(options);
                    }


                    if (options.extractUsers == true)
                    {
                        logger.Info("Extract users");
                        logger.Warn("that's a quite long process");
                        if (options.extractUOFromAD)
                        {
                            logger.Warn("especially with the AD query");
                        }

                        iCommand command = new ExtractUsersListCommand();
                        command.Execute(options);
                    }
                }

                if (processed == false)
                {
                    Console.Write(options.GetUsage());
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                SendMail.SendException(e);
                logger.Info("Mail sent");
                throw;
            }
            finally
            {
                sw.Stop();
                logger.Info("Extract processed in {0:hh\\:mm\\:ss}", sw.Elapsed);
            }
        }
    }
}
