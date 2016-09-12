//using System;
//using System.Reflection;
//using Newtonsoft.Json;

//namespace Ruley.Core.Filters
//{
//    public class PropertyConverter : JsonConverter
//    {
//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            throw new Exception("not written");
//        }

//        private Property<T> Create<T>(object value)
//        {
//            return new Property<T>(value);
//        } 

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
//            JsonSerializer serializer)
//        {
//            MethodInfo method = typeof(PropertyConverter).GetMethod("Create", BindingFlags.Instance | BindingFlags.NonPublic);
//            MethodInfo generic = method.MakeGenericMethod(objectType.GenericTypeArguments[0]);

//            var returnValue = generic.Invoke(this, new object[] { reader.Value });
//            return returnValue;
//        }

//        public override bool CanRead
//        {
//            get { return true; }
//        }

//        public override bool CanConvert(Type objectType)
//        {
//            if (objectType.IsGenericType)
//            {
//                return objectType.GetGenericTypeDefinition() == typeof(Property<>);
//            }
//            return false;
//        }
//    }
//}