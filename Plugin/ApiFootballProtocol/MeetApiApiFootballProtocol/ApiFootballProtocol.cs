using System;
using System.Collections.Generic;
using MeetApi.MeetApiEntities;
using MeetApi.MeetApiInterface;
using RestSharp;

// implement https://www.api-football.com/documentation#leagues
// documentation https://rapidapi.com/api-sports/api/api-football?endpoint=apiendpoint_05d94518-57f3-45f6-b632-3a78baa9b304
namespace MeetApiApiFootballProtocol
{
    internal class ApiFootballProtocol : IMeetApiProtocol
    {
        public void Initialize(ILine ligne)
        {
            throw new NotImplementedException();
        }

        public KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput> > ReadData(IMeetApiConfigurationManager apiConfigurationMngr, 
            DateTime startDate, DateTime enDate, IMeetApiProtocolInput myObject)
        {
            IList<IMeetApiProtocolOutput> outputs = new List<IMeetApiProtocolOutput>();

            
            //  var client = new RestClient("https://api-football-v1.p.rapidapi.com/v2/leagues/country/england/2018");
            var client = new RestClient("https://www.api-football.com/demo/api/v2/leagues");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "api-football.com/demo/api");//"api-football-v1.p.rapidapi.com");
            //TODO gérer la configuration null
            if (apiConfigurationMngr != null)
            {
                request.AddHeader("x-rapidapi-key", ((MeetApiConfigurationModel)apiConfigurationMngr._configurationModel).ConnexionModel.Token);//fa88ba9bc3msh052af174a8bce11p1648fdjsn592c22980464
            }
            IRestResponse response = client.Execute(request);
            outputs.Add(new ApiFootballOutput(response.Content, response.StatusCode));
            
            return new KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>>(myObject, outputs);
        }

        public KeyValuePair<IMeetApiProtocolInput, IMeetApiProtocolOutput> ReadNewDataSite(IMeetApiProtocolInput myObject)
        {
            var client = new RestClient("https://api-football-v1.p.rapidapi.com/v2/leagues/country/england/2018");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "api-football-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fa88ba9bc3msh052af174a8bce11p1648fdjsn592c22980464");
            IRestResponse response = client.Execute(request);
          
            return new KeyValuePair<IMeetApiProtocolInput, IMeetApiProtocolOutput >(myObject, new ApiFootballOutput(response.Content, response.StatusCode));
        }
    }
}