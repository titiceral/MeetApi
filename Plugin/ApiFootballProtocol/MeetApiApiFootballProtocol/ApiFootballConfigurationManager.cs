using MeetApi.MeetApiInterface;
using MeetApi.MeetApiEntities;
using System.IO;

namespace MeetApiApiFootballProtocol
{
    public class ApiFootballConfigurationManager : IMeetApiConfigurationManager
    {
        // read configuration from file
        public override void Initialize(StreamReader fileStream)
        {

            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(MeetApiConfigurationModel));
            _configurationModel = (MeetApiConfigurationModel)reader.Deserialize(fileStream);
        }
    }
}
