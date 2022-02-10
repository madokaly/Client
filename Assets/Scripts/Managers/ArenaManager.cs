using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 竞技场管理器
    /// </summary>
    class ArenaManager : Singleton<ArenaManager>
    {
        /// <summary>
        /// 回合数
        /// </summary>
        public int Round = 0;
        /// <summary>
        /// 竞技信息
        /// </summary>
        private ArenaInfo ArenaInfo;

        public ArenaManager()
        {

        }

        /// <summary>
        /// 进入竞技场
        /// </summary>
        /// <param name="arenaInfo"></param>
        public void EnterArena(ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.EnterArena : {0}", arenaInfo.ArenaId);
            this.ArenaInfo = arenaInfo;
            InputManager.Instance.IsInputMode = true;
        }

        /// <summary>
        /// 离开竞技场
        /// </summary>
        /// <param name="arenaInfo"></param>
        public void ExitArena(ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.ExitArena : {0}", arenaInfo.ArenaId);
            this.ArenaInfo = null;
        }

        /// <summary>
        /// 向服务端发送准备好
        /// </summary>
        internal void SendReady()
        {
            Debug.LogFormat("ArenaManager.SendReady : {0}", this.ArenaInfo.ArenaId);
            ArenaService.Instance.SendArenaReadyRequest(this.ArenaInfo.ArenaId);
        }

        /// <summary>
        /// 准备完成时
        /// </summary>
        /// <param name="round"></param>
        /// <param name="arenaInfo"></param>
        internal void OnReady(int round, ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.OnReady : {0} Round : {1}", arenaInfo.ArenaId, round);
            this.Round = round;
            if(UIArena.Instance != null)
            {
                UIArena.Instance.ShowCountDown();
            }
        }

        /// <summary>
        /// 回合开始时
        /// </summary>
        /// <param name="round"></param>
        /// <param name="arenaInfo"></param>
        internal void OnRoundStart(int round, ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.OnRoundStart : {0} Round : {1}", arenaInfo.ArenaId, round);
            if (UIArena.Instance != null)
            {
                UIArena.Instance.ShowRoundStart(round, arenaInfo);
            }
            InputManager.Instance.IsInputMode = false;
        }

        /// <summary>
        /// 回合结束时
        /// </summary>
        /// <param name="round"></param>
        /// <param name="arenaInfo"></param>
        internal void OnRoundEnd(int round, ArenaInfo arenaInfo)
        {
            Debug.LogFormat("ArenaManager.OnRoundEnd : {0} Round : {1}", arenaInfo.ArenaId, round);
            if(UIArena.Instance != null)
            {
                UIArena.Instance.ShowRoundResult(round, arenaInfo);
            }
        }
    }
}
