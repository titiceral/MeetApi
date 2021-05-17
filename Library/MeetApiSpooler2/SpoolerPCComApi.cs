using DiagBox.Applications.WebApps.PCCOMAPI.PCCOMApiInterface;
using DiagBox.BusinessModels.Foundation.Charting;
using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using DiagBox.DataAccess.Infrastructure;
using DiagBox.DataAccess.Infrastructure.Plumbing;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Acquisition;
using DiagBox.DataContracts.Repositories.CoreRepositories.Acquisition;
using DiagBox.Infrastructure.Core.Application;
using NHibernate;
using NHibernate.Mapping;
using PCCOMAPI.ConnexionMgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
    public class SpoolerPCComApi : ISpoolerPCComApi
    {
        static int G_DAYS_BACKUP = -7;


        private dynamic SpoolerLog { get; set; }
        private dynamic Spooler { get; set; }
        private dynamic Task { get; set; }

        private IScheduler pObserverScheduler;

        Dictionary<Ligne, IApiProtocol> mapLineApiProtocol;
        IDictionary<OperationCyclique, IList<Site>> mapOperationCycliqueSites;
    
   
        private SessionManager dBConnexion;

        public SpoolerPCComApi(SessionManager dBConnexion)
        {
            this.dBConnexion = dBConnexion;
        }

        //public SpoolerPCComApi(ISession dBConnexion)
        //{
        //    this.dBConnexion = dBConnexion;
        //}
        public void spooler_set_context(dynamic context)
        {
            this.SpoolerLog = context.Log;
            this.Spooler = context.Spooler;
            this.Task = context.Task;
        }

        class comparerOperationCyclique : IEqualityComparer<OperationCyclique>
        {
            public bool Equals(OperationCyclique x, OperationCyclique y)
            {
                return x.Minute == y.Minute && x.Heure == y.Heure && x.Seconde == y.Seconde;
            }

            public int GetHashCode(OperationCyclique obj)
            {
                return (obj.Minute.ToString() + "|" + obj.Heure.ToString() + "|" + obj.Seconde.ToString()).GetHashCode();
            }
        }



        public IDictionary<OperationCyclique, IList<Site>> initialize(SessionManager sessionMngr)
        {
            IEnumerable<Ligne> listApiLigne = getAllApiLine();
            IProtocoleRepository protocoleRepo = new ProtocoleRepository();
            ISiteRepository siteRepo = new SiteRepository();

            mapLineApiProtocol = new Dictionary<Ligne, IApiProtocol>();
            //mapSiteOperationCyclique = new Dictionary<Site, IList<OperationCyclique>>();
            mapOperationCycliqueSites = new Dictionary<OperationCyclique, IList<Site>>( new comparerOperationCyclique()) ;

            foreach (var apiLine in listApiLigne)
            {
                var proto = protocoleRepo.FindById(apiLine.Protocole.Id);
                //1- obtention du apiprotocol par ligne

                //swich case en fonction du protocol -> créer le bon api protocol
                IApiProtocol pProtocol = null;
                switch (proto.Nom.ToLower())
                {
                    case "xilog":
                        pProtocol = new ApiProtocolXilog("http://decode.primayer.com/api/xilog/"); // 190415 EC en dur
                        break; ;

                    case "easeegiot":
                        var connexion = new ApiConnexionMQTTMngr(apiLine.apiUrl,apiLine.apiPort,
                            apiLine.apiUsername, apiLine.apiPassword, this);
                        pProtocol = new ApiProtocolEaseEGIot(connexion);
                        connexion.Subscribe(siteRepo.FindByIdLine(apiLine.Id));
                        break; ;

                    case "meteoidex":
                        pProtocol = new ApiProtocolMeteoIDEX("https://api.idex.fr/engine54/52/PortailJSON?flowName=Flux113_REFER_ListeDju&flowType=EAII&actionJSON=launch"); // 190509 EC en dur
                        break; ;
                        
                }
                if (pProtocol != null)
                {
                    mapLineApiProtocol.Add(apiLine, pProtocol);
                }
            
            }// rof each line
            // 2- obtention de l'opération cyclique par site  -- MAJ otention des sites par opération cyclique
               
            var listOC = getAllOperationCyclique();
            foreach (var cyclicOperation in listOC)
            {
                // vérifie que l'opération cyclique est sur un site api
                if( listApiLigne.Where(x => x.Id == cyclicOperation.Site.Ligne.Id).FirstOrDefault() != null)
                    {

                    if (mapOperationCycliqueSites.ContainsKey(cyclicOperation) == false)
                    {
                        mapOperationCycliqueSites.Add(cyclicOperation, new List<Site>());

                    }


                    mapOperationCycliqueSites[cyclicOperation].Add(cyclicOperation.Site);
                }
            }
           

            // retourne le dictionnaire OC : list<site>
            return mapOperationCycliqueSites;
        }

        public IList<Site> getAllSiteForOC(string heure, string minute, string seconde)
        {
            ISiteRepository siteRepo = new SiteRepository();
            IList<Site> site = siteRepo.FindSiteForOC(heure, minute, seconde);

            return site;
        }
        public IList<Ligne> getAllApiLine()
        {
            ILigneRepository ligneRepo = new LigneRepository();
            // Récupère toutes les lignes dont le parametre PCComApi est à true ( champ Bdd : est_api) 
            IList<Ligne> ligneList = ligneRepo.GetAllApiLine();
            return ligneList;
        }

        public string getApiToken(int idSite)
        {
            Protocole proto = getProtocolForSite(idSite);
            ILigneRepository ligneRepo = new LigneRepository();
            Ligne ligne = ligneRepo.GetLineByProtocol(proto.Id);

            return ligne.apiToken;
        }

        public List<OperationCyclique> getAllOperationCyclique()
        {
            IOperationCycliqueRepository opeRepo = new OperationCycliqueRepository();
            List<OperationCyclique> operation = opeRepo.GetAll().ToList();

            return operation;
        }

        public List<OperationCyclique> getOperationCycliqueForSite(short idSite)
        {
            IOperationCycliqueRepository opeRepo = new OperationCycliqueRepository();
            List<OperationCyclique> operation = opeRepo.FindByIdSite(idSite);

            return operation;
        }

        public Protocole getProtocolForSite(int idSite)
        {
            ISiteRepository siteRepo = new SiteRepository();
            Site site = siteRepo.FindById(idSite);

            return site.Protocole;
        }

        public IDictionary<Param, IList<HisValue>> applyScaleFactory(IDictionary<Param, IList<HisValue>> data)
        {
            IParamRepository paramRepo = new ParamRepository();
            var mapValueConverted = new Dictionary<Param, IList<HisValue>>();
            if (data != null)
            {
                foreach (var listValeurs in data)
                {
                    IList<HisValue> ListValeurValide = new List<HisValue>();

                    if (listValeurs.Key.TypeConversion != 0)
                    {
                        foreach (var val in listValeurs.Value)
                        {
                            HisValue convertedValue = paramRepo.ApplyScaleToValue(listValeurs.Key, val);

                            ListValeurValide.Add(convertedValue);
                        }
                    }
                    else
                    {
                        ListValeurValide = listValeurs.Value;
                    }

                     mapValueConverted.Add(listValeurs.Key, ListValeurValide);
                    
                    

                }
            }
            return mapValueConverted;
        }

        public bool InsertPCComApiValues(IDictionary<Param, IList<HisValue>> data, Site site)
        {

            // get des doublons
            var valideParamsValues = checkDoublon(data);

            // gestion des bornes
            var ValuesWithEdge = applyScaleFactory(valideParamsValues);

            bool insertFeedback = false;
            ISiteRepository siteRepo = new SiteRepository();
            IParamRepository paramRepo = new ParamRepository();
           
               IFastHisValeurRepository valeurRepo = new FastHisValeurRepository();

            foreach (var values in ValuesWithEdge.Values)
            {
            try
            {
                valeurRepo.InsertPCComApiValues(values);
            }
            catch (Exception ex)
            {
            }
            }

            try
            {
                insertFeedback = true;

                // insertion de la date d'acquisiton dans site
                var siteLocal = siteRepo.FindById(site.Id);
                siteLocal.DateAcquisition = DateTime.Now;
                siteRepo.Update(siteLocal);
            }
            
            catch (Exception ex)
            {
                insertFeedback = false;
            }
            return insertFeedback;
        }

        #region MQTT
        public void MsgPublishRecieved(string siteCodeMnemonique, string jsonMessage)
        {
            ISiteRepository siteRepo = new SiteRepository();
            Site siteApi = siteRepo.FindByCodeMnemonique(siteCodeMnemonique);

            IApiProtocol apiProtocol = null;
            mapLineApiProtocol.TryGetValue(siteApi.Ligne, out apiProtocol);
            if (apiProtocol != null)
            {
                var recievedParamValues = apiProtocol.readNewDataSiteJson(siteApi, jsonMessage);

            
               // ajout des données en base
                InsertPCComApiValues(recievedParamValues, siteApi);
              }
            this.UpdateSitePosition(siteCodeMnemonique, jsonMessage);


        }

        public void UpdateSitePosition(string siteCodeMnemonique, string jsonMessage)
        {
            ISiteRepository siteRepo = new SiteRepository();
            Site siteApi = siteRepo.FindByCodeMnemonique(siteCodeMnemonique);

            IApiProtocol apiProtocol = null;
            mapLineApiProtocol.TryGetValue(siteApi.Ligne, out apiProtocol);
            if (apiProtocol != null)
            {
                apiProtocol.GetSitePosition(siteApi, jsonMessage);

                try
                {
                    siteRepo.Update(siteApi);
                }
                catch
                {
                    Console.WriteLine("UpdateSitePosition problème lors de l'update du site");
                }
                
            }

       
        }
        #endregion


        public IDictionary<Param, IList<HisValue>> checkDoublon(IDictionary<Param, IList<HisValue>> paramsValues)
        {
           
            IList<HisValue> ListValeurValide = new List<HisValue>();
            IDictionary<Param, IList<HisValue>> paramsValuesTmp = new Dictionary<Param, IList<HisValue>>();
            foreach (var ListValeurs in paramsValues)
            {
          
                paramsValuesTmp.Add(ListValeurs.Key, ListValeurs.Value.Distinct().ToList());
              
            }

            return paramsValuesTmp;
        }


        public void register(IScheduler scheduler)
        {
            pObserverScheduler = scheduler;
        }

        public void unregister()
        {
            pObserverScheduler = null;
        }



        public void onReadSiteDone(int command)
        {
            throw new NotImplementedException();
        }

        void ISpoolerPCComApi.SpoolerPCComApi(SessionManager dBConnexion)
        {
            throw new NotImplementedException();
        }

        public string GetSiteData(IList<Site> listSites)
        {
            IParamRepository paramRepo = new ParamRepository();
          
            string ReadingFeedback = null;
            foreach (var site in listSites)
            {
                IApiProtocol proto = null;
                mapLineApiProtocol.TryGetValue(site.Ligne, out proto);//getProtocolForSite(site.Id);
                if (proto != null)
                {
                    Thread th = Thread.CurrentThread;
                    if (String.IsNullOrEmpty(th.Name))
                    {
                        th.Name = "PCCOMApi_getSiteData_ " + site.Id;
                    }

                    // Insertion de traces dans la table log_application
                    //DiagBoxApp.Instance.GetLogger("EventLogTable").Trace(
                    //          message: string.Format("Launching new thread to retrieve data from PCComApi in the thread {0}", th.Name),
                    //        categories: "DiagBox;Applications;PCCOMAPI;PCCOMAPI;SpoolerPCComApi");


                    // get data from last acquisition to now, if not date acquisition get from 7 days. get from 7 days max
                    lock (proto)
                    {
                        var paramsValues = proto.readDataSite((site.DateAcquisition.HasValue && site.DateAcquisition.Value > DateTime.Now.AddDays(G_DAYS_BACKUP)) ? site.DateAcquisition.Value : DateTime.Now.AddDays(G_DAYS_BACKUP), DateTime.Now, site);


                        // insertion en base dans his_valeur en HQL et 1000lignes max par requêtes
                        bool ReadingOk = InsertPCComApiValues(paramsValues, site);
                        if (!ReadingOk)
                        {
                            ReadingFeedback = "Failed";
                        }
                    }
                 

                }
            }
            return ReadingFeedback;
        }

    }
}
