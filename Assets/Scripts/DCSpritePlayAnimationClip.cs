using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DCSpritePlayAnimationClip : FsmStateAction
{
    public override void OnEnter()
    {
        target.GetSafe(this).GetComponent<DCSpriteAnimator>().curClipName = name.Value;
        Finish();
    }

    public override void Reset()
    {
        name = new FsmString();
    }

    public FsmOwnerDefault target;
    public FsmString name;
}

