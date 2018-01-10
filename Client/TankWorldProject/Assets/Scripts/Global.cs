using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Global : MonoBehaviour {

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


    public static  NetManager Net
    {
        get { return NetManager.GetInstance(); }
    }

    public static EventManager Evt
    {
        get { return EventManager.GetInstance(); }
    }

    public static UIManager Ui
    {
        get { return UIManager.GetInstance(); }
    }

    public static ResManager Res
    {
        get { return ResManager.GetInstance(); }
    }



    #region  单例模式实现

    private static Global _instance;
    public static Global Instance
    {
        get { return _instance; }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            root = this.transform.Find("UI");
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
    private void Init()
    {
        cfg = this.gameObject.AddComponent<ConfigManager>();
        net = this.gameObject.AddComponent<NetManager>();
        evt = this.gameObject.AddComponent<EventManager>();
        res = this.gameObject.AddComponent<ResManager>();
        ui = this.gameObject.AddComponent<UIManager>();


        Global.Ui.SwitchUI(StartUIName);

    }

    #endregion

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void Log(params object[] args)
    {
        string str = "";
        foreach (var item in args)
        {
            str += item.ToString() + " ";
        }
        Debug.Log(str);
    }

    public static void SendSFSRequest(IRequest request)
    {
        Global.Instance.net.SendSFSRequest(request);
    }

    public static void SendExtRequest(string cmd,ISFSObject data=null,Room room=null,bool useUDP = false)
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


    

}
