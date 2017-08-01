using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ScheduleConf
{

    [Serializable]
    public class QuestionandAnswer
    {
        

        public String callKnowledgebase(String query)

        {
            string responseString = string.Empty;
            string knowledgebaseId = "b0e412ce-9d3a-4528-bff0-a06930b28e98";
            string qnamakerSubscriptionKey = "5dc17b39e506468295d9140f73fd6c94";

            //Build the URI
            Uri qnamakerUriBase = new Uri("https://westus.api.cognitive.microsoft.com/qnamaker/v1.0");
            var builder = new UriBuilder($"{qnamakerUriBase}/knowledgebases/{knowledgebaseId}/generateAnswer");

            //Add the question as part of the body
            var postBody = $"{{\"question\": \"{query}\"}}";


            //Send the POST request
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                //Set the encoding to UTF8
                client.Encoding = System.Text.Encoding.UTF8;

                //Add the subscription key header
                client.Headers.Add("Ocp-Apim-Subscription-Key", qnamakerSubscriptionKey);
                client.Headers.Add("Content-Type", "application/json");
                responseString = client.UploadString(builder.Uri, postBody);

                //De-serialize the response
                QnAMakerResult response;
                try
                {
                    response = JsonConvert.DeserializeObject<QnAMakerResult>(responseString);
                }
                catch
                {
                    throw new Exception("Unable to deserialize QnA Maker response string.");
                }
                return response.Answer;

            }
        }//end method
    }//end questionandanswerclass


    [Serializable]
    public class QnAMakerResult
    {
        /// <summary>
        /// The top answer found in the QnA Service.
        /// </summary>
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }

        /// <summary>
        /// The score in range [0, 100] corresponding to the top answer found in the QnA    Service.
        /// </summary>
        [JsonProperty(PropertyName = "score")]
        public double Score { get; set; }


    } 
    
}//namespace


