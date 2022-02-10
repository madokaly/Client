using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkillBridge.Message;
using UnityEngine;
using Common.Data;
using Managers;
using Common.Battle;
using Battle;

namespace Entities
{
    /// <summary>
    /// 生物类
    /// </summary>
    public class Creature : Entity
    {

        public Action<Buff> OnBuffAdd;
        public Action<Buff> OnBuffRemove;

        /// <summary>
        /// 协议玩家类
        /// </summary>
        public NCharacterInfo Info;
        /// <summary>
        /// 数据库基本信息
        /// </summary>
        public CharacterDefine Define;
        /// <summary>
        /// 属性
        /// </summary>
        public Attributes Attributes;
        /// <summary>
        /// 技能管理器
        /// </summary>
        public SkillManager SkillMgr;
        /// <summary>
        /// Buff管理器
        /// </summary>
        public BuffManager BuffMgr;
        /// <summary>
        /// 影响管理器
        /// </summary>
        public EffectManager EffectMgr;

        public int Id
        {
            get { return this.Info.Id; }
        }

        public string Name
        {
            get
            {
                if (this.Info.Type == CharacterType.Player)
                    return this.Info.Name;
                else
                    return this.Define.Name;
            }
        }
        /// <summary>
        /// 是否是玩家
        /// </summary>
        public bool IsPlayer
        {
            get
            {
                return this.Info.Type == CharacterType.Player;
            }
        }
        /// <summary>
        /// 是否是当前玩家
        /// </summary>
        public bool IsCurrentPlayer
        {
            get
            {
                if (!IsPlayer) return false;
                return this.Info.Id == Models.User.Instance.CurrentCharacterInfo.Id;
            }
        }
        /// <summary>
        /// 战斗状态
        /// </summary>
        private bool battleState = false;
        public bool BattleState
        {
            get { return battleState; }
            set
            {
                if(battleState != value)
                {
                    battleState = value;
                    this.SetStandby(battleState);
                }
            }
        }

        public Skill CastringSkill = null;

        public Creature(NCharacterInfo info) : base(info.Entity)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Characters[info.ConfigId];
            this.Attributes = new Attributes();
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr = new SkillManager(this);
            this.BuffMgr = new BuffManager(this);
            this.EffectMgr = new EffectManager(this);
        }
        /// <summary>
        /// 更新角色信息
        /// </summary>
        /// <param name="info"></param>
        public void UpdateInfo(NCharacterInfo info)
        {
            this.SetEntityData(info.Entity);
            this.Info = info;
            this.Attributes.Init(this.Define, this.Info.Level, this.GetEquips(), this.Info.attrDynamic);
            this.SkillMgr.UpdateSkills();
        }
        /// <summary>
        /// 获取装备列表虚函数
        /// </summary>
        /// <returns></returns>
        public virtual List<EquipDefine> GetEquips()
        {
            return null;
        }

        public void MoveForward()
        {
            Debug.LogFormat("MoveForward");
            this.speed = this.Define.Speed;
        }

        public void MoveBack()
        {
            Debug.LogFormat("MoveBack");
            this.speed = -this.Define.Speed;
        }

        public void Stop()
        {
            Debug.LogFormat("Stop");
            this.speed = 0;
        }

        public void SetDirection(Vector3Int direction)
        {
            Debug.LogFormat("SetDirection:{0}", direction);
            this.direction = direction;
        }

