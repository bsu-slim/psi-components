using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Linq;

namespace ActiveMQComponent
{
    public class PsiMQJson
    {
        public string Content { get; set; }
        public DateTime DateTime { get; set; }
        public PsiMQJson(string str, DateTime dt)
        {
            this.Content = str;
            this.DateTime = dt;
        }

        /**
         * Parses json for datetime attribute
         */
        public static PsiMQJson FromJson(string json)
        {
            PsiMQJson obj = JsonConvert.DeserializeObject<PsiMQJson>(json);
            return obj;
        }

        /**
         * Serializes str into a json string packed along with a datetime
         */
        public static string SerializeString(string str, DateTime dt)
        {
            PsiMQJson obj = new PsiMQJson(str, dt);
            return JsonConvert.SerializeObject(obj);
        }

        /**
         * Checks that the message being sent back is a valid PsiMQJson json string.
         */
        public static bool isValid(string str)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(PsiMQJson));
            return JObject.Parse(str).IsValid(schema);
        }
    }
}
