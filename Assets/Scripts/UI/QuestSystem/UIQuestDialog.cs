using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI任务对话框
/// </summary>
public class UIQuestDialog : UIWindow
{
    /// <summary>
    /// 任务面板
    /// </summary>
    public UIQuestInfo questInfo;
    /// <summary>
    /// 任务
    /// </summary>
    public Quest quest;
    /// <summary>
    /// 接受拒绝按钮
    /// </summary>
    public GameObject openButtons;
    /// <summary>
    /// 提交按钮
    /// </summary>
    public GameObject submitButtons;

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
        this.UpdateQuest();
        //未接受过
        if (this.quest.Info == null)
        {
            openButtons.SetActive(true);
            submitButtons.SetActive(false);
        }
        else
        {
            //接受过
            if (this.quest.Info.Status == QuestStatus.Complated)
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(true);
            }
            else
            {
                openButtons.SetActive(false);
                submitButtons.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 更新任务面板
    /// </summary>
    private void UpdateQuest()
    {
        if (this.questInfo != null)
        {
            this.questInfo.SetQuestInfo(quest);
        }
    }
}
