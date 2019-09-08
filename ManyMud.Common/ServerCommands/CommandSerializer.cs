using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ManyMud.Common.Interfaces;

namespace ManyMud.Common.ServerCommands
{
    public class CommandSerializer : ICommandSerializer
    {
        private class Binder : ISerializationBinder
        {
            private static IList<Type> CommandTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == typeof(CommandSerializer).Namespace).ToList();

            public void BindToName(Type serializedType, out string assemblyName, out string typeName)
            {
                assemblyName = null;
                typeName = serializedType.Name;
            }

            public Type BindToType(string assemblyName, string typeName)
            {
                return CommandTypes.SingleOrDefault(t => t.Name == typeName);
            }
        }

        public string Serialize(object obj) => JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);

        public object Deserialize(string json) => JsonConvert.DeserializeObject(json, Settings);

        public readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects,
            SerializationBinder = new Binder(),
        };
    }
}
