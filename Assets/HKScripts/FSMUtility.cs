using System;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

public static class FSMUtility
{
	private static List<PlayMakerFSM> ObtainFsmList()
	{
		if (FSMUtility.fsmListPool.Count > 0)
		{
			List<PlayMakerFSM> result = FSMUtility.fsmListPool[FSMUtility.fsmListPool.Count - 1];
			FSMUtility.fsmListPool.RemoveAt(FSMUtility.fsmListPool.Count - 1);
			return result;
		}
		return new List<PlayMakerFSM>();
	}

	private static void ReleaseFsmList(List<PlayMakerFSM> fsmList)
	{
		fsmList.Clear();
		if (FSMUtility.fsmListPool.Count < 20)
		{
			FSMUtility.fsmListPool.Add(fsmList);
		}
	}

	public static bool ContainsFSM(GameObject go, string fsmName)
	{
		if (go == null)
		{
			return false;
		}
		List<PlayMakerFSM> list = FSMUtility.ObtainFsmList();
		go.GetComponents<PlayMakerFSM>(list);
		bool result = false;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].FsmName == fsmName)
			{
				result = true;
				break;
			}
		}
		FSMUtility.ReleaseFsmList(list);
		return result;
	}

	public static PlayMakerFSM LocateFSM(GameObject go, string fsmName)
	{
		if (go == null)
		{
			return null;
		}
		List<PlayMakerFSM> list = FSMUtility.ObtainFsmList();
		go.GetComponents<PlayMakerFSM>(list);
		PlayMakerFSM result = null;
		for (int i = 0; i < list.Count; i++)
		{
			PlayMakerFSM playMakerFSM = list[i];
			if (playMakerFSM.FsmName == fsmName)
			{
				result = playMakerFSM;
				break;
			}
		}
		FSMUtility.ReleaseFsmList(list);
		return result;
	}

	public static PlayMakerFSM LocateMyFSM(this GameObject go, string fsmName)
	{
		return FSMUtility.LocateFSM(go, fsmName);
	}

	public static PlayMakerFSM GetFSM(GameObject go)
	{
		return go.GetComponent<PlayMakerFSM>();
	}

	public static void SendEventToGameObject(GameObject go, string eventName, bool isRecursive = false)
	{
		if (go != null)
		{
			FSMUtility.SendEventToGameObject(go, FsmEvent.FindEvent(eventName), isRecursive);
		}
	}

	public static void SendEventToGameObject(GameObject go, FsmEvent ev, bool isRecursive = false)
	{
		if (go != null)
		{
			List<PlayMakerFSM> list = FSMUtility.ObtainFsmList();
			go.GetComponents<PlayMakerFSM>(list);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Fsm.Event(ev);
			}
			FSMUtility.ReleaseFsmList(list);
			if (isRecursive)
			{
				Transform transform = go.transform;
				for (int j = 0; j < transform.childCount; j++)
				{
					FSMUtility.SendEventToGameObject(transform.GetChild(j).gameObject, ev, isRecursive);
				}
			}
		}
	}

	public static GameObject GetSafe(this FsmOwnerDefault ownerDefault, FsmStateAction stateAction)
	{
		if (ownerDefault.OwnerOption == OwnerDefaultOption.UseOwner)
		{
			return stateAction.Owner;
		}
		return ownerDefault.GameObject.Value;
	}

	public static bool GetBool(PlayMakerFSM fsm, string variableName)
	{
		return fsm.FsmVariables.FindFsmBool(variableName).Value;
	}

	public static int GetInt(PlayMakerFSM fsm, string variableName)
	{
		return fsm.FsmVariables.FindFsmInt(variableName).Value;
	}

	public static float GetFloat(PlayMakerFSM fsm, string variableName)
	{
		return fsm.FsmVariables.FindFsmFloat(variableName).Value;
	}

	public static string GetString(PlayMakerFSM fsm, string variableName)
	{
		return fsm.FsmVariables.FindFsmString(variableName).Value;
	}

	public static Vector3 GetVector3(PlayMakerFSM fsm, string variableName)
	{
		return fsm.FsmVariables.FindFsmVector3(variableName).Value;
	}

	public static void SetBool(PlayMakerFSM fsm, string variableName, bool value)
	{
		fsm.FsmVariables.GetFsmBool(variableName).Value = value;
	}

	public static void SetInt(PlayMakerFSM fsm, string variableName, int value)
	{
		fsm.FsmVariables.GetFsmInt(variableName).Value = value;
	}

	public static void SetFloat(PlayMakerFSM fsm, string variableName, float value)
	{
		fsm.FsmVariables.GetFsmFloat(variableName).Value = value;
	}

	public static void SetString(PlayMakerFSM fsm, string variableName, string value)
	{
		fsm.FsmVariables.GetFsmString(variableName).Value = value;
	}

	public static PlayMakerFSM FindFSMWithPersistentBool(PlayMakerFSM[] fsmArray)
	{
		for (int i = 0; i < fsmArray.Length; i++)
		{
			if (fsmArray[i].FsmVariables.FindFsmBool("Activated") != null)
			{
				return fsmArray[i];
			}
		}
		return null;
	}

	public static PlayMakerFSM FindFSMWithPersistentInt(PlayMakerFSM[] fsmArray)
	{
		for (int i = 0; i < fsmArray.Length; i++)
		{
			if (fsmArray[i].FsmVariables.FindFsmInt("Value") != null)
			{
				return fsmArray[i];
			}
		}
		return null;
	}

	// Note: this type is marked as 'beforefieldinit'.
	static FSMUtility()
	{
		FSMUtility.fsmListPool = new List<List<PlayMakerFSM>>();
	}

	private const int FsmListPoolSizeMax = 20;

	private static List<List<PlayMakerFSM>> fsmListPool;

	public abstract class CheckFsmStateAction : FsmStateAction
	{
		public abstract bool IsTrue { get; }

		public override void Reset()
		{
			this.trueEvent = null;
			this.falseEvent = null;
		}

		public override void OnEnter()
		{
			if (this.IsTrue)
			{
				base.Fsm.Event(this.trueEvent);
			}
			else
			{
				base.Fsm.Event(this.falseEvent);
			}
			base.Finish();
		}

		protected CheckFsmStateAction()
		{
		}

		public FsmEvent trueEvent;

		public FsmEvent falseEvent;
	}

	public abstract class GetIntFsmStateAction : FsmStateAction
	{
		public abstract int IntValue { get; }

		public override void Reset()
		{
			this.storeValue = null;
		}

		public override void OnEnter()
		{
			if (!this.storeValue.IsNone)
			{
				this.storeValue.Value = this.IntValue;
			}
			base.Finish();
		}

		protected GetIntFsmStateAction()
		{
		}

		[UIHint(UIHint.Variable)]
		public FsmInt storeValue;
	}

	public abstract class SetBoolFsmStateAction : FsmStateAction
	{
		public abstract bool BoolValue { set; }

		public override void Reset()
		{
			this.setValue = null;
		}

		public override void OnEnter()
		{
			if (!this.setValue.IsNone)
			{
				this.BoolValue = this.setValue.Value;
			}
			base.Finish();
		}

		protected SetBoolFsmStateAction()
		{
		}

		public FsmBool setValue;
	}
}
