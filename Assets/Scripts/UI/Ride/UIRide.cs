using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI角色装备类
/// </summary>
public class UIRide : UIWindow
{
    /// <summary>
    /// 描述
    /// </summary>
    public Text descript;
    /// <summary>
    /// 坐骑元素预制件
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 坐骑元素节点
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 已选择坐骑
    /// </summary>
    [HideInInspector]
    public UIRideItem selectedUIRideItem;

    private void Start()
    {
        RefreshUI();
        this.listMain.onItemSelected += this.OnItemSemected;
    }
    private void OnDestroy()
    {
        this.listMain.onItemSelected -= this.OnItemSemected;
    }

    private void OnItemSemected(ListView.ListViewItem item)
    {
        this.selectedUIRideItem = item as UIRideItem;
        this.descript.text = this.selectedUIRideItem.item.Define.Description;
    }

    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        ClearItems();
        InitItems();
    }

    //初始化坐骑列表
    private void InitItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Ride && (kv.Value.Define.LimitClass == CharacterClass.None || kv.Value.Define.LimitClass == User.Instance.CurrentCharacterInfo.Class))
            {
                GameObject go = Instantiate(itemPrefab, this.listMain.transform);
                UIRideItem ui = go.GetComponent<UIRideItem>();
                ui.SetRideItem(kv.Value, this, false);
                this.listMain.AddItem(ui);
            }
        }
    }

    private void ClearItems()
    {
        listMain.RemoveAll();
    }
    /// <summary>
    /// 点击骑行按钮
    /// </summary>
    public void DoRide()
    {
        if(this.selectedUIRideItem == null)
        {
            MessageBox.Show("请选择你的坐骑");
            return;
        }
        User.Instance.Ride(this.selectedUIRideItem.item.Id);
    }
}
