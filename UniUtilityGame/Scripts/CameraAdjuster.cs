using UnityEngine;

namespace Game
{
    public class CameraAdjuster : MonoBehaviour
    {
        [SerializeField]
        private Vector2 aspectVec = new Vector2(16, 9); //目的解像度

        [SerializeField]
        Camera _camera = null;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SetCameraAdjest();
        }


        public void SetCameraAdjest()
        {
            if (_camera == null)
            {
                _camera = gameObject.GetComponent<Camera>();
            }

            float target_aspect = aspectVec.x / aspectVec.y;
            float window_aspect = (float)Screen.width / (float)Screen.height;
            float scale_height = window_aspect / target_aspect;
            Rect rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            if (1.0f > scale_height)
            {
                rect.x = 0;
                rect.y = (1.0f - scale_height) / 2.0f;
                rect.width = 1.0f;
                rect.height = scale_height;
            }
            else
            {
                float scale_width = 1.0f / scale_height;
                rect.x = (1.0f - scale_width) / 2.0f;
                rect.y = 0.0f;
                rect.width = scale_width;
                rect.height = 1.0f;
            }
            _camera.rect = rect;
        }
    }
}
