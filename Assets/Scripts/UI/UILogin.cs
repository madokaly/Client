using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Services;
using SkillBridge.Message;
using System;

/// <summary>
/// 登陆UI
/// </summary>
public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button buttonLogin;
    public Button buttonRegister;

    private void Start()
    {
        UserService.Instance.OnLogin = OnLogin;
    }
    /// <summary>
    /// 监听登陆成功消息
    /// </summary>
    /// <param name="result"></param>
    /// <param name="message"></param>
    private void OnLogin(Result result, string message)
    {
        //登陆成功，进入选择角色场景
        if(result == Result.Success)
        {
            SceneManager.Instance.LoadScene("CharSelect");
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        }
        else
        {
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }
    /// <summary>
    /// 点击登陆
    /// </summary>
    public void OnClickButton()
    {
        if (string.IsNullOrEmpty(username.text))
        {
            MessageBox.Show("请输入账号");
            return;
        }
        if (string.IsNullOrEmpty(password.text))
        {
            MessageBox.Show("请输入密码");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        //向服务端发送账号密码
        UserService.Instance.SendLogin(username.text, password.text);
    }
}
