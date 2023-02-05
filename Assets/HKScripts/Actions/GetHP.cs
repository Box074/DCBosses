using System;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("Hollow Knight")]
public class GetHP : FsmStateAction
{
	public override void Reset()
	{
		this.target = new FsmOwnerDefault();
		this.storeValue = new FsmInt
		{
			UseVariable = true
		};
	}

	public override void OnEnter()
	{
		GameObject safe = this.target.GetSafe(this);
		if (safe != null)
		{
			HealthManager component = safe.GetComponent<HealthManager>();
			if (component != null && !this.storeValue.IsNone)
			{
				this.storeValue.Value = component.hp;
			}
		}
		base.Finish();
	}

	public GetHP()
	{
	}

	[UIHint(UIHint.Variable)]
	public FsmOwnerDefault target;

	[UIHint(UIHint.Variable)]
	public FsmInt storeValue;
}
