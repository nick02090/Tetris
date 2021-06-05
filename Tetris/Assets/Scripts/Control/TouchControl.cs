using UnityEngine;

namespace Tetris.Control
{
    public class TouchControl : MonoBehaviour
    {
        public float stationaryTimeTreshold;
        public bool IsLongStationary { get; private set; }
        public TouchPhase Phase { get; private set; }
        public Vector2 DeltaPosition => HasActiveTouch ? Input.GetTouch(0).deltaPosition : Vector2.zero;
        public bool HasActiveTouch => Input.touchCount > 0;

        private float touchStartTime;

        private void Start()
        {
            IsLongStationary = false;
        }

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
                else if (touch.phase == TouchPhase.Stationary)
                {
                    // Set long stationary flag if touch is stationary for some time
                    if (Time.time - touchStartTime > stationaryTimeTreshold)
                    {
                        IsLongStationary = true;
                    }
                } 
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Set long stationary flag if touch has moved
                    IsLongStationary = true;
                }
                Phase = touch.phase;
            } 
            else
            {
                // Reset long stationary
                IsLongStationary = false;
            }
        }
    }
}
