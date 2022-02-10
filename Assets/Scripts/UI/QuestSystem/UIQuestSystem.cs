using Common.Data;
using Managers;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI任务系统
/// </summary>
public class UIQuestSystem : UIWindow
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// Tab按钮
    /// </summary>
    public TabView Tabs;
    /// <summary>
    /// 主线任务列表
    /// </summary>
    public ListView listMain;
    /// <summary>
    /// 支线任务列表
    /// </summary>
    public ListView listBranch;
    /// <summary>
    /// UI任务信息
    /// </summary>
    public UIQuestInfo questInfo;
    /// <summary>
    /// 任务列表元素预制件
    /// </summary>
    public GameObject itemPrefab;
    /// <summary>
    /// 是否展示可接受任务
    /// </summary>
    private bool showAvailableList = false;

    void Start()
    {
        this.listMain.onItemSelected += this.OnQuestSelected;
        this.listBranch.onItemSelected += this.OnQuestSelected;
        this.Tabs.OnTabSelect += OnSelectTab;
        RefreshUI();

    }
    /// <summary>
    /// 响应Tab按钮点击事件
    /// </summary>
    /// <param name="idx"></param>
    void OnSelectTab(int idx)
    {
        showAvailableList = idx == 1;
        RefreshUI();
    }
    private void OnDestroy()
    {
        
    }
    /// <summary>
    /// 刷新UI任务系统
    /// </summary>
    private void RefreshUI()
    {
        ClearAllQuestList();
        InitAllQuestItems();
    }
    /// <summary>
    /// 初始化UI任务系统
    /// </summary>
    private void InitAllQuestItems()
    {
        foreach (var kv in QuestManager.Instance.allQuests)
        {
            if (showAvailableList)
            {
                //协议任务不为空，表示接受过
                if (kv.Value.Info != null)
                {
                    continue;
                }
            }
            else
            {
                //协议任务不为空，表示未接受
                if (kv.Value.Info == null)
                {
                    continue;
                }
            }
            //创建任务列表元素
            GameObject go = Instantiate(itemPrefab, kv.Value.Define.Type == QuestType.Main ? this.listMain.transform : this.listBranch.transform);
            UIQuestItem ui = go.GetComponent<UIQuestItem>();
            //记下当前列表任务
            ui.SetQuestItemInfo(kv.Value);
            //添加到对应列表集合中
            if (kv.Value.Define.Type == QuestType.Main)
            {
                this.listMain.AddItem(ui as ListView.ListViewItem);
            }
            else
                this.listBranch.AddItem(ui as ListView.ListViewItem);
        }
    }
    private void ClearAllQuestList()
    {
        this.listMain.RemoveAll();
        this.listBranch.RemoveAll();
    }
    /// <summary>
    /// 响应任务选择事件
    /// </summary>
    /// <param name="item"></param>
    private void OnQuestSelected(ListView.ListViewItem item)
    {
        if(item.owner == listMain)
        {
            listBranch.SelectedItem = null;
        }
        if(item.owner == listBranch)
        {
            listMain.SelectedItem = null;
        }
        UIQuestItem questItem = item as UIQuestItem;
        //设置任务面板信息
        this.questInfo.SetQuestInfo(questItem.quest);
    }
}