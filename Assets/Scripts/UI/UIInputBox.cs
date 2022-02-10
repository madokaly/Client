using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI输入框类
/// </summary>
public class UIInputBox : MonoBehaviour
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 提示文本
    /// </summary>
    public Text message;
    /// <summary>
    /// 提示标签
    /// </summary>
    public Text tips;
    /// <summary>
    /// 提交按钮
    /// </summary>
    public Button buttonYes;
    /// <summary>
    /// 取消按钮
    /// </summary>
    public Button buttonNo;
    /// <summary>
    /// 输入框
    /// </summary>
    public InputField input;

    public Text buttonYesTitle;
    public Text buttonNoTitle;

    public delegate bool SubmitHandler(string inputText, out string tips);
    /// <summary>
    /// 提交事件
    /// </summary>
    public event SubmitHandler OnSubmit;
    /// <summary>
    /// 取消事件
    /// </summary>
    public Action OnCancel;

    public string emptyTips;

    public void Init(string title, string message, string btnOK = "", string btnCancel = "", string emptyTips = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.tips.text = null;
        this.OnSubmit = null;
        this.emptyTips = emptyTips;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = title;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = title;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);
    }

    void OnClickYes()
    {
        this.tips.text = "";
        if (string.IsNullOrEmpty(input.text))
        {
            this.tips.text = this.emptyTips;
            return;
        }
        if (OnSubmit != null)
        {
            string tips;
            if (!OnSubmit(this.input.text, out tips))
            {
                this.tips.text = tips;
                return;
            }
        }
        Destroy(this.gameObject);
    }

    void OnClickNo()
    {
        Destroy(this.gameObject);
        if (this.OnCancel != null)
        {
            this.OnCancel();
        }
    }

}
