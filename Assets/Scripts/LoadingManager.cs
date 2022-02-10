using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Services;
using Managers;

public class LoadingManager : MonoBehaviour 
{
    /// <summary>
    /// 过场图片
    /// </summary>
    public GameObject UITips;
    /// <summary>
    /// 加载界面
    /// </summary>
    public GameObject UILoading;
    /// <summary>
    /// 登录界面
    /// </summary>
    public GameObject UILogin;
    /// <summary>
    /// 进度条
    /// </summary>
    public Slider progressBar;
    /// <summary>
    /// 进度条文本
    /// </summary>
    public Text progressText;
    /// <summary>
    /// 进度条数字
    /// </summary>
    public Text progressNumber;

    private IEnumerator Start()
    {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");
        //过场图片
        UITips.SetActive(true);
        UILoading.SetActive(false);
        UILogin.SetActive(false);
        //等待2S
        yield return new WaitForSeconds(2f);
        //加载loading界面
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);
        //等待读取配置文件
        yield return DataManager.Instance.LoadData();

        MapService.Instance.Init();
        UserService.Instance.Init();
        StatusService.Instance.Init();
        FriendService.Instance.Init();
        TeamService.Instance.Init();
        GuildService.Instance.Init();
        ShopManager.Instance.Init();
        ChatService.Instance.Init();
        ArenaService.Instance.Init();
        SoundManager.Instance.PlayMusic(SoundDefine.Music_Login);
        //等待加载
        for (float i = 50; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progressBar.value = i;
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
    }

    private void Update()
    {

    }
}
