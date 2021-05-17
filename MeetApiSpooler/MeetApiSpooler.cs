

using MeetApi.MeetApiEntities;
using MeetApi.MeetApiInterface;
using MeetApi.MeetApiSpooler;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MeetApi.MeetAPiSpooler
{
    public class MeetApiSpooler : IMeetApiSpooler
    {
        static int G_DAYS_BACKUP = -7;


        private dynamic SpoolerLog { get; set; }
        private dynamic Spooler { get; set; }
        private dynamic Task { get; set; }

        private IMeetApiScheduler pObserverScheduler;

        IMeetApiProtocol _apiProtocol;
        IMeetApiConverter _apiConverter;
        ILine _apiLine;
        IMeetApiConfigurationManager _apiConfigurationMngr;

        IDictionary<IOperationCyclique, IList<IMeetApiProtocolInput>> mapOperationCycliqueInputs;

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

       



        public IDictionary<IOperationCyclique, IList<IMeetApiProtocolInput>> Initialize()
        {

            
            // get api protocol from plugin
            IMeetApiPluginFactory pluginFactory = MeetApiPluginLoader.LoadPlugins("D:\\dev\\MeetApi\\Plugin\\ApiFootballProtocol\\MeetApiApiFootballProtocol\\bin\\Debug\\netcoreapp3.0");
            _apiProtocol = pluginFactory.GetApiProtocol();
            _apiLine = pluginFactory.GetApiLine();
            _apiConverter = pluginFactory.GetApiConverter();
            
            _apiConfigurationMngr = pluginFactory.GetApiConfigurationMngr();
            System.IO.StreamReader file = new System.IO.StreamReader("D:\\dev\\MeetApi\\Application\\config\\config.xml");
            _apiConfigurationMngr.Initialize(file);
            file.Close();



            // 2- obtention de l'opération cyclique par site  -- MAJ otention des sites par opération cyclique
            IList<OperationCyclique> listOC = getAllOperationCyclique();
            mapOperationCycliqueInputs = new Dictionary<IOperationCyclique, IList<IMeetApiProtocolInput>>();
            foreach (var cyclicOperation in listOC)
            {
                mapOperationCycliqueInputs[cyclicOperation] = pluginFactory.GetApiProtocolInputs();
       
            }

            // retourne le dictionnaire OC : list<site>
            return mapOperationCycliqueInputs;
        }

        // construit les opérations cyclique en fonction du fichier de configuration
        private IList<OperationCyclique> getAllOperationCyclique()
        {
            var oc = new List<OperationCyclique>()
            {
                new OperationCyclique()
            };
// TODO
            return oc;
        }

        public bool ConvertTo(KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>> data)
        {

            // get des doublons
          //  var valideParamsValues = CheckDoublon(data);
            _apiConverter.ConvertTo(data);
        

            return true;
        }

        private object CheckDoublon(IDictionary<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>> data)
        {
            throw new NotImplementedException();
        }
        /*
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
        #endregion*/


      /*  public IDictionary<Param, IList<HisValue>> checkDoublon(IDictionary<Param, IList<HisValue>> paramsValues)
        {
           
            IList<HisValue> ListValeurValide = new List<HisValue>();
            IDictionary<Param, IList<HisValue>> paramsValuesTmp = new Dictionary<Param, IList<HisValue>>();
            foreach (var ListValeurs in paramsValues)
            {
          
                paramsValuesTmp.Add(ListValeurs.Key, ListValeurs.Value.Distinct().ToList());
              
            }

            return paramsValuesTmp;
        }
        */

        public void Register(IMeetApiScheduler scheduler)
        {
            pObserverScheduler = scheduler;
        }

        public void Unregister()
        {
            pObserverScheduler = null;
        }



        public void onReadSiteDone(int command)
        {
            throw new NotImplementedException();
        }

      

        public string GetData(IList<IMeetApiProtocolInput> myObjects)
        {
            string ReadingFeedback = null;
            foreach (var protocolInput in myObjects)
            {
                
                if (_apiProtocol != null)
                {
                    Thread th = Thread.CurrentThread;
                    if (String.IsNullOrEmpty(th.Name))
                    {
                        th.Name = "MeetApi_getData_ " + protocolInput.Id;
                    }

                

                    // get data from last acquisition to now, if not date acquisition get from 7 days. get from 7 days max
                    lock (_apiProtocol)
                    {
                        var paramsValues = _apiProtocol.ReadData(_apiConfigurationMngr, DateTime.Now, DateTime.Now, protocolInput);


                        // insertion en base dans his_valeur en HQL et 1000lignes max par requêtes
                        bool ReadingOk = ConvertTo(paramsValues);
                        if (!ReadingOk)
                        {
                            ReadingFeedback = "Failed";
                        }
                    }
                 

                }
            }
            return ReadingFeedback;
        }

        public void IMeetApiSpooler()
        {
            throw new NotImplementedException();
        }

        public void onReadObjectDone(int command)
        {
            throw new NotImplementedException();
        }

        public void MsgPublishRecieved(string myObjectCodeMnemonique, string jsonMessage)
        {
            throw new NotImplementedException();
        }
    }
}
