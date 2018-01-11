using Sfs2X.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicGameManager : MonoBehaviour {

     
	// Use this for initialization
	void Start () {
        InitPlayer();
    }

    public void InitPlayer()
    {
        List<User> users = NetManager.GetInstance().sfs.UserManager.GetUserList();

        print(users.Count);
        foreach (User item in users)
        {
            int role = item.GetVariable("role").GetIntValue();
            print("tank"+role);
            GameObject tmpPlayer = ResManager.GetInstance().GetRes<GameObject>("Player/tank" + role, true);
            tmpPlayer.name = "tttt";
            print(tmpPlayer);
            if (!item.IsItMe)
            {
                
            }
            else
            {
             

            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
