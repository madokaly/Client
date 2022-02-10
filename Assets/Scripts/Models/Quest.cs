using Common.Data;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    /// <summary>
    /// 任务模板类
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// 数据库任务类
        /// </summary>
        public QuestDefine Define;
        /// <summary>
        /// 协议任务类
        /// </summary>
        public NQuestInfo Info;

        public Quest()
        {

        }

        public Quest(NQuestInfo info)
        {
            this.Info = info;
            this.Define = DataManager.Instance.Quests[info.QuestId];
        }
        public Quest(QuestDefine define)
        {
            this.Define = define;
            this.Info = null;
        }

        public string GetTypeName()
        {
            return EnumUtil.GetEnumDescription(this.Define.Type);
        }
    }
}
