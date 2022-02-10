using Managers;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArena : MonoSingleton<UIArena>
{
    /// <summary>
    /// 
    /// </summary>
    public Text roundText;
    /// <summary>
    /// 
    /// </summary>
    public Text countDownText;

    protected override void OnStart()
    {
        roundText.enabled = false;
        countDownText.enabled = false;
        ArenaManager.Instance.SendReady();
    }

    /// <summary>
    /// 显示倒计时
    /// </summary>
    public void ShowCountDown()
    {
        StartCoroutine(CountDown(10));
    }
    
    /// <summary>
    /// 倒计时
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator CountDown(int time)
    {
        int total = time;
        roundText.text = "ROUND" + ArenaManager.Instance.Round;
        roundText.enabled = true;
        countDownText.enabled = true;
        while(total > 0)
        {
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_CountDown);
            countDownText.text = total.ToString();
            yield return new WaitForSeconds(1f);
            total--;
        }
        countDownText.text = "READY!";
    }

    /// <summary>
    /// 显示回合开始UI
    /// </summary>
    /// <param name="round"></param>
    /// <param name="arenaInfo"></param>
    internal void ShowRoundStart(int round, ArenaInfo arenaInfo)
    {
        countDownText.text = "FIGHT";
        
    }

    /// <summary>
    /// 显示回合结果UI
    /// </summary>
    /// <param name="round"></param>
    /// <param name="arenaInfo"></param>
    internal void ShowRoundResult(int round, ArenaInfo arenaInfo)
    {
        countDownText.enabled = true;
        countDownText.text = "YOU WIN!";
    }
}
