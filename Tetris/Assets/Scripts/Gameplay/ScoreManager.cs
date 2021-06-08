using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        public delegate void OnScoreAchieved(int additionalScore);
        public OnScoreAchieved scoreAchievedDelegate;

        public int Score { get; private set; }

        // Grid on which the game is played
        public TetrisGrid tetrisGrid;

        // Text that will show current score number
        public Text ScoreText;

        // Array which represents how much points number of cleared rows brings
        // e.g. clearedRows[0] = 100 means that for 1 (0 + 1) cleared row we get 100 points
        public int[] clearedRowsScore;

        private void Start()
        {
            Score = 0;
            tetrisGrid.rowsClearedDelegate += AddForRows;
        }

        private void Update()
        {
            ScoreText.text = Score.ToString();
        }

        /// <summary>
        /// Adds up to current score based on the number of cleared rows
        /// </summary>
        /// <param name="rowsCleared"></param>
        public void AddForRows(int rowsCleared)
        {
            // NOTE: This should never happen! (sanity check)
            if (clearedRowsScore.Length == 0)
                return;
            int additionalScore = clearedRowsScore[Mathf.Clamp(rowsCleared - 1, 0, clearedRowsScore.Length - 1)];
            Score += additionalScore;
            scoreAchievedDelegate(additionalScore);
        }

        /// <summary>
        /// Resets current score
        /// </summary>
        public void Restart()
        {
            Score = 0;
        }
    }
}
