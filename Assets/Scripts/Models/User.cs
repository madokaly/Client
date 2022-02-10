using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Entities;
using Services;
using SkillBridge.Message;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// 当前玩家
    /// </summary>
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        /// <summary>
        /// 当前地图
        /// </summary>
        public MapDefine CurrentMapData { get; set; }
        /// <summary>
        /// 协议当前角色
        /// </summary>
        public SkillBridge.Message.NCharacterInfo CurrentCharacterInfo { get; set; }
        /// <summary>
        /// 当前角色
        /// </summary>
        public Character CurrentCharacter { get; set; }
        /// <summary>
        /// 当前游戏角色的输入控制器
        /// </summary>
        public PlayerInputController CurrentCharacterObject { get; set; }

        public NTeamInfo TeamInfo { get; set; }

        public void AddGold(int gold)
        {
            this.CurrentCharacterInfo.Gold += gold;
        }


        public int CurrentRide = 0;
        internal void Ride(int id)
        {
            if (CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
        }
        /// <summary>
        /// 声明委托
        /// </summary>
        public delegate void CharacterInitHandler();
        /// <summary>
        /// 声明事件
        /// </summary>
        public event CharacterInitHandler OnCharacterInit;

        internal void CharacterInited()
        {
            if(OnCharacterInit != null)
            {
                OnCharacterInit();
            }
        }
    }
}
