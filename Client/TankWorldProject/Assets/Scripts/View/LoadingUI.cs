using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour {

    private string sceneName = "";
    private AsyncOperation async = null;
    private Image bar;
    private Text txtpro;
    void Awake() {
        bar = this.transform.Find("LoadingUI(skin)/progress/bar").GetComponent<Image>();
        txtpro = this.transform.Find("LoadingUI(skin)/progress/txtpro").GetComponent<Text>();
        bar.fillAmount = 0;
        txtpro.text = "加载中(0%)";
    }

    private Global.LoadCompleteHandler handler = null;
    public void LoadScene(string sceneName, Global.LoadCompleteHandler handler=null)
    {
        this.handler = handler;
        Global.isLoading = true;
        UIManager.GetInstance().Display(false);
        StartCoroutine(doload(sceneName));
    }

    private IEnumerator doload(string sceneName)
    {
        this.sceneName = sceneName;
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;
     }

    void Update()
    {
        //始终保持最前
        this.transform.SetAsLastSibling();
        if (async != null)
        {
            float target = async.progress;
            if (async.progress <0.9f)
            {
                target = 0.9f;
               
            }else if(async.progress==0.9f)
            {
                target = 1;
            }
          
            bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, target, Time.deltaTime);
            txtpro.text = Mathf.Ceil(bar.fillAmount * 100) + "%";

        }

        if (bar.fillAmount == 1)
        {
           
            async.allowSceneActivation = true;
            Global.isLoading = false;
            UIManager.GetInstance().Display(true);
            if (handler != null)
            {
                handler(this.sceneName);
            }
        }

    }



}
