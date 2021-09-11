using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message_Service.Models
{
    public class SendMessageRequest
    {
        // Properties for sending message.
        public string Message { get; set; }
        public string SenderId { get; set; }
        public string Subject { get; set; }
        public string ReceiverId { get; set; }
    }
}
