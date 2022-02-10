using Common.Battle;
using Managers;
using Models;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// UI角色装备类
/// </summary>
public class UICharEquip : UIWindow
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 金钱
    /// </summary>
    public Text money;
    /// <summary>
    /// 装备
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 已穿戴装备
    /// </summary>
    public GameObject itemEquipPrefab;
    /// <summary>
    /// 装备列表
    /// </summary>
    public Transform itemListRoot;
    /// <summary>
    /// 已装备列表
    /// </summary>
    public List<Transform> slots;
    /// <summary>
    /// 已选择装备
    /// </summary>
    [HideInInspector]
    public UIEquipItem selectedUIEquipItem;
    /// <summary>
    /// 血量
    /// </summary>
    public Text hp;
    public Slider hpBar;
    /// <summary>
    /// 蓝量
    /// </summary>
    public Text mp;
    public Slider mpBar;
    /// <summary>
    /// 属性数组
    /// </summary>
    public Text[] attrs;

    public Text nameLV;

    private void Start()
    {
        RefreshUI();
        //注册装备改变事件
        EquipManager.Instance.OnEquipChanged += RefreshUI;
    }
    private void OnDestroy()
    {
        EquipManager.Instance.OnEquipChanged -= RefreshUI;
        Debug.Log("消灭掉了");
    }
    /// <summary>
    /// 刷新UI
    /// </summary>
    private void RefreshUI()
    {
        ClearAllEquipList();
        InitAllEquipItems();
        ClearEquipList();
        InitEquipedItems();
        this.money.text = User.Instance.CurrentCharacterInfo.Gold.ToString();
        InitAttributes();
    }
    /// <summary>
    /// 清理左边装备列表
    /// </summary>
    private void ClearAllEquipList()
    {
        foreach (var item in itemListRoot.GetComponentsInChildren<UIEquipItem>())
        {
            Destroy(item.gameObject);
        }
    }

    //初始化左边装备列表
    private void InitAllEquipItems()
    {
        foreach (var kv in ItemManager.Instance.Items)
        {
            if (kv.Value.Define.Type == ItemType.Equip && kv.Value.Define.LimitClass == User.Instance.CurrentCharacterInfo.Class)
            {
                //已经装备就不显示了
                if (EquipManager.Instance.Contains(kv.Key))
                {
                    continue;
                }
                GameObject go = Instantiate(itemPrefab, itemListRoot);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                ui.SetEquipItem(kv.Key, kv.Value, this, false);

            }
        }
    }
    /// <summary>
    /// 清理右边装备栏
    /// </summary>
    private void ClearEquipList()
    {
        foreach (var item in slots)
        {
            if (item.childCount>0)
            {
                Destroy(item.GetChild(0).gameObject);
            }
        }
    }

    //初始化右边装备栏
    private void InitEquipedItems()
    {
        for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
        {
            //获得装备栏装备
            var item = EquipManager.Instance.Equips[i];
            if (item != null)
            {
                GameObject go = Instantiate(itemEquipPrefab, slots[i]);
                UIEquipItem ui = go.GetComponent<UIEquipItem>();
                //设置已装备UI
                ui.SetEquipItem(i, item, this, true);
            }
        }
    }
    /// <summary>
    /// 初始化属性栏
    /// </summary>
    private void InitAttributes()
    {
        var charattr = User.Instance.CurrentCharacter.Attributes;
        this.nameLV.text = User.Instance.CurrentCharacterInfo.Name + "  LV." + User.Instance.CurrentCharacterInfo.Level;
        this.hp.text = string.Format("{0}/{1}", charattr.HP, charattr.MaxHP);
        this.mp.text = string.Format("{0}/{1}", charattr.MP, charattr.MaxMP);
        this.hpBar.maxValue = charattr.MaxHP;
        this.hpBar.value = charattr.HP;
        this.mpBar.maxValue = charattr.MaxMP;
        this.mpBar.value = charattr.MP;

        for(int i = (int)AttributeType.STR; i < (int)AttributeType.MAX; i++)
        {
            if (i == (int)AttributeType.CRI) this.attrs[i - 2].text = string.Format("{0:f2}%", charattr.Final.Data[i] * 100);
            else this.attrs[i - 2].text = ((int)charattr.Final.Data[i]).ToString();
        }
    }

    public void DoEquip(Item item)
    {
        EquipManager.Instance.EquipItem(item);
    }

    public void UnEquip(Item item)
    {
        EquipManager.Instance.UnEquipItem(item);
    }








}
