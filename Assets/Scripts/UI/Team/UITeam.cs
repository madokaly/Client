using Models;
using Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI队伍类
/// </summary>
public class UITeam : MonoBehaviour
{
    /// <summary>
    /// 队伍名字
    /// </summary>
    public Text Title;
    /// <summary>
    /// 队伍元素数组
    /// </summary>
    public UITeamItem[] Members;
    /// <summary>
    /// UIList
    /// </summary>
    public ListView ListMain;

    void Start()
    {
        if (User.Instance.TeamInfo == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        foreach (var item in Members)
        {
            this.ListMain.AddItem(item);
        }
    }

    private void OnEnable()
    {
        UpdateTeamUI();
    }
    /// <summary>
    /// 显示队伍列表
    /// </summary>
    /// <param name="show">是否显示</param>
    public void ShowTeam(bool show)
    {
        this.gameObject.SetActive(show);
        if (show)
        {
            UpdateTeamUI();
        }
    }
    /// <summary>
    /// 刷新队伍UI
    /// </summary>
    private void UpdateTeamUI()
    {
        if (User.Instance.TeamInfo == null)
        {
            return;
        }
        Title.text = "我的队伍" + User.Instance.TeamInfo.Members.Count + "/5";
        for (int i = 0; i < 5; i++)
        {
            if (i < User.Instance.TeamInfo.Members.Count)
            {
                this.Members[i].SetMemberInfo(i, User.Instance.TeamInfo.Members[i], User.Instance.TeamInfo.Members[i].Id == User.Instance.TeamInfo.Leader);
                this.Members[i].gameObject.SetActive(true);
            }
            else
            {
                this.Members[i].gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 离开队伍
    /// </summary>
    public void OnClickLeave()
    {
        MessageBox.Show("确定要离开队伍吗？", "退出队伍", MessageBoxType.Confirm, "确定离开", "取消").OnYes = () =>
              {
                  TeamService.Instance.SendTeamLeaveRequest(User.Instance.TeamInfo.Id);
              };
    }

}
