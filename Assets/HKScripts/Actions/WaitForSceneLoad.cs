using System;
using System.Runtime.CompilerServices;
using HutongGames.PlayMaker;

[ActionCategory("Hollow Knight")]
public class WaitForSceneLoadFinish : FsmStateAction
{
	public override void Reset()
	{
		this.sendEvent = null;
	}

	public override void OnEnter()
	{
		Finish();
	}

	public FsmEvent sendEvent;

}
