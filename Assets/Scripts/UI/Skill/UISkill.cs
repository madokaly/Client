using Common.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能UI
/// </summary>
public class UISkill : UIWindow
{
    /// <summary>
    /// 技能描述
    /// </summary>
    public Text descript;
    /// <summary>
    /// 技能元素预制件
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 技能元素列表
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 所选元素
    /// </summary>
    private UISkillItem selectedItem;

    private void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSelected;
    }

    private void OnDestroy()
    {
        this.listMain.onItemSelected -= this.OnItemSelected;
    }

    public void OnItemSelected(ListView.ListViewItem item)
    {
        this.selectedItem = item as UISkillItem;
        this.descript.text = this.selectedItem.item.Define.Description;
    }
    /// <summary>
    /// 刷新UI
    /// </summary>
    private void RefreshUI()
    {
        ClearItems();
        InitItems();
    }
    /// <summary>
    /// 初始化技能列表
    /// </summary>
    private void InitItems()
    {
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        foreach(var skill in Skills)
        {
            if(skill.Define.Type == Common.Battle.SkillType.Skill)
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UISkillItem ui = go.GetComponent<UISkillItem>();
                ui.SetItem(skill, this, false);
                listMain.AddItem(ui);
            }
        }
    }

    private void ClearItems()
    {
        listMain.RemoveAll();
    }
}
