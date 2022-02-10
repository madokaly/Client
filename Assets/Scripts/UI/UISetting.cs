using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 设置UI
/// </summary>
public class UISetting : UIWindow
{
    /// <summary>
    /// 选择角色
    /// </summary>
    public void ExitToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        Services.UserService.Instance.SendGameLeave();
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void ExitGame()
    {
        Services.UserService.Instance.SendGameLeave(true);
    }
    /// <summary>
    /// 系统设置
    /// </summary>
    public void OnclickSystemConfig()
    {
        this.OnCloseClick();
        UIManager.Instance.Show<UISystemConfig>();
    }
}
