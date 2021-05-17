using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagBox.Applications.PCCOMAPI.PCCOMAPI
{
    interface IApiConnexionManager
    {
        bool CheckConnexion();

        string GetToken();

    }
}
