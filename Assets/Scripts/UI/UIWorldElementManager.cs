using Entities;
using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 世界元素管理器
/// </summary>
public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    /// <summary>
    /// 角色名字血条UI
    /// </summary>
    public GameObject nameBarPrefab;
    /// <summary>
    /// 管理角色名字血条UI字典
    /// </summary>
    private Dictionary<Transform, GameObject> elementNames = new Dictionary<Transform, GameObject>();
    /// <summary>
    /// Npc任务状态UI
    /// </summary>
    public GameObject npcStatusPrefab;
    /// <summary>
    /// 伤害飘字
    /// </summary>
    public GameObject popupTextPrefab;
    /// <summary>
    /// 管理Npc任务状态UI字典
    /// </summary>
    private Dictionary<Transform, GameObject> elementStatus = new Dictionary<Transform, GameObject>();

    protected override void OnStart()
    {
        nameBarPrefab.SetActive(false);
        popupTextPrefab.SetActive(false);
    }

    /// <summary>
    /// 添加角色名字血条UI
    /// </summary>
    /// <param name="owner">拥有者</param>
    /// <param name="character">拥有者角色状态</param>
    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPrefab, transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        elementNames[owner] = goNameBar;
    }
    /// <summary>
    /// 移除角色名字血条UI
    /// </summary>
    /// <param name="owner"></param>
    public void RemoveCharacterNameBar(Transform owner)
    {
        if (elementNames.ContainsKey(owner))
        {
            Destroy(elementNames[owner]);
            elementNames.Remove(owner);
        }
    }
    /// <summary>
    /// 添加Npc任务状态
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="status"></param>
    public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
    {
        if (elementStatus.ContainsKey(owner))
        {
            elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPrefab, transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            elementStatus[owner] = go;
        }
    }
    /// <summary>
    /// 移除Npc任务状态
    /// </summary>
    /// <param name="owner"></param>
    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (elementStatus.ContainsKey(owner))
        {
            Destroy(elementStatus[owner]);
            elementStatus.Remove(owner);
        }
    }
    /// <summary>
    /// 显示伤害飘字
    /// </summary>
    /// <param name="type"></param>
    /// <param name="position"></param>
    /// <param name="damage"></param>
    /// <param name="isCrit"></param>
    public void ShowPopupText(PopupType type, Vector3 position, float damage, bool isCrit)
    {
        GameObject go = Instantiate(popupTextPrefab, position, Quaternion.identity, this.transform);
        go.name = "Popup";
        go.GetComponent<UIPopupText>().InitPopup(type, damage, isCrit);
        go.SetActive(true);
    }
}
