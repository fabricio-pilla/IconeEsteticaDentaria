using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace IconeEsteticaDentaria.Comum.Helpers
{
    public static class SerializacaoHelper
    {
        /// <summary>
        /// Converte um objeto ou lista de objetos em xml, exemplo de chamada:
        /// clsConverterHelper.serializarParaXML<List<clsEmpresa>>(lista)
        /// </summary>
        /// <typeparam name="T">Tipo genérico de objeto, sendo uma collection ou não</typeparam>
        /// <param name="obj">objeto</param>
        /// <returns>String com o xml já serializado do objeto</returns>
        public static String SerializarParaXml<T>(T obj)
        {
            string xmlString = null;
            var memoryStream = new MemoryStream();

            var serializer = new XmlSerializer(typeof(T));
            var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, obj);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            xmlString = Utf8ByteArrayToString(memoryStream.ToArray());
            return xmlString.Replace(" encoding=\"utf-8\"", "");
        }

        /// <summary>
        /// Converte um array de bytes em string
        /// </summary>
        /// <param name="characters">array de bytes</param>
        /// <returns>string com a conversão</returns>
        static string Utf8ByteArrayToString(byte[] characters)
        {
            return new UTF8Encoding().GetString(characters);
        }
    }
}
