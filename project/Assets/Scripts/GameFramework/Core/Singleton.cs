using UnityEngine;


namespace GameFramework.Core{
    public class Singleton<T> : MonoBehaviour where T:Component
    {
        private static T _instance;
        public static T instance{
            get{
                if(_instance == null){
                    T[] objs = FindObjectsOfType<T>();
                    if(objs.Length > 0){
                        _instance = objs[0];   
                    }else{
                        GameObject go = new GameObject();
                        go.name = typeof(T).Name;
                        _instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }
    }
}
