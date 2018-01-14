using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : MonoBehaviour
{
    private static ResManager _instance = null;
    public static ResManager GetInstance()
    {
       return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }
    //根据类型来缓存资源，类型==>(资源路径,预设)
    private Dictionary<string, object> mDic = new Dictionary<string, object>();
    public void Init()
    {
        Global.Log("【" + this.GetType().Name + "】初始化成功！");
    }

    /// <summary>
    /// 通过资源类型和资源名称加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type">资源类型</param>
    /// <param name="resName">资源名称</param>
    /// <param name="isNew">是否返回资源实例</param>
    /// <param name="cache">是否缓存</param>
    /// <returns></returns>
    public T GetRes<T>(string resPath,bool isNew=true,bool cache = false) where T:Object
    {
        T prefab = null;
        ///如果之前没存储过该资源
        if (!mDic.ContainsKey(resPath))
        {
            //加载此资源
            prefab = Resources.Load<T>(resPath);
            //如果开启缓存 ，则会进行缓存 
            if (cache)
            {
                mDic.Add(resPath, prefab);
            }
          
        }else
        {
            //从字典中获取到资源
            prefab = mDic[resPath] as T;
         }
        return isNew ? Instantiate<T>(prefab) : prefab;
    }


    //通过名称加载游戏对象
    public GameObject CreateGameObject(string path)
    {
        try
        {
            GameObject go = Resources.Load<GameObject>(path);
            GameObject ui = Instantiate(go);
            return ui;
        }
        catch
        {
            Debug.Log("为找到皮肤资源" + path);
            return null;
        }
    }
}
