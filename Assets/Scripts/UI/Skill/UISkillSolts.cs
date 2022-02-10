using Common.Data;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能solt列表UI
/// </summary>
public class UISkillSolts : MonoBehaviour
{
    /// <summary>
    /// 技能solt列表
    /// </summary>
    public UISkillSolt[] slots;

    private void Start()
    {

    }

    public void UpdateSkills()
    {
        if (User.Instance.CurrentCharacter == null) return;
        var Skills = User.Instance.CurrentCharacter.SkillMgr.Skills;
        int skillIdx = 0;
        foreach(var skill in Skills)
        {
            slots[skillIdx].SetSkill(skill);
            skillIdx++;
        }
    }
}
