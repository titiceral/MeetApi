using DiagBox.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagBox.BusinessModels.Foundation.Charting;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using PCCOMAPI.JsonResponseModel;
using DiagBox.DataContracts.Repositories.CoreRepositories.Decisionnel;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Decisionnel;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Organisation;
using DiagBox.DataContracts.Repositories.CoreRepositories.Organisation;
using PCCOMAPI.ConnexionMgr;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
    class ApiProtocolEaseEGIot : IApiProtocol
    {
        ApiConnexionMQTTMngr _connexionMgnr;
        public ApiProtocolEaseEGIot(ApiConnexionMQTTMngr connexionMgnr)
        {
            _connexionMgnr = connexionMgnr;
        }

        public List<Site> getSites()
        {
            throw new NotImplementedException();

        }

        public List<Param> getVariables(Site site)
        {
            throw new NotImplementedException();
        }

        public void Initialize(Ligne ligne)
        {
            throw new NotImplementedException();
        }

        public IDictionary<Param, IList<HisValue>> readDataSite(DateTime startDate, DateTime enDate, Site site)
        {
            throw new NotImplementedException();
        }

        public IDictionary<Param, Value> readNewDataSite(Site site)
        {
            throw new NotImplementedException();
        }

        public void GetSitePosition( Site apiSite, string json)
        {
            json = json.Substring(0, json.IndexOf('\0')); // ETS remove strange character at end '\0\0+��\0\'

            var deserialized = JsonConvert.DeserializeObject<JsonResponseMqttEaseEGIotModel>(json);
            if (deserialized.loc != null && deserialized.loc.Length == 2)
            {

                apiSite.Latitude = deserialized.loc[0];
                apiSite.Longitude = deserialized.loc[1];
            }
        }

        public Dictionary<Param, IList<HisValue>> readNewDataSiteJson(Site apiSite, string json)
        {
            var paramValues = new Dictionary<Param, IList<HisValue>>();
            json =  json.Substring(0, json.IndexOf('\0')); // ETS remove strange character at end '\0\0+��\0\'
            
            // get data from string
            var deserialized = JsonConvert.DeserializeObject<JsonResponseMqttEaseEGIotModel>(json);


            // get site variables
            foreach (var param in apiSite.Params)
            {
                IParamAttributRepository paramAttributRepo = new ParamAttributReposiroty();
                IAttributRepository attributRepo = new AttributRepository();

                Attribut attribut = attributRepo.FindByName("Adresse Api");
                ParamAttribut paramAttribut = paramAttributRepo.FindWithParamAttribut(param.Id, attribut.Id);

                // get from "adresse api" attribut
                var hisValues = new List<HisValue>();
                var hisValue = deserialized.GetHisValueFromLabel(paramAttribut.Valeur, param.Id);
                if (hisValue != null)
                {
                    hisValues.Add(hisValue);
                    paramValues.Add(param, hisValues);
                }


            }// rof params


            return paramValues; 
            
                    
        }
    }
}
