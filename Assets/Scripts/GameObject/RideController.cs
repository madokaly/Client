using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;
/// <summary>
/// 坐骑控制器
/// </summary>
public class RideController : MonoBehaviour
{
    /// <summary>
    /// 骑乘点
    /// </summary>
    public Transform mountPoint;
    /// <summary>
    /// 骑乘者
    /// </summary>
    public EntityController rider;
    /// <summary>
    /// 偏移量
    /// </summary>
    public Vector3 offset;

    private Animator anim;

    private void Start()
    {
        this.anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (this.mountPoint == null || this.rider == null) return;
        this.rider.SetRidePotision(this.mountPoint.position + this.mountPoint.TransformDirection(this.offset));
    }

    public void SetRider(EntityController rider)
    {
        this.rider = rider;
    }
    /// <summary>
    /// 根据状态播放动画
    /// </summary>
    /// <param name="entityEvent"></param>
    /// <param name="param"></param>
    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch (entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                break;
            case EntityEvent.MoveFwd:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.MoveBack:
                anim.SetBool("Move", true);
                break;
            case EntityEvent.Jump:
                anim.SetTrigger("Jump");
                break;
        }
    }
}
