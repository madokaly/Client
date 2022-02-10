using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 子弹类
    /// </summary>
    public class Bullet
    {
        /// <summary>
        /// 所属技能
        /// </summary>
        private Skill skill;
        /// <summary>
        /// 打击点
        /// </summary>
        private int hit = 0;
        /// <summary>
        /// 飞行时间
        /// </summary>
        private float flyTime = 0;
        /// <summary>
        /// 持续时间
        /// </summary>
        public float duration = 0;
        /// <summary>
        /// 是否停止
        /// </summary>
        public bool Stoped = false;

        public Bullet(Skill skill)
        {
            this.skill = skill;
            var target = skill.Target;
            this.hit = skill.Hit;
            int distance = skill.Owner.Distance(target);
            duration = distance / this.skill.Define.BulletSpeed;
        }
        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            if (this.Stoped) return;
            this.flyTime += Time.deltaTime;
            if(this.flyTime > duration)
            {
                this.skill.DoHitDamages(this.hit);
                this.Stop();
            }
        }

        internal void Stop()
        {
            this.Stoped = true;
        }
    }
}
