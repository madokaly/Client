using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 公会创建UI
/// </summary>
public class UIGuildPopCreate : UIWindow
{
    /// <summary>
    /// 公会名字
    /// </summary>
    public InputField inputName;
    /// <summary>
    /// 公会宗旨
    /// </summary>
    public InputField inputNotice;

    private void Start()
    {
        GuildService.Instance.OnGuildCreateResult = OnGuildCreate;
    }

    private void OnDestroy()
    {
        GuildService.Instance.OnGuildCreateResult = null;
    }
    /// <summary>
    /// 点击创建按钮
    /// </summary>
    public override void OnYesClick()
    {
        if (string.IsNullOrEmpty(inputName.text))
        {
            MessageBox.Show("请输入公会名称", "错误", MessageBoxType.Error);
            return;
        }
        if(inputName.text.Length < 2 || inputName.text.Length > 10)
        {
            MessageBox.Show("公会名称为4-10字符", "错误", MessageBoxType.Error);
            return;
        }
        if (string.IsNullOrEmpty(inputNotice.text))
        {
            MessageBox.Show("请输入公会宗旨", "错误", MessageBoxType.Error);
            return;
        }
        if (inputNotice.text.Length < 3 || inputNotice.text.Length > 50)
        {
            MessageBox.Show("公会宗旨为3-50字符", "错误", MessageBoxType.Error);
            return;
        }
        GuildService.Instance.SendGuildCreate(inputName.text, inputNotice.text);
    }
    /// <summary>
    /// 响应创建公会事件
    /// </summary>
    /// <param name="result"></param>
    private void OnGuildCreate(bool result)
    {
        if (result)
            Close(WindowResult.Yes);
    }
}
