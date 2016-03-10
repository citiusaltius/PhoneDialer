using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Utilities
{
    public class MainThreadDispatcher : MonoBehaviour
    {

        static Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();
        static object _lock = new object();

        public void Update()
        {
            while (_coroutineQueue.Count > 0)
            {
                StartCoroutine(_coroutineQueue.Dequeue());
            }
        }

        public static void ExecuteCoroutineOnMainThread(IEnumerator coroutine)
        {
            lock (_lock)
            {
                _coroutineQueue.Enqueue(coroutine);
            }
        }

        // Use this for initialization
        void Start()
        {

        }
    }
}