using System;
using System.IO;
using RestSharp;//依赖版本106.15.0 https://www.nuget.org/packages/RestSharp/106.15.0
using Newtonsoft.Json;
using YourNamespace;
using System.Data;
using System.Reflection.Metadata;
using Wen;

namespace SampleApplication
{
    
    public class Sample
    {

        const string API_KEY = "";
        const string SECRET_KEY = "";
        static bool IsInitialize = true;
        static MessageManage message = new MessageManage();
       
        public static void Main(string[] args)
        {
            message.messages = new List<string>();
            var client = new RestClient($"https://aip.baidubce.com/rpc/2.0/ai_custom/v1/wenxinworkshop/chat/completions?access_token={GetAccessToken()}");
            while (true)
            {
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                if (IsInitialize)
                {
                    Initialize(request, client);
                    Console.WriteLine("初始化完成");
                }
                Console.Write("User:");
                string chat = Console.ReadLine();
                //消息编辑
                var Question = @"{ ""role"":""user"",""content"":""消息""}";
                Question = Question.Replace("\"消息\"", $"\"{chat}\"");
                message.messages.Add(Question);
                message.message = string.Join(",", message.messages);
                var body = $@"{{""messages"":[{message.message}],""temperature"":0.95,""top_p"":0.8,""penalty_score"":1,""disable_search"":false,""enable_citation"":false}}";
                message.LimitMessage();

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                ChatCompletionResponse Message = JsonConvert.DeserializeObject<ChatCompletionResponse>(response.Content);
                message.messages.Add(Message.UpdateReply());
                Console.WriteLine($"Result: {Message.Result}");
                //string a = string.Join(" ,", message.messages);
                //Console.WriteLine(a);

            }
            

        }
        //初始化
        static void Initialize(RestRequest request,RestClient client)
        {
            message.message = @"{ ""role"":""user"",""content"":""初始消息""}";
            message.messages.Add(message.message);
            var body = $@"{{""messages"":[{message.message}],""temperature"":0.95,""top_p"":0.8,""penalty_score"":1,""disable_search"":false,""enable_citation"":false}}";
            
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            ChatCompletionResponse Message = JsonConvert.DeserializeObject<ChatCompletionResponse>(response.Content);
            Console.WriteLine($"Result: {Message.Result}");
            message.messages.Add(Message.UpdateReply());
            IsInitialize = false;
            
        }
        

        /**
        * 使用 AK，SK 生成鉴权签名（Access Token）
        * @return 鉴权签名信息（Access Token）
        */
        static string GetAccessToken()
        {
            var client = new RestClient($"https://aip.baidubce.com/oauth/2.0/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", API_KEY);
            request.AddParameter("client_secret", SECRET_KEY);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            var result = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return result.access_token.ToString();
        }

    }
}