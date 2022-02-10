using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI管理器
/// </summary>
public class UIManager : Singleton<UIManager>
{
    /// <summary>
    /// UI元素
    /// </summary>
    class UIElement
    {
        /// <summary>
        /// 位置
        /// </summary>
        public string Resources;
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool Cache;
        /// <summary>
        /// UI对象
        /// </summary>
        public GameObject Instance;
    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();

    public UIManager()
    {
        UIResources.Add(typeof(UITest), new UIElement() { Resources = "UI/UITest",Cache = true });
        UIResources.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = false });
        UIResources.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = false });
        UIResources.Add(typeof(UICharEquip), new UIElement() { Resources = "UI/UICharEquip", Cache = false });
        UIResources.Add(typeof(UIQuestSystem), new UIElement() { Resources = "UI/UIQuestSystem", Cache = false });
        UIResources.Add(typeof(UIQuestDialog), new UIElement() { Resources = "UI/UIQuestDialog", Cache = false });
        UIResources.Add(typeof(UIFriend), new UIElement() { Resources = "UI/UIFriends", Cache = false });
        UIResources.Add(typeof(UIGuild), new UIElement() { Resources = "UI/Guild/UIGuild", Cache = false });
        UIResources.Add(typeof(UIGuildPopCreate), new UIElement() { Resources = "UI/Guild/UIGuildPopCreate", Cache = false });
        UIResources.Add(typeof(UIGuildPopNoGuild), new UIElement() { Resources = "UI/Guild/UIGuildPopNoGuild", Cache = false });
        UIResources.Add(typeof(UIGuildList), new UIElement() { Resources = "UI/Guild/UIGuildList", Cache = false });
        UIResources.Add(typeof(UIGuildApplyList), new UIElement() { Resources = "UI/Guild/UIGuildApplyList", Cache = false });
        UIResources.Add(typeof(UISetting), new UIElement() { Resources = "UI/UISetting", Cache = true });
        UIResources.Add(typeof(UIPopCharMenu), new UIElement() { Resources = "UI/UIPopCharMenu", Cache = false });
        UIResources.Add(typeof(UIRide), new UIElement() { Resources = "UI/UIRide", Cache = false });
        UIResources.Add(typeof(UISystemConfig), new UIElement() { Resources = "UI/UISystemConfig", Cache = false });
        UIResources.Add(typeof(UISkill), new UIElement() { Resources = "UI/UISkill", Cache = false });
    }

    ~UIManager()
    {

    }
    /// <summary>
    /// 显示UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Show<T>()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Win_Open);
        Type type = typeof(T);
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Instance!=null)
            {
                info.Instance.SetActive(true);
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if (prefab==null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab);
            }
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }
    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="type"></param>
    public void Close(Type type)
    {
        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if (info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }

    public void Close<T>()
    {
        this.Close(typeof(T));
    }


}
