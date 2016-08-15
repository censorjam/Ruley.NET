//using System;
//using System.Collections.Generic;
//using System.IO;
//using Newtonsoft.Json;
//using Ruley.Core.Filters;

//namespace Ruley.Core
//{
//    public class RuleProvider
//    {
//        private readonly Preprocessor _preprocessor;

//        public RuleProvider(Preprocessor preprocessor)
//        {
//            _preprocessor = preprocessor;
//        }

//        private string LoadJson(string filename)
//        {
//            const int retries = 3;

//            for (var attempt = 0; attempt < retries; attempt++)
//            {
//                try
//                {
//                    return File.ReadAllText(filename);
//                }
//                catch (Exception e)
//                {
//                    if (attempt == retries - 1)
//                        throw;
//                }
//            }

//            return null;
//        }

//        public RuleSet Create(string filename)
//        {
//            Console.WriteLine("Loading rule ({0})", filename);
//            var json = LoadJson(filename);
//            json = _preprocessor.Process(json);

//            RuleSet rule = JsonConvert.DeserializeObject<RuleSet>(json, 
//                new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto, NullValueHandling = NullValueHandling.Include, 
//                    Converters = new List<JsonConverter>() { new PropertyConverter() }});
//            rule.FileName = filename;
//            rule.Validate();
//            return rule;
//        }
//    }
//}