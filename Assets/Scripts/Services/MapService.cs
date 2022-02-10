using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Common.Data;
using Managers;
using Entities;

namespace Services
{
    /// <summary>
    /// 地图服务类
    /// </summary>
    class MapService : Singleton<MapService>, IDisposable
    {

        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

            SceneManager.Instance.onSceneLoadDone += OnLoadDone;

        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

            SceneManager.Instance.onSceneLoadDone -= OnLoadDone;
        }

        /// <summary>
        /// 当前地图id
        /// </summary>
        public int CurrentMapId { get; set; }
        /// <summary>
        /// 是否加载完地图
        /// </summary>
        private bool loadingDone = true;

        public void Init()
        {

        }

        /// <summary>
        /// 角色进入地图事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response">进入的角色</param>
        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:{0} [{1}]", response.mapId, response.Characters.Count);
            foreach (var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacterInfo == null || (cha.Type == CharacterType.Player && User.Instance.CurrentCharacterInfo.Id == cha.Id))
                {
                    //当前玩家切换地图
                    User.Instance.CurrentCharacterInfo = cha;
                    if (User.Instance.CurrentCharacter == null)
                    {
                        User.Instance.CurrentCharacter = new Character(cha);
                    }
                    else
                    {
                        User.Instance.CurrentCharacter.UpdateInfo(cha);
                    }
                    User.Instance.CurrentCharacter.ready = false;
                    //调用技能槽刷新的事件
                    User.Instance.CharacterInited();
                    CharacterManager.Instance.AddCharacter(User.Instance.CurrentCharacter);
                    //切换地图
                    if (CurrentMapId != response.mapId)
                    {
                        EnterMap(response.mapId);
                        CurrentMapId = response.mapId;
                    }
                    continue;
                }
                //其他生物
                CharacterManager.Instance.AddCharacter(new Character(cha));
            }   
        }
        /// <summary>
        /// 切换地图
        /// </summary>
        /// <param name="mapId">地图id</param>
        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                this.loadingDone = false;
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
                SoundManager.Instance.PlayMusic(map.Music);
            }
            else
            {
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
            }
        }

        /// <summary>
        /// 角色离开地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave：CharacterId: {0} ", response.entityId);
            if (response.entityId != User.Instance.CurrentCharacterInfo.EntityId)
            {
                CharacterManager.Instance.RemoveCharacter(response.entityId);
            }
            else
            {
                if(User.Instance.CurrentCharacterObject != null)
                {
                    User.Instance.CurrentCharacterObject.OnLeaveLevel();
                }
                CharacterManager.Instance.Clear();
            }
        }
        /// <summary>
        /// 发送角色同步信息
        /// </summary>
        /// <param name="entityEvent"></param>
        /// <param name="entity"></param>
        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity, int param)
        {
            if (!this.loadingDone) return;
            Debug.LogFormat("MapEntityUpdateRequest: EntityID:{0} POS:{1} DIR:{2} SPD:{3}", entity.Id, entity.Position.String(), entity.Direction.String(), entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity,
                Param = param
            };
            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 实体同步响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            if (!this.loadingDone) return;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entity:{0}", response.entitySyncs.Count);
            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
                sb.AppendFormat("   [{0}]evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity);
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }
        /// <summary>
        /// 发送地图传送请求
        /// </summary>
        /// <param name="iD"></param>
        public void SendMapTeleporter(int teleporterID)
        {
            Debug.LogFormat("MapTeleporterRequest : teleporterID:[{0}]", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }

        private void OnLoadDone()
        {
            if(User.Instance.CurrentCharacter != null)
                User.Instance.CurrentCharacter.ready = true;
            if (User.Instance.CurrentCharacterObject != null)
            {
                User.Instance.CurrentCharacterObject.OnEnterLevel();
            }
            this.loadingDone = true;
        }
    }
}
