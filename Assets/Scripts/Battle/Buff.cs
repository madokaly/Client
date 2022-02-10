using Common.Battle;
using Common.Data;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// Buff类
    /// </summary>
    public class Buff
    {
        /// <summary>
        /// 所有者
        /// </summary>
        private Creature Owner;
        /// <summary>
        /// buff唯一id
        /// </summary>
        public int BuffId;
        /// <summary>
        /// 数据buff信息
        /// </summary>
        public BuffDefine Define;
        /// <summary>
        /// 释放者id
        /// </summary>
        private int CasterId;
        /// <summary>
        /// 持续的时间
        /// </summary>
        public float time;
        /// <summary>
        /// 是否停止
        /// </summary>
        public bool Stoped;

        public Buff(Creature owner, int buffId, BuffDefine define, int casterId)
        {
            this.Owner = owner;
            this.BuffId = buffId;
            this.Define = define;
            this.CasterId = casterId;
            this.OnAdd();
        }
        /// <summary>
        /// 添加时
        /// </summary>
        private void OnAdd()
        {
            Debug.LogFormat("Buff[{0}:{1}]OnAdd", this.BuffId, this.Define.Name);
            if (this.Define.Effect != BuffEffect.None)
            {
                this.Owner.AddBuffEffect(this.Define.Effect);
            }
            AddAttr();
        }
        /// <summary>
        /// 移除时
        /// </summary>
        internal void OnRemove()
        {
            Debug.LogFormat("Buff[{0}:{1}]OnRemove", this.BuffId, this.Define.Name);
            RemoveAttr();
            this.Stoped = true;
            if (this.Define.Effect != BuffEffect.None)
            {
                this.Owner.RemoveBuffEffect(this.Define.Effect);
            }
        }
        /// <summary>
        /// 增加属性
        /// </summary>
        private void AddAttr()
        {
            if (this.Define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF += this.Owner.Attributes.Basic.DEF * this.Define.DEFRatio;
                this.Owner.Attributes.InitFinalAttributes();
            }
        }
        /// <summary>
        /// 移除属性
        /// </summary>
        private void RemoveAttr()
        {
            if (this.Define.DEFRatio != 0)
            {
                this.Owner.Attributes.Buff.DEF -= this.Owner.Attributes.Basic.DEF * this.Define.DEFRatio;
                this.Owner.Attributes.InitFinalAttributes();
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        internal void OnUpdate(float delta)
        {
            if (Stoped) return;
            this.time += delta;
            if(this.time > this.Define.Duration)
            {
                this.OnRemove();
            }
        }
    }
}
