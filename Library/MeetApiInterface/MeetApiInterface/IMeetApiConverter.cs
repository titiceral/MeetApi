using System;
using System.Collections.Generic;
using System.Text;

namespace MeetApi.MeetApiInterface
{
    public interface IMeetApiConverter
    {
        void ConvertTo(KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>> data);
    }
}
