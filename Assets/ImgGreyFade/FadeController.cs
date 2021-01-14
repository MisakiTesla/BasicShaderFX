using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public RawImage img;

    private void Start()
    {
        img.material.SetFloat("_Fade", 0f);
        Observable.EveryUpdate().Where(_ => Time.time <= 3f).Subscribe(x =>
        {
            Debug.Log($"{Time.time}");
            img.material.SetFloat("_Fade", Time.time/3f);
        }).AddTo(this);
    }
}
