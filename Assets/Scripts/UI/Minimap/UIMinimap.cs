using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Managers;

/// <summary>
/// UI小地图
/// </summary>
public class UIMinimap : MonoBehaviour
{
    /// <summary>
    /// 小地图
    /// </summary>
    public Image minimap;
    /// <summary>
    /// 地图箭头
    /// </summary>
    public Image arrow;
    /// <summary>
    /// 地图名字
    /// </summary>
    public Text mapName;
    /// <summary>
    /// 地图大小
    /// </summary>
    public Collider minimapBoudingBox;
    /// <summary>
    /// 当前游戏对象
    /// </summary>
    private Transform playerTransform;

    private void Start()
    {
        MinimapManager.Instance.minimap = this;
        UpdateMinimap();
    }
    /// <summary>
    /// 初始化小地图
    /// </summary>
    public void UpdateMinimap()
    {
        mapName.text = User.Instance.CurrentMapData.Name;
        minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        minimap.SetNativeSize();
        minimap.rectTransform.position = Vector3.zero;
        minimapBoudingBox = MinimapManager.Instance.minimapBoudingBox;
        playerTransform = null;
    }
    /// <summary>
    /// 更新小地图
    /// </summary>
    private void Update()
    {
        if (playerTransform == null) playerTransform = MinimapManager.Instance.PlayerTransform;
        if (minimapBoudingBox == null || playerTransform == null) return;
        float realWidth = minimapBoudingBox.bounds.size.x;
        float realHeight = minimapBoudingBox.bounds.size.z;

        float relaX = playerTransform.position.x - minimapBoudingBox.bounds.min.x;
        float relaY = playerTransform.position.z - minimapBoudingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;
        minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        minimap.rectTransform.localPosition = Vector2.zero;
        arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y);
    }
}
