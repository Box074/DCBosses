using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("Enemy AI")]
	[Tooltip("Object moves more directly toward target")]
	public class ChaseObjectV2 : RigidBody2dActionBase
	{
		public override void Reset()
		{
			this.gameObject = null;
			this.target = null;
			this.accelerationForce = 0f;
			this.speedMax = 0f;
			this.offsetX = 0f;
			this.offsetY = 0f;
		}

		public override void Awake()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		public override void OnEnter()
		{
			base.CacheRigidBody2d(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			this.self = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			this.DoChase();
		}

		public override void OnFixedUpdate()
		{
			this.DoChase();
		}

		private void DoChase()
		{
			if (this.rb2d == null)
			{
				return;
			}
			Vector2 vector = new Vector2(this.target.Value.transform.position.x + this.offsetX.Value - this.self.Value.transform.position.x, this.target.Value.transform.position.y + this.offsetY.Value - this.self.Value.transform.position.y);
			vector = Vector2.ClampMagnitude(vector, 1f);
			vector = new Vector2(vector.x * this.accelerationForce.Value, vector.y * this.accelerationForce.Value);
			this.rb2d.AddForce(vector);
			Vector2 vector2 = this.rb2d.velocity;
			vector2 = Vector2.ClampMagnitude(vector2, this.speedMax.Value);
			this.rb2d.velocity = vector2;
		}

		public ChaseObjectV2()
		{
		}

		[RequiredField]
		[CheckForComponent(typeof(Rigidbody2D))]
		[UIHint(UIHint.Variable)]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.Variable)]
		public FsmGameObject target;

		public FsmFloat speedMax;

		public FsmFloat accelerationForce;

		public FsmFloat offsetX;

		public FsmFloat offsetY;

		private FsmGameObject self;
	}
}
