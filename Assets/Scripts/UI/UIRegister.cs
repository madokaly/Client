using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Services;

public class UIRegister : MonoBehaviour
{

    public InputField username;
    public InputField password;
    public InputField passwordConfirm;
    public Button buttonRegister;

    public void OnClickRegister()
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
        if (string.IsNullOrEmpty(passwordConfirm.text))
        {
            MessageBox.Show("请输入确认密码");
            return;
        }
        if (password.text != passwordConfirm.text)
        {
            MessageBox.Show("两次输入的密码不一致");
            return;
        }
        UserService.Instance.SendRegister(username.text, password.text);
    }
}
