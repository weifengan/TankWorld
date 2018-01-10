using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmUI : BaseUI {

    public enum ConfirmType
    {
        Yes,No
    }
    public delegate void ConfirmHandler(ConfirmType type);
    public event ConfirmHandler OnConfirmHandler = null;


    private Button btnYes;
    private Button btnNo;
    private Text mContent;
    private Text mTitle;
    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/ConfirmUI");

        mContent = this.GetComponentByName<Text>("content");
        mTitle = this.GetComponentByName<Text>("title");

        btnYes = this.GetComponentByName<Button>("btnYes");
        btnNo = this.GetComponentByName<Button>("btnNo");

        btnYes.onClick.AddListener(delegate () {
            if (OnConfirmHandler != null)
            {
                OnConfirmHandler(ConfirmType.Yes);
            }
            Destroy(this.gameObject);
        });


        btnNo.onClick.AddListener(delegate () {
            if (OnConfirmHandler != null)
            {
                OnConfirmHandler(ConfirmType.No);
            }
            Destroy(this.gameObject);
        });


    }
    public void SetData(string content = "", string title = "消息")
    {
        mContent.text = content;
        mTitle.text = title;

    }


    private void OnDestroy()
    {
        OnConfirmHandler = null;
        btnYes = null;
        btnNo = null;
    }

}
