using Entities;
using Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Services
{
    /// <summary>
    /// 战斗服务类
    /// </summary>
    public class BattleService : Singleton<BattleService>, IDisposable
    {

        public void Init()
        {

        }

        public BattleService()
        {
            MessageDistributer.Instance.Subscribe<SkillCastResponse>(this.OnSkillCast);
            MessageDistributer.Instance.Subscribe<SkillHitResponse>(this.OnSkillHit);
            MessageDistributer.Instance.Subscribe<BuffResponse>(this.OnBuff);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<SkillCastResponse>(this.OnSkillCast);
            MessageDistributer.Instance.Unsubscribe<SkillHitResponse>(this.OnSkillHit);
            MessageDistributer.Instance.Unsubscribe<BuffResponse>(this.OnBuff);
        }
        /// <summary>
        /// 发送释放技能请求
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="casterId"></param>
        /// <param name="targetId"></param>
        /// <param name="position"></param>
        public void SendSkillCast(int skillId, int casterId, int targetId, NVector3 position)
        {
            if (position == null) position = new NVector3();
            Debug.LogFormat("SendSkillCast: skill:{0} caster:{1} target:{2} pos:{3}", skillId, casterId, targetId, position.ToString());
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.skillCast = new SkillCastRequest();
            message.Request.skillCast.castInfo = new NSkillCastInfo();
            message.Request.skillCast.castInfo.skillId = skillId;
            message.Request.skillCast.castInfo.casterId = casterId;
            message.Request.skillCast.castInfo.targetId = targetId;
            message.Request.skillCast.castInfo.Position = position;
            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// 收到释放技能的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnSkillCast(object sender, SkillCastResponse message)
        {
            if (message.Result == Result.Success)
            {
                foreach (var castInfo in message.castInfoes)
                {
                    Debug.LogFormat("OnSkillCast: skill:{0} caster:{1} target:{2} pos:{3} result:{4}", castInfo.skillId, castInfo.casterId, castInfo.targetId, castInfo.Position.String(), message.Result);
                    //释放者
                    Creature caster = EntityManager.Instance.GetEntity(castInfo.casterId) as Creature;
                    if (caster != null)
                    {
                        //目标
                        Creature target = EntityManager.Instance.GetEntity(castInfo.targetId) as Creature;
                        //释放技能
                        caster.CastSkill(castInfo.skillId, target, castInfo.Position);
                    }
                }
            }
            else
            {
                //提示失败
                ChatManager.Instance.AddSystemMessage(message.Errormsg);
            }
        }
        /// <summary>
        /// 收到技能打击的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnSkillHit(object sender, SkillHitResponse message)
        {
            Debug.LogFormat("OnSkillHit: count:{0}", message.Hits.Count);
            foreach(var hit in message.Hits)
            {
                Creature caster = EntityManager.Instance.GetEntity(hit.casterId) as Creature;
                if(caster != null)
                {
                    caster.DoSkillHit(hit);
                }
            }
        }
        /// <summary>
        /// 收到buff的回应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        private void OnBuff(object sender, BuffResponse message)
        {
            Debug.LogFormat("OnBuff: count:{0}", message.Buffs.Count);
            foreach (var buff in message.Buffs)
            {
                Creature owner = EntityManager.Instance.GetEntity(buff.ownerId) as Creature;
                if (owner != null)
                {
                    owner.DoBuffAction(buff);
                }
            }
        }

    }
}
