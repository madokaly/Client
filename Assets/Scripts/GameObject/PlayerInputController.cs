using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Entities;
using SkillBridge.Message;
using Services;
/// <summary>
/// 玩家输入控制器
/// </summary>
public class PlayerInputController : MonoBehaviour
{
    /// <summary>
    /// 刚体
    /// </summary>
    public Rigidbody rb;
    /// <summary>
    /// 状态
    /// </summary>
    SkillBridge.Message.CharacterState state;
    /// <summary>
    /// 玩家
    /// </summary>
    public Character character;
    /// <summary>
    /// 转向速度
    /// </summary>
    public float rotateSpeed = 2.0f;
    /// <summary>
    /// 同步转向最小角度
    /// </summary>
    public float turnAngle = 10;
    /// <summary>
    /// 速度
    /// </summary>
    public int speed;
    /// <summary>
    /// 实体控制器
    /// </summary>
    public EntityController entityController;
    /// <summary>
    /// 
    /// </summary>
    public bool onAir = false;
    /// <summary>
    /// 导航代理
    /// </summary>
    private NavMeshAgent agent;
    /// <summary>
    /// 是否自动寻路
    /// </summary>
    private bool autoNav = false;
    /// <summary>
    /// 是否启用刚体
    /// </summary>
    public bool enableRigidbody
    {
        get { return !this.rb.isKinematic; }
        set
        {
            this.rb.isKinematic = !value;
            this.rb.detectCollisions = value;
        }
    }

    private void Start()
    {
        state = CharacterState.Idle;
        /*if (this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.ConfigId = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            cinfo.attrDynamic = new NAttributeDynamic();
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }*/
        if(agent == null)
        {
            agent = this.gameObject.AddComponent<NavMeshAgent>();
            agent.stoppingDistance = 0.3f;
            agent.updatePosition = false;
        }
    }
    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="target"></param>
    public void StartNav(Vector3 target)
    {
        StartCoroutine(BeginNav(target));
    }
    /// <summary>
    /// 开始寻路
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator BeginNav(Vector3 target)
    {
        agent.updatePosition = true;
        agent.SetDestination(target);
        yield return null;
        autoNav = true;
        if(state != CharacterState.Move)
        {
            state = CharacterState.Move;
            this.character.MoveForward();
            this.SendEntityEvent(EntityEvent.MoveFwd);
            agent.speed = this.character.speed / 100f;
        }
    }
    /// <summary>
    /// 停止寻路
    /// </summary>
    public void StopNav()
    {
        autoNav = false;
        agent.ResetPath();
        if(state != CharacterState.Idle)
        {
            state = CharacterState.Idle;
            this.rb.velocity = Vector3.zero;
            this.character.Stop();
            this.SendEntityEvent(EntityEvent.Idle);
        }
        agent.updatePosition = false;
        NavPathRenderer.Instance.SetPath(null, Vector3.zero);
    }
    /// <summary>
    /// 自动寻路
    /// </summary>
    public void NavMove()
    {
        //寻路是否完成，未完成true
        if (agent.pathPending) return;
        if(agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            StopNav();
            return;
        }
        //寻路是否完成
        if (agent.pathStatus != NavMeshPathStatus.PathComplete) return;
        //停止自动寻路
        if(Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            StopNav();
            return;
        }
        NavPathRenderer.Instance.SetPath(agent.path, agent.destination);
        if(agent.isStopped || agent.remainingDistance < 0.3f)
        {
            StopNav();
            return;
        }
    }


    private void FixedUpdate()
    {
        //为空或者在切换场景
        if (character == null || !character.ready)
            return;
        if (autoNav)
        {
            NavMove();
            return;
        }
        if (InputManager.Instance != null && InputManager.Instance.IsInputMode) return;

        //移动
        float v = Input.GetAxis("Vertical");
        if (v > 0.01)
        {
            //向前
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01)
        {
            //向后
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            //待机
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }
        //转向
        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }

        }
        Debug.LogFormat("velocity {0}", this.rb.velocity.magnitude);
    }
    /// <summary>
    /// 同步前位置
    /// </summary>
    Vector3 lastPos;
    float lastSync = 0;

    private void LateUpdate()
    {
        if (this.character == null || !character.ready) return;

        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        Vector3Int goLogicPos = GameObjectTool.WorldToLogic(this.rb.transform.position);
        //更新逻辑位置和实际位置
        float logicOffset = (goLogicPos - this.character.position).magnitude;
        if (logicOffset > 100)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position;

        //更新朝向
        Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
        Quaternion rot = new Quaternion();
        rot.SetFromToRotation(dir, this.transform.forward);
        if(rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
        {
            character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
            this.SendEntityEvent(EntityEvent.None);
        }
    }
    /// <summary>
    /// 发送同步信息
    /// </summary>
    /// <param name="entityEvent"></param>
    /// <param name="param"></param>
    public void SendEntityEvent(EntityEvent entityEvent, int param = 0)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent, param);
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData, param);
    }

    /// <summary>
    /// 离开关卡时
    /// </summary>
    public void OnLeaveLevel()
    {
        this.enableRigidbody = false;
        this.rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// 进入关卡时
    /// </summary>
    public void OnEnterLevel()
    {
        this.rb.velocity = Vector3.zero;
        this.entityController.UpdateTransform();
        this.lastPos = this.rb.transform.position;
        this.enableRigidbody = true;
    }
}
