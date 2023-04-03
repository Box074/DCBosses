using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DCSpriteClipCollection : ScriptableObject
{
    [Serializable]
    public class Clip
    {
        public string m_name;
        public List<DCSpriteClipAction> m_actions;
    }
    public DCAtlasInstance m_atlas;
    public List<Clip> m_clips = new List<Clip>();
}

[Serializable]
public class DCSpriteClipAction
{
    [Serializable]
    public enum ActionType
    {
        PlayClip,
        PlayClipInRange,
        SendFSMEvent,
        SendMessage,
        JumpToClip
    }

    public ActionType m_actionType;
    public string m_clipName;
    public bool m_loop;
    public float m_time;
    public int m_startFrame;
    public int m_stopFrame;
    public string m_eventName;

    public void DoAction(DCSpriteClipCollection collection, DCSpriteClipCollection.Clip clip,
                    int actionIndex, DCSpriteAnimator animator,
                    DCSpriteRenderer renderer, DCSpriteAnimatorHost host)
    {
        DCSpriteClipAction nextAction;
        if (actionIndex >= clip.m_actions.Count - 1)
        {
            nextAction = null;
        }
        else
        {
            nextAction = clip.m_actions[actionIndex + 1];
        }
        
        IEnumerator WaitAction(object ie, Action action)
        {
            yield return ie;
            action();
        }

        void InvokeNext()
        {
            if (nextAction != null && host.CurrentClipName == clip.m_name)
            {
                nextAction.DoAction(collection, clip, actionIndex + 1, animator, renderer, host);
            }
        }

        if(m_actionType == ActionType.PlayClip)
        {
            
            animator.Play(m_clipName, m_loop, m_startFrame);
            
            if(m_loop && m_time > 0)
            {
                animator.StartCoroutine(WaitAction(new WaitForSeconds(m_time), InvokeNext));
            }
            animator.StartCoroutine(WaitAction(animator.WaitForClip(m_clipName), InvokeNext));
        }
        else if(m_actionType == ActionType.PlayClipInRange)
        {
            animator.Play(m_clipName, m_loop, m_startFrame);
            animator.StartCoroutine(WaitAction(animator.WaitForFrame(m_stopFrame, m_clipName), InvokeNext));
        }
        else if(m_actionType == ActionType.SendMessage)
        {
            animator.gameObject.SendMessage(m_eventName, m_clipName);
            InvokeNext();
        }
        else if(m_actionType == ActionType.SendFSMEvent)
        {
            FSMUtility.SendEventToGameObject(animator.gameObject, m_eventName);
            InvokeNext();
        }
        else if(m_actionType == ActionType.JumpToClip)
        {
            host.curClipName = m_clipName;
            if(m_clipName == clip.m_name)
            {
                nextAction = clip.m_actions[0];
                InvokeNext();
            }
        }
    }
}
