using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace ProductSearch.Helpers
{
    public class XmlHelper
    {
        public static IDictionary<T, string> ParseEnum<T>(XElement element)
        {
            var result = new Dictionary<T, string>();
            var ns = element.Name.Namespace;

            var keys = Enum.GetNames(typeof(T));
            foreach (var key in keys)
            {
                var el = element.Element(ns + key);
                if (el != null)
                {
                    result.Add((T)Enum.Parse(typeof(T), key), el.Value);
                }
            }

            return result;
        }
    }
}