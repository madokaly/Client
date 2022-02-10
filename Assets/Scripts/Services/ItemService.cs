using Managers;
using Models;
using Network;
using SkillBridge.Message;
using System;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 物品服务类
    /// </summary>
    class ItemService : Singleton<ItemService>,IDisposable
    {

        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Subscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(this.OnItemBuy);
            MessageDistributer.Instance.Unsubscribe<ItemEquipResponse>(this.OnItemEquip);
        }

        /// <summary>
        /// 发送购买物品请求
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="shopItemId"></param>
        public void SendBuyItem(int shopId, int shopItemId)
        {
            Debug.LogFormat("SendBuyItem: shopId {0},shopItemId:{1}",shopId,shopItemId);

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyRequest();

            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;

            NetClient.Instance.SendMessage(message);

        }
        /// <summary>
        /// 收到购买物品回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        public void OnItemBuy(object sender, ItemBuyResponse message)
        {
            
            if (message.Result==Result.Success)
            {
                
                MessageBox.Show("购买结果" + message.Result + "/n" + message.Errormsg, "购买完成");
            }
        }
        /// <summary>
        /// 要操作的装备
        /// </summary>
        Item pendingEquip = null;
        /// <summary>
        /// 是否装备
        /// </summary>
        bool isEquip;
        /// <summary>
        /// 发送装备操作请求
        /// </summary>
        /// <param name="equip"></param>
        /// <param name="isEquip"></param>
        /// <returns></returns>
        public bool SendEquipItem(Item equip,bool isEquip)
        {
            if (pendingEquip != null)
            {
                return false;
            }
            Debug.Log("SendEquipItem");
            pendingEquip = equip;
            this.isEquip = isEquip;

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemEquip = new ItemEquipRequest();
            message.Request.itemEquip.Slot =(int)equip.EquipInfo.Slot;
            message.Request.itemEquip.itemId = equip.Id;
            message.Request.itemEquip.isEquip = isEquip;
            NetClient.Instance.SendMessage(message);

            return true;
        }
        /// <summary>
        /// 收到装备操作请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnItemEquip(object sender, ItemEquipResponse response)
        {
            if (response.Result == Result.Success)
            {
                if (pendingEquip != null)
                {
                    if (this.isEquip)
                        EquipManager.Instance.OnEquipItem(this.pendingEquip);
                    else
                        EquipManager.Instance.OnUnEquipItem(this.pendingEquip.EquipInfo.Slot);
                    pendingEquip = null;
                }
            }
        }
    }
}
