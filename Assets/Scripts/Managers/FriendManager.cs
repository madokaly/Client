using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Managers
{
    class FriendManager:Singleton<FriendManager>
    {
        //所有好友信息
        public List<NFriendInfo> allFriends;
        public void Init(List<NFriendInfo> friendInfos)
        {
            this.allFriends = friendInfos;
        }
    }
}
