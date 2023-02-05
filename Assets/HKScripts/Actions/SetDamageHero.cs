using System;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("Hollow Knight")]
public class SetDamageHeroAmount : FsmStateAction
{
	public override void Reset()
	{
		this.target = new FsmOwnerDefault();
		this.damageDealt = null;
	}

	public override void OnEnter()
	{
		GameObject safe = this.target.GetSafe(this);
		if (safe != null)
		{
			DamageHero component = safe.GetComponent<DamageHero>();
			if (component != null && !this.damageDealt.IsNone)
			{
				component.damageDealt = this.damageDealt.Value;
			}
		}
		base.Finish();
	}

	public SetDamageHeroAmount()
	{
	}

	[UIHint(UIHint.Variable)]
	public FsmOwnerDefault target;

	public FsmInt damageDealt;
}
