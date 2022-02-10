using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Managers;

/// <summary>
/// 
/// </summary>
public class UIStory : UIWindow
{
    /// <summary>
    /// 
    /// </summary>
    public Text title;

    public Text description;

    private StoryDefine story;

    public void SetStory(StoryDefine story)
    {
        this.story = story;
        this.title.text = story.Name;
        this.description.text = story.Description;
    }

    public void OnClickStart()
    {
        if (!StoryManager.Instance.StartStory(this.story.ID))
        {

        }

    }
}
