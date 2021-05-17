using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeetApi.MeetApiInterface
{
    public abstract class IMeetApiConfigurationManager
    {
        public IMeetApiConfigurationModel _configurationModel;

        abstract public void Initialize(StreamReader fileStream);
    }
}
