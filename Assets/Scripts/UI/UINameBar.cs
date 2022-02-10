using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 世界角色面板
/// </summary>
public class UINameBar : MonoBehaviour
{
    public Sprite[] sprites;
    /// <summary>
    /// 名字
    /// </summary>
    public Text avatarName;
    /// <summary>
    /// 角色
    /// </summary>
    public Character character;
    /// <summary>
    /// buffUI
    /// </summary>
    public UIBuffIcons buffIcons;
    /// <summary>
    /// 头像
    /// </summary>
    public Image image;

    private void Start()
    {
        if(character != null)
        {
            buffIcons.SetOwner(this.character);
            if (character.Info.Type == SkillBridge.Message.CharacterType.Player)
            {
                if (character.Info.Class == SkillBridge.Message.CharacterClass.Archer)
                {
                    image.sprite = sprites[0];
                }
                else if (character.Info.Class == SkillBridge.Message.CharacterClass.Warrior)
                {
                    image.sprite = sprites[1];
                }
                else
                {
                    image.sprite = sprites[2];
                }
            }
            else
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        UpdateInfo();
    }
    /// <summary>
    /// 更新信息
    /// </summary>
    private void UpdateInfo()
    {
        if(character != null)
        {
            string name = character.Name + "  Lv." + character.Info.Level;
            if(avatarName.text != name)
            {
                avatarName.text = name;
            }
        }
    }
}
