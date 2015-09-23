using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Commands.IO
{
    public static class SerializerService
    {
        private static BinaryFormatter bformatter = new BinaryFormatter();
        /// <summary>
        /// Serializes the given object in the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="obj">The object.</param>
        public static void Serialize(string path, object obj)
        {
            using (Stream stream = File.Open(path, FileMode.Create))
            {
                bformatter.Serialize(stream, obj);
            }
        }

        /// <summary>
        /// Deserializes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>the object</returns>
        /// <exception cref="System.Exception">no serialized file found</exception>
        public static object Deserialize(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream stream = File.Open(path, FileMode.Open))
                { 
                    Object engine = bformatter.Deserialize(stream);
                    return engine;
                }     
            }
            else
            {
                throw new Exception("no serialized file found");
            }
        }
    }
}
