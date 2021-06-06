using UnityEngine;

namespace Tetris.Control
{
    public class TetrominoControl : MonoBehaviour
    {
        public delegate void MoveHorizontal(float movement);
        public MoveHorizontal moveDelegate;

        public delegate void Rotate();
        public Rotate rotateDelegate;

        public delegate void ChangeFalltime(float multiplier);
        public ChangeFalltime falltimeDelegate;

        public void MoveTetrominoLeft()
        {
            moveDelegate(-1.0f);
        }

        public void MoveTetrominoRight()
        {
            moveDelegate(1.0f);
        }

        public void RotateTetromino()
        {
            rotateDelegate();
        }

        public void IncreaseFallTime()
        {
            falltimeDelegate(0.1f);
        }

        public void DecreaseFallTime()
        {
            falltimeDelegate(1.1f);
        }

        public void MaximizeFallTime()
        {
            falltimeDelegate(0.0f);
        }
    }
}
