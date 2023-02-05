using System;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("Hollow Knight")]
public class SetHP : FsmStateAction
{
	public override void Reset()
	{
		this.target = new FsmOwnerDefault();
		this.hp = new FsmInt();
	}

	public override void OnEnter()
	{
		GameObject safe = this.target.GetSafe(this);
		if (safe != null)
		{
			HealthManager component = safe.GetComponent<HealthManager>();
			if (component != null && !this.hp.IsNone)
			{
				component.hp = this.hp.Value;
			}
		}
		base.Finish();
	}

	public SetHP()
	{
	}

	[UIHint(UIHint.Variable)]
	public FsmOwnerDefault target;

	public FsmInt hp;
}
