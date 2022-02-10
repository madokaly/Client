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
    /// 状态类
    /// </summary>
    class StatusService : Singleton<StatusService>, IDisposable
    {
        /// <summary>
        /// 定义状态改变委托
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public delegate bool StatusNotifyHandler(NStatus status);
        /// <summary>
        /// 状态类型+委托
        /// </summary>
        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        /// <summary>
        /// 委托集合
        /// </summary>
        HashSet<StatusNotifyHandler> handles = new HashSet<StatusNotifyHandler>();

        public void Init()
        {

        }
        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="function"></param>
        /// <param name="action"></param>
        public void RegisterStatusNotify(StatusType function,StatusNotifyHandler action)
        {
            if (handles.Contains(action))
                return;
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }
            handles.Add(action);
        }

        public StatusService()
        {
            //注册状态变化事件
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<StatusNotify>(this.OnStatusNotify);
        }
        /// <summary>
        /// 响应状态变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notify"></param>
        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach (NStatus status in notify.Status)
            {
                Notify(status);
            }
        }
        /// <summary>
        /// 处理每个变化的状态
        /// </summary>
        /// <param name="status">状态</param>
        private void Notify(NStatus status)
        {
            Debug.LogFormat("StatusNotify:[{0} {1}] {2} : {3}",status.Type,status.Action,status.Id,status.Value);
            //处理金钱变化
            if (status.Type==StatusType.Money)
            {
                if (status.Action==StatusAction.Add)
                {
                    User.Instance.AddGold(status.Value);
                }
                else if (status.Action==StatusAction.Delete)
                {
                    User.Instance.AddGold(-status.Value);
                }
            }
            //分发其他消息
            StatusNotifyHandler handler;
            if (eventMap.TryGetValue(status.Type, out handler))
            {
                handler(status);
            }
        }
    }
}
