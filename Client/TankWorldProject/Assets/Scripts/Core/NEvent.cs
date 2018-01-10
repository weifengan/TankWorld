using Sfs2X.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sfs2X.Entities.Data;

public class NEvent
{

    /// <summary>
    /// 事件发送者
    /// </summary>
    private object _target = null;

    /// <summary>
    /// 事件类型
    /// </summary>
	private string _type = null;


    private IDictionary _params = null;

    /// <summary>
    /// 事件对象
    /// </summary>
    private BaseEvent _baseEvt = null;

    /// <summary>
    /// 事件信息包括(Ext和Sys)
    /// </summary>
    private SFSObject _data = null;

    /// <summary>
    /// 自定义事件类型,对系统事件与扩展事件进行封闭
    /// </summary>
    /// <param name="target">事件触发目标</param>
    /// <param name="type">事件类型</param>
    /// <param name="data">事件</param>
    /// <param name="evt"></param>
    public NEvent(object target, string type,BaseEvent evt)
    {

        this._target = target;
        this._type = type;
        this._baseEvt = evt;
        this._params = evt.Params;
        this._data = evt.Params["params"] as SFSObject;
        
    }
    /// <summary>
    /// 事件触发对象SmartFox客户端
    /// </summary>
    /// <value>The target.</value>
    public object Target
    {
        get { return this._target; }
    }

    /// <summary>
    /// 事件类型的字符串
    /// </summary>
    /// <value>The type.</value>
    public string Type
    {
        get { return this._type; }
    }

    /// <summary>
    /// 事件的数据
    /// </summary>
    /// <value>The data.</value>
    public SFSObject Data
    {
        get { return this._data; }
    }

    /// <summary>
    /// 参数对象，相当BaseEvent.Params属性
    /// </summary>
    public IDictionary Prarams
    {
        get { return this._params; }
    }
    /// <summary>
    /// Base事件对象
    /// </summary>
    /// <value>The evt.</value>
    public BaseEvent BaseEvt
    {
        get { return this._baseEvt; }
    }

}
