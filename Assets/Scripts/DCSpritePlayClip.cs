using HutongGames.PlayMaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DCSpritePlayClip : FsmStateAction
{
    public override void OnEnter()
    {
        var host = target.GetSafe(this).GetComponent<DCSpriteAnimatorHost>();
        if(host != null)
        {
            host.curClipName = name.Value;
        }
        else
        {
            target.GetSafe(this).GetComponent<DCSpriteAnimator>().Play(name.Value);
        }
        Finish();
    }

    public override void Reset()
    {
        name = new FsmString();
    }

    public FsmOwnerDefault target;
    public FsmString name;
}

