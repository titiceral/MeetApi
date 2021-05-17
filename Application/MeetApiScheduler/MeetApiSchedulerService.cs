using DiagBox.Applications.PCCOMAPI.FactoryPCComApi;
using DiagBox.Applications.WebApps.PCCOMAPI.PCCOMApiInterface;
using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Infrastructure;
using DiagBox.DataAccess.Infrastructure.Plumbing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DiagBox.Applications.PCCOMAPI.SchedulerPCComApi
{
    public class SchedulerPCComApiService : IScheduler
    {
        private CyclicOperationTimer timers;
        SessionManager pSessionMgr; // datebase connexion Manager
        ISpoolerPCComApi pSpooler; // pccom manager

        //IDictionary<Site, IList<OperationCyclique>> mapSiteOperationCycliques;
        IDictionary<OperationCyclique, IList<Site>> mapSiteOperationCycliques;

        private SessionManager GetDBConnexion()
        {
            bool useCache = false;
            string context = ".\\Client.config";
            //Get Connexion informations
            try
            {// Initialisation du contexte de SessionFactory
              var session=  SessionFactoryProvider.Factory(
                    context,
                    useCache,
                    NHibernateConstants.ContextClasses.ThreadStaticContext);
                System.Diagnostics.Trace.WriteLine("[Bootstrap] SessionFactory successfully activated");
                return session;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format(
                    "[Bootstrap] SessionFactory failed initializing database : {0}",
                    ex.Message));
                throw;
            }
 
        }

        protected override void OnStart(string[] args)
        {
            // create dbconnexion from configuration file
            pSessionMgr = GetDBConnexion();

            // create spooler from factory
            pSpooler = (ISpoolerPCComApi)FactoryPccomApi.CreateSpooler(pSessionMgr);

            // register as observer
            pSpooler.register(this);

            // initialize spooler -> get operation cyclique
            mapSiteOperationCycliques = pSpooler.initialize(pSessionMgr);

            // démarrer le timer according to cyclic operations 
            foreach (var listeSite in mapSiteOperationCycliques)
            { //on lance toutes les opérations cycliques par site

               
                var heures = listeSite.Key.Heure;
                var minutes = listeSite.Key.Minute;
                var secondes = listeSite.Key.Seconde;

                int periode = 24; // A lancer toutes les 24 heures
                int heure = 0;
                int minute = 0;
                int seconde = 0;
                var date = DateTime.Now;

                if (heures == "*")
                {
                    periode = 1; // A lancer chaque heure
                    heure =  date.Hour;
                }
                else
                {
                    Int32.TryParse(heures, out heure);
                    Int32.TryParse(minutes, out minute);
                    Int32.TryParse(secondes, out seconde);
                }
                
                  timers = new CyclicOperationTimer(date.Hour, date.Minute, date.Second + 20, periode, listeSite.Value, this);
             //       timers = new CyclicOperationTimer(heure, minute, seconde, periode, listeSite.Value, this);
                
            }
        }
        protected override void OnStop()
        {
        }
        protected override void OnPause()
        {
        }

        public void timerSitesValue(IList<Site> sites)
        {
            var dataReceived = pSpooler.GetSiteData(sites);
        }

        internal void TestStartupAndStop(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
