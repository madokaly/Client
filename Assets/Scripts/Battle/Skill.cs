using Common.Battle;
using Common.Data;
using Entities;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 技能类
    /// </summary>
    public class Skill
    {
        /// <summary>
        /// 协议技能信息
        /// </summary>
        public NSkillInfo Info;
        /// <summary>
        /// 所有者
        /// </summary>
        public Creature Owner;
        /// <summary>
        /// 数据库技能定义
        /// </summary>
        public SkillDefine Define;
        /// <summary>
        /// 目标
        /// </summary>
        public Creature Target { get; private set; }
        /// <summary>
        /// 目标位置
        /// </summary>
        public NVector3 TargetPosition;

        private float cd = 0;
        public float CD { get { return cd; } }
        /// <summary>
        /// 是否正在释放
        /// </summary>
        public bool IsCasting = false;
        /// <summary>
        /// 蓄力时间
        /// </summary>
        private float castTime = 0;
        /// <summary>
        /// 技能伤害时间
        /// </summary>
        private float skillTime = 0;
        /// <summary>
        /// 打击次数
        /// </summary>
        public int Hit = 0;
        /// <summary>
        /// 技能状态
        /// </summary>
        private SkillStatus Status;
        /// <summary>
        /// [打击,造成伤害]字典
        /// </summary>
        private Dictionary<int, List<NDamageInfo>> HitMap = new Dictionary<int, List<NDamageInfo>>();
        /// <summary>
        /// 技能的子弹列表
        /// </summary>
        List<Bullet> Bullets = new List<Bullet>();

        public Skill(NSkillInfo info, Creature owner)
        {
            this.Info = info;
            this.Owner = owner;
            this.Define = DataManager.Instance.Skills[(int)this.Owner.Define.TID][this.Info.Id];
            this.cd = 0;
        }
        /// <summary>
        /// 是否能释放
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public SkillResult CanCast(Creature target)
        {
            if(this.Define.CastTarget == TargetType.Target)
            {
                if(target == null || target == this.Owner)
                    return SkillResult.InvalidTarget;
                int distance = this.Owner.Distance(target);
                if(distance > this.Define.CastRange)
                {
                    return SkillResult.OutOfRange ;
                }
            }
            if(this.Define.CastTarget == TargetType.Position && BattleManager.Instance.CurrentPosition == null)
            {
                return SkillResult.InvalidTarget;
            }
            if(this.Owner.Attributes.MP < this.Define.MPCost)
            {
                return SkillResult.OutOfMp;
            }
            if(this.cd > 0)
            {
                return SkillResult.CoolDown;
            }
            return SkillResult.Ok;
        }
        /// <summary>
        /// 准备开始释放
        /// </summary>
        public void BeginCast(Creature target, NVector3 pos)
        {
            this.IsCasting = true;
            this.castTime = 0;
            this.skillTime = 0;
            this.Hit = 0;
            this.cd = this.Define.CD;
            this.Target = target;
            this.TargetPosition = pos;
            this.Owner.PlayAnim(this.Define.SkillAnim);
            this.Bullets.Clear();
            this.HitMap.Clear();

            if (this.Define.CastTarget == TargetType.Position)
            {
                this.Owner.FaceTo(this.TargetPosition.ToVector3Int());
            }
            else if(this.Define.CastTarget == TargetType.Target)
            {
                this.Owner.FaceTo(this.Target.position);
            }

            if (this.Define.CastTime > 0)
            {
                //蓄力状态
                this.Status = SkillStatus.Casting;
            }
            else
            {
                //持续状态
                this.StartSkill();
            }
        }
        /// <summary>
        /// 开始释放
        /// </summary>
        private void StartSkill()
        {
            this.Status = SkillStatus.Running;
            if (!string.IsNullOrEmpty(this.Define.AOEEffect))
            {
                if(this.Define.CastTarget == TargetType.Position)
                {
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.TargetPosition);
                }
                else if(this.Define.CastTarget == TargetType.Target)
                {
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Target);
                }
                else if(this.Define.CastTarget == TargetType.Self)
                {
                    this.Owner.PlayEffect(EffectType.Position, this.Define.AOEEffect, this.Owner);
                }
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        public void OnUpdate(float delta)
        {
            UpdateCD(delta);
            if (this.Status == SkillStatus.Casting)
            {
                this.UpdateCasting();
            }
            else if (this.Status == SkillStatus.Running)
            {
                this.UpdateSkill();
            }
        }
        /// <summary>
        /// 更新技能蓄力状态
        /// </summary>
        private void UpdateCasting()
        {
            if (this.castTime < this.Define.CastTime)
            {
                this.castTime += Time.deltaTime;
            }
            else
            {
                this.castTime = 0;
                //进入持续状态，开始释放
                this.StartSkill();
                Debug.LogFormat("Skill[{0}].UpdateCasting Finish", this.Define.Name);
            }
        }
        /// <summary>
        /// 更新技能持续状态
        /// </summary>
        private void UpdateSkill()
        {
            this.skillTime += Time.deltaTime;
            if (this.Define.Duration > 0)
            {
                //时间持续技能
                if (this.skillTime > this.Define.Interval * (this.Hit + 1))
                {
                    //达到造成伤害时间
                    this.DoHit();
                }
                if (this.skillTime >= this.Define.Duration)
                {
                    this.Status = SkillStatus.None;
                    this.IsCasting = false;
                    Debug.LogFormat("Skill[{0}].UpdateSkill Duration Finish", this.Define.Name);
                }
            }
            else if (this.Define.HitTimes != null && this.Define.HitTimes.Count > 0)
            {
                //次数持续技能
                if (this.Hit < this.Define.HitTimes.Count)
                {
                    if (this.skillTime > this.Define.HitTimes[this.Hit])
                    {
                        //达到造成伤害时间
                        this.DoHit();
                    }
                }
                else
                {
                    if (!this.Define.Bullet)
                    {
                        //不是子弹
                        this.Status = SkillStatus.None;
                        this.IsCasting = false;
                        Debug.LogFormat("Skill[{0}].UpdateSkill HitTimes Finish", this.Define.Name);
                    }
                }
            }
            if (this.Define.Bullet)
            {
                //子弹技能
                bool finish = true;
                foreach (var bullet in Bullets)
                {
                    bullet.Update();
                    if (!bullet.Stoped) finish = false;
                }
                if (finish && this.Hit > this.Define.HitTimes.Count)
                {
                    this.Status = SkillStatus.None;
                    Debug.LogFormat("Skill[{0}].UpdateSkill BulletHitTimes Finish", this.Define.Name);
                }
            }
        }
        /// <summary>
        /// 释放子弹
        /// </summary>
        private void CastBullet()
        {
            Bullet bullet = new Bullet(this);
            Debug.LogFormat("Skill[{0}].CastBullet[{1}] Target:{2}", this.Define.Name, this.Define.BulletResource, this.Target);
            this.Bullets.Add(bullet);
            this.Owner.PlayEffect(EffectType.Bullet, this.Define.BulletResource, this.Target, bullet.duration);
        }
        /// <summary>
        /// 客户端打击效果
        /// </summary>
        private void DoHit()
        {
            if(this.Define.Bullet)
            {
                this.CastBullet();
            }
            else
            {
                this.DoHitDamages(this.Hit);
            }
            this.Hit++;
        }
        /// <summary>
        /// 效验是否执行伤害
        /// </summary>
        /// <param name="hit"></param>
        internal void DoHit(NSkillHitInfo hit)
        {
            if (hit.isBullet || !this.Define.Bullet)
            {
                this.DoHit(hit.hitId, hit.Damages);
            }
        }
        /// <summary>
        /// 执行打击伤害
        /// </summary>
        /// <param name="hitId"></param>
        /// <param name="damages"></param>
        internal void DoHit(int hitId, List<NDamageInfo> damages)
        {
            if(hitId > this.Hit)
            {
                //服务端的hit包，比客户端执行到hit的时间还早
                this.HitMap[hitId] = damages;
            }
            else
            {
                //晚到直接产生伤害
                this.DoHitDamages(damages);
            }
        }
        /// <summary>
        /// 效验打击伤害是否早到
        /// </summary>
        /// <param name="hit"></param>
        public void DoHitDamages(int hit)
        {
            List<NDamageInfo> damages;
            if(this.HitMap.TryGetValue(hit, out damages))
            {
                //服务器的hit消息已经到了
                this.DoHitDamages(damages);
            }
        }
        /// <summary>
        /// 产生打击的伤害
        /// </summary>
        /// <param name="damages"></param>
        private void DoHitDamages(List<NDamageInfo> damages)
        {
            foreach(var damage in damages)
            {
                Creature target = EntityManager.Instance.GetEntity(damage.entityId) as Creature;
                if (target != null) target.DoDamage(damage, true);
                if(this.Define.HitEffect != null)
                {
                    target.PlayEffect(EffectType.Hit, this.Define.HitEffect, target);
                }
            }
        }
        /// <summary>
        /// 更新CD
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateCD(float delta)
        {
            if (this.cd > 0)
            {
                this.cd -= delta;
                this.cd = Mathf.Max(this.cd, 0);
            }
            else
            {
                this.cd = 0;
            }
        }
    }
}
