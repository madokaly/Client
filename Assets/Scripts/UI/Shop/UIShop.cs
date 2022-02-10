using Common.Data;
using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// UI商店
/// </summary>
public class UIShop : UIWindow
{
    /// <summary>
    /// 商店名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 玩家金钱
    /// </summary>
    public Text money;
    /// <summary>
    /// 商店物品类
    /// </summary>
    public GameObject shopItem;
    /// <summary>
    /// 
    /// </summary>
    ShopDefine shop;
    /// <summary>
    /// 商店页数
    /// </summary>
    public Transform[] itemRoot;

    public int Money
    {
        get { return int.Parse(money.text); }
        set { money.text = value.ToString(); }
    }



    private void Start()
    {
        StartCoroutine(InitItem());
    }


    /// <summary>
    /// 初始化物品
    /// </summary>
    /// <returns></returns>
    IEnumerator InitItem()
    {
        int page = 0;
        int count = 0;
        foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
        {
            if (kv.Value.Status>0)
            {
                GameObject go = Instantiate(shopItem, itemRoot[page]);
                UIShopItem ui = go.GetComponent<UIShopItem>();
                ui.SetShopItem(shop.ID, kv.Key, kv.Value, this);
                count += 1;
                if (count >= 10)
                {
                    page += 1;
                    count = 0;
                }
            }
        }
        yield return null;
    }
    /// <summary>
    /// 设置商店信息
    /// </summary>
    /// <param name="shop"></param>
    internal void SetShop(ShopDefine shop)
    {
        this.shop = shop;
        this.title.text = shop.Name;
        this.money.text = User.Instance.CurrentCharacterInfo.Gold.ToString();

    }

    private UIShopItem selectedItem;
    /// <summary>
    /// 选中商店物品
    /// </summary>
    /// <param name="item"></param>
    public void SelectShopItem(UIShopItem item)
    {
        if (selectedItem != null)
        {
            selectedItem.Selected = false;
        }
        selectedItem = item;
    }
    /// <summary>
    /// 点击购买
    /// </summary>
    public void OnClickBuy()
    {
        if (this.selectedItem == null)
        {
            MessageBox.Show("请选择要购买的道具","购买提示");
            return;
        }
        ShopManager.Instance.BuyItem(this.shop.ID, this.selectedItem.ShopItemID);
    }


}
