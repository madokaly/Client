using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI物品
/// </summary>
public class UIIconItem : MonoBehaviour
{
    public Image Image;
    public Text Count;

    /// <summary>
    /// 设置物品
    /// </summary>
    /// <param name="icon">图片</param>
    /// <param name="count">数量</param>
    public void setMainIcon(string icon, string count)
    {
        this.Image.overrideSprite = Resloader.Load<Sprite>(icon);
        this.Count.text = count;
    }
}
