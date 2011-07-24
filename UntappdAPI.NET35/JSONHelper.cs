using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UntappdAPI.DataContracts;

namespace UntappdAPI
{
    public class JSONHelper
    {
        public static T Deserialise<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();

            var serializer = new JsonSerializer();
            serializer.Converters.Add(new SingleBadgeConverter());

            using (var reader = new StringReader(json))
            using (var jsonReader = new JsonTextReader(reader))
            {
                obj = serializer.Deserialize<T>(jsonReader);
            }
            
            return obj;

            //using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            //{

            //    return new JsonSerializer().Deserialize<new StringReader(json));
            //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //    obj = (T)serializer.ReadObject(ms); // <== Your missing line
            //    return obj;
            //}
        }

        public static string Serialize<T>(T obj)
        {

            var serializer = new JsonSerializer();
            using (var writer = new StringWriter())
            using (var jsonWriter = new JsonTextWriter(writer)) {
                serializer.Serialize(jsonWriter, obj);
                return writer.ToString();
            }
            
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    serializer.WriteObject(ms, obj);
            //    return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
            //}
        }

        internal class SingleBadgeConverter : JsonConverter {
            public override bool CanConvert(Type objectType) {
                return objectType == typeof(Badge[]);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var ser = new JsonSerializer();

                if (reader.TokenType == JsonToken.StartObject) {
                    var badge = ser.Deserialize<Badge>(reader);
                    return new Badge[1] { badge };
                } else if (reader.TokenType == JsonToken.StartArray) {
                    return ser.Deserialize<Badge[]>(reader);
                } else {
                    return new Badge[0];
                }
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                throw new System.NotImplementedException();
            }

        }
    }
}
