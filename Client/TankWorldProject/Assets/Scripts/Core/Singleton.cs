using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : MonoBehaviour,new()  {

    private static T _instance = default(T);
    public static T GetInstance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject(typeof(T).Name);
            _instance = go.AddComponent<T>();
           // _instance = new T();
        }
        return _instance;
    }
	
}
