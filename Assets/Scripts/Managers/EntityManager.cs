using Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    interface IEntityNotify
    {
        void OnEntityRemoved();
        void OnEntityChanged(Entity entity);
        void OnEntityEvent(EntityEvent @event,int param);
    }
    class EntityManager : Singleton<EntityManager>
    {
        /// <summary>
        /// 唯一ID和实体类字典
        /// </summary>
        Dictionary<int, Entity> entities = new Dictionary<int, Entity>();
        /// <summary>
        /// 唯一ID和事件字典
        /// </summary>
        Dictionary<int, IEntityNotify> notifiers = new Dictionary<int, IEntityNotify>();
        /// <summary>
        /// 注册实体事件
        /// </summary>
        /// <param name="entityId">唯一ID</param>
        /// <param name="notify">事件接口</param>
        public void RegisterEntityChangeNotify(int entityId, IEntityNotify notify)
        {
            this.notifiers[entityId] = notify;
        }

        public void AddEntity(Entity entity)
        {
            entities[entity.entityId] = entity;
        }

        public void RemoveEntity(NEntity entity)
        {
            this.entities.Remove(entity.Id);
            if (notifiers.ContainsKey(entity.Id))
            {
                notifiers[entity.Id].OnEntityRemoved();
                notifiers.Remove(entity.Id);
            }
        }
        /// <summary>
        /// 实体同步响应事件
        /// </summary>
        /// <param name="data"></param>
        internal void OnEntitySync(NEntitySync data)
        {
            Entity entity = null;
            entities.TryGetValue(data.Id, out entity);
            if (entity != null)
            {
                if (data.Entity != null)
                {
                    entity.EntityData = data.Entity;
                }
                if (notifiers.ContainsKey(data.Id))
                {
                    notifiers[entity.entityId].OnEntityChanged(entity);
                    notifiers[entity.entityId].OnEntityEvent(data.Event,data.Param);
                }
            }
        }
        /// <summary>
        /// 获得Entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public Entity GetEntity(int entityId)
        {
            Entity entity = null;
            entities.TryGetValue(entityId, out entity);
            return entity;
        }
    }
}
