using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 公会申请列表UI
/// </summary>
public class UIGuildApplyList : UIWindow
{
    public GameObject itemPrefab;

    public ListView listMain;

    public Transform itemRoot;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateList;
        GuildService.Instance.SendGuildListRequest();
        this.UpdateList();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateList;
    }

    private void UpdateList()
    {
        ClearList();
        InitItems();
    }
    /// <summary>
    /// 初始化公会申请列表
    /// </summary>
    private void InitItems()
    {
        foreach(var item in GuildManager.Instance.guildInfo.Applies)
        {
            GameObject go = Instantiate(itemPrefab, this.listMain.transform);
            UIGuildApplyItem ui = go.GetComponent<UIGuildApplyItem>();
            ui.SetItemInfo(item);
            this.listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        this.listMain.RemoveAll();
    }
}
