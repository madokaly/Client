using Common.Data;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// Buff管理器
    /// </summary>
    public class BuffManager
    {
        /// <summary>
        /// 所有者
        /// </summary>
        private Creature Owner;
        /// <summary>
        /// [buffId,buff]字典
        /// </summary>
        public Dictionary<int, Buff> Buffs = new Dictionary<int, Buff>();

        public BuffManager(Creature owner)
        {
            this.Owner = owner;
        }
        /// <summary>
        /// 添加Buff
        /// </summary>
        /// <param name="buffId"></param>
        /// <param name="buffType"></param>
        /// <param name="casterId"></param>
        internal Buff AddBuff(int buffId, int buffType, int casterId)
        {
            BuffDefine define;
            if(DataManager.Instance.Buffs.TryGetValue(buffType, out define))
            {
                Buff buff = new Buff(this.Owner, buffId, define, casterId);
                this.Buffs[buffId] = buff;
                return buff;
            }
            return null;
        }
        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="buffId"></param>
        internal Buff RemoveBuff(int buffId)
        {
            Buff buff;
            if(this.Buffs.TryGetValue(buffId, out buff))
            {
                buff.OnRemove();
                this.Buffs.Remove(buffId);
                return buff;
            }
            return null;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="delta"></param>
        internal void OnUpdate(float delta)
        {
            List<int> needRemove = new List<int>();
            foreach(var kv in Buffs)
            {
                kv.Value.OnUpdate(delta);
                if (kv.Value.Stoped)
                {
                    needRemove.Add(kv.Key);
                }
            }
            foreach(int buffId in needRemove)
            {
                this.Owner.RemoveBuff(buffId);
            }
        }
    }
}
