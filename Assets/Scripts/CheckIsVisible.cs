using System;
using System.Security.Cryptography;
using UnityEngine;
using HutongGames.PlayMaker;

public class CheckIsVisible : FsmStateAction
{
    private bool Check()
    {
        var go = gameObject.GetSafe(this);
        if(go.GetComponent<Renderer>().isVisible) return true;
        foreach(var v in go.GetComponentsInChildren<Renderer>())
        {
            if(v.isVisible && v.enabled) return true;
        }
        return false;
    }
    private void DoCheck()
    {
        result.Value = Check();
    }
    public override void OnEnter()
    {
        DoCheck();
        if(!everyFrame) Finish();
    }
    public override void OnUpdate()
    {
        if(everyFrame)
        {
            DoCheck();
        }
    }
    public FsmOwnerDefault gameObject;
    [UIHint(UIHint.Variable)]
    public FsmBool result;
    public bool everyFrame;
}
