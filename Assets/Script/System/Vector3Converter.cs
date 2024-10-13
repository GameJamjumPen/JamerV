// using System;
// using Newtonsoft.Json;
// using UnityEngine;

// public class Vector3Converter : JsonConverter
// {
//     public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//     {
//         Vector3 vector = (Vector3)value;
//         writer.WriteStartObject();
//         writer.WritePropertyName("x");
//         writer.WriteValue(vector.x);
//         writer.WritePropertyName("y");
//         writer.WriteValue(vector.y);
//         writer.WritePropertyName("z");
//         writer.WriteValue(vector.z);
//         writer.WriteEndObject();
//     }

//     public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//     {
//         float x = 0f, y = 0f, z = 0f;

//         while (reader.Read())
//         {
//             if (reader.TokenType == JsonToken.PropertyName)
//             {
//                 string propertyName = (string)reader.Value;
//                 reader.Read();

//                 switch (propertyName)
//                 {
//                     case "x":
//                         x = (float)(double)reader.Value;
//                         break;
//                     case "y":
//                         y = (float)(double)reader.Value;
//                         break;
//                     case "z":
//                         z = (float)(double)reader.Value;
//                         break;
//                 }
//             }
//             else if (reader.TokenType == JsonToken.EndObject)
//             {
//                 return new Vector3(x, y, z);
//             }
//         }

//         throw new JsonSerializationException("Unexpected end when reading Vector3.");
//     }

//     public override bool CanConvert(Type objectType)
//     {
//         return objectType == typeof(Vector3);
//     }
// }
