using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Services
{
    /// <summary>
    /// 剧情副本服务类
    /// </summary>
    public class StoryService : Singleton<StoryService>, IDisposable
    {
       
        public void Init()
        {
            StoryManager.Instance.Init();
        }
        public StoryService()
        {
            MessageDistributer.Instance.Subscribe<StoryStartResponse>(this.OnStoryStart);
            MessageDistributer.Instance.Subscribe<StoryEndResponse>(this.OnStoryEnd);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StoryStartResponse>(this.OnStoryStart);
            MessageDistributer.Instance.Unsubscribe<StoryEndResponse>(this.OnStoryEnd);
        }

        /// <summary>
        /// 发送剧情副本开始的请求
        /// </summary>
        /// <param name="storyId"></param>
        public void SendStartStory(int storyId)
        {
            Debug.Log("SendStartStory" + storyId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.storyStart = new StoryStartRequest();
            message.Request.storyStart.storyId = storyId;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到剧情副本开始的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnStoryStart(object sender, StoryStartResponse message)
        {
            Debug.Log("OnStoryStart" + message.storyId);
            StoryManager.Instance.OnStoryStart(message.storyId);
        }

        /// <summary>
        /// 发送剧情副本结束的请求
        /// </summary>
        public void SendEndStory(int storyId)
        {
            Debug.Log("SendEndStory" + storyId);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.storyEnd = new StoryEndRequest();
            message.Request.storyEnd.storyId = storyId;
            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 收到剧情副本结束的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnStoryEnd(object sender, StoryEndResponse message)
        {
            Debug.Log("OnStoryEnd" + message.storyId);
            if(message.Result == Result.Success)
            {

            }
        }
    }
}
