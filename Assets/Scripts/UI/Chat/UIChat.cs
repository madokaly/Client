using Candlelight.UI;
using Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 聊天UI
/// </summary>
public class UIChat : MonoBehaviour
{
    /// <summary>
    /// 聊天内容显示区
    /// </summary>
    public HyperText textArea;
    /// <summary>
    /// 聊天频道切换控件
    /// </summary>
    public TabView channelTab;
    /// <summary>
    /// 聊天输入控件
    /// </summary>
    public InputField chatText;
    /// <summary>
    /// 私聊对象
    /// </summary>
    public Text chatTarget;
    /// <summary>
    /// 聊天频道下拉框
    /// </summary>
    public Dropdown channelSelect;

    private void Start()
    {
        this.channelTab.OnTabSelect += OnDisplayChannelSelected;
        ChatManager.Instance.OnChat += RefreshUI;
    }

    private void OnDestroy()
    {
        ChatManager.Instance.OnChat -= RefreshUI;
    }

    private void Update()
    {
        InputManager.Instance.IsInputMode = chatText.isFocused;
    }
    /// <summary>
    /// 显示频道
    /// </summary>
    /// <param name="index"></param>
    public void OnDisplayChannelSelected(int index)
    {
        ChatManager.Instance.displayChannel = (ChatManager.LocalChannel)index;
        this.RefreshUI();
    }
    /// <summary>
    /// 刷新UI
    /// </summary>
    public void RefreshUI()
    {
        //得到当前消息
        this.textArea.text = ChatManager.Instance.GetCurrentMessages();
        //更新频道选择
        this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        if(ChatManager.Instance.SendChannel == ChatChannel.Private)
        {
            this.chatTarget.gameObject.SetActive(true);
            if(ChatManager.Instance.PrivateID != 0)
            {
                this.chatTarget.text = ChatManager.Instance.PrivateName + "：";
            }
            else
            {
                this.chatTarget.text = "<无>";
            }
        }
        else
        {
            this.chatTarget.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// HyperText的点击响应事件
    /// </summary>
    /// <param name="text"></param>
    /// <param name="link"></param>
    public void OnclickChatLink(HyperText text, HyperText.LinkInfo link)
    {
        if (string.IsNullOrEmpty(link.Name))
        {
            return;
        }
        //<a name="c:Id:Name" class="player">Name</a>
        if (link.Name.StartsWith("c:"))
        {
            string[] strs = link.Name.Split(":".ToCharArray());
            UIPopCharMenu menu = UIManager.Instance.Show<UIPopCharMenu>();
            menu.targetId = int.Parse(strs[1]);
            menu.targetName = strs[2];
        }
    }
    /// <summary>
    /// 点击发送按钮
    /// </summary>
    public void OnClickSend()
    {
        OnEndInput(this.chatText.text);
    }
    /// <summary>
    /// 发送消息清空输入框
    /// </summary>
    /// <param name="text"></param>
    public void OnEndInput(string text)
    {
        if (!string.IsNullOrEmpty(text.Trim()))
        {
            this.SendChat(text);
        }
        this.chatText.text = "";
    }
    /// <summary>
    /// 发送聊天消息
    /// </summary>
    /// <param name="message"></param>
    private void SendChat(string message)
    {
        ChatManager.Instance.SendChat(message, ChatManager.Instance.PrivateID, ChatManager.Instance.PrivateName);
    }
    /// <summary>
    /// 切换频道
    /// </summary>
    /// <param name="index"></param>
    public void OnSendChannelChanged(int index)
    {
        if(ChatManager.Instance.sendChannel == (ChatManager.LocalChannel)(index + 1))
        {
            return;
        }
        if(!ChatManager.Instance.SetSendChannel((ChatManager.LocalChannel)index + 1))
        {
            this.channelSelect.value = (int)ChatManager.Instance.sendChannel - 1;
        }
        else
        {
            this.RefreshUI();
        }
    }
}
