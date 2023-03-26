using System;
using HutongGames.PlayMaker;
using UnityEngine;

public class SetPlayerData : FsmStateAction
{
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
#if BUILD_HKMOD
        var type = value.GetType();
        var md = typeof(PlayerData).GetMethod(nameof(PlayerData.SetVariable)).MakeGenericMethod(type);
        md.Invoke(PlayerData.instance, new object[] { pdName.Value, value });
#endif
    }
    
    
    public FsmString pdName;
    public FsmVar value;
    public bool everyFrame;
}
