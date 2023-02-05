using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Sets the velocity of all children of chosen GameObject")]
	public class FlingObjects : RigidBody2dActionBase
	{
		public override void Reset()
		{
			this.containerObject = null;
			this.adjustPosition = null;
			this.speedMin = null;
			this.speedMax = null;
			this.angleMin = null;
			this.angleMax = null;
		}

		public override void OnEnter()
		{
			GameObject value = this.containerObject.Value;
			if (value != null)
			{
				int childCount = value.transform.childCount;
				for (int i = 1; i <= childCount; i++)
				{
					GameObject gameObject = value.transform.GetChild(i - 1).gameObject;
					base.CacheRigidBody2d(gameObject);
					if (this.rb2d != null)
					{
						float num = UnityEngine.Random.Range(this.speedMin.Value, this.speedMax.Value);
						float num2 = UnityEngine.Random.Range(this.angleMin.Value, this.angleMax.Value);
						this.vectorX = num * Mathf.Cos(num2 * 0.017453292f);
						this.vectorY = num * Mathf.Sin(num2 * 0.017453292f);
						Vector2 velocity;
						velocity.x = this.vectorX;
						velocity.y = this.vectorY;
						this.rb2d.velocity = velocity;
						if (!this.adjustPosition.IsNone)
						{
							if (this.randomisePosition.Value)
							{
								gameObject.transform.position = new Vector3(gameObject.transform.position.x + UnityEngine.Random.Range(-this.adjustPosition.Value.x, this.adjustPosition.Value.x), gameObject.transform.position.y + UnityEngine.Random.Range(-this.adjustPosition.Value.y, this.adjustPosition.Value.y), gameObject.transform.position.z);
							}
							else
							{
								gameObject.transform.position += this.adjustPosition.Value;
							}
						}
					}
				}
			}
			base.Finish();
		}

		public FlingObjects()
		{
		}

		[RequiredField]
		[Tooltip("Object containing the objects to be flung.")]
		public FsmGameObject containerObject;

		public FsmVector3 adjustPosition;

		public FsmBool randomisePosition;

		[Tooltip("Minimum speed clones are fired at.")]
		public FsmFloat speedMin;

		[Tooltip("Maximum speed clones are fired at.")]
		public FsmFloat speedMax;

		[Tooltip("Minimum angle clones are fired at.")]
		public FsmFloat angleMin;

		[Tooltip("Maximum angle clones are fired at.")]
		public FsmFloat angleMax;

		private float vectorX;

		private float vectorY;

		private bool originAdjusted;
	}
}
