using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 小地图管理器
    /// </summary>
    public class MinimapManager : Singleton<MinimapManager>
    {

        /// <summary>
        /// 小地图
        /// </summary>
        public UIMinimap minimap;
        /// <summary>
        /// 地图大小
        /// </summary>
        public Collider minimapBoudingBox;
        public Collider MinimapBoudingBox
        {
            get { return minimapBoudingBox; }
        }

        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null) return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        /// <summary>
        /// 加载小地图
        /// </summary>
        /// <returns>小地图</returns>
        public Sprite LoadCurrentMinimap()
        {
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.MiniMap);
        }
        /// <summary>
        /// 更新小地图
        /// </summary>
        /// <param name="minimapBoudingBox"></param>
        public void UpdateMinimap(Collider minimapBoudingBox)
        {
            this.minimapBoudingBox = minimapBoudingBox;
            if(minimap != null) minimap.UpdateMinimap();
        }
    }
}
