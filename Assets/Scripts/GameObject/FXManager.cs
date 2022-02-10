using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 全局特效管理器
/// </summary>
public class FXManager : MonoSingleton<FXManager>
{
    /// <summary>
    /// 预制件
    /// </summary>
    public GameObject[] prefabs;
    /// <summary>
    /// [名字，预制件]字典
    /// </summary>
    private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();

    protected override void OnStart()
    {
        for(int i = 0; i < prefabs.Length; i++)
        {
            prefabs[i].SetActive(false);
            this.Effects[this.prefabs[i].name] = this.prefabs[i];
        }
    }

    private EffectController CreateEffect(string name, Vector3 pos)
    {
        GameObject prefab;
        if(this.Effects.TryGetValue(name, out prefab))
        {
            GameObject go = Instantiate(prefab, FXManager.Instance.transform, true);
            go.transform.position = pos;
            return go.GetComponent<EffectController>();
        }
        return null;
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="target"></param>
    /// <param name="pos"></param>
    /// <param name="duration"></param>
    internal void PlayEffect(EffectType type, string name, Transform target, Vector3 pos, float duration)
    {
        EffectController effectController = this.CreateEffect(name, pos);
        if(effectController == null)
        {
            Debug.LogFormat("effect :{0} not found", name);
            return;
        }
        effectController.Init(type, this.transform, target, pos, duration);
        effectController.gameObject.SetActive(true);
    }
}
