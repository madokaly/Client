using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI装备物品类
/// </summary>
public class UIEquipItem : MonoBehaviour,IPointerClickHandler
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 等级
    /// </summary>
    public Text level;
    /// <summary>
    /// 限制职业
    /// </summary>
    public Text limitClass;
    /// <summary>
    /// 限制类别
    /// </summary>
    public Text limitCategory;
    /// <summary>
    /// 图标
    /// </summary>
    public Image icon;
    /// <summary>
    /// 背景图
    /// </summary>
    public Image background;
    /// <summary>
    /// 未选中
    /// </summary>
    public Sprite normalBg;
    /// <summary>
    /// 选中
    /// </summary>
    public Sprite selectBg;
    /// <summary>
    /// 是否被选中
    /// </summary>
    private bool selected;

    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.background.overrideSprite = selected ? selectBg : normalBg;
        }
    }
    /// <summary>
    /// 索引
    /// </summary>
    public int index { get; set; }
    /// <summary>
    /// UI角色装备类
    /// </summary>
    public UICharEquip owner;
    /// <summary>
    /// 物品类
    /// </summary>
    private Item item;
    /// <summary>
    /// 是否穿戴
    /// </summary>
    bool isEquiped = false;
    /// <summary>
    /// 设置装备物品
    /// </summary>
    /// <param name="idx">索引</param>
    /// <param name="item">物品</param>
    /// <param name="owner">所有者UI装备类</param>
    /// <param name="equiped">是否穿戴</param>
    public void SetEquipItem(int idx, Item item, UICharEquip owner, bool equiped)
    {
        this.owner = owner;
        this.index = idx;
        this.item = item;
        this.isEquiped = equiped;

        if (this.title != null) this.title.text = item.Define.Name;
        if (this.level != null) this.level.text = item.Define.Level.ToString();
        if (this.limitClass != null) this.limitClass.text = item.Define.LimitClass.ToString();
        if (this.limitCategory != null) this.limitCategory.text = item.Define.Category;
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
    /// <summary>
    /// 鼠标点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isEquiped)
        {
            UnEquip();
        }
        else
        {
            if (this.Selected)
            {
                //穿戴
                DoEquip();
                this.Selected = false;
            }
            else
            {   
                //取消之前选择项
                if(owner.selectedUIEquipItem != null)
                {
                    owner.selectedUIEquipItem.Selected = false;
                }
                this.Selected = true;
                //设置当前项选中
                owner.selectedUIEquipItem = this;
            }
        }
    }
    /// <summary>
    /// 穿戴
    /// </summary>
    private void DoEquip()
    {
        var msg = MessageBox.Show(string.Format("要装备{0}吗？",this.item.Define.Name), "确定", MessageBoxType.Confirm);
        msg.OnYes = () => 
        {
            var oldEquip = EquipManager.Instance.GetItem(item.EquipInfo.Slot);
            if (oldEquip != null)
            {
                var newmsg = MessageBox.Show(string.Format("确定要替换掉{0}吗?", oldEquip.Define.Name), "确认", MessageBoxType.Confirm);
                newmsg.OnYes = () =>
                {
                    //this.owner.UnEquip(oldEquip);
                    this.owner.DoEquip(this.item);
                };
            }
            else
                this.owner.DoEquip(this.item);
        };
    

    }
    /// <summary>
    /// 脱下
    /// </summary>
    private void UnEquip()
    {
        var msg = MessageBox.Show(string.Format("要取下装备{0}吗？", this.item.Define.Name, "确认", MessageBoxType.Confirm));
        msg.OnYes = () =>
        {
            this.owner.UnEquip(this.item);
        };
    }
}