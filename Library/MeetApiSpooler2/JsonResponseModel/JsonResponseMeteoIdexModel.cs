using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagBox.DataAccess.Domain.Entities.Acquisition;
using Newtonsoft.Json;

namespace PCCOMAPI.JsonResponseModel
{
   public  class JsonResponseMeteoIdexModel: IJsonResponseModel
    {
        [JsonProperty(PropertyName = "ListeDju.OUTPUT")]
        public IEnumerable<JsonResponseMeteoIdex_Template> outData { get; set; }


     
        public IList<HisValue> toHisValues(int paramId, string strField)
        {
            IList<HisValue> result = new List<HisValue>();

            object valueOut = outData;
            foreach (var outDataItem in outData)
            foreach (var item in outDataItem.dju)
            {
                DateTime dateDJU =  DateTime.Parse(item.dateDju);
                double valReel = Convert.ToDouble(item.nbrDjuCOSTIC);

             // ETS 190825 : todo en fonction d'un nom de champ   valueOut = valueOut.GetType().GetProperty(strField).GetValue(valueOut, null);
                result.Add( new HisValue() { paramId = paramId, Date = dateDJU, Value = valReel/*(double)valueOut*/ });
            }

            return result;
        }
    }

    public class JSonReponseMeteoIdexHisValueModel
    {
        public DateTime Timestamp { get; set; }
        public float Value { get; set; }
    }


    public class JsonResponseMeteoIdex_Template
    {
        public string message { get; set; }
        public string codeRetour { get; set; }
        public List<Dju> dju { get; set; }
    }


    public class Dju
    {
        public string codeStationMeteo { get; set; }
        public string tempBaseDju { get; set; }
        public string MethodeCalcul { get; set; }
        public string costic { get; set; }
        public string verrou { get; set; }
        public string verrouMF { get; set; }
        public string libelleStationMeteo { get; set; }
        public string dateDju { get; set; }
        public string nbrDjuMF { get; set; }
        public virtual string nbrDjuCOSTIC { get; set; }
    }
}
