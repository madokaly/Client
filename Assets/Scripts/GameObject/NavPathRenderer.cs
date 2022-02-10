using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 路点渲染
/// </summary>
public class NavPathRenderer : MonoSingleton<NavPathRenderer>
{
    LineRenderer pathRenderer;

    NavMeshPath path;

    private void Start()
    {
        pathRenderer = this.GetComponent<LineRenderer>();
        pathRenderer.enabled = false;
    }
    /// <summary>
    /// 渲染路径
    /// </summary>
    /// <param name="path"></param>
    /// <param name="target"></param>
    public void SetPath(NavMeshPath path, Vector3 target)
    {
        this.path = path;
        if(this.path == null)
        {
            pathRenderer.enabled = false;
            pathRenderer.positionCount = 0;
        }
        else
        {
            pathRenderer.enabled = true;
            //拐点+终点
            pathRenderer.positionCount = path.corners.Length + 1;
            //拐点
            pathRenderer.SetPositions(path.corners);
            //终点
            pathRenderer.SetPosition(pathRenderer.positionCount - 1, target);
            for(int i = 0; i < pathRenderer.positionCount; i++)
            {
                pathRenderer.SetPosition(i, pathRenderer.GetPosition(i) + Vector3.up * 0.2f);
            }
        }
    }
}
