using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None = 0,//无任务
        Complete,//拥有已完成可提交任务
        Available,//拥有可接受任务
        Incomplete,//拥有未完成任务
    }

    class QuestManager : Singleton<QuestManager>
    {
        /// <summary>
        /// 已接受任务
        /// </summary>
        public List<NQuestInfo> questInfos;
        /// <summary>
        /// 所有任务
        /// </summary>
        public Dictionary<int, Quest> allQuests = new Dictionary<int, Quest>();
        /// <summary>
        /// int NPCID
        /// Status 任务可接状态
        /// </summary>
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> npcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();
        /// <summary>
        /// 任务状态变化事件
        /// </summary>
        public UnityAction<Quest> onQuestStatusChanged;


        public void Init(List<NQuestInfo> quests)
        {
            this.questInfos = quests;
            allQuests.Clear();
            this.npcQuests.Clear();
            InitQuests();
        }

        private void InitQuests()
        {
            //初始化已有任务
            foreach (var info in this.questInfos)
            {
                Quest quest = new Quest(info);
                this.allQuests[quest.Info.QuestId] = quest;
            }

            this.CheckAvailableQuests();

            foreach (var kv in this.allQuests)
            {
                //为对应Npc添加接受任务
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                //为对应Npc添加提交任务
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }
        }
        //初始化可用任务
        private void CheckAvailableQuests()
        {

            foreach (var kv in DataManager.Instance.Quests)
            {
                //不符合职业
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacterInfo.Class)
                    continue;
                //不符合等级
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacterInfo.Level)
                    continue;
                //任务已经存在
                if (this.allQuests.ContainsKey(kv.Key))
                    continue;
                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    //获取前置任务
                    if (this.allQuests.TryGetValue(kv.Value.PreQuest, out preQuest))
                    {
                        //前置任务未获取
                        if (preQuest.Info == null)
                            continue;
                        //前置任务未完成
                        if (preQuest.Info.Status != QuestStatus.Finished)
                            continue;
                    }
                    else
                        //前置任务没有接
                        continue;
                }
                Quest quest = new Quest(kv.Value);
                this.allQuests[quest.Define.ID] = quest;
            }
        }
        /// <summary>
        /// 添加Npc任务
        /// </summary>
        /// <param name="npcId"></param>
        /// <param name="quest"></param>
        private void AddNpcQuest(int npcId, Quest quest)
        {
            if (!this.npcQuests.ContainsKey(npcId))
            {
                this.npcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }
            List<Quest> availables;
            List<Quest> complates;
            List<Quest> incomplates;

            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Available] = availables;
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out complates))
            {
                complates = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Complete] = complates;
            }
            if (!this.npcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incomplates))
            {
                incomplates = new List<Quest>();
                this.npcQuests[npcId][NpcQuestStatus.Incomplete] = incomplates;
            }

            if (quest.Info == null)
            {
                if (npcId == quest.Define.AcceptNPC && !this.npcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    this.npcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.Complated)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                    {
                        this.npcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                    }
                }
                if (quest.Define.SubmitNPC == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!this.npcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        this.npcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }

        }
        /// <summary>
        /// 获得Npc任务状态
        /// </summary>
        /// <param name="npcId">Npc编号</param>
        /// <returns></returns>
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            //获取Npc任务
            if (this.npcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return NpcQuestStatus.Complete;
                if (status[NpcQuestStatus.Available].Count > 0)
                    return NpcQuestStatus.Available;
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return NpcQuestStatus.Incomplete;
            }
            return NpcQuestStatus.None;
        }
        /// <summary>
        /// 是否打开Npc任务对话框
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (npcQuests.TryGetValue(npcId, out status))//获取Npc任务
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                if (status[NpcQuestStatus.Available].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
            }
            return false;
        }
        /// <summary>
        /// 打开Npc任务对话框
        /// </summary>
        /// <param name="quest">任务</param>
        /// <returns></returns>
        private bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Complated)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                //注册对话框关闭事件
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.Complated)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete)) /*空语句 ;*/
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;

        }
        /// <summary>
        /// 响应对话框关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="result"></param>
        private void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                //接受
                if (dlg.quest.Info == null)
                {
                    QuestService.Instance.SendQuestAccept(dlg.quest);
                }
                //提交
                else if (dlg.quest.Info.Status == QuestStatus.Complated)
                {
                    QuestService.Instance.SendQuestSubmit(dlg.quest);
                }
            }
            //关闭
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.quest.Define.DialogDeny);
            }
        }
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="quest"></param>
        /// <returns></returns>
        Quest RefreshQuestStatus(NQuestInfo quest)
        {
            this.npcQuests.Clear();
            Quest result;
            if (this.allQuests.ContainsKey(quest.QuestId))
            {
                //更新新的任务状态
                this.allQuests[quest.QuestId].Info = quest;
                result = this.allQuests[quest.QuestId];
            }
            else
            {
                result = new Quest(quest);
                this.allQuests[quest.QuestId] = result;
            }
            CheckAvailableQuests();
            foreach (var kv in this.allQuests)
            {
                this.AddNpcQuest(kv.Value.Define.AcceptNPC, kv.Value);
                this.AddNpcQuest(kv.Value.Define.SubmitNPC, kv.Value);
            }

            if (onQuestStatusChanged != null)
            {
                onQuestStatusChanged(result);
            }
            return result;
        }
        /// <summary>
        /// 响应任务接受事件
        /// </summary>
        /// <param name="info"></param>
        public void OnQuestAccepted(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogAccept);
        }
        /// <summary>
        /// 响应任务提交事件
        /// </summary>
        /// <param name="info"></param>
        public void OnQuestSubmited(NQuestInfo info)
        {
            var quest = this.RefreshQuestStatus(info);
            MessageBox.Show(quest.Define.DialogFinish);
        }
    }
}

