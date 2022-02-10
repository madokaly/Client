using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;
using Models;
using Services;
using SkillBridge.Message;

namespace Managers
{
    /// <summary>
    /// 装备管理器
    /// </summary>
    class EquipManager:Singleton<EquipManager>
    {
        /// <summary>
        /// 装备改变委托
        /// </summary>
        public delegate void OnEquipChangedHandler();
        /// <summary>
        /// 装备改变事件
        /// </summary>
        public event OnEquipChangedHandler OnEquipChanged;
        /// <summary>
        /// 可装备数组
        /// </summary>
        public Item[] Equips = new Item[(int)EquipSlot.SlotMax];
        /// <summary>
        /// 二进制装备数据
        /// </summary>
        byte[] Data;
        /// <summary>
        /// 初始化装备
        /// </summary>
        /// <param name="data"></param>
        unsafe public void Init(byte[] data)
        {
            this.Data = data;
            this.ParseEquipData(data);
        }
        /// <summary>
        /// 是否已穿戴该装备
        /// </summary>
        /// <param name="equipId"></param>
        /// <returns></returns>
        public bool Contains(int equipId)
        {
            for (int i = 0; i < this.Equips.Length; i++)
            {
                if (Equips[i] != null && Equips[i].Id == equipId)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获得装备
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public Item GetItem(EquipSlot slot)
        {
            return Equips[(int)slot];
        }
        /// <summary>
        /// 二进制转换成装备
        /// </summary>
        /// <param name="data"></param>
        unsafe void ParseEquipData(byte[] data)
        {
            fixed (byte* pt = data)
            {
                for (int i = 0; i < this.Equips.Length; i++)
                {
                    int itemId = *(int*)(pt + i * sizeof(int));
                    if (itemId > 0)
                    {
                        Equips[i] = ItemManager.Instance.Items[itemId];
                    }
                    else
                        Equips[i] = null;
                }
            }
        }
        /// <summary>
        /// 装备转换成二进制
        /// </summary>
        /// <returns></returns>
        unsafe public byte[] GetEquipData()
        {
            fixed (byte* pt = Data)
            {
                for (int i = 0; i <(int)EquipSlot.SlotMax; i++)
                {
                    int* itemId = (int*)(pt + i * sizeof(int));
                    if (Equips[i] == null)
                    {
                        *itemId = 0;
                    }
                    else
                        *itemId = Equips[i].Id;
                }
            }
            return this.Data;
        }
        /// <summary>
        /// 穿戴装备
        /// </summary>
        /// <param name="equip"></param>
        public void EquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip,true);
        }
        /// <summary>
        /// 脱下装备
        /// </summary>
        /// <param name="equip"></param>
        public void UnEquipItem(Item equip)
        {
            ItemService.Instance.SendEquipItem(equip, false);
        }
        /// <summary>
        /// 响应穿戴装备事件
        /// </summary>
        /// <param name="equip"></param>
        public void OnEquipItem(Item equip)
        {
            if (this.Equips[(int)equip.EquipInfo.Slot] != null && this.Equips[(int)equip.EquipInfo.Slot].Id == equip.Id)
            {
                return;
            }
            this.Equips[(int)equip.EquipInfo.Slot] = ItemManager.Instance.Items[equip.Id];
            if (OnEquipChanged != null)
            {
                OnEquipChanged();
            }
        }
        /// <summary>
        /// 响应脱下装备事件
        /// </summary>
        /// <param name="slot"></param>
        public void OnUnEquipItem(EquipSlot slot)
        {
            if (this.Equips[(int)slot] != null)
            {
                this.Equips[(int)slot] = null;
                if (OnEquipChanged != null)
                {
                    OnEquipChanged();
                }
            }
        }
        /// <summary>
        /// 获取所有已装备的信息
        /// </summary>
        /// <returns></returns>
        public List<EquipDefine> GetEquipDefines()
        {
            List<EquipDefine> result = new List<EquipDefine>();
            for(int i = 0; i < (int)EquipSlot.SlotMax; i++)
            {
                if(Equips[i] != null)
                {
                    result.Add(Equips[i].EquipInfo);
                }
            }
            return result;
        }
    }
}
