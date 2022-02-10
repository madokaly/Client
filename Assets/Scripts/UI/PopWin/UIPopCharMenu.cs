using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 聊天弹窗UI
/// </summary>
public class UIPopCharMenu : UIWindow, IDeselectHandler
{
    /// <summary>
    /// 目标Id
    /// </summary>
    public int targetId;
    /// <summary>
    /// 目标名字
    /// </summary>
    public string targetName;
    /// <summary>
    /// 响应取消选择事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDeselect(BaseEventData eventData)
    {
        var ed = eventData as PointerEventData;
        if (ed.hovered.Contains(this.gameObject)) return;
        this.Close();
    }

    private void OnEnable()
    {
        this.GetComponent<Selectable>().Select();
        this.Root.transform.position = Input.mousePosition + new Vector3(80, 0, 0);
    }
    /// <summary>
    /// 点击私聊
    /// </summary>
    public void OnChat()
    {
        ChatManager.Instance.StartPrivateChat(targetId, targetName);
        this.Close(WindowResult.No);
    }
    /// <summary>
    /// 点击添加好友
    /// </summary>
    public void OnAddFriend()
    {
        FriendService.Instance.SendFriendAddRequest(targetId, targetName);
        this.Close(WindowResult.No);
    }
    /// <summary>
    /// 点击组队
    /// </summary>
    public void OnInviteTeam()
    {
        TeamService.Instance.SendTeamInviteRequest(targetId, targetName);
        this.Close(WindowResult.No);
    }
}
