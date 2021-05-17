using System;
using System.Collections.Generic;
using System.Text;

namespace MeetApi.MeetApiInterface
{
    public interface IMeetApiProtocolOutput
    {
         String ToString();
         String ToStringRaw();
         IMeetApiProtocolOutputModel ToConverted();
    }
}
