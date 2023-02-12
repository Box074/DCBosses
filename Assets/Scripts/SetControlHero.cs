using System;
using HutongGames.PlayMaker;
using UnityEngine;

public class SetControlHero : FsmStateAction
{
    public static Action<bool> onSetControlHero;
    public override void OnEnter()
    {
        DoSet();
        if(!everyFrame)
        {
            Finish();
        }
    }
    public override void OnUpdate()
    {
        if(everyFrame)
        {
            DoSet();
        }
    }
    private void DoSet()
    {
        onSetControlHero?.Invoke(value?.Value ?? false);
    }
    
    public FsmBool value;
    public bool everyFrame;
}
