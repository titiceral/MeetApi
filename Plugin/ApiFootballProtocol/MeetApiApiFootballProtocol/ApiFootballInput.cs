using MeetApi.MeetApiInterface;

namespace MeetApiApiFootballProtocol
{
    public class ApiFootballInput : IMeetApiProtocolInput
    {
        public string Id { get; set; }

        override public string ToString()
        {
            return "toto";
        }
    }
}