using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : BaseUI {

    protected override void OnAwake()
    {
        base.OnAwake();
        this.InitSkin("UI/GameUI");
    }
}
