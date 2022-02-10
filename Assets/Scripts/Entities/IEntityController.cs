using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Entities
{
    public interface IEntityController
    {
        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="name"></param>
        void PlayAnim(string name);
        /// <summary>
        /// 设置战斗状态
        /// </summary>
        /// <param name="standby"></param>
        void SetStandby(bool standby);
        /// <summary>
        /// 更新方向
        /// </summary>
        void UpdateDirection();
        /// <summary>
        /// 产生特效
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        void PlayEffect(EffectType type, string name, Creature target, float duration);
        void PlayEffect(EffectType type, string name, NVector3 position, float duration);
        /// <summary>
        /// 获得位置信息
        /// </summary>
        /// <returns></returns>
        Transform GetTransform();
    }
}
