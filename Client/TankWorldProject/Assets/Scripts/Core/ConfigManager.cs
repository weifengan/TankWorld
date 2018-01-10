using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : MonoBehaviour {

    private static ConfigManager _instance = null;

    private TankConfig _tankConfig;
    public TankConfig Tank
    {
        get { return _tankConfig; }
    }
    public static ConfigManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        Init();
    }

    private void Init()
    {
        _tankConfig = new TankConfig();
        _tankConfig.Init();
    }
}
