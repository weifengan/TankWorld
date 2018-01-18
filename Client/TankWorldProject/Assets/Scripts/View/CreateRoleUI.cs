using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CreateRoleUI : BaseUI {

    private Transform tankContainer = null;

    private Camera tankCamera = null;
    private TankData curTankData;
    private int curIndex = 0;
    private GameObject curTankModel = null;

    private Text txtTitle;
    private Text txtBlood;
    private Text txtAttack;
    private Text txtSpeed;
    private Text txtDesc;
    private Text txtTip;
    private InputField inputNick;
    
    private string tmpNick = "";

    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/CreateRoleUI");
        tankContainer = this.Skin.transform.Find("tankcontainer");
        tankCamera = this.Skin.transform.Find("tankcontainer/camera").GetComponent<Camera>() ;

        inputNick = this.FetchComponentByName<InputField>("InputNick");
        txtTitle  = this.Skin.transform.Find("detail/title").GetComponent<Text>(); 
        txtBlood  = this.Skin.transform.Find("detail/blood").GetComponent<Text>();
        txtAttack = this.Skin.transform.Find("detail/attack").GetComponent<Text>();
        txtSpeed  = this.Skin.transform.Find("detail/speed").GetComponent<Text>();
        txtDesc = this.Skin.transform.Find("detail/desc").GetComponent<Text>();
        txtTip = this.Skin.transform.Find("tip").GetComponent<Text>();

        curTankData = ConfigManager.GetInstance().Tank.Data[curIndex];

        txtTip.text = (curIndex+1) + "/" + ConfigManager.GetInstance().Tank.Data.Count;
        
        DisplayModel();

        Global.AddEventListener(ExtType.UpdateRole, OnUpdateRoleHandler);
   }

    private void OnUpdateRoleHandler(NEvent evt)
    {
        //如果操作成功
        if (evt.Data.GetBool("success"))
        {
            //更新昵称和角色id
            Global.GetInstance().me.Nick = inputNick.text;
            Global.GetInstance().me.Role = curTankData.id;
            //返回Lobby
            UIManager.GetInstance().SwitchUI("HallUI");
        }else
        {
            UIManager.GetInstance().Alert(evt.Data.GetUtfString("info"));
        }
    }

    private void DisplayModel()
    {
        //如果之前已经有模型，则销毁模型
        if (curTankModel != null)
        {
            Destroy(curTankModel);
        }

        curTankModel = ResManager.GetInstance().GetRes<GameObject>("Player/tank" + curTankData.id, true,true);
        curTankModel.transform.SetParent(tankContainer);
        curTankModel.transform.localPosition = Vector3.zero;

        txtTitle.text = curTankData.title;
        
        txtBlood.text = "【血量】"+curTankData.blood.ToString();
        txtAttack.text = "【攻击】"+curTankData.attack.ToString();
        txtSpeed.text = "【速度】"+curTankData.speed.ToString();
        txtDesc.text = curTankData.desc;


    }

    protected override void OnClickHandler(GameObject go)
    {
        base.OnClickHandler(go);
        int oldIndex = curIndex;
        switch (go.name)
        {
            case "btnPrev":
                curIndex = curIndex - 1 <= 0 ? 0 : curIndex - 1; 
                break;
            case "btnNext":
                curIndex = curIndex +1 >=ConfigManager.GetInstance().Tank.Data.Count-1 ? ConfigManager.GetInstance().Tank.Data.Count - 1 : curIndex +1;
                break;
            case "btnPlay":
                if (string.IsNullOrEmpty(inputNick.text.Trim()))
                {
                    UIManager.GetInstance().Alert("请正确输入呢称!", "警告");
                    return;
                }
                SFSObject data = new SFSObject();
                data.PutUtfString("nick", inputNick.text.Trim());
                data.PutInt("role", curTankData.id);
                Global.SendExtRequest(ExtType.UpdateRole, data);
                break;
        }

        //如果模型id发生改变 ，刷新Tank模型
        if (oldIndex != curIndex)
        {
            curTankData = ConfigManager.GetInstance().Tank.Data[curIndex];
            txtTip.text = curIndex + "/" + ConfigManager.GetInstance().Tank.Data.Count;
            DisplayModel();
        }
        
    }





}
