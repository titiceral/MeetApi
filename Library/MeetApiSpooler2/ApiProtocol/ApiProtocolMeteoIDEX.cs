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
using System.Net.Http;
using System.Text;
using PCCOMAPI.JsonResponseModel;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
    public class ApiProtocolMeteoIDEX : IApiProtocol
    {

        /// <summary>
        /// The root URL of the WebServiceToolkit
        /// </summary>
        public string Url;
        IDictionary<Param, IList<HisValue>> mapParamListValue;
        /// <summary>
        /// The default constructor
        /// </summary>
        /// 


        public ApiProtocolMeteoIDEX(string url)
        {
            if (url != "")
            {
                this.Url = url;
            }
        }


        public IDictionary<Param, IList<HisValue>> readDataSite(DateTime startDate, DateTime enDate, Site sites)
        {

            var url = Url + "&dateDebutDju={0}&dateFinDju={1}&codeStationMeteo={2}";
        

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

               // if (attribut != null)
                {
                    WebRequest request = WebRequest.Create(string.Format(url, startDate.ToString("yyyyMMdd"), enDate.ToString("yyyyMMdd"), sites.CodeExterne)); //TODO ajouter get getData pour tous les param du site contenant un attribut "adresse api"

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

                        Result = responseFromServer;//.Replace(",", "\r\n");
                        reader.Close();
                        response.Close();
                        JsonResponseMeteoIdexModel valueList = JsonConvert.DeserializeObject<JsonResponseMeteoIdexModel>(Result);
                        mapParamListValue.Add(par, valueList.toHisValues(par.Id, paramAttribut != null ? paramAttribut.Valeur : ""));
                    }
                    catch (Exception ex)
                    {
                        Result = ex.Message;
                    }

                }
            }
            return mapParamListValue;
        }





        //public async Task<IList<OutputDataMeteo>> getDataFromDju(DateTime startDate, DateTime enDate, Site sites)
        //{
        //    var url = Url;

        //    // renvoi une liste de tous les params du site contenant un attribut (ref_attribut) au nom : "Adresse Api"
        //    var paramList = getVariables(sites);
        //    //mapParamListValue = new Dictionary<Param, IList<HisValue>>(); // objet his_value contenant des données horodatés dateTime et une valeur double
        //    mapParamListValue = new Task<List<OutputDataMeteo>>(); // objet his_value contenant des données horodatés dateTime et une valeur double

        //    foreach (var par in paramList)
        //    {
        //        var Result = "";
        //        // On récupère la valeur des attributs de ParamAttribut correspondant à à l'id_param et à l'id_attribut
        //        IParamAttributRepository paramAttributRepo = new ParamAttributReposiroty();
        //        IAttributRepository attributRepo = new AttributRepository();
        //        var Item = "";
        //        Attribut attribut = attributRepo.FindByName("Adresse Api");
        //        ParamAttribut paramAttribut = paramAttributRepo.FindWithParamAttribut(par.Id, attribut.Id);

        //        if (attribut != null)
        //        {
        //            //WebRequest request = WebRequest.Create(string.Format(url, sites.Mnemonique, paramAttribut.Valeur, startDate.ToString("MM/dd/yyyy"), enDate.ToString("MM/dd/yyyy"))); //TODO ajouter get getData pour tous les param du site contenant un attribut "adresse api"
        //            //request.Credentials = CredentialCache.DefaultCredentials;
        //            //request.Method = "GET";
        //            //request.Timeout = System.Threading.Timeout.Infinite;
        //            try
        //            {
        //                using (var client = new HttpClient())
        //                {

        //                    using (var response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json"))
        //                    {
        //                        if (response.IsSuccessStatusCode)
        //                        {
        //                            return JsonConvert.DeserializeObject<OutputDataMeteo>(await response.Content.ReadAsStringAsync());
        //                        }
        //                    }

        //                }
        //                return default(TDataOut);
        //            }
        //            catch (Exception ex)
        //            {
        //                Result = ex.Message;
        //            }

        //        }
        //    }
        //    return mapParamListValue;
        //}


        //public async Task<string> DispatchGetMethod(string OAuthToken, string UrlEndpoint, string Id, string Property)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        //client.DefaultRequestHeaders.Add("Accept", "application/json";
        //        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthToken);

        //        using (var response = await client.GetAsync(Url, new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json"))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return JsonHelper.ConvertJsonStringToList<string>((await response.Content.ReadAsStringAsync() as string))[0];
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public async Task<TDataOut> DispatchPostMethod<TDataIn, TDataOut>(string OAuthToken, string UrlEndpoint, TDataIn Item)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Accept", "application/json";
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OAuthToken);

        //        using (var response = await client.PostAsync(ConfigManager.Config.Url + $"{UrlEndpoint}/Create",
        //                                                  new StringContent(JsonConvert.SerializeObject(Item), Encoding.UTF8, "application/json"))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return JsonConvert.DeserializeObject<TDataOut>(await response.Content.ReadAsStringAsync());
        //            }
        //        }

        //    }
        //    return default(TDataOut);
        //}



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

        public void GetSitePosition(Site apiSite, string json)
        {
            throw new NotImplementedException();
        }
    }
}
