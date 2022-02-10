using Common.Data;
using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// npc控制器
/// </summary>
public class NpcController : MonoBehaviour
{
    /// <summary>
    /// npcID
    /// </summary>
    public int NpcID;
    /// <summary>
    /// Npc动画
    /// </summary>
    private Animator anim;
    /// <summary>
    /// Npc数据
    /// </summary>
    private NpcDefine npc;
    /// <summary>
    /// 渲染组件
    /// </summary>
    private SkinnedMeshRenderer render;
    /// <summary>
    /// 初始颜色
    /// </summary>
    private Color orignColor;
    /// <summary>
    /// 是否在交互状态
    /// </summary>
    private bool inInteractive = false;

    private NpcQuestStatus questStatus;

    private void Start()
    {
        render = this.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        orignColor = render.sharedMaterial.color;
        anim = GetComponent<Animator>();
        npc = NpcManager.Instance.GetNpcDefine(NpcID);
        NpcManager.Instance.UpdateNpcPostion(this.NpcID, this.transform.position);
        this.StartCoroutine(Actions());
        RefreshNpcStatus();
        //注册任务状态变化事件
        QuestManager.Instance.onQuestStatusChanged += OnQuestStatusChanged;
    }
    /// <summary>
    /// 响应任务状态变化事件
    /// </summary>
    /// <param name="quest"></param>
    void OnQuestStatusChanged(Quest quest)
    {
        this.RefreshNpcStatus();
    }

    /// <summary>
    /// 刷新Npc任务状态
    /// </summary>
    private void RefreshNpcStatus()
    {
        questStatus = QuestManager.Instance.GetQuestStatusByNpc(this.NpcID);
        UIWorldElementManager.Instance.AddNpcQuestStatus(this.transform, questStatus);
    }

    private void OnDestroy()
    {
        QuestManager.Instance.onQuestStatusChanged -= OnQuestStatusChanged;
        if (UIWorldElementManager.Instance != null)
        {
            UIWorldElementManager.Instance.RemoveNpcQuestStatus(this.transform);
        }
    }
    /// <summary>
    /// 播放休息动画
    /// </summary>
    /// <returns></returns>
    private IEnumerator Actions()
    {
        while (true)
        {
            if (inInteractive)
            {
                yield return new WaitForSeconds(2f);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
            }
            this.Relax();
        }
    }

    private void Relax()
    {
        anim.SetTrigger("Relax");
    }
    /// <summary>
    /// 交互
    /// </summary>
    private void Interactive()
    {
        if (!inInteractive)
        {
            inInteractive = true;
            StartCoroutine(DoInteractive());
        }
    }
    /// <summary>
    /// 开始交互
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoInteractive()
    {
        yield return FaceToPlayer();
        if (NpcManager.Instance.Interactive(npc))
        {
            anim.SetTrigger("Talk");
        }
        yield return new WaitForSeconds(3f);
        inInteractive = false;
    }
    /// <summary>
    /// 面向玩家
    /// </summary>
    /// <returns></returns>
    private IEnumerator FaceToPlayer()
    {
        Vector3 faceTo = (User.Instance.CurrentCharacterObject.transform.position - this.transform.position).normalized;
        while (Mathf.Abs(Vector3.Angle(this.gameObject.transform.forward, faceTo)) > 5)
        {
            this.gameObject.transform.forward = Vector3.Lerp(this.gameObject.transform.forward, faceTo, Time.deltaTime * 5f);
            yield return null;
        }
    }

    private void OnMouseDown()
    {
        if (Vector3.Distance(this.transform.position, User.Instance.CurrentCharacterObject.transform.position) > 5f)
        {
            User.Instance.CurrentCharacterObject.StartNav(this.transform.position);
        }
        Interactive();    
    }
    private void OnMouseOver()
    {
        Highlight(true);
    }
    private void OnMouseEnter()
    {
        Highlight(true);
    }
    private void OnMouseExit()
    {
        Highlight(false);
    }
    /// <summary>
    /// 高亮
    /// </summary>
    /// <param name="highlight"></param>
    private void Highlight(bool highlight)
    {
        if (highlight)
        {
            if (render.sharedMaterial.color != Color.green)
            {
                render.sharedMaterial.color = Color.green;
            }
        }
        else
        {
            if (render.sharedMaterial.color != orignColor)
            {
                render.sharedMaterial.color = orignColor;
            }
        }
    }
}
