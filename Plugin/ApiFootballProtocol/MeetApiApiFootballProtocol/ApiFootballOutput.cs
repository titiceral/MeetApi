using MeetApi.MeetApiInterface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MeetApiApiFootballProtocol
{
    public class ApiFootballOutput : IMeetApiProtocolOutput
    {
        public string JSonMsgRaw { get;  set; }
        HttpStatusCode StatusCode { get; set; }
        public ApiFootballOutput(string jsonRaw, HttpStatusCode statusCode)
        {
            this.JSonMsgRaw = jsonRaw;
            StatusCode = statusCode;
        }

        public class ApiFootballConverted
        {
        }
      
        public IMeetApiProtocolOutputModel ToConverted()
        {
            
                return JsonSerializer.Deserialize<ApiFootballOutputModel>(JSonMsgRaw);

            
        }
        public String ToStringRaw()
        {
            return JSonMsgRaw;
        }

        public override string ToString()
        {
            return ToConverted().ToString();

        }

    }

   
}
