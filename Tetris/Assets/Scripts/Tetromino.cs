using UnityEngine;
using UnityEngine.UI;

public enum TetrominoState
{
    Idle = 0,   //  State when tetromino is falling down without any additional input
    Controlled = 1, // State when tetromino has started to being controlled (it can be rotated and moved from this state)
    Moving = 2, //  State when tetromino can be moved in any direction (up/down/left/right)
    HorizontalMove = 3, // State when tetromino can be moved only in horizontal direction (left/right)
    VerticalMove = 4    // State when tetromino can be moved only in vertical direction (up/down)
}

public class Tetromino : MonoBehaviour
{
    public static float fallTime = 1.0f;
    public float horizontalMovementSpeed = 10.0f;
    public Vector3 rotationPoint;
    public Text info;
    public Transform[] squares;

    public bool gameSettingsLeft = false;

    public int gridWidth = 10;
    public int gridHeight = 20;

    private TetrominoState state = TetrominoState.Idle;
    private float previousFallTime;
    private float touchStartTime;
    private Vector2 touchInitialPosition;
    private Vector2 touchPreviousPosition;

    private void Update()
    {
        // Input has recognized a touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                state = TetrominoState.Controlled;
                touchStartTime = Time.time;
                info.text = "Touch started";
                touchInitialPosition = touch.position;
                touchPreviousPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                // If it's stationary for the whole fallTime then it won't be rotated (can only be moved)
                // Otherwise it still can be rotated
                if (Time.time - touchStartTime > fallTime)
                {
                    state = TetrominoState.Moving;
                    info.text = "Holding";
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                state = TetrominoState.Moving;
                // Move tetromino in the right direction
                info.text = "Moving";
                Vector2 deltaPosition = touch.position - touchInitialPosition;
                // Move tetromino left/right
                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y))
                {
                    info.text = "Horizontal move";
                    float horizontalMovement = (touch.position - touchPreviousPosition).x / Screen.width;
                    if (Mathf.Abs(horizontalMovement) > horizontalMovementSpeed * Time.deltaTime)
                    {
                        while (Mathf.Abs(horizontalMovement) > horizontalMovementSpeed * Time.deltaTime)
                        {
                            Move(horizontalMovement > 0.0f ? Vector3.right : Vector3.left);
                            horizontalMovement += (horizontalMovement > 0.0f ? -horizontalMovementSpeed : horizontalMovementSpeed) * Time.deltaTime;
                        }
                        touchPreviousPosition = touch.position;
                    }
                } 
                // Move tetromino up/down
                else if (Mathf.Abs(deltaPosition.x) < Mathf.Abs(deltaPosition.y))
                {
                    info.text = "Vertical move";

                }
            } 
            else if (touch.phase == TouchPhase.Ended)
            {
                if (state == TetrominoState.Controlled)
                {
                    // Rotate tetromino
                    info.text = "Rotating";
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, gameSettingsLeft ? 90.0f : -90.0f);
                }
            }
        }

            // Apply "gravity"
        if (Time.time - previousFallTime > fallTime)
        {
            Move(Vector3.down);
            previousFallTime = Time.time;
        }
    }

    private void Move(Vector3 translation)
    {
        // Check if the translation is valid by checking each square (block) of this tetromino 
        foreach (Transform square in squares)
        {
            int x = Mathf.RoundToInt((square.transform.position + translation).x);
            int y = Mathf.RoundToInt((square.transform.position + translation).y);
            if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            {
                return;
            }
        }
        // If every square can be moved to this location then simply move the whole tetromino
        transform.position += translation;
    }
}
