using System;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Sends a Random Event picked from an array of Events. Optionally set the relative weight of each event. Use ints to keep events from being fired x times in a row.")]
	public class SendRandomEventV3 : FsmStateAction
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
			bool flag2 = false;
			int num = 0;
			while (!flag)
			{
				int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
				if (randomWeightedIndex != -1)
				{
					for (int i = 0; i < this.trackingIntsMissed.Length; i++)
					{
						if (this.trackingIntsMissed[i].Value >= this.missedMax[i].Value)
						{
							flag2 = true;
							num = i;
						}
					}
					if (flag2)
					{
						flag = true;
						for (int j = 0; j < this.trackingInts.Length; j++)
						{
							this.trackingInts[j].Value = 0;
							this.trackingIntsMissed[j].Value++;
						}
						this.trackingIntsMissed[num].Value = 0;
						this.trackingInts[num].Value = 1;
						base.Fsm.Event(this.events[num]);
					}
					else if (this.trackingInts[randomWeightedIndex].Value < this.eventMax[randomWeightedIndex].Value)
					{
						int value = ++this.trackingInts[randomWeightedIndex].Value;
						for (int k = 0; k < this.trackingInts.Length; k++)
						{
							this.trackingInts[k].Value = 0;
							this.trackingIntsMissed[k].Value++;
						}
						this.trackingInts[randomWeightedIndex].Value = value;
						this.trackingIntsMissed[randomWeightedIndex].Value = 0;
						flag = true;
						base.Fsm.Event(this.events[randomWeightedIndex]);
					}
				}
				this.loops++;
				if (this.loops > 100)
				{
					base.Fsm.Event(this.events[0]);
					flag = true;
					base.Finish();
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

		[UIHint(UIHint.Variable)]
		public FsmInt[] trackingIntsMissed;

		public FsmInt[] missedMax;

		private int loops;

		private DelayedEvent delayedEvent;
	}
}
