using System;
using System.Threading;
using UnityEngine;

namespace UnityThreading.Events
{
    public sealed partial class UnityEvents : MonoBehaviour
    {
        private static UnityEvents _instance;
        public static UnityEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log(Thread.CurrentThread.Name);
                    _instance = new GameObject("UnityEvents").AddComponent<UnityEvents>();
                }
                return _instance;
            }
        }

        public event Action AplicationQuit;

        private void Awake()
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = UnityThread.MAIN_THREAD_NAME;
            }
            DontDestroyOnLoad(this);
        }

        private void OnApplicationQuit()
        {
            if (AplicationQuit != null)
            {
                AplicationQuit.Invoke();
            }
            AplicationQuit = null;
        }
    }
}

