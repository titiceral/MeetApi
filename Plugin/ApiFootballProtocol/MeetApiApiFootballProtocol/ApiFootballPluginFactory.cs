using MeetApi.MeetApiInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetApiApiFootballProtocol
{
    public class ApiFootballPluginFactory : IMeetApiPluginFactory
    {
        public IMeetApiConfigurationManager GetApiConfigurationMngr()
        {
            return new ApiFootballConfigurationManager();
        }

        public IMeetApiConverter GetApiConverter()
        {
            return new ApiFootballConverter();
        }

        public ILine GetApiLine()
        {
            return null;
        }

        public IMeetApiProtocol GetApiProtocol()
        {
            return new ApiFootballProtocol();
        }

        public IList<IMeetApiProtocolInput> GetApiProtocolInputs()
        {
            return new List<IMeetApiProtocolInput>()
            {
                new ApiFootballInput()
            };

        }
       
    }
}
