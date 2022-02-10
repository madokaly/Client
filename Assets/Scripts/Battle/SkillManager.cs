using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    /// <summary>
    /// 技能管理器
    /// </summary>
    public class SkillManager
    {
        /// <summary>
        /// 拥有者
        /// </summary>
        private Creature owner;
        /// <summary>
        /// 技能列表
        /// </summary>
        public List<Skill> Skills { get; private set; }

        public SkillManager(Creature owner)
        {
            this.owner = owner;
            this.Skills = new List<Skill>();
            this.InitSkills();
        }
        /// <summary>
        /// 初始化技能列表
        /// </summary>
        private void InitSkills()
        {
            this.Skills.Clear();
            foreach (var skillInfo in this.owner.Info.Skills)
            {
                Skill skill = new Skill(skillInfo, this.owner);
                this.AddSkill(skill);
            }
        }
        /// <summary>
        /// 更新技能列表
        /// </summary>
        public void UpdateSkills()
        {
            foreach (var skillInfo in this.owner.Info.Skills)
            {
                Skill skill = this.GetSkill(skillInfo.Id);
                if (skill != null)
                {
                    skill.Info = skillInfo;
                }
                else
                {
                    this.AddSkill(skill);
                }
            }
        }
        /// <summary>
        /// 添加技能
        /// </summary>
        /// <param name="skill"></param>
        private void AddSkill(Skill skill)
        {
            this.Skills.Add(skill);
        }
        /// <summary>
        /// 获得指定技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public Skill GetSkill(int skillId)
        {
            for (int i = 0; i < this.Skills.Count; i++)
            {
                if (this.Skills[i].Define.ID == skillId)
                {
                    return this.Skills[i];
                }
            }
            return null;
        }

        public void OnUpdate(float delta)
        {
            for (int i = 0; i < this.Skills.Count; i++)
            {
                this.Skills[i].OnUpdate(delta);
            }
        }
    }
}
