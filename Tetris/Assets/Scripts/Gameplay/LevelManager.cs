using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Gameplay
{
    [RequireComponent(typeof(AudioSource))]
    public class LevelManager : MonoBehaviour
    {
        public int Level { get; private set; }

        [SerializeField]
        private AudioClip levelUpAudio;

        [SerializeField]
        // Manager that handles the score
        private ScoreManager scoreManager;

        [SerializeField]
        // Array which represents at what fallTime will tetrominos fall at certain level
        // e.g. levelFallTimes[0] = 1.0f means that at level 1 (0 + 1) tetrominos will fall with fallTime of 1.0f
        private float[] levelFallTimes;

        [SerializeField]
        // Text that will show current level
        private Text LevelText;

        private void Start()
        {
            Level = 1;
            UpdateFallTime();
            scoreManager.scoreAchievedDelegate += ScoreChanged;
        }

        private void ScoreChanged(int additionalScore)
        {
            // Level is increased once the highest possible score points multiplied with current level have been achieved
            // e.g. if for clearing 4 rows you get score of 3000 then that is the treshold for the next level
            if (scoreManager.Score >= Level * scoreManager.ClearedRowsScore[scoreManager.ClearedRowsScore.Length - 1])
            {
                GetComponent<AudioSource>().PlayOneShot(levelUpAudio);
                Level++;
                UpdateFallTime();
            }
        }

        private void Update()
        {
            LevelText.text = Level.ToString();
        }

        /// <summary>
        /// Updates maximum fall time for tetrominos based on current level
        /// </summary>
        private void UpdateFallTime()
        {
            // NOTE: This should never happen! (sanity check)
            if (levelFallTimes.Length == 0)
                return;
            Tetromino.maximumFallTime = levelFallTimes[Mathf.Clamp(Level - 1, 0, levelFallTimes.Length - 1)];
        }

        public void Restart()
        {
            Level = 1;
            UpdateFallTime();
        }
    }
}
