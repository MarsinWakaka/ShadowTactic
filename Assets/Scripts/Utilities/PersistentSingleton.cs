using UnityEngine;

namespace Utilities
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        // public static T Instance { get; protected set; }
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
                return _instance;
            }
        }
        
        public static bool IsInstanceNull()
        {
            return _instance == null;
        }

        protected virtual void Awake()
        {
            if(_instance == null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}