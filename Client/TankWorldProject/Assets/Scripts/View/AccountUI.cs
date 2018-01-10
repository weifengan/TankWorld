using Sfs2X.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sfs2X.Requests;
using UnityEngine.UI;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;

public class AccountUI : BaseUI
{


    public enum AccountUIType
    {
        Login, Register
    }

    public AccountUIType _type = AccountUIType.Login;


    private GameObject mLogin;
    private GameObject mReg;

    private InputField loginInptUser;
    private InputField loginInptPwd;
    private InputField regInptUser;
    private InputField regInptPwd;
    private InputField regInptPwd2;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/AccountUI");
    }

    protected override void OnStart()
    {
        base.OnStart();
        //隐藏UI
        mLogin = this.Skin.transform.Find("LoginUI").gameObject;
        mReg = this.Skin.transform.Find("RegUI").gameObject;
        mLogin.gameObject.SetActive(false);
        mReg.gameObject.SetActive(false);


        loginInptUser = mLogin.transform.Find("InputName").GetComponent<InputField>();
        loginInptPwd = mLogin.transform.Find("InputPwd").GetComponent<InputField>();



        regInptUser = mReg.transform.Find("InputName").GetComponent<InputField>();
        regInptPwd = mReg.transform.Find("InputPwd").GetComponent<InputField>();
        regInptPwd2 = mReg.transform.Find("InputPwd2").GetComponent<InputField>();

        Button[] btns = this.transform.GetComponentsInChildren<Button>(true);
        foreach (Button item in btns)
        {
            item.onClick.AddListener(delegate ()
            {
                OnButtonClickHandler(item);
            });
        }

        Global.AddEventListener(SFSEvent.CONNECTION, OnConnectionHandler);

        //用户匿名登录到区
        Global.AddEventListener(SFSEvent.LOGIN, (NEvent e) =>
        {
            Type = AccountUIType.Login;
        });

        Global.AddEventListener("dologin", OnLoginResultHandler);
        Global.AddEventListener("doreg", OnRegResultHandler);
        NetManager.GetInstance().Connect(Global.Instance.ServerIP, Global.Instance.ServerPort);

    }

    private void OnRegResultHandler(NEvent evt)
    {

        print(evt.Data.GetUtfString("info"));
    }

    private void OnLoginResultHandler(NEvent evt)
    {
        if (!evt.Data.GetBool("success"))
        {
            UIManager.GetInstance().Alert(evt.Data.GetUtfString("info"));
            return;
        }

        //存储自身基本信息
        Global.Instance.me.isLogin = true;
        Global.Instance.me.UserName = evt.Data.GetUtfString("username");
        Global.Instance.me.Diamond = evt.Data.GetInt("diamond");
        Global.Instance.me.Coin = evt.Data.GetInt("coin");
        Global.Instance.me.Nick = evt.Data.GetUtfString("nick");
        Global.Instance.me.Role = evt.Data.GetInt("role");

        ///根据呢称是否存在，进行场景跳转
        if (string.IsNullOrEmpty(Global.Instance.me.Nick))
        {
            ///创建角色场景 
            UIManager.GetInstance().SwitchUI("CreateRoleUI");
        }else
        {
            UIManager.GetInstance().SwitchUI("HallUI");
        }


        //UIManager.GetInstance().Alert("登录成功!");
        
    }

    private void OnButtonClickHandler(Button button)
    {
        
        switch (button.name)
        {
            case "btnLogin":
                if(string.IsNullOrEmpty(loginInptUser.text) || string.IsNullOrEmpty(loginInptPwd.text))
                {
                    UIManager.GetInstance().Alert("用户名和密码不能为空!");
                    return;
                }
                SFSObject data = new SFSObject();
                data.PutUtfString("username", loginInptUser.text.Trim());
                data.PutUtfString("password", loginInptPwd.text.Trim());
                Global.SendExtRequest("dologin", data);
                break;
            case "btnReg":

                if (string.IsNullOrEmpty(regInptUser.text) || string.IsNullOrEmpty(regInptPwd.text) || string.IsNullOrEmpty(regInptPwd2.text))
                {
                    UIManager.GetInstance().Alert("用户名和密码不能为空!", "警告");
                    return;
                }

                if (!regInptPwd.text.Trim().Equals(regInptPwd2.text.Trim()))
                {
                    UIManager.GetInstance().Alert("两次输入的密码一致!","警告");
                    return;
                }
                SFSObject regData = new SFSObject();
                regData.PutUtfString("username", regInptUser.text.Trim());
                regData.PutUtfString("password", regInptPwd.text.Trim());
                Global.SendExtRequest("doreg", regData);

                break;
            case "btn2Reg":
                Type = AccountUIType.Register;
                regInptUser.text = "";
                regInptPwd.text = "";
                regInptPwd2.text = "";
                break;
            case "btn2Login":
                Type = AccountUIType.Login;
                loginInptUser.text = "";
                loginInptPwd.text = "";
                break;
        }
    }

    public AccountUIType Type
    {
        set
        {
            _type = value;
            mLogin.gameObject.SetActive(_type == AccountUIType.Login ? true : false);
            mReg.gameObject.SetActive(!mLogin.gameObject.activeSelf);
        }
        get
        {
            return _type;
        }
    }


    /// <summary>
    /// 登录按钮被点击处理
    /// </summary>
    private void OnStart2LoginHandler()
    {

    }



    private void OnLoginHandler(NEvent evt)
    {

    }

    private void OnConnectionHandler(NEvent evt)
    {
       //匿名登录到区
      Global.SendSFSRequest(new LoginRequest("", "", Global.Instance.defaultZone));
    }


}
