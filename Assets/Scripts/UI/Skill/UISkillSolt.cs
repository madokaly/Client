using Battle;
using Common.Battle;
using Common.Data;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 技能soltUI
/// </summary>
public class UISkillSolt : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 图标
    /// </summary>
    public Image icon;
    /// <summary>
    /// 遮罩
    /// </summary>
    public Image overlay;
    /// <summary>
    /// cd文本
    /// </summary>
    public Text cdText;
    /// <summary>
    /// 技能
    /// </summary>
    private Skill skill;

    private void Start()
    {
        overlay.enabled = false;
        cdText.enabled = false;
    }

    private void Update()
    {
        if (this.skill == null) return;
        if (this.skill.CD > 0)
        {
            if (!overlay.enabled) overlay.enabled = true;
            if (!cdText.enabled) cdText.enabled = true;
            overlay.fillAmount = this.skill.CD / this.skill.Define.CD;
            this.cdText.text = ((int)Math.Ceiling(this.skill.CD)).ToString();
        }
        else
        {
            if (overlay.enabled) overlay.enabled = false;
            if (cdText.enabled) cdText.enabled = false;
        }
    }
    /// <summary>
    /// 技能指示器回调
    /// </summary>
    /// <param name="pos"></param>
    public void OnPositionSelected(Vector3 pos)
    {
        BattleManager.Instance.CurrentPosition = GameObjectTool.WorldToLogicN(pos);
        this.CastSkill();
    }
    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.skill.Define.CastTarget == TargetType.Position)
        {
            TargetSelector.ShowSelector(this.skill.Owner.position, this.skill.Define.CastRange, this.skill.Define.AOERange, OnPositionSelected);
        }
        else
        {
            this.CastSkill();
        }
    }
    /// <summary>
    /// 判断释放技能
    /// </summary>
    private void CastSkill()
    {
        SkillResult result = this.skill.CanCast(BattleManager.Instance.CurrentTarget);
        switch (result)
        {
            case SkillResult.InvalidTarget:
                //MessageBox.Show("技能" + this.skill.Define.Name + "目标无效");
                return;
            case SkillResult.OutOfMp:
                //MessageBox.Show("技能" + this.skill.Define.Name + "蓝量不足");
                return;
            case SkillResult.CoolDown:
                //MessageBox.Show("技能" + this.skill.Define.Name + "正在冷却");
                return;
            case SkillResult.OutOfRange:
                //MessageBox.Show("技能" + this.skill.Define.Name + "超出攻击范围");
                return;
        }
        BattleManager.Instance.CastSkill(this.skill);
    }
    /// <summary>
    /// 设置技能
    /// </summary>
    /// <param name="item"></param>
    internal void SetSkill(Skill item)
    {
        this.skill = item;
        if (this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.skill.Define.Icon);
            //强制刷新
            this.icon.SetAllDirty();
        }
    }
}