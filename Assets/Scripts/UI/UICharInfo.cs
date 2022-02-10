using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 角色信息UI
/// </summary>
public class UICharInfo : MonoBehaviour {


    public SkillBridge.Message.NCharacterInfo info;
    /// <summary>
    /// 角色职业
    /// </summary>
    public Text charClass;
    /// <summary>
    /// 角色名字
    /// </summary>
    public Text charName;
    /// <summary>
    /// 高亮图片
    /// </summary>
    public Image highlight;

    public bool Selected
    {
        get { return highlight.IsActive(); }
        set
        {
            highlight.gameObject.SetActive(value);
        }
    }

    void Start () {
		if(info!=null)
        {
            this.charClass.text = this.info.Class.ToString();
            this.charName.text = this.info.Name;
        }
	}
	
	void Update () {
		
	}
}
