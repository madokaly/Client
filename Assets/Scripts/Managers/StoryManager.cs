using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 剧情副本管理器
    /// </summary>
    public class StoryManager : Singleton<StoryManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcFunction.InvokeStory, OnOpenStory);
        }

        private bool OnOpenStory(NpcDefine npc)
        {
            this.ShowStoryUI(npc.Param);
            return true;
        }

        public void ShowStoryUI(int storyId)
        {
            StoryDefine story;
            if(DataManager.Instance.Storys.TryGetValue(storyId, out story))
            {
                UIStory uiStory = UIManager.Instance.Show<UIStory>();
                if(uiStory != null)
                {
                    uiStory.SetStory(story);
                }
            }
        }

        public bool StartStory(int storyId)
        {
            StoryService.Instance.SendStartStory(storyId);
            return true;
        }

        internal void OnStoryStart(int storyId)
        {

        }
    }
}
