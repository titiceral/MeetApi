using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagBox.DataAccess.Domain.Entities.Acquisition;

namespace PCCOMAPI.JsonResponseModel
{
    public class JSonReponseXilogModel : IJsonResponseModel
    {

        public JSonReponseXilogModel() {
            ReadingsData = new List<JSonReponseXilogHisValueModel>();
        }
       public IEnumerable<JSonReponseXilogHisValueModel> ReadingsData { get; set; }

        public IList<HisValue> toHisValues(int paramId, string strField)
        {
            return ReadingsData.Select(x => new HisValue() { paramId = paramId, Date = x.Timestamp, Value = x.Value}).ToList();
        }
    }
    public class JSonReponseXilogHisValueModel

    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }
}