        public void SetPosition(Vector3Int position)
        {
            Debug.LogFormat("SetPosition:{0}", position);
            this.position = position;
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="damage"></param>
        public void CastSkill(int skillId, Creature target, NVector3 position)
        {
            this.SetStandby(true);
            var skill = this.SkillMgr.GetSkill(skillId);
            skill.BeginCast(target, position);
        }
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="name"></param>
        public void PlayAnim(string name)
        {
            if(this.Controller != null)
            {
                this.Controller.PlayAnim(name);
            }
        }
        /// <summary>
        /// 设置目标
        /// </summary>
        /// <param name="standby"></param>
        public void SetStandby(bool standby)
        {
            if (this.Controller != null)
            {
                this.Controller.SetStandby(standby);
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        public override void OnUpdate(float delta)
        {
            base.OnUpdate(delta);
            this.SkillMgr.OnUpdate(delta);
            this.BuffMgr.OnUpdate(delta);
        }
        /// <summary>
        /// 受到伤害
        /// </summary>
        /// <param name="damage"></param>
        public void DoDamage(NDamageInfo damage, bool playHurt)
        {
            Debug.LogFormat("DoDamage: {0}", damage);
            this.Attributes.HP -= damage.Damage;
            if(playHurt) this.PlayAnim("Hurt");
            if(this.Controller != null)
            {
                UIWorldElementManager.Instance.ShowPopupText(PopupType.Damage, this.Controller.GetTransform().position + this.GetPopupOffset(), -damage.Damage, damage.Crit);
            }
        }
        /// <summary>
        /// 造成技能打击伤害
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="hitId"></param>
        /// <param name="damages"></param>
        internal void DoSkillHit(NSkillHitInfo hitInfo)
        {
            Skill skill = this.SkillMgr.GetSkill(hitInfo.skillId);
            if (skill != null) skill.DoHit(hitInfo);
        }
        /// <summary>
        /// 处理buff信息
        /// </summary>
        /// <param name="buff"></param>
        internal void DoBuffAction(NBuffInfo buff)
        {
            switch (buff.Action)
            {
                case BuffAction.Add:
                    this.AddBuff(buff.buffId, buff.buffType, buff.casterId);
                    break;
                case BuffAction.Remove:
                    this.RemoveBuff(buff.buffId);
                    break;
                case BuffAction.Hit:
                    this.DoDamage(buff.Damage, false);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 增加buff
        /// </summary>
        /// <param name="buffId"></param>
        /// <param name="buffType"></param>
        /// <param name="casterId"></param>
        private void AddBuff(int buffId, int buffType, int casterId)
        {
            Buff buff = this.BuffMgr.AddBuff(buffId, buffType, casterId);
            if(buff != null && this.OnBuffAdd != null)
            {
                this.OnBuffAdd(buff);
            }
        }

        /// <summary>
        /// 移除buff
        /// </summary>
        /// <param name="buffId"></param>
        public void RemoveBuff(int buffId)
        {
            Buff buff = this.BuffMgr.RemoveBuff(buffId);
            if (buff != null && this.OnBuffRemove != null)
            {
                this.OnBuffRemove(buff);
            }
        }

        /// <summary>
        /// 添加技能影响
        /// </summary>
        /// <param name="effect"></param>
        internal void AddBuffEffect(BuffEffect effect)
        {
            this.EffectMgr.AddEffect(effect);
        }

        /// <summary>
        /// 移除技能影响
        /// </summary>
        /// <param name="effect"></param>
        internal void RemoveBuffEffect(BuffEffect effect)
        {
            this.EffectMgr.RemoveEffect(effect);
        }

        internal int Distance(Creature target)
        {
            return (int)Vector3Int.Distance(this.position, target.position);
        }
        /// <summary>
        /// 面朝
        /// </summary>
        /// <param name="pos"></param>
        internal void FaceTo(Vector3Int pos)
        {
            this.SetDirection(GameObjectTool.WorldToLogic(GameObjectTool.LogicToWorld(pos - this.position).normalized));
            //更新协议实体信息
            this.UpdateEntityData();
            if(this.Controller != null)
            {
                this.Controller.UpdateDirection();
            }
        }

        /// <summary>
        /// 产生特效
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        public void PlayEffect(EffectType type, string name, Creature target, float duration = 0)
        {
            if (string.IsNullOrEmpty(name)) return;
            if(this.Controller != null)
            {
                this.Controller.PlayEffect(type, name, target, duration);
            }
        }
        public void PlayEffect(EffectType type, string name, NVector3 position)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (this.Controller != null)
            {
                this.Controller.PlayEffect(type, name, position, 0);
            }
        }
        /// <summary>
        /// 击中点偏移量
        /// </summary>
        /// <returns></returns>
        public Vector3 GetHitOffset()
        {
            return new Vector3(0, this.Define.Height * 0.8f, 0);
        }
        /// <summary>
        /// 伤害飘字偏移量
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPopupOffset()
        {
            return new Vector3(0, this.Define.Height, 0);
        }
    }
}
