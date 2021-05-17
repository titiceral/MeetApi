using MeetApi.MeetApiInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetApi.MeetApiInterface
{
    public interface IMeetApiPluginFactory
    {
        public IMeetApiProtocol GetApiProtocol();

        public IList<IMeetApiProtocolInput> GetApiProtocolInputs();

        public ILine GetApiLine();

        public IMeetApiConverter GetApiConverter();
        public IMeetApiConfigurationManager GetApiConfigurationMngr();
    }
}
