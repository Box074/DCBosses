using System;
using System.Security.Cryptography;
using UnityEngine;
using HutongGames.PlayMaker;

public class CheckHeroStatus : FsmStateAction
{
    public static Func<bool> onCheckMoveable;
    public static Func<bool> onCheckFocus;
    public static Func<bool> onCheckQuake;
    public static Func<bool> onCheckFocusWithShield;
    private void Check(FsmBool result, Func<bool> check, bool defaultValue = false) 
    {
        if(result?.IsNone ?? true) return;
        result.Value = check?.Invoke() ?? defaultValue;
    }
    private void DoCheck()
    {
        Check(isMoveable, onCheckMoveable);
        Check(isFocus, onCheckFocus);
        Check(isQuake, onCheckQuake);
        Check(isFocusWithShield, onCheckFocusWithShield);
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
    [UIHint(UIHint.Variable)]
    public FsmBool isMoveable;
    [UIHint(UIHint.Variable)]
    public FsmBool isFocus;
    [UIHint(UIHint.Variable)]
    public FsmBool isFocusWithShield;
    [UIHint(UIHint.Variable)]
    public FsmBool isQuake;
    public bool everyFrame;
}
