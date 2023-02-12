using System;
using HutongGames.PlayMaker;
using UnityEngine;

public class SetPlayerData : FsmStateAction
{
    public static Action<string, object> onSetPlayerData;
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
        onSetPlayerData?.Invoke(pdName?.Value, value?.GetValue());
    }
    
    
    public FsmString pdName;
    public FsmVar value;
    public bool everyFrame;
}
