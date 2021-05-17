
using DiagBox.DataAccess.Domain.Entities;
using DiagBox.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiagBox.Applications.WebApps.PCCOMAPI.PCCOMApiInterface
{
    public interface ISpoolerPCComApi
    {
        void SpoolerPCComApi(SessionManager dBConnexion);

        void register(IScheduler scheduler);

        void unregister();

        String GetSiteData(IList<Site> site);

        void onReadSiteDone(int command);

        IDictionary<OperationCyclique, IList<Site>> initialize(SessionManager sessionMngr);
        void MsgPublishRecieved(string siteCodeMnemonique, string jsonMessage);
    }
}
