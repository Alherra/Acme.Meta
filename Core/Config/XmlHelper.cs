using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TencentCloud.Npp.V20190823.Models;

namespace Meta.Tools
{
    public class XmlHelper
    {
        /// <summary>
        /// XML缓存
        /// </summary>
        [Description("XML缓存")]
        private static readonly XmlDocument doc = new();

        private static XmlElement xml = null;

        private static XmlElement GetXml()
        {
            doc.Load(Path.Combine(Directory.GetCurrentDirectory(), "application.yml"));
            return doc.DocumentElement!;
        }




        public static string Get(string section)
        {
            if (xml is null)
                xml = GetXml();

            var sections = section.Split('.');
            XmlNodeList list = null!;
            try
            {
                for (int i = 0; i < sections.Length; i++)
                {
                    if (i == sections.Length - 1)
                    {
                        return list?.ToString()!;
                    }
                    list = xml.GetElementsByTagName(sections[i]);
                }
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }
    }
}
