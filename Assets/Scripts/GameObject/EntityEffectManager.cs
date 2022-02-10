using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效管理器
/// </summary>
public class EntityEffectManager : MonoBehaviour
{
    /// <summary>
    /// 特效根节点
    /// </summary>
    public Transform Root;
    /// <summary>
    /// [特效名字，特效预制件]字典
    /// </summary>
    private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();
    /// <summary>
    /// 特效数组
    /// </summary>
    public Transform[] Props;

    private void Start()
    {
        this.Effects.Clear();
        if(this.Root != null && this.Root.childCount > 0)
        {
            for(int i = 0; i < this.Root.childCount; i++)
            {
                this.Effects[this.Root.GetChild(i).name] = this.Root.GetChild(i).gameObject;
            }
        }
        if(Props != null)
        {
            for (int i = 0; i < this.Props.Length; i++)
            {
                this.Effects[this.Props[i].name] = this.Props[i].gameObject;
            }
        }
    }
    /// <summary>
    /// 动画事件特效
    /// </summary>
    /// <param name="name"></param>
    public void PlayEffect(string name)
    {
        Debug.LogFormat("PlayEffect: {0} : {1}", this.name, name);
        if (this.Effects.ContainsKey(name))
        {
            this.Effects[name].SetActive(true);
        }
    }
    /// <summary>
    /// 产生特效
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    internal void PlayEffect(EffectType type, string name, Transform target, Vector3 pos, float duration)
    {
        if (type == EffectType.Bullet)
        {
            EffectController effect = this.InstantiateEffect(name);
            effect.Init(type, this.transform, target, pos, duration);
            effect.gameObject.SetActive(true);
        }
        else
        {
            PlayEffect(name);
        }
    }

    //private EffectController InstantiateEffect(string name)
    //{
    //    GameObject prefab;
    //    if(this.Effects.TryGetValue(name, out prefab))
    //    {
    //        GameObject go = Instantiate(prefab, GameObjectManager.Instantiate.transform, true);
    //        go.transform.position = prefab.transform.position;
    //        go.transform.rotation = prefab.transform.rotation;
    //        return go.GetComponent<EffectController>();
    //    }
    //    return null;
    //}

    /// <summary>
    /// 实例化
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private EffectController InstantiateEffect(string name)
    {
        GameObject prefab;
        if(this.Effects.TryGetValue(name, out prefab))
        {
            GameObject go = Instantiate(prefab, GameObjectManager.Instance.transform, true);
            go.transform.position = prefab.transform.position;
            go.transform.rotation = prefab.transform.rotation;
            return go.GetComponent<EffectController>();
        }
        return null;
    }

}
