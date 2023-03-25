using System;
using HutongGames.PlayMaker;
using UnityEngine;

public class SetControlHero : FsmStateAction
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
        var hc = HeroController.instance;
        if (value.Value)
        {
            hc.RelinquishControlNotVelocity();
            hc.StopAnimationControl();
            hc.GetComponent<tk2dSpriteAnimator>().Play("Idle");
        }
        else
        {
            hc.StartAnimationControl();
            hc.RegainControl();
        }
#endif
    }
    
    public FsmBool value;
    public bool everyFrame;
}
