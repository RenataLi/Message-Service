using Message_Service.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Message_Service.Service
{
    public class Storage
    {
        string directoryname = Path.Combine(Environment.GetFolderPath
                              (Environment.SpecialFolder.LocalApplicationData),
                              "Message Service");
        // List of users.
        private  UserList users = new UserList();
        // List of messages.
        public  List<MessageInfo> messages = new List<MessageInfo>();
        /// <summary>
        /// Class constructor.
        /// </summary>
        public Storage()
        {
            if (!Directory.Exists(directoryname))
            {
                Directory.CreateDirectory(directoryname);
            }
        }
        /// <summary>
        /// Class usersList.
        /// </summary>
        public class UserList : KeyedCollection<string, UserInfo>
        {
            protected override string GetKeyForItem(UserInfo item)
            {
                return item.Email;
            }
        }
        /// <summary>
        /// Method for serislizing.
        /// </summary>
        public void Serialize()
        {
            using (FileStream fs = new FileStream(Path.Combine(directoryname, "data.json"), FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer format = new DataContractJsonSerializer(typeof(Storage));
                format.WriteObject(fs, this);
            }
        }
        /// <summary>
        /// Method for deserializing.
        /// </summary>
        public void Deserialize()
        {
            Storage storage;
            using (FileStream fs = new FileStream(Path.Combine(directoryname, "data.json"), FileMode.OpenOrCreate))
            {
                DataContractJsonSerializer format = new DataContractJsonSerializer(typeof(Storage));
                storage = (Storage)format.ReadObject(fs);
                users = storage.users;
                messages = storage.messages;
            }
        }
        /// <summary>
        /// Creating user.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <param name="name">Name of user.</param>
        /// <returns>User.</returns>
        public UserInfo CreateUser(string id, string name)
        {
            var user = new UserInfo() { Email = id, UserName = name };
            if (users.Contains(id))
                throw new ArgumentException("Такой пользователь уже существует!");
            users.Add(user);
            return user;
        }
        /// <summary>
        /// Getting user by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>User.</returns>
        public UserInfo GetById(string id)
        {
            if (users.Contains(id))
                return users[id];
            else
                return null;
        }
        /// <summary>
        /// Getting All Users.
        /// </summary>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        /// <returns></returns>
        public List<UserInfo> GetAllUsesrs(int limit, int offset) { return users.Skip(offset).Take(limit).OrderBy(x=>x.Email).ToList(); }

        /// <summary>
        /// Getting all users.
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetAllUsesrs() { return users.OrderBy(x=>x.Email).ToList(); }
        /// <summary>
        /// Getting messages by sender Id.
        /// </summary>
        /// <param name="senderId">Sender Id.</param>
        /// <returns>List of messages.</returns>
       
        public List<MessageInfo> GetMessagesBySenderId(string senderId)
        {
            if (!users.Contains(senderId))
                throw new ArgumentException("Пользователь с таким E-mail не существует!");
            var result = messages.Where(x => x.SenderId
                == senderId ).ToList();
            return result;
        }
        /// <summary>
        /// Getting messages by receiver Id.
        /// </summary>
        /// <param name="receiverId">Receiver Id.</param>
        /// <returns>List of messages.</returns>
        public List<MessageInfo> GetMessagesByReceiverId(string receiverId)
        {
            if (!users.Contains(receiverId))
                throw new ArgumentException("Пользователь с таким E-mail не существует!");
            var result = messages.Where(x => receiverId == x.ReceiverId).ToList();
            return result;
        }
        /// <summary>
        /// Getting messages by receiver Id and sender Id.
        /// </summary>
        /// <param name="receiverId">Receiver Id.</param>
        /// <param name="senderId">Sender Id.</param>
        /// <returns>List of messages.</returns>
        public List<MessageInfo> GetMessagesByReceiverAndSenderId(string receiverId,string senderId)
        {
            if (!users.Contains(receiverId)||!users.Contains(senderId))
                throw new ArgumentException("Пользователь с таким E-mail не существует!");
            var result = messages.Where(x => receiverId == x.ReceiverId&&senderId==x.SenderId).ToList();
            return result;
        }
        /// <summary>
        /// Sending messages.
        /// </summary>
        /// <param name="receiverId">Receiver Id.</param>
        /// <param name="senderId">Sender Id.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="message">Message.</param>
        /// <returns>Message.</returns>
        public MessageInfo SendMessage(string receiverId,string senderId,string subject,string message)
        {
            // Checking for correct users.
            if (!users.Contains(senderId))
            {
                throw new ArgumentException($"Пользователь-отправитель senderId = {senderId} не найден");
            }
            if (!users.Contains(receiverId))
            {
                throw new ArgumentException($"Пользователь-отправитель senderId = {receiverId} не найден");
            }
            MessageInfo messageInfo = new MessageInfo()
            {
                Message = message,
                Subject = subject,
                ReceiverId = receiverId,
                SenderId = senderId,
            };
            messages.Add(messageInfo);
            return messageInfo;
        }
    }
}
