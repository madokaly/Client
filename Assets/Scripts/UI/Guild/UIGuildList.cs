using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 公会UI
/// </summary>
public class UIGuildList : UIWindow
{
    /// <summary>
    /// 公会元素预制件
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 公会元素列表
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 元素根节点
    /// </summary>
    public Transform itemRoot;
    /// <summary>
    /// 公会信息UI
    /// </summary>
    public UIGuildInfo uiInfo;
    /// <summary>
    /// 公会元素UI
    /// </summary>
    public UIGuildItem selectedItem;

    private void Start()
    {
        listMain.onItemSelected += OnGuildMemberSelected;
        uiInfo.Info = null;
        GuildService.Instance.OnGuildListResult += UpdateGuildList;
        GuildService.Instance.SendGuildListRequest();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildListResult -= UpdateGuildList;
    }
    /// <summary>
    /// 更新列表
    /// </summary>
    /// <param name="guilds"></param>
    private void UpdateGuildList(List<NGuildInfo> guilds)
    {
        ClearList();
        InitItems(guilds);
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        selectedItem = item as UIGuildItem;
        uiInfo.Info = selectedItem.Info;
    }
    /// <summary>
    /// 初始化公会列表
    /// </summary>
    /// <param name="guilds"></param>
    private void InitItems(List<NGuildInfo> guilds)
    {
        foreach (var item in guilds)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildItem ui = go.GetComponent<UIGuildItem>();
            ui.SetGuildInfo(item);
            listMain.AddItem(ui);
        }
    }
    /// <summary>
    /// 清空列表
    /// </summary>
    private void ClearList()
    {
        listMain.RemoveAll();
    }
    /// <summary>
    /// 点击加入按钮
    /// </summary>
    public void OnclickJoin()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要加入的公会");
            return;
        }
        MessageBox.Show(string.Format("确定要加入公会【{0}】吗？", selectedItem.Info.GuildName),"申请加入公会", MessageBoxType.Confirm).OnYes =
            () => {
                GuildService.Instance.SendGuildJoinRequest(selectedItem.Info.Id);
            };
    }
}
