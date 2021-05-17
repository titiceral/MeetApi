using DiagBox.DataAccess.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DiagBox.BusinessModels.Foundation.Charting;
using System.Net;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Acquisition;
using DiagBox.Infrastructure.Core.Application;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Decisionnel;
using DiagBox.DataContracts.Interfaces.CoreInterfaces.Organisation;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using Newtonsoft.Json;
using DiagBox.DataContracts.Repositories.CoreRepositories.Organisation;
using DiagBox.DataContracts.Repositories.CoreRepositories.Decisionnel;
using DiagBox.DataContracts.Repositories.CoreRepositories.Acquisition;
using PCCOMAPI.JsonResponseModel;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
   public class ApiProtocolXilog : IApiProtocol
    {

        /// <summary>
        /// The root URL of the WebServiceToolkit
        /// </summary>
        public string Url;
        IDictionary<Param, IList<HisValue>> mapParamListValue;
        /// <summary>
        /// The default constructor
        /// </summary>
        public ApiProtocolXilog(string url)
        {
            if(url != "")
            {
                this.Url = url;
            }
        }
     
         IDictionary<Param, IList<HisValue>> IApiProtocol.readDataSite(DateTime startDate, DateTime enDate, Site sites)
        {
            var token = sites.Ligne.apiToken;
            var url = Url + "getdatatounits?siteID={0}&channelID={1}&token={2}&startDate={3}&endDate={4}&units=3";
            //var channel = "D1a";

            // renvoi une liste de tous les params du site contenant un attribut (ref_attribut) au nom : "Adresse Api"
            var paramList = getVariables(sites);
            mapParamListValue = new Dictionary<Param, IList<HisValue>>(); // objet his_value contenant des données horodatés dateTime et une valeur double

            foreach (var par in paramList)
            {
                var Result = "";
               // On récupère la valeur des attributs de ParamAttribut correspondant à à l'id_param et à l'id_attribut
               IParamAttributRepository paramAttributRepo = new ParamAttributReposiroty();
                IAttributRepository attributRepo = new AttributRepository();

                Attribut attribut = attributRepo.FindByName("Adresse Api");
                ParamAttribut paramAttribut = paramAttributRepo.FindWithParamAttribut(par.Id, attribut.Id);

                if(attribut!=null)
                {
                    WebRequest request = WebRequest.Create(string.Format(url, sites.Mnemonique, paramAttribut.Valeur, token, startDate.ToString("MM/dd/yyyy"), enDate.ToString("MM/dd/yyyy"))); //TODO ajouter get getData pour tous les param du site contenant un attribut "adresse api"

                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = "GET";
                    request.Timeout = 60000;// System.Threading.Timeout.Infinite;

                    try
                    {
                        WebResponse response = request.GetResponse();

                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();

                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);

                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();

                        // Clean up the streams and the response.

                        Result = responseFromServer;//.Replace(",", "\r\n");
                      reader.Close();
                        response.Close();
                        JSonReponseXilogModel valueList = JsonConvert.DeserializeObject<JSonReponseXilogModel>(Result);
                        if (mapParamListValue.ContainsKey(par) == false)
                        {
                            mapParamListValue.Add(par, valueList.toHisValues(par.Id, paramAttribut.Valeur));
                        }
                    }
                    catch (Exception ex)
                    {
                        Result = ex.Message;
                    }

                }
            }
            return mapParamListValue;
        }

        public IDictionary<Param, Value> readNewDataSite(Site site)
        {
            throw new NotImplementedException();
        }

        public void Initialize(Ligne ligne)
        {
            throw new NotImplementedException();
        }

        public List<Site> getSites()
        {
            throw new NotImplementedException();
        }

        public List<Param> getVariables(Site site)
        {
            IParamRepository paramRepo = new ParamRepository();
            List<Param> paramListe = (List<Param>)paramRepo.ForSite(site.Id, "Adresse API", null);

            return paramListe; 
        }

        public Dictionary<Param, IList<HisValue>> readNewDataSiteJson(Site site, string Json)
        {
            throw new NotImplementedException();
        }

       

        IDictionary<Param, Value> IApiProtocol.readNewDataSite(Site site)
        {
            throw new NotImplementedException();
        }

        Dictionary<Param, IList<HisValue>> IApiProtocol.readNewDataSiteJson(Site site, string Json)
        {
            throw new NotImplementedException();
        }

        void IApiProtocol.Initialize(Ligne ligne)
        {
            throw new NotImplementedException();
        }

        List<Site> IApiProtocol.getSites()
        {
            throw new NotImplementedException();
        }

        List<Param> IApiProtocol.getVariables(Site site)
        {
            throw new NotImplementedException();
        }

        void IApiProtocol.GetSitePosition(Site apiSite, string json)
        {
            throw new NotImplementedException();
        }
    }
}
