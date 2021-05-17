using DiagBox.DataAccess.Domain.Entities.Acquisition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCOMAPI.JsonResponseModel
{
    public interface IJsonResponseModel
    {
        IList<HisValue> toHisValues(int paramId, string strField);

    }
}
