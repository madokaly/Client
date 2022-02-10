using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI抽象类
/// </summary>
public abstract class UIWindow : MonoBehaviour
{
    /// <summary>
    /// 关闭委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="result"></param>
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    /// <summary>
    /// 关闭事件
    /// </summary>
    public event CloseHandler OnClose;
    public virtual System.Type Type { get { return this.GetType(); } }
    /// <summary>
    /// 根节点
    /// </summary>
    public GameObject Root;

    public enum WindowResult
    {
        None=0,
        Yes,
        No
    }
    public void Close(WindowResult result = WindowResult.None)
    {
        //关闭UI
        UIManager.Instance.Close(this.Type);
        //响应事件执行
        if (this.OnClose != null)
        {
            this.OnClose(this, result);
        }
        this.OnClose = null;
    }

    public virtual void OnCloseClick()
    {
        this.Close();
    }
    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }
    public virtual void OnNoClick()
    {
        this.Close(WindowResult.No);
    }

    //private void OnMouseDown()
    //{
    //    Debug.LogFormat(this.name+"Click");
    //}
}
