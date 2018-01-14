using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    private Camera cm;

    public Dictionary<string, GameObject> mUsers = new Dictionary<string, GameObject>();
    // Use this for initialization
    void Start()
    {

        Global.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoomHandler);
        Global.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoomHandler);
        InitPlayer();

    }

    /// <summary>
    /// 用户推出房间
    /// </summary>
    /// <param name="evt"></param>
    private void OnUserExitRoomHandler(NEvent evt)
    {
       User u= evt.BaseEvt.Params["user"] as User;
        print("用户梨花开" + u.Name);
        if (mUsers.ContainsKey(u.Name))
        {
            GameObject go = mUsers[u.Name];
            mUsers.Remove(u.Name);
            Destroy(go);
        }
    }

    /// <summary>
    /// 有新用户进入
    /// </summary>
    /// <param name="evt"></param>
    private void OnUserEnterRoomHandler(NEvent evt)
    {
        User u = evt.BaseEvt.Params["user"] as User;
        print("新用户进入" + u.Name);
        if (u != null)
        {
            int role = u.GetVariable("role").GetIntValue();
            GameObject tmpPlayer = ResManager.GetInstance().GetRes<GameObject>("Player/tank" + role, true);
            tmpPlayer.name = u.Name;
            tmpPlayer.AddComponent<AsyncPlayer>();
            mUsers.Add(u.Name, tmpPlayer);
        }
    }

    public void InitPlayer()
    {
        List<User> users = NetManager.GetInstance().sfs.UserManager.GetUserList();
        foreach (User item in users)
        {
            int role = item.GetVariable("role").GetIntValue();
            GameObject tmpPlayer = ResManager.GetInstance().GetRes<GameObject>("Player/tank" + role, true);
            tmpPlayer.name = item.Name;
            mUsers.Add(item.Name, tmpPlayer);
            if (item.IsItMe)
            {

                tmpPlayer.AddComponent<TankController>();
                Vector3 v = tmpPlayer.transform.position;
                tmpPlayer.transform.position = this.transform.Find("b" + UnityEngine.Random.Range(0, 2)).position;
                CameraCtrl cc = GameObject.Find("CameraCtrl").GetComponent<CameraCtrl>();
                cc.target = tmpPlayer.transform;
                cc.transform.position = tmpPlayer.transform.position - tmpPlayer.transform.forward.normalized * 2;
            }
            else
            {

                tmpPlayer.AddComponent<AsyncPlayer>();
                tmpPlayer.transform.position = this.transform.Find("b" + UnityEngine.Random.Range(0, 2)).position;
            }
        }


    }
 
 
}
