using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SkillBridge.Message;

namespace Entities
{
    /// <summary>
    /// 角色实体类
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// 角色唯一ID
        /// </summary>
        public int entityId;

        public Vector3Int position;

        public Vector3Int direction;

        public int speed;

        public bool ready = true;

        public IEntityController Controller;

        /// <summary>
        /// 协议角色实体
        /// </summary>
        private NEntity entityData;
        public NEntity EntityData
        {
            get {
                UpdateEntityData();
                return entityData;
            }
            set {
                entityData = value;
                this.SetEntityData(value);
            }
        }

        public Entity(NEntity entity)
        {
            this.SetEntityData(entity);
        }

        public virtual void OnUpdate(float delta)
        {
            if (this.speed != 0)
            {
                Vector3 dir = this.direction;
                this.position += Vector3Int.RoundToInt(dir * speed * delta / 100f);
            }
        }
        /// <summary>
        /// 设置实体信息
        /// </summary>
        /// <param name="entity"></param>
        public void SetEntityData(NEntity entity)
        {
            if (!ready) return;
            this.entityId = entity.Id;
            this.entityData = entity;
            this.position = this.position.FromNVector3(entity.Position);
            this.direction = this.direction.FromNVector3(entity.Direction);
            this.speed = entity.Speed;
        }
        /// <summary>
        /// 更新实体信息
        /// </summary>
        public void UpdateEntityData()
        {
            entityData.Position.FromVector3Int(this.position);
            entityData.Direction.FromVector3Int(this.direction);
            entityData.Speed = this.speed;
        }
    }
}
