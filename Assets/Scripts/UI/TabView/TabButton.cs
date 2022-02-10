using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Tab页面按钮类
/// </summary>
public class TabButton : MonoBehaviour
{
    /// <summary>
    /// 激活时图片
    /// </summary>
    public Sprite activeImage;
    /// <summary>
    /// 未激活图片
    /// </summary>
    private Sprite normalSprite;

    public TabView tabView;
    public bool selected = false;
    /// <summary>
    /// 按钮image组件
    /// </summary>
    private Image tableImage;
    /// <summary>
    /// 按钮序号
    /// </summary>
    public int tabIndex = -1;

    private void Start()
    {
        this.tableImage = this.gameObject.GetComponent<Image>();
        normalSprite = tableImage.sprite;
        this.gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Select(bool select)
    {
        tableImage.overrideSprite = select ? activeImage : normalSprite;
    }

    private void OnClick()
    {
        tabView.SelectTab(this.tabIndex);
    }
}
