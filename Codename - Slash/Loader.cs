using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Codename___Slash
{
    public class Loader
    {
        
        /// <summary>
        /// XML Reader generic method that takes in a xml file and pushes its contents into the given object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <param name="infoObj"></param>
        public static void ReadXML<T>(string filename, ref T infoObj)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    infoObj = (T)new XmlSerializer(typeof(T)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }

    }
}
