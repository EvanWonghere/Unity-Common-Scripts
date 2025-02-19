using UnityEngine;

namespace Utilities.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// 单例模式，适用于 MonoBehavior 对象
        /// </summary>
        private static T _instance;
        private static bool _isApplicationQuitting;

        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting)
                    return null;

                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<T>();
                if (_instance != null) return _instance;
                
                var singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
                DontDestroyOnLoad(singletonObject);
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        protected Singleton() { }
    }
}