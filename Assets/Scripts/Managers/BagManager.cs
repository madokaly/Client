using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    /// <summary>
    /// 背包管理类
    /// </summary>
    class BagManager:Singleton<BagManager>
    {
        /// <summary>
        /// 解锁的格子
        /// </summary>
        public int Unlocked;
        /// <summary>
        /// 物品
        /// </summary>
        public BagItem[] Items;
        /// <summary>
        /// 背包信息
        /// </summary>
        NBagInfo Info;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="info"></param>
        unsafe public void Init(NBagInfo info)
        {
            this.Info = info;
            this.Unlocked = info.Unlocked;
            Items = new BagItem[this.Unlocked];
            if (info.Items != null && info.Items.Length >= this.Unlocked)
            {
                Analyze(info.Items);
            }
            else
            {
                Info.Items = new byte[sizeof(BagItem) * this.Unlocked];
                Reset();
            }
        }
        /// <summary>
        /// 整理背包
        /// </summary>
        public void Reset()
        {
            int i = 0;
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.Count <= kv.Value.Define.StackLimit)
                {
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)kv.Value.Count;
                }
                else
                {
                    int count = kv.Value.Count;
                    while (count > kv.Value.Define.StackLimit)
                    {
                        this.Items[i].ItemId = (ushort)kv.Key;
                        this.Items[i].Count = (ushort)kv.Value.Define.StackLimit;
                        i++;
                        count -= kv.Value.Define.StackLimit;
                    }
                    this.Items[i].ItemId = (ushort)kv.Key;
                    this.Items[i].Count = (ushort)count;
                }
                i++;
            }
        }
        /// <summary>
        /// 解析二进制数据
        /// </summary>
        /// <param name="data"></param>
        unsafe void Analyze(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    Items[i] = *item;
                }
            }
        }
        /// <summary>
        /// 转换成二进制数据
        /// </summary>
        /// <returns></returns>
        unsafe public NBagInfo GetBagInfo()
        {
            fixed (byte* pt = Info.Items)
            {
                for (int i = 0; i < this.Unlocked; i++)
                {
                    BagItem* item = (BagItem*)(pt + i * sizeof(BagItem));
                    *item = Items[i];
                }
            }
            return this.Info;
        }
        /// <summary>
        /// 增加物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void AddItem(int itemId, int count)
        {
            ushort addCount = (ushort)count;
            for (int i = 0; i < Items.Length; i++)
            {
                if (this.Items[i].ItemId == itemId)
                {
                    ushort canAdd = (ushort)(DataManager.Instance.Items[itemId].StackLimit - Items[i].Count);
                    if (canAdd > count)
                    {
                        this.Items[i].Count += addCount;
                        addCount = 0;
                        break;

                    }
                    else
                    {
                        this.Items[i].Count += canAdd;
                        addCount -= canAdd;
                    }
                }
            }
            if (addCount > 0)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i].ItemId == 0)
                    {
                        this.Items[i].ItemId = (ushort)itemId;
                        this.Items[i].Count = addCount;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        internal void RemoveItem(int itemId, int count)
        {
            throw new NotImplementedException();
        }

    }
}
