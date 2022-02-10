using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI任务元素类
/// </summary>
public class UIQuestItem : ListView.ListViewItem
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 背景
    /// </summary>
    public Image background;
    /// <summary>
    /// 未选择图片
    /// </summary>
    public Sprite normalBg;
    /// <summary>
    /// 选择图片
    /// </summary>
    public Sprite selectedBg;
    /// <summary>
    /// 更改选择状态执行
    /// </summary>
    /// <param name="selected"></param>
    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }
    /// <summary>
    /// 任务
    /// </summary>
    public Quest quest;
    /// <summary>
    /// 设置任务元素信息
    /// </summary>
    /// <param name="item"></param>
    public void SetQuestItemInfo(Quest item)
    {
        this.quest = item;
        if (this.title!=null)
        {
            this.title.text = this.quest.Define.Name;
        }
    }

}
