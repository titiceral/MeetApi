
using System;
using System.Collections.Generic;


namespace MeetApi.MeetApiInterface
{
    // T  : input data
    // Y ; output data
    public interface IMeetApiProtocol
    {
        void Initialize(ILine ligne);
        KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>> ReadData(IMeetApiConfigurationManager _apiConfigurationMngr, DateTime startDate, DateTime enDate,  IMeetApiProtocolInput myObject);
        KeyValuePair<IMeetApiProtocolInput, IMeetApiProtocolOutput> ReadNewDataSite(IMeetApiProtocolInput myOIbject);

    }
}
