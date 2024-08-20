using System;

namespace YourNamespace
{
    public class ChatCompletionResponse
    {
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; } // 假设这是时间戳，通常使用long来存储  
        public string Result { get; set; }
        public bool IsTruncated { get; set; }
        public bool NeedClearHistory { get; set; }
        public string FinishReason { get; set; }
        public Usage usage { get; set; }
        //更新回复
      
        public string UpdateReply()
        {
            string message = null;
            if (Result !=null)
            {
                message = Result.Replace("\n", " ");
            }
            var body = @"{ ""role"":""assistant"",""content"":""消息""}";
            var replay = $"{message}";
            body = body.Replace("\"消息\"", $"\"{replay}\"");
            return body;
        }

        // 嵌套类，用于表示"usage"对象  
        public class Usage
        {
            public int PromptTokens { get; set; }
            public int CompletionTokens { get; set; }
            public int TotalTokens { get; set; }
        }
    }
}