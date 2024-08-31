using UnityEngine;
using UnityEngine.Assertions;

namespace Game
{
    public static class SingletonAttacher<T>
    where T : Object
    {
        static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);
                    Assert.IsNotNull(_instance, $"Singleton取得エラーです。{typeof(T).Name}が存在していません。");
                }
                return _instance;
            }
        }
    }
}