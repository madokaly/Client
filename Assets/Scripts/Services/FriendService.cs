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
    class FriendService : Singleton<FriendService>, IDisposable
    {
        /// <summary>
        /// 好友状态变化事件
        /// </summary>
        public UnityAction OnFriendUpdate;
        public void Init()
        {

        }
        public FriendService()
        {
            MessageDistributer.Instance.Subscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Subscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Subscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Subscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }



        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<FriendAddRequest>(this.OnFriendAddRequest);
            MessageDistributer.Instance.Unsubscribe<FriendAddResponse>(this.OnFriendAddResponse);
            MessageDistributer.Instance.Unsubscribe<FriendListResponse>(this.OnFriendList);
            MessageDistributer.Instance.Unsubscribe<FriendRemoveResponse>(this.OnFriendRemove);
        }
        /// <summary>
        /// 发送好友添加的请求
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="friendName"></param>
        public void SendFriendAddRequest(int friendId, string friendName)
        {
            Debug.Log("SendFirendAddRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddReq = new FriendAddRequest();
            message.Request.friendAddReq.FromId = User.Instance.CurrentCharacterInfo.Id;
            message.Request.friendAddReq.FromName = User.Instance.CurrentCharacterInfo.Name;
            message.Request.friendAddReq.ToId = friendId;
            message.Request.friendAddReq.ToName = friendName;

            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 发送好友添加的回应
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="request"></param>
        public void SendFriendAddResponse(bool accept, FriendAddRequest request)
        {
            Debug.Log("SendFriendAddResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendAddRes = new FriendAddResponse();
            message.Request.friendAddRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.friendAddRes.Errormsg = accept ? "对方同意" : "对方拒绝添加你为好友";
            message.Request.friendAddRes.Request = request;

            NetClient.Instance.SendMessage(message);

        }
        /// <summary>
        /// 收到添加好友的请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnFriendAddRequest(object sender, FriendAddRequest request)
        {
            var config = MessageBox.Show(string.Format("{0}请求加你为好友", request.FromName), "好友请求", MessageBoxType.Confirm, "接受", "拒绝");
            config.OnYes = () =>
              {
                  this.SendFriendAddResponse(true, request);
              };
            config.OnNo = () =>
              {
                  this.SendFriendAddResponse(false, request);
              };
        }
        /// <summary>
        /// 收到添加好友的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnFriendAddResponse(object sender, FriendAddResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.FromName + "接受了您的请求", "添加好友成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "添加好友失败");
            }
        }
        /// <summary>
        /// 响应好友状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnFriendList(object sender, FriendListResponse message)
        {
            Debug.Log("OnFriendList");
            FriendManager.Instance.Init(message.Friends);
            if (OnFriendUpdate != null)
            {
                //更新好友列表
                OnFriendUpdate();
            }
        }
        /// <summary>
        /// 发送好友移除请求
        /// </summary>
        /// <param name="id"></param>
        /// <param name="friendId"></param>
        public void OnFriendRemoveRequest(int id, int friendId)
        {
            Debug.Log("OnFriendRemoveRequest");

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.friendRemove = new FriendRemoveRequest();

            message.Request.friendRemove.Id = id;
            message.Request.friendRemove.friendId = friendId;

            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 响应好友移除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnFriendRemove(object sender, FriendRemoveResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show("删除成功", "删除好友");
            }
            else
            {
                MessageBox.Show("删除失败", "删除好友", MessageBoxType.Error);
            }
        }

    }
}
