using System;
using System.Security.Cryptography;
using UnityEngine;
using HutongGames.PlayMaker;

public class CheckHeroStatus : FsmStateAction
{

    private void DoCheck()
    {
#if BUILD_HKMOD
        isFocus.Value = HeroController.instance.cState.focusing;
        isFocusWithShield.Value = HeroController.instance.cState.focusing && PlayerData.instance.equippedCharm_5;
        isQuake.Value = HeroController.instance.cState.spellQuake;
#endif
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
