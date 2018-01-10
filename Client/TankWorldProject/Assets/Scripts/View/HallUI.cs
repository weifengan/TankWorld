using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallUI : BaseUI {

    private Text txtNick;
    private Text txtCoin;
    private Text txtDiamond;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/HallUI");
        txtNick = this.Skin.transform.Find("top/txtNick").GetComponent<Text>();
        txtCoin = this.Skin.transform.Find("top/itemCoin/Text").GetComponent<Text>();
        txtDiamond = this.Skin.transform.Find("top/itemDiamond/Text").GetComponent<Text>();

        txtNick.text = Global.Instance.me.Nick;
        txtCoin.text = Global.Instance.me.Coin.ToString();
        txtDiamond.text = Global.Instance.me.Diamond.ToString();
    }
}
