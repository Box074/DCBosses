using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	public abstract class RigidBody2dActionBase : FsmStateAction
	{
		protected void CacheRigidBody2d(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			this.rb2d = go.GetComponent<Rigidbody2D>();
			if (this.rb2d == null)
			{
				base.LogWarning("Missing rigid body 2D: " + go.name);
				return;
			}
		}

		protected RigidBody2dActionBase()
		{
		}

		protected Rigidbody2D rb2d;
	}
}
