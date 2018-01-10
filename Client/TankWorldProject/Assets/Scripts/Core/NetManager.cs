using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sfs2X.Requests;
using Sfs2X.Entities;

public class NetManager : MonoBehaviour
{

    private static NetManager _instance = null;
    public static NetManager GetInstance()
    {
       return _instance;
    }

    private void Awake()
    {
        _instance = this;
        Init();
    }

    /// <summary>
    /// smartfoxserver 客户端对象
    /// </summary>
    private SmartFox _sfs = new SmartFox();

    /// <summary>
    /// 返回SmartFox对象
    /// </summary>
    public SmartFox sfs
    {
        get { return _sfs; }
    }
    /// <summary>
    /// 网络管理器初始化函数
    /// </summary>
    private void Init()
    {
        //用于单例化

        //监听客户端事件
        sfs.AddEventListener(SFSEvent.CONNECTION, OnSFSEventHandler);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnSFSEventHandler);

        //监听客户端登录事件
        sfs.AddEventListener(SFSEvent.LOGIN, OnSFSEventHandler);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnSFSEventHandler);

        //监听用户加入 房间
        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnSFSEventHandler);
        //监听扩展反馈
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionHandler);

        //监听用户变量变化
        sfs.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnSFSEventHandler);
        //监听有用户进入房间
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnSFSEventHandler);
        //监听用户离开房间
        sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnSFSEventHandler);

        //监听房间变变量
        sfs.AddEventListener(SFSEvent.ROOM_VARIABLES_UPDATE, OnSFSEventHandler);

        //监听Socket Error;
        sfs.AddEventListener(SFSEvent.SOCKET_ERROR, OnSFSEventHandler);
        sfs.AddEventListener(SFSEvent.ROOM_ADD, OnSFSEventHandler);
        sfs.AddEventListener(SFSEvent.ROOM_NAME_CHANGE, OnSFSEventHandler);
        Global.Log("【"+this.GetType().Name+"】初始化成功！");
    }

    /// <summary>
    /// 链接到服务器
    /// </summary>
    /// <param name="serverIP">服务器IP</param>
    /// <param name="serverPort">端口</param>
    public void Connect(string serverIP = "127.0.0.1", int serverPort = 9933)
    {
        sfs.Connect(serverIP, serverPort);
    }
    /// <summary>
    /// 向服务器发送系统请求
    /// </summary>
    /// <param name="sfsReqest">系统请求对象</param>
    public void SendSFSRequest(IRequest sfsReqest)
    {
        this._sfs.Send(sfsReqest);
    }
    /// <summary>
    /// 向服务器发送扩展
    /// </summary>
    /// <param name="extName">扩展名称</param>
    /// <param name="data">SFSObject数据对象</param>
    /// <param name="room">room为null时，扩展发送到ZoneExtension,房间不为null,扩展发送到指定房间</param>
    public void SendExtRequest(string extName, ISFSObject data = null, Room room = null)
    {
        if (data == null)
        {
            data = new SFSObject();
        }
        this._sfs.Send(new ExtensionRequest(extName, data, room));
    }
    /// <summary>
    /// 向服务器发送扩展
    /// </summary>
    /// <param name="extName">扩展名称</param>
    /// <param name="data">SFSObject数据对象</param>
    /// <param name="room">room为null时，扩展发送到ZoneExtension,房间不为null,扩展发送到指定房间</param>
    /// <param name="useUdp">是否使用UDP</param>
    public void SendExtRequest(string extName, ISFSObject data = null, Room room = null, bool useUdp = false)
    {
        if (data == null)
        {
            data = new SFSObject();
        }
        this._sfs.Send(new ExtensionRequest(extName, data, room, useUdp));
    }
    /// <summary>
    /// 自定义扩展事件
    /// </summary>
    /// <param name="evt">事件对象</param>
    private void OnExtensionHandler(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        NEvent e = new NEvent(evt.Target, cmd, evt);
        EventManager.GetInstance().DispatchEvent(e);
    }

    /// <summary>
    /// SmartFoxServer系统事件
    /// </summary>
    /// <param name="evt"></param>
    private void OnSFSEventHandler(BaseEvent evt)
    {
        object target = evt.Target;
        string cmd = evt.Type.ToString();
        NEvent e = new NEvent(target, cmd, evt);
        switch (cmd)
        {
            case "connectionLost":
                UIManager.GetInstance().Alert("与服务器的连接丢失!");
                break;
        }
        EventManager.GetInstance().DispatchEvent(e);
    }


    private void Update()
    {
        if (sfs != null)
        {
            sfs.ProcessEvents();
        }
    }

    private void OnDestroy()
    {
        if (sfs != null)
        {
            sfs.Disconnect();
        }   
    }

}
