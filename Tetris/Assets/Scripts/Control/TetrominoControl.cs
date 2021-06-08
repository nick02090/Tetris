using UnityEngine;

namespace Tetris.Control
{
    [RequireComponent(typeof(AudioSource))]
    public class TetrominoControl : MonoBehaviour
    {
        public AudioClip rotateAudio;
        public AudioClip moveAudio;

        public delegate void MoveHorizontal(float movement);
        public MoveHorizontal moveDelegate;

        public delegate void Rotate(bool leftSide);
        public Rotate rotateDelegate;

        public delegate void ChangeFalltime(float multiplier);
        public ChangeFalltime falltimeDelegate;

        public void MoveTetrominoLeft()
        {
            moveDelegate(-1.0f);
            GetComponent<AudioSource>().PlayOneShot(moveAudio);
        }

        public void MoveTetrominoRight()
        {
            moveDelegate(1.0f);
            GetComponent<AudioSource>().PlayOneShot(moveAudio);
        }

        public void RotateTetrominoLeft()
        {
            rotateDelegate(true);
            GetComponent<AudioSource>().PlayOneShot(rotateAudio);
        }

        public void RotateTetrominoRight()
        {
            rotateDelegate(false);
            GetComponent<AudioSource>().PlayOneShot(rotateAudio);
        }

        public void MinimizeFallTime()
        {
            falltimeDelegate(0.0f);
        }
    }
}
