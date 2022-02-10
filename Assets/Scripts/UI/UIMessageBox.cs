using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 消息弹出框UI
/// </summary>
public class UIMessageBox : MonoBehaviour
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 消息
    /// </summary>
    public Text message;
    /// <summary>
    /// 图标
    /// </summary>
    public Image[] icons;
    /// <summary>
    /// 确认按钮
    /// </summary>
    public Button buttonYes;
    /// <summary>
    /// 取消按钮
    /// </summary>
    public Button buttonNo;
    /// <summary>
    /// 关闭按钮
    /// </summary>
    public Button buttonClose;
    /// <summary>
    /// 确认按钮文本
    /// </summary>
    public Text buttonYesTitle;
    /// <summary>
    /// 取消按钮文本
    /// </summary>
    public Text buttonNoTitle;
    /// <summary>
    /// 委托OnYes
    /// </summary>
    public UnityAction OnYes;
    /// <summary>
    /// 委托OnNo
    /// </summary>
    public UnityAction OnNo;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="title"></param>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <param name="btnOK"></param>
    /// <param name="btnCancel"></param>
    public void Init(string title, string message, MessageBoxType type = MessageBoxType.Information, string btnOK = "", string btnCancel = "")
    {
        if (!string.IsNullOrEmpty(title)) this.title.text = title;
        this.message.text = message;
        this.icons[0].enabled = type == MessageBoxType.Information;
        this.icons[1].enabled = type == MessageBoxType.Confirm;
        this.icons[2].enabled = type == MessageBoxType.Error;

        if (!string.IsNullOrEmpty(btnOK)) this.buttonYesTitle.text = btnOK;
        if (!string.IsNullOrEmpty(btnCancel)) this.buttonNoTitle.text = btnCancel;

        this.buttonYes.onClick.AddListener(OnClickYes);
        this.buttonNo.onClick.AddListener(OnClickNo);

        this.buttonNo.gameObject.SetActive(type == MessageBoxType.Confirm);

        if (type == MessageBoxType.Error)
            SoundManager.Instance.PlaySound(SoundDefine.SFX_Message_Error);
        else
            SoundManager.Instance.PlaySound(SoundDefine.SFX_Message_Info);
    }

    void OnClickYes()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Confirm);
        Destroy(this.gameObject);
        if (this.OnYes != null)
            this.OnYes();
    }

    void OnClickNo()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Close);
        Destroy(this.gameObject);
        if (this.OnNo != null)
            this.OnNo();
    }
}
