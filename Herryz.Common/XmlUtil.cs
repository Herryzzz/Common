using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Herryz.Common
{
    public class XMLUtil
    {
        /// <summary>
        /// 序列化XMLNODE
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="path">完整的XML路径</param>
        /// <returns>T</returns>
        public static T XMLConvertNode<T>(string path) where T : class
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            return XMLConvertNode<T>(xml);
        }
        /// <summary>
        /// 序列化XMLNODE
        /// </summary>
        /// <typeparam name="T">模型类型</typeparam>
        /// <param name="node">XML节点</param>
        /// <returns>T</returns>
        public static T XMLConvertNode<T>(XmlNode node) where T : class
        {
            MemoryStream stm = new MemoryStream();

            StreamWriter stw = new StreamWriter(stm);
            stw.Write(node.OuterXml);
            stw.Flush();

            stm.Position = 0;

            XmlSerializer ser = new XmlSerializer(typeof(T));
            T result = (ser.Deserialize(stm) as T);
            return result;
        }
    }
}
