using DiagBox.BusinessModels.Foundation.Charting;
using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
    interface IApiProtocol
    {
        IDictionary<Param, IList<HisValue>> readDataSite(DateTime startDate, DateTime enDate, Site site);
        IDictionary<Param,Value> readNewDataSite(Site site);

        Dictionary<Param, IList<HisValue>> readNewDataSiteJson(Site site, string Json);

        void Initialize(Ligne ligne);
        List<Site> getSites();
        List<Param> getVariables(Site site);

        void GetSitePosition(Site apiSite, string json);

    }
}
