using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 聊天服务类
    /// </summary>
    class ChatService : Singleton<ChatService>, IDisposable
    {
        public void Init()
        {

        }

        public ChatService()
        {
            MessageDistributer.Instance.Subscribe<ChatResponse>(this.OnChat);
        }
       
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ChatResponse>(this.OnChat);
        }
        /// <summary>
        /// 发送聊天消息请求
        /// </summary>
        public void SendChat(ChatChannel channel, string content, int toId, string toName)
        {
            Debug.Log("SendChatRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.Chat = new ChatRequest();
            message.Request.Chat.Message = new ChatMessage();
            message.Request.Chat.Message.Channel = channel;
            message.Request.Chat.Message.Message = content;
            message.Request.Chat.Message.ToId = toId;
            message.Request.Chat.Message.ToName = toName;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到聊天消息的响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnChat(object sender, ChatResponse message)
        {
            if(message.Result == Result.Success)
            {
                ChatManager.Instance.AddMessages(ChatChannel.Local, message.localMessages);
                ChatManager.Instance.AddMessages(ChatChannel.World, message.worldMessages);
                ChatManager.Instance.AddMessages(ChatChannel.System, message.systemMssages);
                ChatManager.Instance.AddMessages(ChatChannel.Private, message.privateMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Team, message.teamMessages);
                ChatManager.Instance.AddMessages(ChatChannel.Guild, message.guildMessages);
            }
            else
            {
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }
    }
}
