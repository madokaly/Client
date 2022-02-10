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
    class TeamService : Singleton<TeamService>, IDisposable
    {
        public void Init()
        {

        }
        public TeamService()
        {
            MessageDistributer.Instance.Subscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Subscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Subscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Subscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<TeamInviteRequest>(this.OnTeamInviteRequest);
            MessageDistributer.Instance.Unsubscribe<TeamInviteResponse>(this.OnTeamInviteResponse);
            MessageDistributer.Instance.Unsubscribe<TeamInfoResponse>(this.OnTeamInfo);
            MessageDistributer.Instance.Unsubscribe<TeamLeaveResponse>(this.OnTeamLeave);
        }

        /// <summary>
        /// 发送邀请组队请求
        /// </summary>
        /// <param name="friendId"></param>
        /// <param name="friendName"></param>
        public void SendTeamInviteRequest(int friendId, string friendName)
        {
            Debug.Log("SendTeamInviteRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteReq = new TeamInviteRequest();
            message.Request.teamInviteReq.FromId = User.Instance.CurrentCharacterInfo.Id;
            message.Request.teamInviteReq.FromName = User.Instance.CurrentCharacterInfo.Name;
            message.Request.teamInviteReq.ToId = friendId;
            message.Request.teamInviteReq.ToName = friendName;

            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 接收反馈的组队邀请
        /// </summary>
        /// <param name="accept"></param>
        /// <param name="request"></param>
        public void SendTeamInviteResponse(bool accept, TeamInviteRequest request)
        {
            Debug.Log("SendTeamInviteResponse");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamInviteRes = new TeamInviteResponse();
            message.Request.teamInviteRes.Result = accept ? Result.Success : Result.Failed;
            message.Request.teamInviteRes.Errormsg = accept ? "组队成功" : "对方拒绝组队";
            message.Request.teamInviteRes.Request = request;

            NetClient.Instance.SendMessage(message);

        }

        /// <summary>
        /// 接收队伍邀请通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="request"></param>
        private void OnTeamInviteRequest(object sender, TeamInviteRequest request)
        {
            var config = MessageBox.Show(string.Format("{0}邀请你进入组队", request.FromName), "组队请求", MessageBoxType.Confirm, "接受", "拒绝");
            config.OnYes = () =>
            {
                this.SendTeamInviteResponse(true, request);
            };
            config.OnNo = () =>
            {
                this.SendTeamInviteResponse(false, request);
            };
        }

        /// <summary>
        /// 接收队伍邀请响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInviteResponse(object sender, TeamInviteResponse message)
        {
            if (message.Result == Result.Success)
            {
                MessageBox.Show(message.Request.ToName + "进入您的队伍", "邀请组队成功");
            }
            else
            {
                MessageBox.Show(message.Errormsg, "邀请组队失败");
            }
        }

        /// <summary>
        /// 发送离开队伍请求
        /// </summary>
        /// <param name="id">队伍id</param>
        public void SendTeamLeaveRequest(int id)
        {
            Debug.Log("SendTeamLeaveRequest");

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.teamLeave = new TeamLeaveRequest();
            message.Request.teamLeave.TeamId = User.Instance.TeamInfo.Id;
            message.Request.teamLeave.characterId = User.Instance.CurrentCharacterInfo.Id;

            NetClient.Instance.SendMessage(message);
        }

        /// <summary>
        /// 刷新队伍信息（响应队伍变化事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnTeamInfo(object sender, TeamInfoResponse message)
        {
            Debug.Log("OnIteamInfo");
            TeamManager.Instance.UpdateTeamInfo(message.Team);
        }
        /// <summary>
        /// 退出队伍响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnTeamLeave(object sender, TeamLeaveResponse message)
        {
            if (message.Result == Result.Success)
            {
                TeamManager.Instance.UpdateTeamInfo(null);
                MessageBox.Show("退出成功", "退出队伍");
            }
            else
            {
                MessageBox.Show("退出失败", "退出队伍", MessageBoxType.Error);
            }
        }
    }
}
