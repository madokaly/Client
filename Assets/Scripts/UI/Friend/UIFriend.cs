using Managers;
using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI好友面板类
/// </summary>
public class UIFriend : UIWindow
{
    /// <summary>
    /// UI好友元素预制体
    /// </summary>
    public GameObject itemPrefabs;
    /// <summary>
    /// 好友列表
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 好友元素的父节点
    /// </summary>
    public Transform listRoot;
    /// <summary>
    /// 选择元素
    /// </summary>
    public UIFriendItem selecteItem;

    private void Start()
    {
        //注册好友变化事件
        FriendService.Instance.OnFriendUpdate += RefreshUI;
        //注册选择事件
        this.listMain.onItemSelected += this.OnFriendSelected;
        RefreshUI();
    }
    private void OnDestroy()
    {
        FriendService.Instance.OnFriendUpdate -= RefreshUI;
    }
    private void OnFriendSelected(ListView.ListViewItem item)
    {
        this.selecteItem = item as UIFriendItem;
    }


    public void OnClickAddFriend()
    {
        InputBox.Show("请输入要添加的好友名称或者ID", "添加好友").OnSubmit += OnFriendAddSubmit;
    }
    /// <summary>
    /// 添加好友
    /// </summary>
    /// <param name="input"></param>
    /// <param name="tips"></param>
    /// <returns></returns>
    private bool OnFriendAddSubmit(string input, out string tips)
    {
        tips = "";
        int friendId = 0;
        string friendName = "";
        if (!int.TryParse(input, out friendId))
        {
            friendName = input;
        }
        if (friendId == User.Instance.CurrentCharacterInfo.Id || friendName == User.Instance.CurrentCharacterInfo.Name)
        {
            tips = "不能添加自己为好友哦";
            return false;
        }
        //发送添加服务请求
        FriendService.Instance.SendFriendAddRequest(friendId, friendName);
        return true;
    }

    public void OnClickFriendChat()
    {
        MessageBox.Show("暂未开放");
    }
    /// <summary>
    /// 邀请组队
    /// </summary>
    public void OnClickFriendTeamInvite()
    {
        if (selecteItem == null)
        {
            MessageBox.Show("请选择要邀请的好友");
            return;
        }
        if (selecteItem.Info.Status == 0)
        {
            MessageBox.Show("请选择在线的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要邀请好友【{0}】加入队伍吗？", selecteItem.Info.friendInfo.Name), "邀请好友组队", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            TeamService.Instance.SendTeamInviteRequest(this.selecteItem.Info.friendInfo.Id, this.selecteItem.Info.friendInfo.Name);
        };
    }
    /// <summary>
    /// 移除好友
    /// </summary>
    public void OnClickFriendRemove()
    {
        if (selecteItem == null)
        {
            MessageBox.Show("请选择要删除的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要删除{0}吗?", selecteItem.Info.friendInfo.Name), "删除好友", MessageBoxType.Confirm, "删除", "取消").OnYes = () =>
          {
              //发送移除好友请求
              FriendService.Instance.OnFriendRemoveRequest(this.selecteItem.Info.Id, this.selecteItem.Info.friendInfo.Id);
          };
    }

    /// <summary>
    /// 邀请挑战
    /// </summary>
    public void OnClickChallenge()
    {
        if (selecteItem == null)
        {
            MessageBox.Show("请选择要挑战的好友");
            return;
        }
        if (selecteItem.Info.Status == 0)
        {
            MessageBox.Show("请选择在线的好友");
            return;
        }
        MessageBox.Show(string.Format("确定要挑战好友【{0}】吗？", selecteItem.Info.friendInfo.Name), "挑战好友", MessageBoxType.Confirm, "确定", "取消").OnYes = () =>
        {
            ArenaService.Instance.SendArenaChallengeRequest(this.selecteItem.Info.friendInfo.Id, this.selecteItem.Info.friendInfo.Name);
        };
    }

    void RefreshUI()
    {
        ClearAllFriendItems();
        InitAllFriendItems();
    }

    void ClearAllFriendItems()
    {
        this.listMain.RemoveAll();
    }
    /// <summary>
    /// 初始化所有好友列表
    /// </summary>
    void InitAllFriendItems()
    {
        foreach (var item in FriendManager.Instance.allFriends)
        {
            GameObject go = Instantiate(itemPrefabs, listRoot.transform);
            var ui = go.GetComponent<UIFriendItem>();
            this.listMain.AddItem(ui as UIFriendItem);
            ui.SetFirendInfo(item);
        }
    }
}

