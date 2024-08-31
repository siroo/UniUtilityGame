using UnityEngine;

namespace Game
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        private Transform mainCameraTransform;
        [SerializeField]
        bool LockY = true;

        private void Start()
        {
            // カメラを取得
            mainCameraTransform = Camera.main.transform;
        }

        /*
        private void Update()
        {
            // カメラ方向を計算
            Vector3 lookDir = mainCameraTransform.position - transform.position;
            if (LockY)
            {
                lookDir.y = 0f; // Y軸回転のみなので、Y成分を0に設定する
            }

            // オブジェクトのY軸のみカメラの方向に向ける
            if (lookDir != Vector3.zero)
            {
                transform.forward = lookDir.normalized;
            }
        }
        */

        void Update()
        {
            Vector3 p = mainCameraTransform.position;
            if (LockY)
            {
                p.y = transform.position.y;
            }
            transform.LookAt(p);
            transform.Rotate(0, 180, 0);
        }
    }
}