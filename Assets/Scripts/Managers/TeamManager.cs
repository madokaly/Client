using Models;
using SkillBridge.Message;

namespace Managers
{
    class TeamManager : Singleton<TeamManager>
    {
        public void Init()
        {
        }
        /// <summary>
        /// 更新队伍信息
        /// </summary>
        /// <param name="team"></param>
        public void UpdateTeamInfo(NTeamInfo team)
        {
            User.Instance.TeamInfo = team;
            ShowTeamUI(team != null);
        }
        /// <summary>
        /// 展现UI队伍
        /// </summary>
        /// <param name="show"></param>
        private void ShowTeamUI(bool show)
        {
            if (UIMain.Instance != null)
            {
                UIMain.Instance.ShowTeamUI(show);
            }
        }
    }
}
