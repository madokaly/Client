using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class NpcManager : Singleton<NpcManager>
    {
        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public delegate bool NpcActionHandler(NpcDefine npc);
        /// <summary>
        /// Npc委托字典
        /// </summary>
        Dictionary<NpcFunction, NpcActionHandler> eventMap = new Dictionary<NpcFunction, NpcActionHandler>();
        /// <summary>
        /// npc位置字典
        /// </summary>
        Dictionary<int, Vector3> npcPositions = new Dictionary<int, Vector3>();
        /// <summary>
        /// 注册委托
        /// </summary>
        /// <param name="function"></param>
        /// <param name="action"></param>
        public void RegisterNpcEvent(NpcFunction function, NpcActionHandler action)
        {
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
                eventMap[function] += action;
        }

        /// <summary>
        /// 获得Npc数据信息
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public NpcDefine GetNpcDefine(int npcId)
        {
            NpcDefine npc = null;
            DataManager.Instance.Npcs.TryGetValue(npcId, out npc);
            return npc;
        }
        /// <summary>
        /// 判断Npc是否合法
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public bool Interactive(int npcId)
        {
            if (DataManager.Instance.Npcs.ContainsKey(npcId))
            {
                var npc = DataManager.Instance.Npcs[npcId];
                return Interactive(npc);
            }
            return false;
        }
        /// <summary>
        /// 交互Npc
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public bool Interactive(NpcDefine npc)
        {
            if (DoTaskInteractive(npc))
            {
                return true;
            }
            else if (npc.Type == NpcType.Functional)
            {
                return DoFunctionInteractive(npc);
            }
            return false;
        }

        /// <summary>
        /// 任务交互
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private bool DoTaskInteractive(NpcDefine npc)
        {
            var status = QuestManager.Instance.GetQuestStatusByNpc(npc.ID);
            if (status == NpcQuestStatus.None)
            {
                return false;
            }
            return QuestManager.Instance.OpenNpcQuest(npc.ID);
        }
        /// <summary>
        /// 功能交互
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        private bool DoFunctionInteractive(NpcDefine npc)
        {
            if (npc.Type != NpcType.Functional)
            {
                return false;
            }
            if (!eventMap.ContainsKey(npc.Function))
            {
                return false;
            }
            return eventMap[npc.Function](npc);
        }
        /// <summary>
        /// 设置npc坐标
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="position"></param>
        internal void UpdateNpcPostion(int npc, Vector3 position)
        {
            this.npcPositions[npc] = position;
        }
        internal Vector3 GetNpcPosition(int npc)
        {
            return this.npcPositions[npc];
        }
    }
}
