using DiagBox.Applications.PCCOMAPI.PCCOMAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagBox.BusinessModels.Foundation.Charting;
using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using PCCOMAPI.ConnexionMgr;
using DiagBox.Applications.WebApps.PCCOMAPI.PCCOMApiInterface;

namespace PCCOMAPI.ApiProtocol
{
    public class ApiProtocolMqtt : IApiProtocol
    {
        private ApiConnexionMQTTMngr ConnexionManager;
        private ISpoolerPCComApi Spooler;

        /// <summary>
        /// The root URL of the WebServiceToolkit
        /// </summary>
        public string Url;
        IDictionary<Param, IList<HisValue>> mapParamListValue;
        /// <summary>
        /// The default constructor
        /// </summary>
    


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
           // TODO pratiquement la seule fonction à remplir
        }

        public IDictionary<Param, IList<HisValue>> readDataSite(DateTime startDate, DateTime enDate, Site site)
        {
            throw new NotImplementedException();
        }

        public IDictionary<Param, Value> readNewDataSite(Site site)
        {
            throw new NotImplementedException();
        }

        public Dictionary<Param, IList<HisValue>> readNewDataSiteJson(Site site, string Json)
        {
            throw new NotImplementedException();
        }

        IDictionary<Param, IList<HisValue>> IApiProtocol.readDataSite(DateTime startDate, DateTime enDate, Site site)
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
