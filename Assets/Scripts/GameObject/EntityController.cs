using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities;
using Managers;
/// <summary>
/// 实体控制器（其他玩家和当前玩家）
/// </summary>
public class EntityController : MonoBehaviour, IEntityNotify, IEntityController
{

    public Animator anim;
    public Rigidbody rb;
    private AnimatorStateInfo currentBaseState;
    /// <summary>
    /// 实体类
    /// </summary>
    public Entity entity;
    /// <summary>
    /// 位置
    /// </summary>
    public UnityEngine.Vector3 position;
    /// <summary>
    /// 方向
    /// </summary>
    public UnityEngine.Vector3 direction;
    /// <summary>
    /// 角度
    /// </summary>
    Quaternion rotation;
    /// <summary>
    /// 上次位置
    /// </summary>
    public UnityEngine.Vector3 lastPosition;
    /// <summary>
    /// 上次角度
    /// </summary>
    Quaternion lastRotation;
    /// <summary>
    /// 速度
    /// </summary>
    public float speed;

    public float animSpeed = 1.5f;
    /// <summary>
    /// 跳跃
    /// </summary>
    public float jumpPower = 3.0f;

    public bool isPlayer = false;
    /// <summary>
    /// 坐骑控制器
    /// </summary>
    public RideController rideController;
    /// <summary>
    /// 当前坐骑
    /// </summary>
    private int currentRide = 0;
    /// <summary>
    /// 坐骑绑定位置
    /// </summary>
    public Transform rideBone;
    /// <summary>
    /// 特效管理器
    /// </summary>
    public EntityEffectManager EffectMgr;

    void Start () {
        if (entity != null)
        {
            EntityManager.Instance.RegisterEntityChangeNotify(entity.entityId, this);
            this.UpdateTransform();
        }

        if (!this.isPlayer)
            rb.useGravity = false;
    }
    /// <summary>
    /// 更新位置
    /// </summary>
    public void UpdateTransform()
    {
        this.position = GameObjectTool.LogicToWorld(entity.position);
        this.direction = GameObjectTool.LogicToWorld(entity.direction);

        this.rb.MovePosition(this.position);
        this.transform.forward = this.direction;
        this.lastPosition = this.position;
        this.lastRotation = this.rotation;
    }
    /// <summary>
    /// 更新朝向
    /// </summary>
    public void UpdateDirection()
    {
        this.direction = GameObjectTool.LogicToWorld(entity.direction);
        this.transform.forward = this.direction.normalized;
        this.lastRotation = this.rotation;
    }

    void OnDestroy()
    {
        if (entity != null)
            Debug.LogFormat("{0} OnDestroy :ID:{1} POS:{2} DIR:{3} SPD:{4} ", this.name, entity.entityId, entity.position, entity.direction, entity.speed);

        if(UIWorldElementManager.Instance!=null)
        {
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        }
    }

    void FixedUpdate()
    {
        if (this.entity == null)
            return;

        this.entity.OnUpdate(Time.fixedDeltaTime);

        if (!this.isPlayer)
        {
            this.UpdateTransform();
        }
    }
	/// <summary>
    /// 实体状态改变时
    /// </summary>
    /// <param name="entity"></param>
	public void OnEntityChanged(Entity entity)
    {
        Debug.LogFormat("OnEntityChanged :ID:{0} POS:{1} DIR:{2} SPD:{3} ", entity.entityId, entity.position, entity.direction, entity.speed);
    }

    /// <summary>
    /// 实体移除时
    /// </summary>
    public void OnEntityRemoved()
    {
        if(UIWorldElementManager.Instance != null)
            UIWorldElementManager.Instance.RemoveCharacterNameBar(this.transform);
        Destroy(this.gameObject);
    }
    /// <summary>
    /// 实现接口播放动画
    /// </summary>
    /// <param name="entityEvent"></param>
    /// <param name="param"></param>
    public void OnEntityEvent(EntityEvent entityEvent, int param)
    {
        switch(entityEvent)
        {
            case EntityEvent.Idle:
                anim.SetBool("Move", false);
                anim.SetTrigger("Idle");
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
            case EntityEvent.Ride:
                {
                    this.Ride(param);
                }
                break;
        }
        if (this.rideController != null) this.rideController.OnEntityEvent(entityEvent, param);
    }

    /// <summary>
    /// 骑行状态
    /// </summary>
    /// <param name="rideId"></param>
    public void Ride(int rideId)
    {
        if (currentRide == rideId) return;
        currentRide = rideId;
        if (rideId >0)
        {
            this.rideController = GameObjectManager.Instance.LoadRide(rideId, this.transform);
        }
        else
        {
            Destroy(this.rideController.gameObject);
            this.rideController = null;
        }

        if (this.rideController == null)
        {
            this.anim.transform.localPosition = Vector3.zero;
            this.anim.SetLayerWeight(1, 0);
        }
        else
        {
            this.rideController.SetRider(this);
            this.anim.SetLayerWeight(1, 1);
        }
    }
    /// <summary>
    /// 设置坐骑位置
    /// </summary>
    /// <param name="position"></param>
    public void SetRidePotision(Vector3 position)
    {
        this.anim.transform.position = position + (this.anim.transform.position - this.rideBone.position);
    }

    private void OnMouseDown()
    {
        Creature target = this.entity as Creature;
        if (target.IsCurrentPlayer) return;
        BattleManager.Instance.CurrentTarget = target;
    }
    /// <summary>
    /// 播放技能释放动画
    /// </summary>
    /// <param name="name"></param>
    public void PlayAnim(string name)
    {
        this.anim.SetTrigger(name);
    }
    /// <summary>
    /// 设置攻击状态
    /// </summary>
    /// <param name="standby"></param>
    public void SetStandby(bool standby)
    {
        this.anim.SetBool("Standby", standby);
    }

    /// <summary>
    /// 产生子弹特效
    /// </summary>
    /// <param name="type"></param>
    /// <param name="name"></param>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    public void PlayEffect(EffectType type, string name, Creature target, float duration)
    {
        Transform transform = target.Controller.GetTransform();
        if (type == EffectType.Position || type == EffectType.Hit)
        {
            FXManager.Instance.PlayEffect(type, name, transform, target.GetHitOffset(), duration);
        }
        else
        {
            this.EffectMgr.PlayEffect(type, name, transform, target.GetHitOffset(), duration);
        }
    }
    public void PlayEffect(EffectType type, string name, NVector3 position, float duration)
    {
        if(type == EffectType.Position || type == EffectType.Hit)
        {
            FXManager.Instance.PlayEffect(type, name, null, GameObjectTool.LogicToWorld(position), duration);
        }
        else
        {
            this.EffectMgr.PlayEffect(type, name, null, GameObjectTool.LogicToWorld(position), duration);
        }
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}
