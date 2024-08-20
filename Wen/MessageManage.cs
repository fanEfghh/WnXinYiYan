using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wen
{
    internal class MessageManage
    {
        public string message {  get; set; }
        public List<string> messages { get; set; }
        public delegate void Addmessage();
        public  Addmessage addmessage { get; set; }
        public void LimitMessage()
        {
            if(messages.Count>=11)
            {
                messages.RemoveRange(1, 2);
            }
        }
    }
}
