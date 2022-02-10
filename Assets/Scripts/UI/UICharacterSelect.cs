using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using Models;

/// <summary>
/// 角色选择UI
/// </summary>
public class UICharacterSelect : MonoBehaviour
{
    /// <summary>
    /// 创建窗口
    /// </summary>
    public GameObject panelCreate;
    /// <summary>
    /// 选择窗口
    /// </summary>
    public GameObject panelSelect;
    /// <summary>
    /// 取消创建按钮
    /// </summary>
    public GameObject btnCreateCancel;
    /// <summary>
    /// 名字输入
    /// </summary>
    public InputField charName;
    /// <summary>
    /// 角色职业
    /// </summary>
    private CharacterClass charClass;
    /// <summary>
    /// 角色列表位置
    /// </summary>
    public Transform uiCharList;
    /// <summary>
    /// 角色信息
    /// </summary>
    public GameObject uiCharInfo;
    /// <summary>
    /// 角色列表
    /// </summary>
    public List<GameObject> uiChars = new List<GameObject>();
    /// <summary>
    /// 职业图片数组
    /// </summary>
    public Image[] titles;
    /// <summary>
    /// 描述
    /// </summary>
    public Text descs;
    /// <summary>
    /// 职业名字数组
    /// </summary>
    public Text[] names;

    private int selectCharacterIdx = -1;

    public UICharacterView characterView;

    private void Start()
    {
        InitCharacterSelect(true);
        UserService.Instance.OnCharacterCreate = OnCharacterCreate;
    }
    /// <summary>
    /// 初始化角色创建
    /// </summary>
    public void InitCharacterCreate()
    {
        panelCreate.SetActive(true);
        panelSelect.SetActive(false);
        OnSelectClass(1);
    }
    /// <summary>
    /// 创建角色
    /// </summary>
    public void OnClickCreate()
    {
        if (string.IsNullOrEmpty(this.charName.text))
        {
            MessageBox.Show("请输入角色名称");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendCharacterCreate(this.charName.text, this.charClass);
    }

    /// <summary>
    /// 选择职业
    /// </summary>
    /// <param name="charClass">职业类型</param>
    public void OnSelectClass(int charClass)
    {
        this.charClass = (CharacterClass)charClass;

        characterView.CurrentCharacter = charClass - 1;

        for (int i = 0; i < 3; i++)
        {
            titles[i].gameObject.SetActive(i == charClass - 1);
            names[i].text = DataManager.Instance.Characters[i + 1].Name;
        }

        descs.text = DataManager.Instance.Characters[charClass].Description;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    /// <summary>
    /// 角色创建后响应服务器
    /// </summary>
    /// <param name="result">服务器结果</param>
    /// <param name="message">服务器消息</param>
    private void OnCharacterCreate(Result result, string message)
    {
        if (result == Result.Success)
        {
            InitCharacterSelect(true);
        }
        else
            MessageBox.Show(message, "错误", MessageBoxType.Error);
    }
    /// <summary>
    /// 初始化角色选择界面
    /// </summary>
    /// <param name="init"></param>
    public void InitCharacterSelect(bool init)
    {
        panelCreate.SetActive(false);
        panelSelect.SetActive(true);

        if (init)
        {
            foreach (var old in uiChars)
            {
                Destroy(old);
            }
            uiChars.Clear();

            for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
            {

                GameObject go = Instantiate(uiCharInfo, this.uiCharList);
                UICharInfo chrInfo = go.GetComponent<UICharInfo>();
                chrInfo.info = User.Instance.Info.Player.Characters[i];

                Button button = go.GetComponent<Button>();
                int idx = i;
                button.onClick.AddListener(() =>
                {
                    OnSelectCharacter(idx);
                });

                uiChars.Add(go);
                go.SetActive(true);
            }
        }
    }
    /// <summary>
    /// 选择的角色
    /// </summary>
    /// <param name="idx">选择的序号</param>
    public void OnSelectCharacter(int idx)
    {
        this.selectCharacterIdx = idx;
        var cha = User.Instance.Info.Player.Characters[idx];
        Debug.LogFormat("Select Char:[{0}]{1}[{2}]", cha.Id, cha.Name, cha.Class);
        characterView.CurrentCharacter = ((int)cha.Class - 1);
        for (int i = 0; i < User.Instance.Info.Player.Characters.Count; i++)
        {
            UICharInfo ci = this.uiChars[i].GetComponent<UICharInfo>();
            ci.Selected = idx == i;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }
    /// <summary>
    /// 点击开始游戏
    /// </summary>
    public void OnClickPlay()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        if (selectCharacterIdx >= 0)
        {
            UserService.Instance.SendGameEnter(selectCharacterIdx);
        }
    }
}
