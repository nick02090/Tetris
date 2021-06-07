using UnityEngine;

namespace Tetris.Control
{
    public class TetrominoControl : MonoBehaviour
    {
        public delegate void MoveHorizontal(float movement);
        public MoveHorizontal moveDelegate;

        public delegate void Rotate(bool leftSide);
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

        public void RotateTetrominoLeft()
        {
            rotateDelegate(true);
        }

        public void RotateTetrominoRight()
        {
            rotateDelegate(false);
        }

        public void DecreaseFallTime()
        {
            falltimeDelegate(0.1f);
        }

        public void IncreaseFallTime()
        {
            falltimeDelegate(1.1f);
        }

        public void MinimizeFallTime()
        {
            falltimeDelegate(0.0f);
        }
    }
}
