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
    /// <summary>
    /// Handles JSON parsing for json strings being sent and received with ActiveMQ.
    /// </summary>
    public class PsiMQJson
    {
        /// <summary>
        /// The message content. Can be any valid string (possibly more json)
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// The DateTime, should be used to track the origin time of a message. If the messag is a response/translation
        /// of a previos message then the DateTime should match that original message. The DateTime should be originally
        /// set by a sensor or whatever originally generated the source data.
        /// </summary>
        public DateTime DateTime { get; set; }


        public PsiMQJson(string str, DateTime dt)
        {
            this.Content = str;
            this.DateTime = dt;
        }

        
        /// <summary>
        /// Parses JSON string and creates PsiMQJson object with a content string and Date Time.
        /// </summary>
        /// <param name="json">Json string to be parsed</param>
        /// <returns></returns>
        public static PsiMQJson FromJson(string json)
        {
            PsiMQJson obj = JsonConvert.DeserializeObject<PsiMQJson>(json);
            return obj;
        }

        /// <summary>
        /// Serializes a content string and date time into a json string that can be parsed into
        /// a PsiMQJson object
        /// </summary>
        /// <param name="str">Content String</param>
        /// <param name="dt">Date Time</param>
        /// <returns></returns>
        public static string SerializeString(string str, DateTime dt)
        {
            PsiMQJson obj = new PsiMQJson(str, dt);
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Checks that a string can be parsed as json into a PsiMQJson object. The string much be json that contains
        /// a conent field with a string, and a DateTime field with a Date Time in this format (yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzz)
        /// </summary>
        /// <param name="str">Json String to be validated</param>
        /// <returns></returns>
        public static bool isValid(string str)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(PsiMQJson));
            return JObject.Parse(str).IsValid(schema);
        }
    }
}
