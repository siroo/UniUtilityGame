using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Game
{
    // ゲーム共通の便利クラス
    public static class Utils
    {
        /// <summary>
        /// 秒数を0：00の文字列に変換
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static string GetTextTimer(float timer)
        {
            int seconds = (int)timer % 60;
            int minutes = (int)timer / 60;
            return String.Format("{0}:{1:00}", minutes, seconds);
        }

        /// <summary>
        /// あたり判定のあるタイルかどうか調べる
        /// </summary>
        /// <param name="tilemapCollider"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static bool IsColliderTile(Tilemap tilemapCollider, Vector2 position)
        {
            // セル位置に変換
            Vector3Int cellPosition = tilemapCollider.WorldToCell(position);

            if (tilemapCollider.GetTile(cellPosition))
                return true;
            return false;
        }

        public static bool IsColliderTileList(List<Tilemap> tilemapColliderList, Vector2 position)
        {
            foreach (var tilemap in tilemapColliderList)
            {
                if (IsColliderTile(tilemap, position)) return true;
            }
            return false;
        }

        /// <summary>
        /// アルファ値設定
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="alpha"></param>
        public static void SetAlpha(Graphic graphic, float alpha)
        {
            //
            Color color = graphic.color;
            //
            color.a = alpha;
            graphic.color = color;

        }

        /// <summary>
        /// アルファ値設定（ボタン）
        /// </summary>
        /// <param name="button"></param>
        /// <param name="alpha"></param>
        public static void SetAlpha(Button button, float alpha)
        {
            //ボタン
            SetAlpha(button.image, alpha);
            //子オブジェクトすべて
            foreach (var item in button.GetComponentsInChildren<Graphic>())
            {
                SetAlpha(item, alpha);
            }
        }

        public static float CalcFPSTime(int FPS)
        {
            return 1f / FPS;
        }

        public static bool FloatEquals(float a, float b, float epsilon = 0.000001f)
        {
            return Mathf.Abs(a - b) <= epsilon;
        }

        public static bool IsZero(float a, float epsilon = 0.0000001f)
        {
            return FloatEquals(a, 0.0f, epsilon);
        }

        public static bool IsZero(Vector3 v, float epsilon = 0.000001f)
        {
            return (IsZero(v.x, epsilon) && IsZero(v.y, epsilon) && IsZero(v.z, epsilon));
        }

        public static IEnumerator StartDelayCoroutine(float seconds, UnityAction onAct)
        {
            yield return new WaitForSeconds(seconds);
            onAct?.Invoke();
        }
    }
}
