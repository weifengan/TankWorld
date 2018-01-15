using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class HallUI : BaseUI
{

    private Text txtNick;
    private Text txtCoin;
    private Text txtDiamond;
    private Transform mTank;

    private Toggle mTlgJingJi;
    private Toggle mTlgWinBoard;

    private GraphicRaycaster raycaster = null;

    //默认模式
    private int curMode = 1;

    private Transform tankContainer;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/HallUI");
        txtNick = this.Skin.transform.Find("top/txtNick").GetComponent<Text>();
        txtCoin = this.Skin.transform.Find("top/itemCoin/Text").GetComponent<Text>();
        txtDiamond = this.Skin.transform.Find("top/itemDiamond/Text").GetComponent<Text>();
        mTank = this.Skin.transform.Find("tank3d/container").transform;

    

        txtNick.text = Global.GetInstance().me.Nick;
        txtCoin.text = Global.GetInstance().me.Coin.ToString();
        txtDiamond.text = Global.GetInstance().me.Diamond.ToString();

        mTlgJingJi = this.Skin.transform.Find("left/tab/TlgJingJi").GetComponent<Toggle>();
        mTlgWinBoard = this.Skin.transform.Find("left/tab/TlgWinBoard").GetComponent<Toggle>();


        ///根据角色id创建坦克
        tankContainer = this.Skin.transform.Find("tank3d/container");
        GameObject tank = ResManager.GetInstance().GetRes<GameObject>("StaticPlayer/tank" + Global.GetInstance().me.Role);
        tank.transform.SetParent(tankContainer);
        tank.transform.localScale = Vector3.one;
        tank.transform.localPosition = Vector3.zero;

        mTlgJingJi.gameObject.AddComponent<HallBoardToggle>().Init(this.Skin.transform.Find("left/content/jingji").gameObject);
        mTlgWinBoard.gameObject.AddComponent<HallBoardToggle>().Init(this.Skin.transform.Find("left/content/shengju").gameObject);

        Global.AddEventListener(ExtType.JoinGame, OnJoinGameHandler);

    }

    private void OnJoinGameHandler(NEvent evt)
    {
        if (evt.Data.GetBool("success"))
        {

            Global.GetInstance().LoadScene("Game",(string sn)=>{
              
                UIManager.GetInstance().SwitchUI("GameUI");
            });
            

        }

    }

    //是否可以拖动
    private bool isdraging = false;

    //旋转功能实现
    protected override void OnUpdate()
    {
        base.OnUpdate();



        ///实现坦克模型拖动旋转
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        //按下那一刻，是否在tank上
        if (Input.GetMouseButtonDown(0))
        {
            List<RaycastResult> uis = new List<RaycastResult>();
            UIManager.GetInstance().Raycaster.Raycast(ped, uis);
            if (uis.Count > 0 && uis[0].gameObject.name.Equals("tank3d"))
            {
                isdraging = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isdraging = false;
        }

        if (isdraging)
        {
            mTank.transform.Rotate(-Vector3.up * Input.GetAxis("Mouse X") * 10);
        }

        if (mTank != null)
        {
            mTank.transform.Rotate(Vector3.up * Time.deltaTime*5);
        }


    }

    protected override void OnClickHandler(GameObject go)
    {
        SFSObject data = new SFSObject();
        base.OnClickHandler(go);
        switch (go.name)
        {
            case "btnClasic"://经典模式
                curMode = 1;
                break;
            case "btnTeam": //组队
                curMode = 2;
                break;
            case "btnLife"://绝地模式
                curMode = 3;

                break;
        }
        data.PutInt("mode", curMode);
        Global.SendExtRequest(ExtType.JoinGame, data, Global.curRoom);
    }

}
