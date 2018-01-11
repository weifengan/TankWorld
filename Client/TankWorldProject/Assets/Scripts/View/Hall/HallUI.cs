using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HallUI : BaseUI
{

    private Text txtNick;
    private Text txtCoin;
    private Text txtDiamond;
    private Transform mTank;

    private Toggle mTlgJingJi;
    private Toggle mTlgWinBoard;

    private GraphicRaycaster raycaster = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/HallUI");
        txtNick = this.Skin.transform.Find("top/txtNick").GetComponent<Text>();
        txtCoin = this.Skin.transform.Find("top/itemCoin/Text").GetComponent<Text>();
        txtDiamond = this.Skin.transform.Find("top/itemDiamond/Text").GetComponent<Text>();
        mTank = this.Skin.transform.Find("tank3d/container").transform;

    

        txtNick.text = Global.Instance.me.Nick;
        txtCoin.text = Global.Instance.me.Coin.ToString();
        txtDiamond.text = Global.Instance.me.Diamond.ToString();

        mTlgJingJi = this.Skin.transform.Find("left/tab/TlgJingJi").GetComponent<Toggle>();
        mTlgWinBoard = this.Skin.transform.Find("left/tab/TlgWinBoard").GetComponent<Toggle>();

        mTlgJingJi.gameObject.AddComponent<HallBoardToggle>().Init(this.Skin.transform.Find("left/content/jingji").gameObject);
        mTlgWinBoard.gameObject.AddComponent<HallBoardToggle>().Init(this.Skin.transform.Find("left/content/shengju").gameObject);

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

}
