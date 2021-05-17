﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DiagBox.Applications.PCCOMAPI.SchedulerPCComApi
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                SchedulerPCComApiService service = new SchedulerPCComApiService();
                service.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                 new SchedulerPCComApiService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
