using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI商店物品类
/// </summary>
public class UIShopItem : MonoBehaviour, ISelectHandler
{
    /// <summary>
    /// 物品图片
    /// </summary>
    public Image icon;
    /// <summary>
    /// 物品名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 物品价格
    /// </summary>
    public Text price;
    /// <summary>
    /// 物品数量
    /// </summary>
    public Text count;
    /// <summary>
    /// 物品限制购买者
    /// </summary>
    public Text limitClass;
    /// <summary>
    /// 物品背景图片
    /// </summary>
    public Image bacground;
    /// <summary>
    /// 未选中图片
    /// </summary>
    public Sprite normalBg;
    /// <summary>
    /// 选中图片
    /// </summary>
    public Sprite selectedBg;
    /// <summary>
    /// 是否被选中
    /// </summary>
    private bool selected;

    public bool Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            this.bacground.overrideSprite = selected ? selectedBg : normalBg;
        }
    }

    /// <summary>
    /// 商店物品ID
    /// </summary>
    public int ShopItemID { get; set; }
    /// <summary>
    /// 商店ID
    /// </summary>
    private int shopID { get; set; }
    /// <summary>
    /// 当前商店
    /// </summary>
    private UIShop shop;
    /// <summary>
    /// 物品信息
    /// </summary>
    private ItemDefine item;
    /// <summary>
    /// 商店信息
    /// </summary>
    private ShopItemDefine ShopItem { get; set; }
    /// <summary>
    /// 设置商店物品
    /// </summary>
    /// <param name="shopId"></param>
    /// <param name="id"></param>
    /// <param name="shopItem"></param>
    /// <param name="owner"></param>
    public void SetShopItem(int shopId,int id,ShopItemDefine shopItem,UIShop owner)
    {
        this.shopID = shopId;
        this.shop = owner;
        this.ShopItemID = id;
        this.ShopItem = shopItem;
        this.item = DataManager.Instance.Items[this.ShopItem.ItemID];

        this.title.text = item.Name;
        this.count.text = ShopItem.Count.ToString();
        this.price.text = ShopItem.Price.ToString();
        if (shopID==2)
        {
            this.limitClass.text = item.LimitClass.ToString();
        }
        else
            this.limitClass.text ="";

        this.icon.overrideSprite = Resloader.Load<Sprite>(item.Icon);


    }
    /// <summary>
    /// 响应选中事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSelect(BaseEventData eventData)
    {
        this.Selected = true;
        Debug.Log("您点击的道具是:"+ ShopItemID);
        this.shop.SelectShopItem(this);
    }
}
