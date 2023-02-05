using System;
using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory("Hollow Knight")]
public class SetInvincible : FsmStateAction
{
	public override void Reset()
	{
		this.target = new FsmOwnerDefault();
		this.Invincible = null;
		this.InvincibleFromDirection = null;
	}

	public override void OnEnter()
	{
		GameObject safe = this.target.GetSafe(this);
		if (safe != null)
		{
			HealthManager component = safe.GetComponent<HealthManager>();
			if (component != null)
			{
				if (!this.Invincible.IsNone)
				{
					component.IsInvincible = this.Invincible.Value;
				}
				if (!this.InvincibleFromDirection.IsNone)
				{
					component.InvincibleFromDirection = this.InvincibleFromDirection.Value;
				}
			}
		}
		base.Finish();
	}

	public SetInvincible()
	{
	}

	[UIHint(UIHint.Variable)]
	public FsmOwnerDefault target;

	public FsmBool Invincible;

	public FsmInt InvincibleFromDirection;
}
