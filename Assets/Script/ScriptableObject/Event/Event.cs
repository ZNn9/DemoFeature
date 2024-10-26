using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;


namespace Systems.Scriptable.Event
{
    [CreateAssetMenu(menuName = "Events/Event")]
    public class Event<T> : ScriptableObject
    {
        private readonly List<UnityAction> unityActionListeners = new List<UnityAction>();
        private readonly List<Action<List<T>>> actionListeners = new List<Action<List<T>>>();

        public void RaiseUnityAction()
        {
            for (int i = unityActionListeners.Count - 1; i >= 0; i--)
            {
                unityActionListeners[i]?.Invoke();
            }
        }

        public void RaiseAction(List<T> parameters)
        {
            for (int i = actionListeners.Count - 1; i >= 0; i--)
            {
                actionListeners[i]?.Invoke(parameters);
            }
        }

        public void RegisterListener(UnityAction listener)
        {
            if (!unityActionListeners.Contains(listener))
            {
                unityActionListeners.Add(listener);
            }
        }

        public void UnregisterListener(UnityAction listener)
        {
            if (unityActionListeners.Contains(listener))
            {
                unityActionListeners.Remove(listener);
            }
        }
        // Đăng ký Action listener
        public void RegisterActionListener(Action<List<T>> listener)
        {
            actionListeners.Add(listener);
        }

        // Hủy đăng ký Action listener
        public void UnregisterActionListener(Action<List<T>> listener)
        {
            actionListeners.Remove(listener);
        }

    }
}
