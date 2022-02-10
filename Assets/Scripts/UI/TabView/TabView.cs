using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tab页面类
/// </summary>
public class TabView : MonoBehaviour
{
    /// <summary>
    /// 按钮数组
    /// </summary>
    public TabButton[] tabButtons;
    /// <summary>
    /// 页面数组
    /// </summary>
    public GameObject[] tabPages;
    /// <summary>
    /// 事件
    /// </summary>
    public UnityAction<int> OnTabSelect;

    public int index = -1;
    /// <summary>
    /// 初始化按钮序号
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabButtons[i].tabView = this;
            tabButtons[i].tabIndex = i;
        }
        yield return new WaitForEndOfFrame();
        SelectTab(0);
    }
    /// <summary>
    /// 点击按钮激活对应页面
    /// </summary>
    /// <param name="index"></param>
    public void SelectTab(int index)
    {
        if (this.index != index)
        {
            for (int i = 0; i < tabButtons.Length; i++)
            {
                tabButtons[i].Select(i == index);
                if (i < tabPages.Length)
                {
                    tabPages[i].SetActive(i == index);
                    this.index = index;
                }
                
            }
        }
        if (OnTabSelect != null)
        {
            OnTabSelect(index);
        }
    }
}
