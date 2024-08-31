using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

namespace Game
{
    [AddComponentMenu("Input/On-Screen Stick Custom")]
    public class OnScreenStickCustom : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }


        [SerializeField] private Image bgImage;
        [SerializeField] private Image stickImage;
        [SerializeField] private float range = 10f;
        [SerializeField, Range(0f, 1f)] private float deadZone = 0.1f;

        private Transform bgImageTransform, stickImageTransform;

        Vector2 DownPosition = Vector2.zero;
        static public Vector2 Direction = Vector2.zero;


        private void Start()
        {
            bgImageTransform = bgImage.transform;
            stickImageTransform = stickImage.transform;
            bgImage.enabled = false;
            stickImage.enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            bgImageTransform.position = eventData.position;
            stickImageTransform.position = eventData.position;
            bgImage.enabled = true;
            stickImage.enabled = true;
            DownPosition = eventData.position;
            //Debug.Log("クリックされたで");
        }
        public void OnDrag(PointerEventData eventData)
        {
            var pos = (Vector2)Calc(eventData.position);
            SendValueToControl(pos);
            Direction = eventData.position - DownPosition;
            Direction.Normalize();
            //Debug.Log("ドラッグされたで" + pos.ToString());
        }
        public void OnPointerUp(PointerEventData data)
        {
            SendValueToControl(Vector2.zero);
            bgImage.enabled = false;
            stickImage.enabled = false;
            DownPosition = Vector2.zero;
            Direction = Vector2.zero;
            //Debug.Log("離れたされたで");
        }

        private Vector3 Calc(Vector2 pos)
        {
            var currentRange = range * bgImageTransform.lossyScale.x;
            var vector = (Vector3)pos - bgImageTransform.position;
            var magnitude = vector.magnitude;

            if (magnitude < currentRange * deadZone) vector = Vector3.zero;
            else if (magnitude > currentRange) vector *= currentRange / magnitude;

            stickImageTransform.position = bgImage.transform.position + vector;

            vector /= currentRange;
            return vector;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Direction = Vector2.zero;
        }
    }
}
