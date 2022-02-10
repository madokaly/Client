using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Npc任务状态UI
/// </summary>
public class UIQuestStatus : MonoBehaviour
{
    /// <summary>
    /// 任务状态图
    /// </summary>
    public Image[] statusImages;
    /// <summary>
    /// 任务状态
    /// </summary>
    private NpcQuestStatus questStatus;
    /// <summary>
    /// 设置任务状态
    /// </summary>
    /// <param name="status"></param>
    public void SetQuestStatus(NpcQuestStatus status)
    {
        this.questStatus = status;
        for (int i = 0; i < 4; i++)
        {
            if (this.statusImages[i] != null)
            {
                this.statusImages[i].gameObject.SetActive(i == (int)questStatus);
            }

        }
    }
}
