using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 世界元素管理类
/// </summary>
public class UIWorldElement : MonoBehaviour
{
    /// <summary>
    /// 拥有者
    /// </summary>
    public Transform owner;
    /// <summary>
    /// 高度
    /// </summary>
    public float height = 2f;

    private void Update()
    {
        if(owner != null)
        {
            transform.position = owner.position + Vector3.up * height;
        }
        if(Camera.main != null)
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}
