using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;
using UnityEngine.Events;

using Entities;
using SkillBridge.Message;
using Models;

namespace Managers
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    class CharacterManager : Singleton<CharacterManager>, IDisposable
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();


        public UnityAction<Character> OnCharacterEnter;
        public UnityAction<Character> OnCharacterLeave;

        public CharacterManager()
        {

        }

        public void Dispose()
        {
        }

        public void Init()
        {

        }

        public void Clear()
        {
            int[] keys = this.Characters.Keys.ToArray();
            foreach (var key in keys)
            {
                this.RemoveCharacter(key);
            }
            this.Characters.Clear();
        }
        /// <summary>
        /// 增加角色
        /// </summary>
        /// <param name="cha"></param>
        public void AddCharacter(Character character)
        {
            Debug.LogFormat("AddCharacter:{0}:{1} Map:{2} Entity:{3}", character.Id, character.Name, character.Info.mapId, character.entityId);
            this.Characters[character.entityId] = character;
            EntityManager.Instance.AddEntity(character);
            if (OnCharacterEnter != null)
            {
                OnCharacterEnter(character);
            }
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="entityId"></param>
        public void RemoveCharacter(int entityId)
        {
            Debug.LogFormat("RemoveCharacter:{0}", entityId);
            if (this.Characters.ContainsKey(entityId))
            {
                EntityManager.Instance.RemoveEntity(this.Characters[entityId].Info.Entity);
                if (OnCharacterLeave != null)
                {
                    OnCharacterLeave(this.Characters[entityId]);
                }
                this.Characters.Remove(entityId);
            }
        }
        /// <summary>
        /// 获得角色实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Character GetCharacter(int id)
        {
            Character character;
            this.Characters.TryGetValue(id, out character);
            return character;
        }
    }
}
