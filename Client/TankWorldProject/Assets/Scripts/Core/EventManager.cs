using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X.Core;
/**
 * 
 *   事件管理器
 *    说明:用于管理监听系统与扩展的事件管理器，可添加移除事件监听 
 * 
 * 
 * */
 public class EventManager:MonoBehaviour {
    private static EventManager _instance = null;
    public static EventManager GetInstance()
    {
         return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// 基本事件委托
    /// </summary>
    /// <param name="evt">事件对象</param>
    public delegate void EventHandler(NEvent evt);
    private Dictionary<string, EventHandler> eventListeners;

    public void Init()
    {
        eventListeners = new Dictionary<string, EventHandler>();
        Global.Log("【EventManager】初始化成功！");
    }
    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="listener">监听回调委托</param>
    public void AddEventListener(string type, EventHandler listener)
    {
        if (!eventListeners.ContainsKey(type))
        {
            EventHandler onNote = null;
            eventListeners[type] = onNote;
        }
        eventListeners[type] += listener;
    }
    /// <summary>
    /// 添加系统事件监听
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="listener">委托函数</param>
    public void AddEventListener(SFSEvent type, EventHandler listener)
    {
        if (!eventListeners.ContainsKey(type.Type))
        {
            EventHandler callback = null;
            eventListeners[type.Type] = callback;
        }
        eventListeners[type.Type] += listener;

    }


    /// <summary>
    /// 移除监听事件
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listener"></param>
    public void RemoveListener(string type, EventHandler listener)
    {
        if (eventListeners.ContainsKey(type))
        {
            return;
        }
        eventListeners[type] -= listener;
    }

    /// <summary>
    /// 移除系统事件监听
    /// </summary>
    /// <param name="type"></param>
    /// <param name="listener"></param>
    public void RemoveListener(SFSEvent type, EventHandler listener)
    {
        if (eventListeners.ContainsKey(type.Type))
        {
            return;
        }
        eventListeners[type.Type] -= listener;
    }

    /// <summary>
    ///移除事件
    /// </summary>
    /// <param name="type">事件类型</param>
    public void RemoveListener(string type)
    {
        if (eventListeners.ContainsKey(type))
        {
            eventListeners.Remove(type);
        }
    }

    /// <summary>
    /// 移除系统事件
    /// </summary>
    /// <param name="type"></param>
    public void RemoveListener(SFSEvent type)
    {
        if (eventListeners.ContainsKey(type.Type))
        {
            eventListeners.Remove(type.Type);
        }
    }

    public void DispatchEvent(NEvent note)
    {
        if (eventListeners.ContainsKey(note.Type))
        {
			eventListeners[note.Type](note);
        }
    }

    /// <summary>
    /// 检测是否存在监听
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool HasEventListener(string type)
    {
        return eventListeners.ContainsKey(type);
    }


    
}


