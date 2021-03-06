using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Managers;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {

        public UnityEngine.Events.UnityAction<Result, string> OnLogin;
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result, string> OnCharacterCreate;

        NetMessage pendingMessage = null;

        bool connected = false;

        bool isQuitGame = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(this.OnGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(this.OnGameLeave);
            //MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnCharacterEnter);
            
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(this.OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(this.OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(this.OnGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(this.OnGameLeave);
            //MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnCharacterEnter);
            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }
        /// <summary>
        /// ???????????????
        /// </summary>
        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }

        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        /// <param name="result"></param>
        /// <param name="reason"></param>
        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if(this.pendingMessage!=null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("??????????????????????????????????????????\n RESULT:{0} ERROR:{1}", result, reason), "??????", MessageBoxType.Error);
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result,string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userLogin!=null)
                {
                    if (this.OnLogin != null)
                    {
                        this.OnLogin(Result.Failed, string.Format("??????????????????\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else if(this.pendingMessage.Request.userRegister!=null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("??????????????????\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                else
                {
                    if (this.OnCharacterCreate != null)
                    {
                        this.OnCharacterCreate(Result.Failed, string.Format("??????????????????\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendLogin(string user, string psw)
        {
            Debug.LogFormat("UserLoginRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = user;
            message.Request.userLogin.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnUserLogin(object sender, UserLoginResponse response)
        {
            Debug.LogFormat("OnLogin:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {//??????????????????
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            };
            if (this.OnLogin != null)
            {
                this.OnLogin(response.Result, response.Errormsg);

            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="user"></param>
        /// <param name="psw"></param>
        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userRegister = new UserRegisterRequest();
            message.Request.userRegister.User = user;
            message.Request.userRegister.Passward = psw;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// ??????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);

            }
        }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cls"></param>
        public void SendCharacterCreate(string name, CharacterClass cls)
        {
            Debug.LogFormat("UserCreateCharacterRequest::name :{0} class:{1}", name, cls);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Name = name;
            message.Request.createChar.Class = cls;

            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
        {
            Debug.LogFormat("OnUserCreateCharacter:{0} [{1}]", response.Result, response.Errormsg);

            if(response.Result == Result.Success)
            {
                Models.User.Instance.Info.Player.Characters.Clear();
                Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
            }

            if (this.OnCharacterCreate != null)
            {
                this.OnCharacterCreate(response.Result, response.Errormsg);

            }
        }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="characterIdx"></param>
        public void SendGameEnter(int characterIdx)
        {
            Debug.LogFormat("UserGameEnterRequest::characterId :{0}", characterIdx);

            ChatManager.Instance.Init();//????????????????????????

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = characterIdx;
            NetClient.Instance.SendMessage(message); 
        }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat("OnGameEnter:{0} [{1}]", response.Result, response.Errormsg);

            if (response.Result == Result.Success)
            {
                if(response.Character != null)
                {
                    User.Instance.CurrentCharacterInfo = response.Character;
                    ItemManager.Instance.Init(response.Character.Items);
                    BagManager.Instance.Init(response.Character.Bag);
                    EquipManager.Instance.Init(response.Character.Equips);
                    QuestManager.Instance.Init(response.Character.Quests);
                    FriendManager.Instance.Init(response.Character.Friends);
                    GuildManager.Instance.Init(response.Character.Guild);
                }
            }
        }

        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="isQuitGame"></param>
        public void SendGameLeave(bool isQuitGame = false)
        {
            this.isQuitGame = isQuitGame;
            Debug.Log("UserGameLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }
        /// <summary>
        /// ????????????????????????
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        void OnGameLeave(object sender, UserGameLeaveResponse response)
        {
            MapService.Instance.CurrentMapId = 0;
            User.Instance.CurrentCharacterInfo = null;
            User.Instance.CurrentCharacter = null;
            Debug.LogFormat("OnGameLeave:{0} [{1}]", response.Result, response.Errormsg);
            if(this.isQuitGame)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
        ///// <summary>
        ///// ??????????????????????????????
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="message"></param>
        //private void OnCharacterEnter(object sender, MapCharacterEnterResponse message)
        //{
        //    Debug.LogFormat("OnCharacterEnter:{0}", message.mapId);
        //    NCharacterInfo info = null;
        //    foreach (var character in message.Characters)
        //    {
        //        if(User.Instance.CurrentCharacterInfo == character)
        //        {
        //            info = character;
        //        }
        //    }
        //    //NCharacterInfo info = message.Characters[0];
        //    //User.Instance.CurrentCharacter = info;
        //    SceneManager.Instance.LoadScene(DataManager.Instance.Maps[message.mapId].Resource);
        //}
    }
}
