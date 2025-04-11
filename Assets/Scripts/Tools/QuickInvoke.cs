using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

namespace Hypernex.Tools
{
    [RequireComponent(typeof(DontDestroyMe))]
    public class QuickInvoke : MonoBehaviour
    {
        private static UnityMainThreadDispatcher threadDispatcher;
        
        public static void InvokeActionOnMainThread(Delegate action, params object[] args) => queue.Enqueue((action, args));
        
        public static void InvokeActionOnMainThreadObject(Delegate action, object[] args) => queue.Enqueue((action, args));

        public static void InvokeImmediate(Action a)
        {
            if(threadDispatcher == null) threadDispatcher = UnityMainThreadDispatcher.Instance();
            threadDispatcher.Enqueue(a);
        }
        
        public static void OverwriteListener(UnityEvent u, Action newAction)
        {
            u.RemoveAllListeners();
            u.AddListener(newAction.Invoke);
        }
        
        public static void OverwriteListener<T0>(UnityEvent<T0> u, Action<T0> newAction)
        {
            u.RemoveAllListeners();
            u.AddListener(newAction.Invoke);
        }

        public static QuickInvoke Instance { get; private set; }
        public static ConcurrentQueue<(Delegate, object[])> queue = new ConcurrentQueue<(Delegate, object[])>();
        private Stopwatch sw = new Stopwatch();
        public int maxInvokes = 1000;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            int i = 0;
            sw.Restart();
            while (sw.ElapsedTicks < Stopwatch.Frequency / 2000 && i < maxInvokes && queue.TryDequeue(out (Delegate action, object[] args) item))
            {
                item.action.DynamicInvoke(item.args);
                i++;
            }
        }
    }
}
