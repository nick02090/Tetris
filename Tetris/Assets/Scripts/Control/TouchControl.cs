using UnityEngine;

namespace Tetris.Control
{
    public class TouchControl : MonoBehaviour
    {
        public TouchPhase Phase { get; private set; }
        public Vector2 DeltaPosition => HasActiveTouch ? Input.GetTouch(0).deltaPosition : Vector2.zero;
        public bool HasActiveTouch => Input.touchCount > 0;
        public bool IsLongTouch => HasActiveTouch && Time.time - touchStartTime > 0.2f;

        private float touchStartTime;

        private void Update()
        {
            // Input has recognized a touch
            if (HasActiveTouch)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchStartTime = Time.time;
                }
                Phase = touch.phase;
            }
        }
    }
}
