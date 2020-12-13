using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ForStock.Server.Common
{
    public static class CorpCodeHelper{

        private static string CorpCode { get; set; }

        public static string GetCorpCode(string stockCode = null, string corpName = null)
        {
            XElement root = XElement.Load("D:\\MyProjects\\ForStock\\Client\\Shared\\CORPCODE.xml");

            IEnumerable<XElement> result =
                from el in root.Elements("list")
                where (string) el.Element("stock_code") == stockCode || (string) el.Element("corp_name") == corpName
                select el;

            if(result.Count() > 0)
            {
                return (string) result.First().Element("corp_code");
            }
            return null;
        }
    }
}