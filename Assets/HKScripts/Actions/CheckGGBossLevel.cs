using System;
using HutongGames.PlayMaker;

[ActionCategory("Hollow Knight/GG")]
public class CheckGGBossLevel : FsmStateAction
{
	public override void Reset()
	{
		this.notGG = null;
		this.level1 = null;
		this.level2 = null;
		this.level3 = null;
	}

	public override void OnEnter()
	{
		base.Finish();
	}

	public FsmEvent notGG;

	public FsmEvent level1;

	public FsmEvent level2;

	public FsmEvent level3;
}
