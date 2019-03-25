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
        /// Reads a csv file to a 2d array of strings
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="output"></param>
        public static void ReadCSVFileTo2DArray(string filename, ref string[,] output)
        {
            try
            {
                output = ConvertFromJaggedtoMulti(File.ReadAllLines(filename).Select(l => l.Split(',').ToArray()).ToArray());
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR: 2D File could not be read!");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }


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

        public static void ReadXMLList<T>(string filename, ref List<T> infoObjList)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    infoObjList = (List<T>)new XmlSerializer(typeof(List<T>)).Deserialize(reader.BaseStream);
                }
            }
            catch (Exception e)
            {
                // If we've caught an exception, output an error message
                // describing the error
                Console.WriteLine("ERROR: XML List File could not be deserialized!");
                Console.WriteLine("Exception Message: " + e.Message);
            }
        }


        // Helper Method to convert from a jagged array to a 2d multi dimensional array
        // Courtesy of https://highfieldtales.wordpress.com/2013/08/17/convert-a-jagged-array-into-a-2d-array/
        private static string[,] ConvertFromJaggedtoMulti(string[][] source)
        {
            return new[] { new string[source.Length, source[0].Length] }
                .Select(_ => new { x = _, y = source.Select((a, ia) => a.Select((b, ib) => _[ia, ib] = b).Count()).Count() })
                .Select(_ => _.x)
                .First();
        }

        /// <summary>
        /// Takes in a dictionary of type K and V, and a filename, uses the READXML method to convert the file into a keyvaluepair list and then adds it to a dictionary
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="dict"></param>
        public static void XMLToDictionary<K, V>(string fileName, ref Dictionary<K, V> dict)
        {
            List<KeyValuePair<K, V>> keyValueList = new List<KeyValuePair<K, V>>();

            ReadXMLList(fileName, ref keyValueList);

            int size = keyValueList.Count;
            for(int i = 0; i < size; i++)
            {
                dict.Add(keyValueList[i].Key, keyValueList[i].Value);
            }
            
        }


    }
}
