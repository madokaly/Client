using Battle;
using Entities;
using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 战斗管理器
    /// </summary>
    public class BattleManager : Singleton<BattleManager>
    {
        /// <summary>
        /// 声明委托
        /// </summary>
        /// <param name="target"></param>
        public delegate void TargetChangedHandler(Creature target);
        /// <summary>
        /// 定义目标改变事件
        /// </summary>
        public event TargetChangedHandler OnTargetChanged;

        /// <summary>
        /// 当前目标
        /// </summary>
        private Creature currentTarget; 
        public Creature CurrentTarget
        {
            get { return currentTarget; }
            set{ this.SetTarget(value); }
        }
        /// <summary>
        /// 当前释放坐标
        /// </summary>
        private NVector3 currentPosition;
        public NVector3 CurrentPosition
        {
            get { return currentPosition; }
            set { this.SetPosition(value); }
        }

        public void Init()
        {

        }
        /// <summary>
        /// 设置目标
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(Creature target)
        {
            if(this.CurrentTarget != target && this.OnTargetChanged != null)
            {
                //调用事件
                this.OnTargetChanged(target);
            }
            this.currentTarget = target;
            Debug.LogFormat("BattleManager.SetTarget:[{0}:{1}]", target.entityId, target.Name);
        }

        public void SetPosition(NVector3 position)
        {
            this.currentPosition = position;
            Debug.LogFormat("BattleManager.SetPosition:[{0}]", position);
        }
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skill"></param>
        public void CastSkill(Skill skill)
        {
            int target = currentTarget != null ? currentTarget.entityId : 0;
            BattleService.Instance.SendSkillCast(skill.Define.ID, skill.Owner.entityId, target, currentPosition);
        }
    }
}
