using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertUI : BaseUI {

    public delegate void OnAlertComplete(string type);
    public event OnAlertComplete OnComplete = null;

    private Text mContent;
    private Text mTitle;
    private Button btnOk;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/AlertUI");

        mContent = this.FetchComponentByName<Text>("content");
        mTitle = this.FetchComponentByName<Text>("title");
        btnOk = this.FetchComponentByName<Button>("btnOK");
   

        btnOk.onClick.AddListener(delegate ()
        {
            if (OnComplete != null)
            {
                OnComplete("ok");
            }

            Destroy(this.gameObject);
        });

        //调整层次
        this.transform.SetAsLastSibling();
       
    }

    public void SetData(string content="", string title = "消息") {
        mContent.text = content;
        mTitle.text = title;

    }

    







}
