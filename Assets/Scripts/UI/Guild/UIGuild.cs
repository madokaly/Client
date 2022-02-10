using Services;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 公会UI
/// </summary>
public class UIGuild : UIWindow
{
    /// <summary>
    /// 公会成员预制件
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 公会成员列表
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 公会成员根节点
    /// </summary>
    public Transform itemRoot;
    /// <summary>
    /// 公会信息UI
    /// </summary>
    public UIGuildInfo uiInfo;
    /// <summary>
    /// 公会成员UI
    /// </summary>
    public UIGuildMemberItem selectedItem;

    public GameObject panelAdmin;

    public GameObject panelLeader;

    private void Start()
    {
        GuildService.Instance.OnGuildUpdate += UpdateUI;
        listMain.onItemSelected += OnGuildMemberSelected;
        UpdateUI();
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildUpdate -= UpdateUI;
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        uiInfo.Info = GuildManager.Instance.guildInfo;
        ClearList();
        InitItems();
        this.panelAdmin.SetActive(GuildManager.Instance.myMemberInfo.Title > GuildTitle.None);
        this.panelLeader.SetActive(GuildManager.Instance.myMemberInfo.Title == GuildTitle.President);
    }

    private void OnGuildMemberSelected(ListView.ListViewItem item)
    {
        selectedItem = item as UIGuildMemberItem;
    }

    /// <summary>
    /// 初始化公会UI
    /// </summary>
    private void InitItems()
    {
        foreach(var item in GuildManager.Instance.guildInfo.Members)
        {
            GameObject go = Instantiate(itemPrefab, listMain.transform);
            UIGuildMemberItem ui = go.GetComponent<UIGuildMemberItem>();
            ui.SetGuildMemberInfo(item);
            listMain.AddItem(ui);
        }
    }

    private void ClearList()
    {
        listMain.RemoveAll();
    }

    public void OnclickAppliesList()
    {
        UIManager.Instance.Show<UIGuildApplyList>();
    }

    public void OnclickLeave()
    {
        MessageBox.Show("确定要离开公会吗？", "离开公会", MessageBoxType.Confirm).OnYes = () =>
          {
              GuildService.Instance.SendGuildLeaveRequest();
          };     
    }

    public void OnclickChat()
    {

    }

    public void OnclickKickout()
    {
        if(selectedItem == null)
        {
            MessageBox.Show("请选择要踢出的成员");
            return;
        }
        MessageBox.Show(string.Format("确定要踢[{0}]出公会吗？", selectedItem.Info.Info.Name), "踢出公会", MessageBoxType.Confirm).OnYes = () =>
           {
               GuildService.Instance.SendAdminCommand(GuildAdminCommand.Kickout, this.selectedItem.Info.characterId);
           };
    }

    public void OnclickPromote()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要晋升的成员");
            return;
        }
        if(selectedItem.Info.Title != GuildTitle.None)
        {
            MessageBox.Show("对方地位超然");
            return;
        }
        MessageBox.Show(string.Format("确定要晋升[{0}]为副会长吗？", selectedItem.Info.Info.Name), "晋升成员", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Promote, this.selectedItem.Info.characterId);
        };
    }

    public void OnclickDepose()
    {
        if (selectedItem == null)
        {
            MessageBox.Show("请选择要罢免的成员");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.None)
        {
            MessageBox.Show("对方无职一身轻");
            return;
        }
        if (selectedItem.Info.Title == GuildTitle.President)
        {
            MessageBox.Show("会长太重踢不动");
            return;
        }
        MessageBox.Show(string.Format("确定要罢免[{0}]的副会长吗？", selectedItem.Info.Info.Name), "罢免职务", MessageBoxType.Confirm).OnYes = () =>
        {
            GuildService.Instance.SendAdminCommand(GuildAdminCommand.Depost, this.selectedItem.Info.characterId);
        };
    }
}
