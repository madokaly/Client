using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Managers;
using SkillBridge.Message;

namespace Entities
{
    public class Character : Creature
    {
        public Character(NCharacterInfo info) : base(info)
        {
        }

        /// <summary>
        /// 重写虚函数
        /// </summary>
        /// <returns></returns>
        public override List<EquipDefine> GetEquips()
        {
            return EquipManager.Instance.GetEquipDefines();
        }
    }
}
