using Entities;
using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主UI
/// </summary>
public class UIMain : MonoSingleton<UIMain>
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text avatarName;
    /// <summary>
    /// 等级
    /// </summary>
    public Text avatarLevel;
    /// <summary>
    /// UI队伍
    /// </summary>
    public UITeam TeamWindow;
    /// <summary>
    /// 目标UI
    /// </summary>
    public UICreatureInfo targetUI;
    /// <summary>
    /// 技能槽列表UI
    /// </summary>
    public UISkillSolts skillSolts;
    /// <summary>
    /// Hp
    /// </summary>
    public Slider HpBar;
    /// <summary>
    /// Mp
    /// </summary>
    public Slider MpBar;
    /// <summary>
    /// Hp文本
    /// </summary>
    public Text HpText;
    /// <summary>
    /// Mp文本
    /// </summary>
    public Text MpText;
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show
    {
        get { return Show; }
        set
        {
            if (Show == value) return;
            Show = value;
            this.gameObject.SetActive(Show);
        }
    }

    protected override void OnStart()
    {
        this.Show = true;
        UpdateAvatar();
        UpdateUI();
        this.targetUI.gameObject.SetActive(false);
        //注册事件
        BattleManager.Instance.OnTargetChanged += OnTargetChanged;
        User.Instance.OnCharacterInit += this.skillSolts.UpdateSkills;
        //刷新技能槽
        this.skillSolts.UpdateSkills();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void OnTargetChanged(Creature target)
    {
        if(target != null && User.Instance.CurrentCharacter != target)
        {
            if (!this.targetUI.isActiveAndEnabled) targetUI.gameObject.SetActive(true);
            targetUI.Target = target;
        }
        else
        {
            this.targetUI.gameObject.SetActive(false);
        }
    }

    private void UpdateUI()
    {
        Creature user = User.Instance.CurrentCharacter;
        this.HpBar.maxValue = user.Attributes.MaxHP;
        this.HpBar.value = user.Attributes.HP;
        this.HpText.text = string.Format("{0}/{1}", user.Attributes.HP, user.Attributes.MaxHP);
        this.MpBar.maxValue = user.Attributes.MaxMP;
        this.MpBar.value = user.Attributes.MP;
        this.MpText.text = string.Format("{0}/{1}", user.Attributes.MP, user.Attributes.MaxMP);
    }
    /// <summary>
    /// 更新信息
    /// </summary>
    private void UpdateAvatar()
    {
        avatarName.text = string.Format("{0}[{1}]",User.Instance.CurrentCharacterInfo.Name,User.Instance.CurrentCharacterInfo.Id);
        avatarLevel.text = User.Instance.CurrentCharacterInfo.Level.ToString();
    }
    /// <summary>
    /// 点击背包
    /// </summary>
    public void OnclickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
    /// <summary>
    /// 点击角色
    /// </summary>
    public void OnclickCharEquip()
    {
        UIManager.Instance.Show<UICharEquip>();
    }
    /// <summary>
    /// 点击任务
    /// </summary>
    public void OnclickQuest()
    {
        UIManager.Instance.Show<UIQuestSystem>();
    }
    /// <summary>
    /// 点击好友
    /// </summary>
    public void OnclickFriend()
    {
        UIManager.Instance.Show<UIFriend>();
    }
    /// <summary>
    /// 显示队伍UI
    /// </summary>
    /// <param name="show"></param>
    public void ShowTeamUI(bool show)
    {
        TeamWindow.ShowTeam(show);
    }
    /// <summary>
    /// 点击公会
    /// </summary>
    public void OnclickGuild()
    {
        GuildManager.Instance.ShowGuild();
    }
    /// <summary>
    /// 点击坐骑
    /// </summary>
    public void OnclickRide()
    {
        UIManager.Instance.Show<UIRide>();
    }
    /// <summary>
    /// 点击设置
    /// </summary>
    public void OnclickSetting()
    {
        UIManager.Instance.Show<UISetting>();
    }
    /// <summary>
    /// 点击技能
    /// </summary>
    public void OnclickSkill()
    {
        UIManager.Instance.Show<UISkill>();
    }

    public void OnDestroy()
    {
        BattleManager.Instance.OnTargetChanged -= OnTargetChanged;
    }
}
