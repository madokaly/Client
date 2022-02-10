using SkillBridge.Message;
using UnityEngine;
using UnityEngine.UI;
using Common;

/// <summary>
/// 公会信息UI
/// </summary>
public class UIGuildInfo : MonoBehaviour
{
    /// <summary>
    /// 公会名字
    /// </summary>
    public Text guildName;
    /// <summary>
    /// 公会ID
    /// </summary>
    public Text guildID;
    /// <summary>
    /// 公会会长
    /// </summary>
    public Text leader;
    /// <summary>
    /// 公会宣言
    /// </summary>
    public Text notice;
    /// <summary>
    /// 公会成员数量
    /// </summary>
    public Text memberNumber;
    /// <summary>
    /// 协议公会信息
    /// </summary>
    private NGuildInfo info;
    public NGuildInfo Info
    {
        get
        {
            return this.info;
        }
        set
        {
            this.info = value;
            this.UpdateUI();
        }
    }
    /// <summary>
    /// 更新公会信息UI
    /// </summary>
    private void UpdateUI()
    {
        if(this.info == null)
        {
            this.guildName.text = "无";
            this.guildID.text = "ID：0";
            this.leader.text = "会长：无";
            this.notice.text = "";
            this.memberNumber.text = string.Format("成员数量：0/{0}", 30/*GameDefine.GuildMaxMemberCount*/);
        }
        else
        {
            this.guildName.text = this.Info.GuildName;
            this.guildID.text = "ID：" + this.Info.Id;
            this.leader.text = "会长：" + this.Info.leaderName;
            this.notice.text = this.Info.Notice;
            this.memberNumber.text = string.Format("成员数量：{0}/{1}",this.Info.memberCount, 30/*GameDefine.GuildMaxMemberCount*/);
        }
    }
}