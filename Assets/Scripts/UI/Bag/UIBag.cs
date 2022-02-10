using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI背包类
/// </summary>
public class UIBag : UIWindow
{
    /// <summary>
    /// 钱
    /// </summary>
    public Text money;
    /// <summary>
    /// 背包
    /// </summary>
    public Transform[] pages;

    public GameObject bagItem;
    /// <summary>
    /// 槽
    /// </summary>
    List<Image> slots;


    private void Start()
    {
        if (slots==null)
        {
            slots = new List<Image>();
            for (int page = 0; page < this.pages.Length; page++)
            {
                slots.AddRange(this.pages[page].GetComponentsInChildren<Image>(true));
            }
        }
        StartCoroutine(InitBag());
    }
    /// <summary>
    /// 初始化背包
    /// </summary>
    /// <returns></returns>
    IEnumerator InitBag()
    {
        for (int i = 0; i < BagManager.Instance.Items.Length; i++)
        {
            var item = BagManager.Instance.Items[i];
            if (item.ItemId > 0)
            {
                GameObject go = Instantiate(bagItem, slots[i].transform);
                var ui = go.GetComponent<UIIconItem>();
                var def = ItemManager.Instance.Items[item.ItemId].Define;
                ui.setMainIcon(def.Icon, item.Count.ToString());
            }

        }
        for (int i = BagManager.Instance.Items.Length; i <slots.Count; i++)
        {
            slots[i].color = Color.gray;
        }
        money.text = User.Instance.CurrentCharacterInfo.Gold.ToString();
        yield return null;
    }

    public void SetTitle()
    {

    }
    /// <summary>
    /// 清理背包
    /// </summary>
    private void Clear()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if(slots[i].transform.childCount > 0)
            {
                Destroy(slots[i].transform.GetChild(0).gameObject);
            }
        }
    }
    /// <summary>
    /// 整理背包
    /// </summary>
    public void OnReset()
    {
        BagManager.Instance.Reset();
        Clear();
        StartCoroutine(InitBag());
    }

}
