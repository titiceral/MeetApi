
using DiagBox.Applications.PCCOMAPI.PCCOMAPI;
using DiagBox.DataAccess.Infrastructure;
using System;

namespace DiagBox.Applications.PCCOMAPI.FactoryPCComApi
{
    public class FactoryPccomApi
    {
   
        public static Object CreateSpooler(SessionManager DBConnexion)
        {
            Object spooler = new SpoolerPCComApi(DBConnexion);

            return spooler;
        }

    }
}
