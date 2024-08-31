using System.Collections.Generic;
using System.Linq;

namespace Game
{
    /// <summary>
    /// List 型の拡張メソッドを管理するクラス
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// サイズを設定します
        /// </summary>
        public static void SetSize<T>(this List<T> self, int size)
        {
            if (self.Count <= size) return;
            self.RemoveRange(size, self.Count - size);
        }

        /// <summary>
        /// ランダム取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T RandamAt<T>(this List<T> self)
        {
            return self.Any() ? self.ElementAt(UnityEngine.Random.Range(0, self.Count)) : default;
        }

        public static T RandamAtAndRemove<T>(this List<T> self)
        {
            var val = self.Any() ? self.ElementAt(UnityEngine.Random.Range(0, self.Count)) : default;
            self.Remove(val);
            return val;
        }

        public static bool IsEmpty<T>(this List<T> self)
        {
            return (self.Count == 0);
        }

        public static bool IsHas<T>(this List<T> self)
        {
            return (self.Count > 0);
        }
    }
}
