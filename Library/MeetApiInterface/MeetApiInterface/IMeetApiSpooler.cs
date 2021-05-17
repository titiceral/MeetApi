

using System;
using System.Collections.Generic;



namespace MeetApi.MeetApiInterface{
    public interface IMeetApiSpooler
    {
        void IMeetApiSpooler( );
        IDictionary<IOperationCyclique, IList<IMeetApiProtocolInput>> Initialize();

        void Register(IMeetApiScheduler scheduler);

        void Unregister();

        String GetData(IList<IMeetApiProtocolInput> myObject);

        // for MQTT
        void onReadObjectDone(int command);

        void MsgPublishRecieved(string myObjectCodeMnemonique, string jsonMessage);
    }
}
