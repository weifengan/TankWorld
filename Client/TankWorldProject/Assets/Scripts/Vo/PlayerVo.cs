using Sfs2X.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerVo  {

    public User sfsUser;

    public int UserId=-1;
    public string UserName="";
    public int Role = -1;
    public string Nick = "";
    public int Coin = 0;
    public int Diamond = 0;

    //是否登录
    public bool isLogin = false;
    
    
}
