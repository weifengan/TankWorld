using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private static UIManager _instance = null;
    public static UIManager GetInstance()
    {
        return _instance;
    }


    private RectTransform root = null;
    private GraphicRaycaster raycaster = null;

    public GraphicRaycaster Raycaster
    {
        get
        {
            return raycaster;
        }
    }

    private GameObject currentUI = null;

    public void Display(bool isShow=true)
    {
        if (currentUI != null)
        {
            currentUI.gameObject.SetActive(isShow);
        }
    }

    private void Awake()
    {
        _instance = this;
        root = this.transform.Find("UI").GetComponent<RectTransform>();
        raycaster = root.GetComponent<GraphicRaycaster>();
        Init();
    }
    // Use this for initialization
    void Init() {
        Global.Log("【" + this.GetType().Name + "】初始化成功！");
    }

    // Update is called once per frame
    public T GetNewUI<T>() where T : BaseUI
    {
        GameObject go = new GameObject(typeof(T).Name);
        RectTransform rtf = go.AddComponent<RectTransform>();
        rtf.SetParent(root);
        rtf.anchoredPosition = Vector2.zero;
        T tmp = go.AddComponent<T>();



        // RectTransform rtf = tmp.transform as RectTransform;
        //rtf.transform.position = Vector3.one;
        return tmp;
    }

    /// <summary>
    /// 跳转游戏界面
    /// </summary>
    /// <param name="name">场景名称</param>
    /// <param name="args">参数列表</param>
    public void SwitchUI(string name, params object[] args)
    {
        print("switch UI" + name);
        GameObject scene = new GameObject(name);
        RectTransform rtf = scene.AddComponent<RectTransform>();
        //设置UI容器大小
        rtf.anchorMin = Vector2.zero;
        rtf.anchorMax = Vector2.one;
        rtf.offsetMin = new Vector2(0, 0);
        rtf.offsetMax = new Vector2(0, 0);
        rtf.sizeDelta = new Vector2(Screen.width, Screen.height);

        BaseUI bi = scene.AddComponent(Type.GetType(name)) as BaseUI;
        if (bi != null)
        {
            bi.args = args;
        }
        if (currentUI != null)
        {
            Destroy(currentUI);
        }
        scene.transform.SetParent(root);
        scene.transform.localScale = Vector3.one;
        scene.transform.localPosition = Vector3.zero;
        currentUI = scene;
    }

    public AlertUI Alert(string content = "", string title = "消息")
    {
        AlertUI malert = GetNewUI<AlertUI>();
        RectTransform pref = malert.GetComponent<RectTransform>();
        pref.SetParent(root);
        pref.localPosition = Vector3.zero;
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        pref.offsetMin = new Vector2(0, 0);
        pref.offsetMax = new Vector2(0, 0);
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        malert.SetData(content, title);
        pref.localScale = Vector3.one;

        return malert;
    }

    public ConfirmUI Confirm(string content = "", string title = "消息",ConfirmUI.ConfirmHandler handler=null)
    {
        ConfirmUI malert = GetNewUI<ConfirmUI>();
        RectTransform pref = malert.GetComponent<RectTransform>();
        pref.SetParent(root);
        pref.localPosition = Vector3.zero;
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        pref.offsetMin = new Vector2(0, 0);
        pref.offsetMax = new Vector2(0, 0);
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        pref.localScale = Vector3.one;
        malert.SetData(content, title);
        
        malert.OnConfirmHandler+= handler;
        return malert;
    }


    public T PopUpUI<T>() where T : BaseUI
    {
        T mUI = GetNewUI<T>();
        RectTransform pref = mUI.GetComponent<RectTransform>();
        pref.SetParent(root);
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        pref.anchorMin = Vector2.zero;
        pref.anchorMax = Vector2.one;
        pref.offsetMin = new Vector2(0, 0);
        pref.offsetMax = new Vector2(0, 0);
        pref.sizeDelta = new Vector2(Screen.width, Screen.height);
        return mUI;
    }

    
 
}
