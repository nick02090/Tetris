using UnityEngine;
using Tetris.Control;

namespace Tetris.Gameplay
{
    [RequireComponent(typeof(TouchControl))]
    public class Tetromino : MonoBehaviour
    {
        // TODO: Move this to game settings/manager
        public static float fallTime = 1.0f;
        public bool gameSettingsLeft = false;

        // TODO: Move this to tetris grid
        public const int gridWidth = 10;
        public const int gridHeight = 21;

        // Squares that this tetromino consists of
        public Transform[] squares;
        // Speed at which tetromino is dragged on horizontal axis
        public float horizontalMovementSpeed;
        // Local point around which tetromino is rotated
        public Vector3 rotationPoint;

        // Touch control system that manouvers tetromino
        private TouchControl touchControl;
        // Last time at which the tetromino has fell down one step
        private float previousFallTime;

        private void Start()
        {
            touchControl = GetComponent<TouchControl>();
        }

        private void Update()
        {
            if (touchControl.HasActiveTouch)
            {
                // Move tetromino in the correct direction
                if (touchControl.Phase == TouchPhase.Moved)
                {
                    // Move tetromino left/right
                    if (Mathf.Abs(touchControl.DeltaPosition.x) > Mathf.Abs(touchControl.DeltaPosition.y))
                    {
                        float horizontalMovement = Mathf.RoundToInt(touchControl.DeltaPosition.x * horizontalMovementSpeed * Time.deltaTime);
                        Move(horizontalMovement * Vector3.right);
                    }
                    // Slow/Accelerate tetromino down movement
                    else if (Mathf.Abs(touchControl.DeltaPosition.x) < Mathf.Abs(touchControl.DeltaPosition.y))
                    {
                        if (touchControl.DeltaPosition.y < 0.0f)
                        {
                            fallTime -= 0.01f * Mathf.Abs(touchControl.DeltaPosition.y);
                        }
                        else
                        {
                            fallTime += 0.01f * Mathf.Abs(touchControl.DeltaPosition.y);
                            fallTime = Mathf.Clamp01(fallTime);
                        }
                    }
                }
                // Rotate tetromino if it was a tap (short stationary touch) 
                else if (touchControl.Phase == TouchPhase.Ended && !touchControl.IsLongStationary)
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, gameSettingsLeft ? 90.0f : -90.0f);
                }
            }

            // Apply "gravity"
            if (Time.time - previousFallTime > fallTime)
            {
                // Move tetromino one step down
                Move(Vector3.down);

                // Update previous fall time
                previousFallTime = Time.time;

                // Disable this tetromino if it's not active anymore
                if (!IsActive())
                {
                    enabled = false;
                    Reset();
                }
            }
        }

        /// <summary>
        /// This function is called upon current tetromino becoming inactive
        /// </summary>
        private void Reset()
        {
            fallTime = 1.0f;
        }

        /// <summary>
        /// Checks if the tetromino can be translated to given translastion and then translates it if true.
        /// </summary>
        /// <param name="translation"></param>
        private void Move(Vector3 translation)
        {
            // If every square can be moved to this location then simply move the whole tetromino
            if (CheckMove(translation))
                transform.position += translation;
        }

        /// <summary>
        /// Checks if the tetromino can move it's position by the given translation.
        /// </summary>
        /// <param name="translation"></param>
        /// <returns></returns>
        private bool CheckMove(Vector3 translation)
        {
            // Check if the translation is valid by checking each square (block) of this tetromino 
            foreach (Transform square in squares)
            {
                int x = Mathf.RoundToInt((square.transform.position + translation).x);
                int y = Mathf.RoundToInt((square.transform.position + translation).y);
                if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tetromino is no longer active when it no longer can move down.
        /// Reasons for that are:
        ///     1) tetromino has reached the bottom of the grid
        ///     2) tetromino has interacted with its bottom parts with another inactive tetromino
        /// </summary>
        /// <returns></returns>
        private bool IsActive()
        {
            // Check if tetromino can fall down for one step
            return CheckMove(Vector3.down);
        }
    }
}
