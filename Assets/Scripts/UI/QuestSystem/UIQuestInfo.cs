using Common.Data;
using Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI任务面板
/// </summary>
public class UIQuestInfo : MonoBehaviour
{
    /// <summary>
    /// 任务名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 目标
    /// </summary>
    public Text[] targets;
    /// <summary>
    /// 任务描述
    /// </summary>
    public Text description;
    /// <summary>
    /// 任务预览
    /// </summary>
    public Text overview;
    /// <summary>
    /// 奖励预制件
    /// </summary>
    public GameObject rewardPrefab;
    /// <summary>
    /// 奖励数组
    /// </summary>
    public Transform[] rewards;
    /// <summary>
    /// 任务信息
    /// </summary>
    private ItemDefine item;
    /// <summary>
    /// 奖励金钱
    /// </summary>
    public Text rewardMoney;
    /// <summary>
    /// 奖励经验
    /// </summary>
    public Text rewardExp;

    public Button navButton;

    private int npc = 0;

    private void Start()
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            rewards[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 设置任务面板
    /// </summary>
    /// <param name="quest"></param>
    public void SetQuestInfo(Quest quest)
    {
        this.title.text = string.Format("[{0}]{1}",quest.Define.Type,quest.Define.Name);
        if (this.overview != null) this.overview.text = quest.Define.OverView;
        if (this.description != null)
        {
            if (quest.Info == null)
            {
                this.description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == QuestStatus.Complated)
                {
                    this.description.text = quest.Define.DialogFinish;
                }
            }

            if (quest.Define.RewardItem1 > 0)
            {
                rewards[0].gameObject.SetActive(true);
                GameObject go = Instantiate(rewardPrefab, rewards[0]);
                var ui = go.GetComponent<UIIconItem>();
                item = DataManager.Instance.Items[quest.Define.RewardItem1];
                ui.setMainIcon(item.Icon, quest.Define.RewardItem1Count.ToString());
            }
            if (quest.Define.RewardItem2 > 0)
            {
                rewards[1].gameObject.SetActive(true);
                GameObject go = Instantiate(rewardPrefab, rewards[1]);
                var ui = go.GetComponent<UIIconItem>();
                item = DataManager.Instance.Items[quest.Define.RewardItem2];
                ui.setMainIcon(item.Icon, quest.Define.RewardItem1Count.ToString());
            }
            if (quest.Define.RewardItem3 > 0)
            {
                rewards[2].gameObject.SetActive(true);
                GameObject go = Instantiate(rewardPrefab, rewards[2]);
                var ui = go.GetComponent<UIIconItem>();
                item = DataManager.Instance.Items[quest.Define.RewardItem3];
                ui.setMainIcon(item.Icon, quest.Define.RewardItem1Count.ToString());
            }
        }

        this.rewardMoney.text = quest.Define.RewardGold.ToString();
        this.rewardExp.text = quest.Define.RewardExp.ToString();

        if(quest.Info == null)
        {
            //接取任务npc
            this.npc = quest.Define.AcceptNPC;
        }else if(quest.Info.Status == QuestStatus.Complated)
        {
            //提交任务npc
            this.npc = quest.Define.SubmitNPC;
        }
        this.navButton.gameObject.SetActive(this.npc > 0);
        foreach (var fitter in GetComponentsInChildren<ContentSizeFitter>())
        {
            //重新布局ContentSizeFitter
            fitter.SetLayoutVertical();
        }
    }

    public void OnClickAbandon()
    {

    }

    public void OnclickNav()
    {
        Vector3 pos = NpcManager.Instance.GetNpcPosition(this.npc);
        User.Instance.CurrentCharacterObject.StartNav(pos);
        UIManager.Instance.Close<UIQuestSystem>();
    }

}
