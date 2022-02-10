using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 技能指示器
/// </summary>
public class TargetSelector : MonoSingleton<TargetSelector>
{
    /// <summary>
    /// 投影组件
    /// </summary>
    private Projector projector;

    private bool actived = false;
    /// <summary>
    /// 中心点
    /// </summary>
    private Vector3 center;
    /// <summary>
    /// 范围
    /// </summary>
    private float range;
    /// <summary>
    /// 大小
    /// </summary>
    private float size;

    Vector3 offset = new Vector3(0f, 2f, 0f);

    protected Action<Vector3> selectPoint;

    protected override void OnStart()
    {
        projector = this.GetComponentInChildren<Projector>();
        projector.gameObject.SetActive(actived);
    }
    /// <summary>
    /// 设置激活状态
    /// </summary>
    /// <param name="active"></param>
    public void Active(bool active)
    {
        this.actived = active;
        if (this.projector == null) return;
        projector.gameObject.SetActive(this.actived);
        projector.orthographicSize = this.size * 0.5f;
    }
    /// <summary>
    /// 更新位置
    /// </summary>
    private void Update()
    {
        if (!actived) return;
        if (this.projector == null) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Terrain")))
        {
            Vector3 hitPoint = hit.point;
            Vector3 dist = hitPoint - this.center;
            if(dist.magnitude > this.range)
            {
                hitPoint = this.center + dist.normalized * this.range;
            }
            this.projector.gameObject.transform.position = hitPoint + offset;
            if (Input.GetMouseButtonDown(0))
            {
                this.selectPoint(hitPoint);
                this.Active(false);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            this.Active(false);
        }
    }
    /// <summary>
    /// 显示技能指示器
    /// </summary>
    /// <param name="center"></param>
    /// <param name="range"></param>
    /// <param name="size"></param>
    /// <param name="onPositionSelected"></param>
    public static void ShowSelector(Vector3Int center, int range, int size, Action<Vector3> onPositionSelected)
    {
        if (TargetSelector.Instance == null) return;
        TargetSelector.Instance.center = GameObjectTool.LogicToWorld(center);
        TargetSelector.Instance.range = GameObjectTool.LogicToWorld(range);
        TargetSelector.Instance.size = GameObjectTool.LogicToWorld(size);
        //注册委托
        TargetSelector.Instance.selectPoint = onPositionSelected;
        TargetSelector.Instance.Active(true);
    }

}
