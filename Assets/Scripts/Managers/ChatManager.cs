using System;
using System.Collections.Generic;
using System.Text;
using Models;
using Services;
using SkillBridge.Message;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 聊天管理器
    /// </summary>
    class ChatManager : Singleton<ChatManager>
    {
        public enum LocalChannel
        {
            All = 0,
            Local = 1,
            World = 2,
            Team = 3,
            Guild = 4,
            Private = 5
        }

        private ChatChannel[] ChannelFilter = new ChatChannel[6]
        {
            ChatChannel.Local | ChatChannel.World | ChatChannel.Guild | ChatChannel.Team | ChatChannel.Private | ChatChannel.System,
            ChatChannel.Local,
            ChatChannel.World,
            ChatChannel.Team,
            ChatChannel.Guild,
            ChatChannel.Private
        };
        /// <summary>
        /// 私聊对象ID
        /// </summary>
        public int PrivateID = 0;
        /// <summary>
        /// 私聊对象名字
        /// </summary>
        public string PrivateName = "";
        /// <summary>
        /// 消息列表
        /// </summary>
        public List<ChatMessage>[] Messages = new List<ChatMessage>[6] 
        {
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
            new List<ChatMessage>(),
        };
        /// <summary>
        /// 展示频道
        /// </summary>
        public LocalChannel displayChannel;
        /// <summary>
        /// 发送频道
        /// </summary>
        public LocalChannel sendChannel;
        /// <summary>
        /// 定义委托
        /// </summary>
        public Action OnChat { get; internal set; }
        /// <summary>
        /// 服务器频道
        /// </summary>
        public ChatChannel SendChannel
        {
            get
            {
                switch (sendChannel)
                {
                    case LocalChannel.Local: return ChatChannel.Local;
                    case LocalChannel.World: return ChatChannel.World;
                    case LocalChannel.Team: return ChatChannel.Team;
                    case LocalChannel.Guild: return ChatChannel.Guild;
                    case LocalChannel.Private: return ChatChannel.Private;
                }
                return ChatChannel.Local;
            }
        }

        /// <summary>
        /// 私聊
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="targetName"></param>
        internal void StartPrivateChat(int targetId, string targetName)
        {
            this.PrivateID = targetId;
            this.PrivateName = targetName;
            this.sendChannel = LocalChannel.Private;
            if(this.OnChat != null)
            {
                this.OnChat();
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            foreach(var message in this.Messages)
            {
                message.Clear();
            }
        }
        /// <summary>
        /// 构建发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="toId"></param>
        /// <param name="toName"></param>
        public void SendChat(string message, int toId = 0, string toName = "")
        {
            ChatService.Instance.SendChat(this.SendChannel, message, toId, toName);
        }
        /// <summary>
        /// 更换聊天频道下拉框
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public bool SetSendChannel(LocalChannel channel)
        {
            if(channel == LocalChannel.Team)
            {
                if(User.Instance.TeamInfo == null)
                {
                    this.AddSystemMessage("你没有加入任何队伍！");
                    return false;
                }
            }
            if (channel == LocalChannel.Guild)
            {
                if (User.Instance.CurrentCharacterInfo.Guild == null)
                {
                    this.AddSystemMessage("你没有加入任何公会！");
                    return false;
                }
            }
            this.sendChannel = channel;
            Debug.LogFormat("Set Channel:[{0}]",this.sendChannel);
            return true;
        }
        /// <summary>
        /// 添加服务端发来的信息
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="messages"></param>
        internal void AddMessages(ChatChannel channel, List<ChatMessage> messages)
        {
            for(int ch = 0; ch < 6; ch++)
            {
                if((this.ChannelFilter[ch] & channel) == channel)
                {
                    this.Messages[ch].AddRange(messages);
                }
            }
            if(this.OnChat != null)
            {
                this.OnChat();
            }
        }
        /// <summary>
        /// 添加系统消息
        /// </summary>
        /// <param name="v"></param>
        public void AddSystemMessage(string message, string from = "")
        {
            this.Messages[(int)LocalChannel.All].Add(new ChatMessage()
            {
                Channel = ChatChannel.System,
                Message = message,
                FromName = from
            });
            if(this.OnChat != null)
            {
                this.OnChat();
            }
        }
        /// <summary>
        /// 获得当前所有信息
        /// </summary>
        /// <returns></returns>
        public string GetCurrentMessages()
        {
            StringBuilder builder = new StringBuilder();
            foreach(var message in this.Messages[(int)displayChannel])
            {
                builder.AppendLine(FormatMessage(message));
            }
            return builder.ToString();
        }
        /// <summary>
        /// 格式化信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string FormatMessage(ChatMessage message)
        {
            switch (message.Channel)
            {
                case ChatChannel.Local:
                    return string.Format("[本地]{0}{1}", FormatFromPlayer(message), message.Message);
                case ChatChannel.World:
                    return string.Format("<color=cyan>[世界]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.System:
                    return string.Format("<color=yellow>[系统]{0}</color>", message.Message);
                case ChatChannel.Private:
                    return string.Format("<color=magenta>[私聊]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Team:
                    return string.Format("<color=green>[队伍]{0}{1}</color>", FormatFromPlayer(message), message.Message);
                case ChatChannel.Guild:
                    return string.Format("<color=blue>[公会]{0}{1}</color>", FormatFromPlayer(message), message.Message);
            }
            return "";
        }
        /// <summary>
        /// 格式化名字信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string FormatFromPlayer(ChatMessage message)
        {
            if(message.FromId == User.Instance.CurrentCharacterInfo.Id)
            {
                return "<a name=\"\" class=\"player\">[我]</a>";
            }
            else
            {
                return string.Format("<a name=\"c:{0}:{1}\" class=\"player\">[{1}]</a>", message.FromId, message.FromName);
            }
        }
    }
}
