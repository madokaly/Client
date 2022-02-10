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
    /// 任务服务类
    /// </summary>
    class QuestService : Singleton<QuestService>, IDisposable
    {
        public QuestService()
        {
            //注册任务接受回应事件
            MessageDistributer.Instance.Subscribe<QuestAcceptResponse>(this.OnQuestAccept);
            //注册任务提交回应事件
            MessageDistributer.Instance.Subscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }
        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<QuestAcceptResponse>(this.OnQuestAccept);
            MessageDistributer.Instance.Unsubscribe<QuestSubmitResponse>(this.OnQuestSubmit);
        }
        /// <summary>
        /// 发送任务接受请求
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public bool SendQuestAccept(Quest quest)
        {
            Debug.Log("SendQuestAccept");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questAccept = new QuestAcceptRequest();
            message.Request.questAccept.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);

            return true;
        }
        /// <summary>
        /// 响应任务接受事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnQuestAccept(object sender, QuestAcceptResponse message)
        {
            Debug.LogFormat("OnQuestAccept:{0},ERR:{1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                //任务管理类处理任务接受
                QuestManager.Instance.OnQuestAccepted(message.Quest);
            }
            else
            {
                MessageBox.Show("任务接受失败", "错误", MessageBoxType.Error);
            }
        }
        /// <summary>
        /// 发送任务提交请求
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        public bool SendQuestSubmit(Quest quest)
        {
            Debug.Log("SendQuestSubmit");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.questSubmit = new QuestSubmitRequest();
            message.Request.questSubmit.QuestId = quest.Define.ID;
            NetClient.Instance.SendMessage(message);
            return true;
        }
        /// <summary>
        /// 响应任务提交请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnQuestSubmit(object sender, QuestSubmitResponse message)
        {
            Debug.LogFormat("OnQuestSubmit:{0},ERR:{1}", message.Result, message.Errormsg);
            if (message.Result == Result.Success)
            {
                //任务管理类处理任务提交
                QuestManager.Instance.OnQuestSubmited(message.Quest);
            }
            else
            {
                MessageBox.Show("任务完成失败", "错误", MessageBoxType.Error);
            }
        }

    }
}
