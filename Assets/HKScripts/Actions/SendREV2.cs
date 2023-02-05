using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random Event picked from an array of Events. Optionally set the relative weight of each event. Use ints to keep events from being fired x times in a row.")]
	public class SendRandomEventV2 : FsmStateAction
	{
		public override void Reset()
		{
			this.events = new FsmEvent[3];
			this.weights = new FsmFloat[]
			{
				1f,
				1f,
				1f
			};
		}

		public override void OnEnter()
		{
			bool flag = false;
			while (!flag)
			{
				int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
				if (randomWeightedIndex != -1 && this.trackingInts[randomWeightedIndex].Value < this.eventMax[randomWeightedIndex].Value)
				{
					int value = ++this.trackingInts[randomWeightedIndex].Value;
					for (int i = 0; i < this.trackingInts.Length; i++)
					{
						this.trackingInts[i].Value = 0;
					}
					this.trackingInts[randomWeightedIndex].Value = value;
					flag = true;
					base.Fsm.Event(this.events[randomWeightedIndex]);
				}
			}
			base.Finish();
		}

		[CompoundArray("Events", "Event", "Weight")]
		public FsmEvent[] events;

		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		[UIHint(UIHint.Variable)]
		public FsmInt[] trackingInts;

		public FsmInt[] eventMax;

		private DelayedEvent delayedEvent;
	}
}
