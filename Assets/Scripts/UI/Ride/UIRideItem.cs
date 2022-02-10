using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 坐骑元素UI
/// </summary>
public class UIRideItem : ListView.ListViewItem
{
    /// <summary>
    /// 图标
    /// </summary>
    public Image icon;
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 等级
    /// </summary>
    public Text level;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

    public Item item;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    /// <summary>
    /// 设置坐骑元素信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="owner"></param>
    /// <param name="equiped"></param>
    public void SetRideItem(Item item, UIRide owner, bool equiped)
    {
        this.item = item;
        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = this.item.Define.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}
