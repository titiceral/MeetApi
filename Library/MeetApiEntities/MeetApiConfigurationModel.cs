using MeetApi.MeetApiInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace MeetApi.MeetApiEntities
{
    [XmlRoot(ElementName = "MeetApi")]
    public class MeetApiConfigurationModel: IMeetApiConfigurationModel
    {
        [XmlElement(ElementName = "api_connexion")]
        public MeetApiConnexionModel ConnexionModel { get; set; }


    }

    public class MeetApiConnexionModel
    {
        [XmlAttribute(AttributeName ="login")]
        public string Login { get; set; }

        [XmlAttribute(AttributeName = "password")]
        public string Password { get; set; }

        [XmlAttribute(AttributeName = "token")]
        public string Token { get; set; }


    }
}
