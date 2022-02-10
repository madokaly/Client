using Battle;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BuffUI
/// </summary>
public class UIBuffIcons : MonoBehaviour
{
    /// <summary>
    /// 拥有者
    /// </summary>
    private Creature owner;
    /// <summary>
    /// Buff元素预制件
    /// </summary>
    public GameObject prefabBuff;
    /// <summary>
    /// [buffId,buff元素UI]字典
    /// </summary>
    private Dictionary<int, GameObject> buffs = new Dictionary<int, GameObject>();

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="owner"></param>
    public void SetOwner(Creature owner)
    {
        if(this.owner != null && this.owner != owner)
        {
            this.Clear();
        }
        this.owner = owner;
        //监听事件
        this.owner.OnBuffAdd += OnBuffAdd;
        this.owner.OnBuffRemove += OnBuffRemove;
        //初始化已有buff
        this.InitBuffs();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitBuffs()
    {
        foreach(var buff in this.owner.BuffMgr.Buffs)
        {
            this.OnBuffAdd(buff.Value);
        }
    }

    /// <summary>
    /// 清理
    /// </summary>
    private void Clear()
    {
        if(this.owner != null)
        {
            this.owner.OnBuffAdd -= OnBuffAdd;
            this.owner.OnBuffRemove -= OnBuffRemove;
        }
        foreach(var buff in buffs)
        {
            Destroy(buff.Value);
        }
        this.buffs.Clear();
    }

    /// <summary>
    /// buff添加事件响应
    /// </summary>
    /// <param name="buff"></param>
    private void OnBuffAdd(Buff buff)
    {
        GameObject go = Instantiate(prefabBuff, this.transform);
        go.name = buff.Define.ID.ToString();
        UIBuffItem item = go.GetComponent<UIBuffItem>();
        item.SetItem(buff);
        go.SetActive(true);
        this.buffs[buff.BuffId] = go;
    }
    /// <summary>
    /// buff移除事件响应
    /// </summary>
    /// <param name="buff"></param>
    private void OnBuffRemove(Buff buff)
    {
        GameObject go;
        if (this.buffs.TryGetValue(buff.BuffId, out go))
        {
            this.buffs.Remove(buff.BuffId);
            Destroy(go);
        }
    }

    private void Awake()
    {
        prefabBuff.SetActive(false);
    }

    private void OnDestroy()
    {
        this.Clear();
    }
}
