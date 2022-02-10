using Common.Battle;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// Buff影响管理器
    /// </summary>
    public class EffectManager
    {
        /// <summary>
        /// 所有者
        /// </summary>
        private Creature Owner;
        /// <summary>
        /// [影响类型，次数]字典
        /// </summary>
        private Dictionary<BuffEffect, int> Effects = new Dictionary<BuffEffect, int>();

        public EffectManager(Creature owner)
        {
            this.Owner = owner;
        }
        /// <summary>
        /// 是否有指定影响类型
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        internal bool HasEffect(BuffEffect effect)
        {
            int val;
            if (this.Effects.TryGetValue(effect, out val))
            {
                return val > 0;
            }
            return false;
        }
        /// <summary>
        /// 添加buff影响
        /// </summary>
        /// <param name="effect"></param>
        internal void AddEffect(BuffEffect effect)
        {
            Debug.LogFormat("[{0}.AddEffect {1}]", this.Owner.Name, effect);
            if (!this.Effects.ContainsKey(effect))
            {
                this.Effects[effect] = 1;
            }
            else
            {
                this.Effects[effect]++;
            }
        }
        /// <summary>
        /// 移除buff影响
        /// </summary>
        /// <param name="effect"></param>
        internal void RemoveEffect(BuffEffect effect)
        {
            Debug.LogFormat("[{0}.RemoveEffect {1}]", this.Owner.Name, effect);
            if (this.Effects[effect] > 0)
            {
                this.Effects[effect]--;
            }
        }
    }
}
