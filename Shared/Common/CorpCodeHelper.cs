using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace ForStock.Server.Common
{
    public static class CorpCodeHelper{

        public static string GetCorpCode(string key_word)
        {
            XElement root = XElement.Load("./Common/CORPCODE.xml");

            IEnumerable<XElement> result =
                from el in root.Elements("list")
                where (string) el.Element("stock_code") == key_word || (string) el.Element("corp_name") == key_word
                select el;

            if(result.Count() > 0)
            {
                return (string) result.First().Element("corp_code");
            }
            
            return null;
        }
    }
}