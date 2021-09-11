using Message_Service.Models;
using Message_Service.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Message_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        // Messages.
        private static List<MessageInfo> messages = new List<MessageInfo>();
        // Storage where information about messages and users is stored.
        private Storage _storage;
        public MessageController(Storage storage)
        {
            _storage = storage;
        }

       /// <summary>
       /// Send message to user.
       /// </summary>
       /// <param name="req">Request.</param>
       /// <returns></returns>
        [HttpPost("/send-message")]
        public IActionResult SendMessage(SendMessageRequest req)
        {
            try
            {
                MessageInfo messageInfo = _storage.SendMessage(req.ReceiverId, req.SenderId,req.Subject, req.Message);
                return Ok(messageInfo);
            }
            catch (Exception)
            {
                return NotFound(new
                {
                    Message = $"Пользователь-получатель receiverId = {req.ReceiverId} или senderId = {req.SenderId} не найден"
                });

            }
        }
        /// <summary>
        /// Getting messages by senderId.
        /// </summary>
        /// <param name="senderId">SenderId.</param>
        /// <returns>List of Messages.</returns>
        [HttpPost("/get-messages-by-senderId")]
        public IActionResult GetMessagesBySenderId(string senderId)
        {
            try
            {
                return Ok(_storage.GetMessagesBySenderId(senderId));
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }
        /// <summary>
        /// Getting messages by receiverId.
        /// </summary>
        /// <param name="receiverId">ReceiverId.</param>
        /// <returns>List of messages.</returns>
        [HttpPost("/get-messages-by-receiverId")]
        public IActionResult GetMessagesByReceiverId(string receiverId)
        {
            try
            {
                return Ok(_storage.GetMessagesByReceiverId(receiverId));
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }
        /// <summary>
        /// Getting messages by receiverId and SenderId.
        /// </summary>
        /// <param name="receiverId">ReceiverId.</param>
        /// <param name="senderId">SenderId.</param>
        /// <returns></returns>
        [HttpPost("/get-messages-by-receiverId-and-senderId")]
        public IActionResult GetMessagesByReceiverId(string receiverId,string senderId)
        {
            try
            {
                return Ok(_storage.GetMessagesByReceiverAndSenderId(receiverId,senderId));
            }
            catch (Exception ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
