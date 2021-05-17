using MeetApi.MeetApiInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeetApiApiFootballProtocol
{
    public class ApiFootballOutputModelFlatten
    {
        // convertit un model hirarchisé JSon en un model plat 
       static public IList<LeaguesApiFootballModel> ConvertToFlattenModels(IList<IMeetApiProtocolOutput> outputs)
        {
            List<LeaguesApiFootballModel> flattenModels = new List<LeaguesApiFootballModel>();

            List<ApiFootballOutputModel> apiFootModels = outputs.Select(x => (ApiFootballOutputModel)x.ToConverted()).ToList();
            apiFootModels.ForEach(x => flattenModels.AddRange(x.api.leagues));
            return flattenModels;
        }

    }
}
