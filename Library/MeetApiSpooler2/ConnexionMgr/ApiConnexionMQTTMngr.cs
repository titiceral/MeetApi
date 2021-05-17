using DiagBox.Applications.PCCOMAPI.PCCOMAPI;
using DiagBox.Applications.WebApps.PCCOMAPI.PCCOMApiInterface;
using DiagBox.DataAccess.Domain.Entities;
using Newtonsoft.Json;
using OpenSSL.X509Certificate2Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace PCCOMAPI.ConnexionMgr
{
    public class ApiConnexionMQTTMngr : IApiConnexionManager
    {
        #region Properties
        public string Token { get; set; }
        public bool HasRecievedMessage { get; private set; } = false;
        #endregion

        #region Fields
        private MqttClient _client;
        ISpoolerPCComApi _pSpooler;
        #endregion

        #region Constructors
        public ApiConnexionMQTTMngr(string Url, int Port, string Username, string Password, ISpoolerPCComApi pSpooler)
        {
            // Create certificates
            _pSpooler = pSpooler;

            // Establish connexion
            _client = new MqttClient(Url, Port, false, null, null, MqttSslProtocols.None, null);
            var connectcode = _client.Connect(Guid.NewGuid().ToString(), Username, Password);
            _client.MqttMsgSubscribed += this.MqttMsgSubscribed;
            _client.MqttMsgPublishReceived += this.MqttMsgPublishRecieved;
            _client.MqttMsgPublished += this.MqttMsgPublished;
            Token = connectcode.ToString();

        }

        private void MqttMsgPublishRecieved(object sender, MqttMsgPublishEventArgs e)
        {
            _pSpooler.MsgPublishRecieved(e.Topic, Encoding.UTF8.GetString(e.Message, 0, e.Message.Length));
        }

        private void MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            int i = 0;
            Console.WriteLine("MqttMsgPublished for id = " + e.MessageId);

            //  _pSpooler.MsgPublishRecieved(e.MessageId);
        }
        #endregion

        #region Interface methods
        public bool CheckConnexion()
        {
            return _client.IsConnected;
        }

        public string GetToken()
        {
            return Token;
        }
        #endregion

        #region Public methods
        public ushort Subscribe(IList<Site> Topics)
        {
            byte[] QoSLevels = Enumerable.Repeat(MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, Topics.Count).ToArray();
            return _client.Subscribe(Topics.Select(x => x.Mnemonique).ToArray(), QoSLevels);
        }
        #endregion

        #region Internal methods
        private void MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)        {
            
            Console.WriteLine("Subscribed for id = " + e.MessageId);
        }

       
        
        #endregion


    }
}
