using Sfs2X.Entities.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HallBoardToggle : MonoBehaviour {

    private Toggle toggle;
    private GameObject content;
    

    public void Init(GameObject content)
    {
        toggle = this.gameObject.GetComponent<Toggle>();
        this.content = content;
        toggle.onValueChanged.AddListener(OnToggleChanged);
        if (this.name == "TlgJingJi")
        {
            //RefreshData();
        }

        Global.AddEventListener(ExtType.FetchBoardList, onFetchBoardListHandler);
    }

    private void onFetchBoardListHandler(NEvent evt)
    {
        int count = evt.Data.GetInt("count");



        SFSArray list = (SFSArray)evt.Data.GetSFSArray("list");

        //获取数据容器
        RectTransform grid = content.transform.Find("scrollRect/grid") as RectTransform;
        RectTransform prefab = content.transform.Find("scrollRect/templete") as RectTransform;

        while (grid.childCount > 0){
            Destroy(grid.GetChild(0).gameObject);
        }


        grid.sizeDelta = new Vector2(grid.sizeDelta.x, (prefab.sizeDelta.y+10) * count);
        for (int i = 0; i < count; i++)
        {
            RectTransform item = Instantiate<RectTransform>(prefab, grid);
            item.gameObject.SetActive(true);
        }


    }

 

    private void RefreshData()
    {
        SFSObject data = new SFSObject();
        data.PutInt("type", 1);
        Global.SendExtRequest(ExtType.FetchBoardList,data);
    }

    private void OnToggleChanged(bool arg0)
    {
        if (toggle.isOn)
        {
            //请求数据
            print("请求数据" + this.name);
            RefreshData();
        }
    }


}
