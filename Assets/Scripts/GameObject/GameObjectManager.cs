using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using Services;
using SkillBridge.Message;
using Models;
using Managers;

/// <summary>
/// 游戏对象管理器
/// </summary>
public class GameObjectManager : MonoSingleton<GameObjectManager>
{

    Dictionary<int, GameObject> Characters = new Dictionary<int, GameObject>();

    protected override void OnStart()
    {
        StartCoroutine(InitGameObjects());
        CharacterManager.Instance.OnCharacterEnter += OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave += OnCharacterLeave;
    }

    private void OnDestroy()
    {
        CharacterManager.Instance.OnCharacterEnter -= OnCharacterEnter;
        CharacterManager.Instance.OnCharacterLeave -= OnCharacterLeave;
    }

    /// <summary>
    /// 角色进入
    /// </summary>
    /// <param name="cha"></param>
    void OnCharacterEnter(Character cha)
    {
        CreateCharacterObject(cha);
    }
    /// <summary>
    /// 角色离开
    /// </summary>
    /// <param name="character"></param>
    /// <param name="entityId">内存id唯一</param>
    void OnCharacterLeave(Character character)
    {
        if (!Characters.ContainsKey(character.entityId))
            return;

        if (Characters[character.entityId] != null)
        {
            Destroy(Characters[character.entityId]);
            this.Characters.Remove(character.entityId);
        }
    }
    /// <summary>
    /// 初始化管理器已存在所有角色
    /// </summary>
    /// <returns></returns>
    IEnumerator InitGameObjects()
    {
        foreach (var cha in CharacterManager.Instance.Characters.Values)
        {
            CreateCharacterObject(cha);
            yield return null;
        }
    }
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="character"></param>
    private void CreateCharacterObject(Character character)
    {
        //判断角色是否已经添加入管理器
        if (!Characters.ContainsKey(character.entityId) || Characters[character.entityId] == null)
        {
            Object obj = Resloader.Load<Object>(character.Define.Resource);
            if(obj == null)
            {
                Debug.LogErrorFormat("Character[{0}] Resource[{1}] not existed.",character.Define.TID, character.Define.Resource);
                return;
            }
            GameObject go = (GameObject)Instantiate(obj, this.transform);
            go.name = "Character_" + character.Id + "_" + character.Name;
            Characters[character.entityId] = go;
            //初始化世界角色面板
            UIWorldElementManager.Instance.AddCharacterNameBar(go.transform, character);
        }
        this.InitGameObject(Characters[character.entityId], character);
    }
    /// <summary>
    /// 初始化角色
    /// </summary>
    /// <param name="go"></param>
    /// <param name="character"></param>
    private void InitGameObject(GameObject go, Character character)
    {
        go.transform.position = GameObjectTool.LogicToWorld(character.position);
        go.transform.forward = GameObjectTool.LogicToWorld(character.direction);
        Characters[character.entityId] = go;
        EntityController ec = go.GetComponent<EntityController>();
        if (ec != null)
        {
            ec.entity = character;
            ec.isPlayer = character.IsCurrentPlayer;
            ec.Ride(character.Info.Ride);
            character.Controller = ec;
        }
        PlayerInputController pc = go.GetComponent<PlayerInputController>();
        if (pc != null)
        {
            //角色是否为当前玩家
            if (character.IsCurrentPlayer)
            {
                User.Instance.CurrentCharacterObject = pc;
                MainPlayerCamera.Instance.player = go;
                pc.enabled = true;
                pc.character = character;
                pc.entityController = ec;
            }
            else
            {
                pc.enabled = false;
            }
        }
    }

    /// <summary>
    /// 加载坐骑
    /// </summary>
    /// <param name="rideId"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public RideController LoadRide(int rideId, Transform parent)
    {
        var rideDefine = DataManager.Instance.Rides[rideId];
        Object obj = Resloader.Load<Object>(rideDefine.Resource);
        if (obj == null)
        {
            Debug.LogErrorFormat("Ride[{0}] Resource[{1}] not existed.", rideDefine.ID, rideDefine.Resource);
            return null;
        }
        GameObject go = (GameObject)Instantiate(obj, parent);
        go.name = "Ride_" + rideDefine.ID + "_" + rideDefine.Name;
        return go.GetComponent<RideController>();
    }
}

