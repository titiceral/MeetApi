using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagBox.DataAccess.Domain.Entities.Acquisition;

namespace PCCOMAPI.JsonResponseModel
{
    public class JsonResponseMqttEaseEGIotModel : IJsonResponseModel
    {
        public string s { get; set; }
        public DateTime ts { get; set; }
        public string m { get; set; }
        public JsonResponseMqtt_V v { get; set; }

        public string[] loc { get; set; }
     
        public string[] t { get; set; }


        public IList<HisValue> toHisValues(int idParam, string strField)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  Get json field from code "field1">"field2">"field3"...
        /// </summary>
        /// <param name="strKeyIn"></param>
        /// <param name="idParam"></param>
        /// <returns></returns>
        public HisValue GetHisValueFromLabel(string strKeyIn, int idParam)
        {
            try
            {
                var strKeys = strKeyIn.Split('>');
                object valueOut = v;

                foreach (var strKey in strKeys)
                {
                    valueOut = valueOut.GetType().GetProperty(strKey).GetValue(valueOut, null);
                }

                if (valueOut != null && IsNumericType(valueOut))
                {
                    return new HisValue()
                    {
                        paramId = idParam,
                        Date = ts,
                        Value = Convert.ToDouble(valueOut)
                    };
                }
            }
            catch
            {
                //ETS 190514 TODO
            }
            return null;


        }
        public bool IsNumericType( object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }

  
    public class JsonResponseMqtt_V
    {
        public JsonResponseMqtt_Temperature temperature { get; set; }
        public JsonResponseMqtt_Battery battery { get; set; }
        public JsonResponseMqtt_GPS GPS { get; set; }
        public JsonResponseMqtt_Network network { get; set; }
   
        public int opcode { get; set; }
        public JsonResponseMqtt_Input input1 { get; set; }
        public JsonResponseMqtt_Input input2 { get; set; }
         }

    public class JsonResponseMqtt_Temperature
    {
        public double value { get; set; }
        public string unit { get; set; }
    }

    public class JsonResponseMqtt_Battery
    {
        public double value { get; set; }
        public string unit { get; set; }
        public double remaining { get; set; }

    }

    public class JsonResponseMqtt_Input
    {
        // TODO verify the types are OK
        public object state { get; set; }
        public double counter { get; set; }
    }
    public class JsonResponseMqtt_Network
    {
        public int rsrp { get; set; }
        public int rsrq { get; set; }
    }

    public class JsonResponseMqtt_GPS
    {
        public int validPosition { get; set; }
        public JsonResponseMqtt_Speed speed { get; set; }
        public int onMove { get; set; }
    }

    public class JsonResponseMqtt_Speed
    {
        public double value { get; set; }
        public string unit { get; set; }
    }
}
