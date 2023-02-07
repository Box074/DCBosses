using System;
using System.Security.Cryptography;
using UnityEngine;
using HutongGames.PlayMaker;

public class GetOrAddScreenCut : FsmStateAction
{
    public FsmObject result;
    public Shader shader;
    public override void OnEnter()
    {
        var mainCam = Camera.main.gameObject;
        var r = mainCam.GetComponent<ScreenCut>() ?? mainCam.AddComponent<ScreenCut>();
        result.Value = r;
        r.mat ??= new Material(shader ?? Shader.Find("DC/CutScreen"));
        r.bindObj = Fsm.GameObject;
        r.cutWidth = 0.07f;
        Finish();
    }
}
