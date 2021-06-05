using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public float horizontalMovementSpeed = 1.0f;
    public Vector3 rotationPoint;
    public Transform[] squares;

    public bool gameSettingsLeft = false;

    public int gridWidth = 10;
    public int gridHeight = 21;

    private TetrominoState state = TetrominoState.Idle;
    private float previousFallTime;
    private float touchStartTime;

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
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                // If it's stationary for the whole fallTime then it won't be rotated (can only be moved)
                // Otherwise it still can be rotated
                if (Time.time - touchStartTime > fallTime)
                {
                    state = TetrominoState.Moving;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                state = TetrominoState.Moving;
                // Move tetromino left/right
                if (Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y))
                {
                    state = TetrominoState.HorizontalMove;
                    float horizontalMovement = Mathf.RoundToInt(touch.deltaPosition.x * horizontalMovementSpeed * Time.deltaTime);
                    Move(horizontalMovement * Vector3.right);
                } 
                // Move tetromino up/down
                else if (Mathf.Abs(touch.deltaPosition.x) < Mathf.Abs(touch.deltaPosition.y))
                {
                    state = TetrominoState.VerticalMove;
                    if (touch.deltaPosition.y < 0.0f)
                    {
                        fallTime -= 0.01f * Mathf.Abs(touch.deltaPosition.y);
                    } else
                    {
                        fallTime += 0.01f * Mathf.Abs(touch.deltaPosition.y);
                        fallTime = Mathf.Clamp01(fallTime);
                    }
                }
            } 
            else if (touch.phase == TouchPhase.Ended)
            {
                if (state == TetrominoState.Controlled)
                {
                    // Rotate tetromino
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, gameSettingsLeft ? 90.0f : -90.0f);
                }
                // Set tetromino to IDLE state
                state = TetrominoState.Idle;
            }
        }

        // Apply "gravity"
        if (Time.time - previousFallTime > fallTime)
        {
            // Tetromino won't fall down while it's moving left/right
            if (state != TetrominoState.HorizontalMove && state != TetrominoState.VerticalMove)
            {
                Move(Vector3.down);
                previousFallTime = Time.time;

                // Disable this tetromino if it's not active anymore
                if (!IsActive())
                {
                    enabled = false;
                    Debug.Log("Disabled");
                }
            }
        }
    }

    private void Move(Vector3 translation)
    {
        // If every square can be moved to this location then simply move the whole tetromino
        if (CheckMove(translation))
            transform.position += translation;
    }

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
    /// Tetromino is no longer active when it reaches bottom of the grid or when it interacts with another tetromino
    /// </summary>
    /// <returns></returns>
    private bool IsActive()
    {
        // Check if tetromino can fall down for one step
        return CheckMove(Vector3.down);
    }
}
