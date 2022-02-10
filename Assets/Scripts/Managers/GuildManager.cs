using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 公会管理类
/// </summary>
public class GuildManager : Singleton<GuildManager>
{
    /// <summary>
    /// 协议公会信息
    /// </summary>
    public NGuildInfo guildInfo;
    /// <summary>
    /// 协议公会成员信息
    /// </summary>
    public NGuildMemberInfo myMemberInfo;
    /// <summary>
    /// 是否拥有公会
    /// </summary>
    public bool HasGuild
    {
        get
        {
            return guildInfo != null;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="guild"></param>
    public void Init(NGuildInfo guild)
    {
        guildInfo = guild;
        if(guild == null)
        {
            myMemberInfo = null;
            return;
        }
        foreach(var member in guild.Members)
        {
            if(member.characterId == User.Instance.CurrentCharacterInfo.Id)
            {
                myMemberInfo = member;
                break;
            }
        }
    }
    /// <summary>
    /// 打开对应公会UI
    /// </summary>
    public void ShowGuild()
    {
        if (HasGuild)
        {
            UIManager.Instance.Show<UIGuild>();
        }
        else
        {
            var win = UIManager.Instance.Show<UIGuildPopNoGuild>();
            //注册关闭事件
            win.OnClose += PopNoGuild_OnClose;
        }
    }

    private void PopNoGuild_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        //创建公会UI
        if(result == UIWindow.WindowResult.Yes)
        {
            UIManager.Instance.Show<UIGuildPopCreate>();
        }else if(result == UIWindow.WindowResult.No)
        {
            //加入公会UI
            UIManager.Instance.Show<UIGuildList>();
        }
    }
}
