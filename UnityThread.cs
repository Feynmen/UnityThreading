using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityThreading.Events;

namespace UnityThreading
{
    public class UnityThread
    {
        /// <summary>
        /// Main thread name
        /// </summary>
        public const string MAIN_THREAD_NAME = "MainUnityThread";

        private const string THREAD_PREFIX = "UnityThread#";
        private static int _threadId;

        private readonly object _mutex = new object();
        private readonly Thread _thread;
        private Action _action;

        /// <summary>
        /// Thread completed event
        /// </summary>
        public event Action Completed;

        /// <summary>
        /// Thread aborted event
        /// </summary>
        public event Action Aborted;

        private bool _isAlive;

        /// <summary>
        /// Check is alive thread
        /// </summary>
        public bool IsAlive
        {
            private set
            {
                lock (_mutex)
                {
                    _isAlive = value;
                }
            }
            get
            {
                lock (_mutex)
                {
                    return _isAlive;
                }
            }
        }

        /// <summary>
        /// Create thread
        /// </summary>
        public UnityThread(Action action)
        {
            UnityEvents.Instance.AplicationQuit += Clear;
            _thread = new Thread(ThreadFunc)
            {
                Name = THREAD_PREFIX + _threadId++,
                IsBackground = true
            };
            lock (_mutex)
            {
                _action = action;
            }
        }

        public void Start()
        {
            IsAlive = true;
            UnityEvents.Instance.StartCoroutine(ThreadState());
            _thread.Start();
        }

        public void Abort()
        {
            UnityEvents.Instance.StopCoroutine(ThreadState());
            if (Aborted != null)
            {
                Aborted.Invoke();
            }
            Clear();
        }

        private IEnumerator ThreadState()
        {
            while (IsAlive)
            {
                yield return null;
            }
            if (Completed != null)
            {
                Completed.Invoke();
            }
            Clear();
        }

        private void ThreadFunc()
        {
            _action.Invoke();
            IsAlive = false;
        }

        private void Clear()
        {
            UnityEvents.Instance.AplicationQuit -= Clear;
            Aborted = null;
            Completed = null;
            if (_thread.IsAlive)
            {
                _thread.Abort();
                _thread.Join();
            }
            Debug.Log("Clear ThreadId " + _threadId);
        }
    }
}