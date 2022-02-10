using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 道具管理类
    /// </summary>
    public class ItemManager : Singleton<ItemManager>
    {
        /// <summary>
        /// 道具字典
        /// </summary>
        public Dictionary<int, Item> Items = new Dictionary<int,Item>();
        /// <summary>
        /// 初始化道具
        /// </summary>
        /// <param name="items"></param>
        public void Init(List<NItemInfo> items)
        {
            this.Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                this.Items.Add(item.Id, item);
                Debug.LogFormat("ItemManager ,ItemId:{0}  Count:{1}",item.Id,item.Count);
            }
            //状态类道具改动事件
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
            Debug.Log("成功注册道具监听事件");
        }
        /// <summary>
        /// 获得道具信息
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public ItemDefine GetItem(int itemId)
        {
            return null;
        }
        /// <summary>
        /// 响应道具改动事件
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
            {
                this.AddItem(status.Id,status.Value);
            }
            if (status.Action == StatusAction.Delete)
            {
                this.RemoveItem(status.Id, status.Value);
            }
            return true;
        }
        /// <summary>
        /// 增加物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void AddItem(int itemId, int count)
        {
            Item item = null;
            if (this.Items.TryGetValue(itemId, out item))
            {
                item.Count += count;
            }
            else
            {
                item = new Item(itemId,count);
                this.Items.Add(itemId, item);
            }
            //背包增加
            BagManager.Instance.AddItem(itemId,count);
        }
        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void RemoveItem(int itemId, int count)
        {
            if (!this.Items.ContainsKey(itemId))
            {
                return;
            }

            Item item =this.Items[itemId];
            if (item.Count < count)
            {
                return;
            }
            item.Count -= count;
            //背包删除
            BagManager.Instance.RemoveItem(itemId,count);
        }


        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool UseItem(ItemDefine item)
        {
            return false;
        }

    }
}

