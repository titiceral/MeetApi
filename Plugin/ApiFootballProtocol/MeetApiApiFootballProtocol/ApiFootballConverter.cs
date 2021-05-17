using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ClosedXML.Excel;
using System.Data;
using MeetApi.MeetApiInterface;
using Newtonsoft.Json;

namespace MeetApiApiFootballProtocol
{
    // realise une sortie console
    internal class ApiFootballConverter : IMeetApiConverter
    {
        public void ConvertTo(KeyValuePair<IMeetApiProtocolInput, IList<IMeetApiProtocolOutput>> answerResponse)
        {
            Console.Out.WriteLine("input " + answerResponse.Key.ToString() );
            answerResponse.Value.ToList().ForEach(x => Console.Out.WriteLine("output " + x.ToString()));

            // convertit le model Json en un model plat afin de l'exporter en xlsx
            var flattenModels = ApiFootballOutputModelFlatten.ConvertToFlattenModels(answerResponse.Value);

            // export xslsx
            using XLWorkbook wb = new XLWorkbook();
            var serialized = JsonConvert.SerializeObject(flattenModels);
            DataTable xlsxSheet = JsonConvert.DeserializeObject<DataTable>(serialized);

             wb.Worksheets.Add(xlsxSheet, "Api Football Leagues");

   
            wb.SaveAs("d:/test/toto.xlsx");

        }
    }
}