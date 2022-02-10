using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特效控制器
/// </summary>
public class EffectController : MonoBehaviour
{
    /// <summary>
    /// 生命周期
    /// </summary>
    public float lifeTime = 1f;
    /// <summary>
    /// 当前时间
    /// </summary>
    float time = 0;
    /// <summary>
    /// 类型
    /// </summary>
    private EffectType type;
    /// <summary>
    /// 目标
    /// </summary>
    private Transform target;
    /// <summary>
    /// 目标位置
    /// </summary>
    private Vector3 targetPos;
    /// <summary>
    /// 开始位置
    /// </summary>
    private Vector3 startPos;
    /// <summary>
    /// 偏移量
    /// </summary>
    private Vector3 offset;

    private void OnEnable()
    {
        if(type != EffectType.Bullet)
        {
            StartCoroutine(Run());
        }
    }
    /// <summary>
    /// 开启非子弹特效
    /// </summary>
    /// <returns></returns>
    private IEnumerator Run()
    {
        yield return new WaitForSeconds(this.lifeTime);
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="type"></param>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="duration">生命周期</param>
    internal void Init(EffectType type, Transform source, Transform target, Vector3 offset, float duration)
    {
        this.type = type;
        this.target = target;
        if(duration > 0)
            this.lifeTime = duration;
        this.time = 0;
        if(type == EffectType.Bullet)
        {
            this.startPos = this.transform.position;
            this.offset = offset;
            this.targetPos = target.position + offset;
        }
        else if (type == EffectType.Hit)
        {
            this.transform.position = target.position + offset;
        }
    }
    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if(type == EffectType.Bullet)
        {
            this.time += Time.deltaTime;
            if(this.target != null)
            {
                this.targetPos = this.target.position + this.offset;
            }
            this.transform.LookAt(this.targetPos);
            if(Vector3.Distance(this.transform.position, this.targetPos) < 0.5f)
            {
                Destroy(this.gameObject);
                return;
            }
            if(this.lifeTime > 0 && this.time >= this.lifeTime)
            {
                Destroy(this.gameObject);
                return;
            }
            this.transform.position = Vector3.Lerp(this.transform.position, this.targetPos, Time.deltaTime / (this.lifeTime - this.time));           
        }
    }
}
