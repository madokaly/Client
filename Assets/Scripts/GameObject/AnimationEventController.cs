using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画事件控制器
/// </summary>
public class AnimationEventController : MonoBehaviour
{
    /// <summary>
    /// 特效管理器
    /// </summary>
    public EntityEffectManager EffectMgr;

    public void PlayEffect(string name)
    {
        Debug.LogFormat("AnimationEventController:PlayEffect: {0} : {1}", this.name, name);
        EffectMgr.PlayEffect(name);
    }

    public void PlaySound(string name)
    {
        Debug.LogFormat("AnimationEventController:PlaySound: {0} : {1}", this.name, name);
    }
}
