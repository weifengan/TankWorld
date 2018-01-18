/**************************************
*  文 件 名: UIBase
*  描    述：
*  作　　者: 魏凤安
*  Q     Q: 1327797237
*  手机号码: 17746514110
*  电子邮箱: wefengan@163.com
*  博客地址: http://www.weifengan.com/
**************************************/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class BaseUI : MonoBehaviour
{

    public object[] args;
    #region 用于托管系统生命周期内部分函数
    void Awake()
    {
        OnAwake();
    }
    /// <summary>
    /// 托管系统Awake()的函数名
    /// </summary>
    protected virtual void OnAwake()
    {
    }

    private IEnumerator FadeIn()
    {
        yield return null;
    }
    void Start()
    {
        OnStart();
    }
    /// <summary>
    /// 托管系统Start()的函数名
    /// </summary>
    protected virtual void OnStart()
    {

    }




    void Update()
    {
        OnUpdate();
    }
    /// <summary>
    /// 托管系统Update()的函数名
    /// </summary>
    protected virtual void OnUpdate()
    {

    }


    void OnDestory()
    {
        OnExit();
    }
    /// <summary>
    /// 托管系统OnDestroy()的函数名
    /// </summary>
    protected virtual void OnExit()
    {

    }
    #endregion



    #region 皮肤相关属性

    private GameObject _skin;
    /// <summary>
    /// 皮肤GameObject
    /// </summary>
    public GameObject Skin
    {
        get { return _skin; }
    }

    private string _skinPath;

    /// <summary>
    /// 皮肤路径
    /// </summary>
    public string SkinPath
    {
        get { return _skinPath; }
    }

    #endregion


    /// <summary>
    /// 初始化皮肤
    /// </summary>
    /// <param name="skinPath">此场景需要使用的皮肤路径</param>
    public void InitSkin(string skinPath)
    {
        if (!string.IsNullOrEmpty(skinPath))
        {
            _skinPath = skinPath;
            //使用资源管理器去加载需要加载的UI预置体
            _skin = ResManager.GetInstance().GetRes<GameObject>(_skinPath,true,true);
            //调整父级容器
            _skin.transform.SetParent(this.transform);
            _skin.name = "skin(" + this.name + ")";


            RectTransform rtf1 = _skin.transform as RectTransform;
            rtf1.anchorMin = Vector2.zero;
            rtf1.anchorMax = Vector2.one;
            rtf1.offsetMin = new Vector2(0, 0);
            rtf1.offsetMax = new Vector2(0, 0);
            //rtf1.sizeDelta = new Vector2(Screen.width, Screen.height);

        }
        //皮肤初始化完成后，调用初始化函数
        OnInit();
    }

    public void SetSizeDelta(int width, int height)
    {
        SetSizeDelta(new Vector2(width, height));
    }
    /// <summary>
    /// 设置UI尺寸大小
    /// </summary>
    /// <param name="size"></param>
    public void SetSizeDelta(Vector2 size)
    {
        //调整容器
        RectTransform rtf = this.transform as RectTransform;
        rtf.anchorMin = Vector2.zero;
        rtf.anchorMax = Vector2.one;
        rtf.offsetMin = new Vector2(0, 0);
        rtf.offsetMax = new Vector2(0, 0);
        rtf.sizeDelta = size;

        //调整内容
        rtf = this.Skin.transform as RectTransform;
        rtf.anchorMin = Vector2.zero;
        rtf.anchorMax = Vector2.one;
        rtf.offsetMin = new Vector2(0, 0);
        rtf.offsetMax = new Vector2(0, 0);
    }
    /// <summary>
    /// 用于在子类中处理初始化功能
    /// </summary>
    public virtual void OnInit()
    {
        //统一处理可以点击交互UI的点击事件
        Button[] btns = this.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < btns.Length; i++)
        {
            //从找到的数组中，读取按钮组件，并添加点击监听
            Button btn = btns[i];
            btn.onClick.AddListener(delegate ()
            {
                OnClickHandler(btn.gameObject);
            });
        }
    }

    /// <summary>
    /// 按钮点击虚函数
    /// </summary>
    /// <param name="go"></param>
    protected virtual void OnClickHandler(GameObject go)
    {

    }

    /// <summary>
    /// 通过名称获取游戏物体的Transform
    /// </summary>
    /// <param name="name">游戏对象</param>
    /// <returns></returns>
    public RectTransform FetchTransform(string name)
    {
        List<RectTransform> trans = new List<RectTransform>();
        //获取皮肤上的所有RectTransform组件
        this.Skin.transform.GetComponentsInChildren<RectTransform>(true, trans);
        foreach (var item in trans)
        {
            if (item.name.Equals(name))
            {
                return item;
            }
        }
        return null;
    }

    /// <summary>
    /// 获取特定游戏对象上的特定组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T FetchComponentByName<T>(string name) where T : MonoBehaviour
    {
        T[] trans = this.Skin.transform.GetComponentsInChildren<T>();

        foreach (T rtf in trans)
        {
            if (rtf.name == name)
            {
                return rtf;
            }
        }
        return default(T);

    }
}
