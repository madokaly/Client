using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 商店管理类
    /// </summary>
    public class ShopManager:Singleton<ShopManager>
    {
        /// <summary>
        /// 商店类
        /// </summary>
        private UIShop uiShop = null;

        public void Init()
        {
            //注册金钱状态变化事件
            StatusService.Instance.RegisterStatusNotify(StatusType.Money, OnMoneyNotify);
            //注册Npc的点击响应事件
            NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeShop,OnOpenShop);
        }
        /// <summary>
        /// 打开Npc商店
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public bool OnOpenShop(NpcDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }
        /// <summary>
        /// 打开商店
        /// </summary>
        /// <param name="shopId"></param>
        public void ShowShop(int shopId)
        {
            ShopDefine shop;
            if (DataManager.Instance.Shops.TryGetValue(shopId,out shop))
            {
                uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop!=null)
                {
                    uiShop.SetShop(shop);
                }
            }
        }
        /// <summary>
        /// 购买物品
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopItemId"></param>
        /// <returns></returns>
        public bool BuyItem(int shopId,int shopItemId)
        {
            Debug.LogFormat("BuyItem::ShopId :{0} ItemId:{1}", shopId, shopItemId);
            ItemService.Instance.SendBuyItem(shopId,shopItemId);
            return true;
        }
        /// <summary>
        /// 注册状态类金钱减少事件
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private bool OnMoneyNotify(NStatus status)
        {
            if (uiShop != null)
            {
                if (status.Type == StatusType.Money && status.Action == StatusAction.Delete)
                {
                    uiShop.Money -= status.Value;
                }
            }
            return true;
        }

    }
}
