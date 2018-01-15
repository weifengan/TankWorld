using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Global : MonoBehaviour
{

    public string ServerIP = "127.0.0.1";
    public int ServerPort = 9933;
    public string defaultZone = "BasicZone";
    public string StartUIName = "AccountUI";


    private NetManager net;
    private EventManager evt;
    private UIManager ui;
    private ResManager res;
    private ConfigManager cfg;


    private Transform root;

    public PlayerVo me = new PlayerVo();

    public static LoadingUI mLoadingUI;

    private Transform uiroot2d = null;
    private static Global _instance;

    //
    public static Global GetInstance()
    {
          return _instance;
    }
    /// <summary>
    /// 当前自己所处房间
    /// </summary>
    public static Room curRoom
    {
        get { return NetManager.GetInstance().sfs.LastJoinedRoom; }
    }

    /// <summary>
    /// 网络通信类
    /// </summary>
    public static NetManager Net
    {
        get { return NetManager.GetInstance(); }
    }

    /// <summary>
    /// 网络消息管理器
    /// </summary>
    public static EventManager Evt
    {
        get { return EventManager.GetInstance(); }
    }

    /// <summary>
    /// UI管理器，负责管理场景中所有UI
    /// </summary>
    public static UIManager Ui
    {
        get { return UIManager.GetInstance(); }
    }

    /// <summary>
    /// 资源管理器
    /// </summary>
    public static ResManager Res
    {
        get { return ResManager.GetInstance(); }
    }



    #region  单例模式实现

   
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            root = this.transform.Find("UI2D");

            //创建LoadingUI
            GameObject go = new GameObject(typeof(LoadingUI).Name);
            go.transform.SetParent(root);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            GameObject skin = Instantiate<GameObject>(Resources.Load<GameObject>("UI/LoadingUI"));
            skin.name = typeof(LoadingUI).Name + "(skin)";
            skin.transform.SetParent(go.transform);

            RectTransform rtf = skin.GetComponent<RectTransform>();
            //设置UI容器大小
            rtf.anchorMin = Vector2.zero;
            rtf.anchorMax = Vector2.one;
            rtf.offsetMin = new Vector2(0, 0);
            rtf.offsetMax = new Vector2(0, 0);
            rtf.sizeDelta = new Vector2(Screen.width, Screen.height);
            skin.transform.localScale = Vector3.one;
            skin.transform.localPosition = Vector3.zero;

            mLoadingUI = go.AddComponent<LoadingUI>();

            isLoading = false;

            Screen.SetResolution(1280, 800, false);
            //框架主入口
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 全局类初始化
    /// </summary>
    public void Init()
    {
        cfg = this.gameObject.AddComponent<ConfigManager>();
        cfg.Init();
        net = this.gameObject.AddComponent<NetManager>();
        net.Init();
        evt = this.gameObject.AddComponent<EventManager>();
        evt.Init();
        res = this.gameObject.AddComponent<ResManager>();
        res.Init();
        ui = this.gameObject.AddComponent<UIManager>();
        ui.Init();




        Global.Ui.SwitchUI(StartUIName);

    }

    #endregion


    public static void Log(params object[] args)
    {
        string str = "";
        foreach (var item in args)
        {
            str += item.ToString() + " ";
        }
        Debug.Log(str);
    }
    /// <summary>
    /// 向服务器发送系统请求
    /// </summary>
    /// <param name="cmd">扩展指令</param>
    /// <param name="data">要发送的参数</param>
    public static void SendSFSRequest(IRequest request)
    {
        Global.GetInstance().net.SendSFSRequest(request);
    }

    /// <summary>
    /// 向服务器发送扩展请求
    /// </summary>
    /// <param name="cmd">请求指令</param>
    /// <param name="data">要发送的参数</param>
    /// <param name="room">null,扩展请求将发送区，不为空，扩展将发送到指定房间</param>
    /// <param name="useUDP">是否使用UDP</param>
    public static void SendExtRequest(string cmd, ISFSObject data = null, Room room = null, bool useUDP = false)
    {
        NetManager.GetInstance().SendExtRequest(cmd, data, room, useUDP);
    }
    public static void AddEventListener(string cmd, EventManager.EventHandler handler)
    {
        EventManager.GetInstance().AddEventListener(cmd, handler);
    }

    public static void RemoveEventListener(string cmd, EventManager.EventHandler handler)
    {
        EventManager.GetInstance().RemoveListener(cmd, handler);
    }


    public delegate void LoadCompleteHandler(string scenename);
    public event LoadCompleteHandler OnLoadComplete = null;
    public void LoadScene(string sceneName, LoadCompleteHandler complete = null)
    {
        isLoading = true;
        mLoadingUI.LoadScene(sceneName, complete);
    }

    public static bool isLoading
    {
        set
        {
            mLoadingUI.gameObject.SetActive(value);
            if (!value)
            { mLoadingUI.transform.SetAsFirstSibling(); }

        }
        get
        {
            return mLoadingUI.gameObject.activeSelf;
        }
    }




}
