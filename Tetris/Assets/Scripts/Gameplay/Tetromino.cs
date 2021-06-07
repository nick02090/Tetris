using UnityEngine;
using Tetris.Control;

namespace Tetris.Gameplay
{
    public class Tetromino : MonoBehaviour
    {
        // Time that is needed for tetromino to fall down one step
        public static float fallTime = 1.0f;

        public delegate void OnDeath();
        public OnDeath onDeathDelegate;

        // Grid where all the tetrominos are placed
        public TetrisGrid tetrisGrid;
        // Control system that manouvers tetromino
        public TetrominoControl control;
        // Corresponding sprite for this tetromino
        public Sprite sprite;

        // Squares that this tetromino consists of
        public Transform[] squares;

        // Last time at which the tetromino has fell down one step
        private float previousFallTime;

        private void Start()
        {
            // Subscribe to controls
            control.moveDelegate += MoveHorizontal;
            control.falltimeDelegate += ChangeFalltime;
            control.rotateDelegate += Rotate;
        }

        private void Update()
        {
            // Apply "gravity"
            if (Time.time - previousFallTime > fallTime)
            {
                // Move tetromino one step down
                Move(Vector3.down);

                // Disable this tetromino if it's not active anymore
                if (!IsActive())
                {
                    // Give player one more fallTime chance before total disabling
                    if (Time.time - previousFallTime < fallTime * 2.0f)
                    {
                        return;
                    }
                    else
                    {
                        Disable();
                    }
                }

                // Update previous fall time
                previousFallTime = Time.time;
            }
        }

        private void UnsubscribeFromControls()
        {
            // Unsubscribe from the controls
            control.moveDelegate -= MoveHorizontal;
            control.falltimeDelegate -= ChangeFalltime;
            control.rotateDelegate -= Rotate;
        }

        private void OnDestroy()
        {
            UnsubscribeFromControls();
        }

        private void Disable()
        {
            enabled = false;
            fallTime = 1.0f;
            UnsubscribeFromControls();
            tetrisGrid.AddToGrid(this);
            onDeathDelegate();
        }

        public void Rotate(bool rotateLeft)
        {
            // Rotate tetromino
            transform.Rotate(Vector3.forward, rotateLeft ? 90.0f : -90.0f);
            // Try to fix rotation if it's illegal
            if (!CheckMove(Vector3.zero))
            {
                // Try fix by moving left
                if (CheckMove(Vector3.left))
                {
                    Move(Vector3.left);
                    return;
                } 
                else if (CheckMove(Vector3.left * 2.0f))
                {
                    Move(Vector3.left * 2.0f);
                    return;
                }
                // Try fix by moving right
                if (CheckMove(Vector3.right))
                {
                    Move(Vector3.right);
                    return;
                }
                else if (CheckMove(Vector3.right * 2.0f))
                {
                    Move(Vector3.right * 2.0f);
                    return;
                }
                // Undo rotation if nothing works
                transform.Rotate(Vector3.forward, rotateLeft ? -90.0f : 90.0f);
            }
        }

        public void MoveHorizontal(float move)
        {
            Move(move * Vector3.right);
        }

        public void ChangeFalltime(float multiplier)
        {
            fallTime = multiplier * fallTime;
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
                // Check if tetromino is inside the grid
                if (x < 0 || x >= TetrisGrid.gridWidth || y < 0 || y >= TetrisGrid.gridHeight)
                {
                    return false;
                }
                // Check if tetromino collides with another tetromino in the grid
                if (tetrisGrid.BlockTaken(y, x))
                    return false;
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
