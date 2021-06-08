using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Tetris.Core
{
    [RequireComponent(typeof(Text))]
    public class FadeInOutEffect : MonoBehaviour
    {
        public float timeInBetween;
        public Color textColor;
        public bool shouldAnimate = true;

        private bool isAnimating = false;

        private void Update()
        {
            if (shouldAnimate && !isAnimating)
                StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            isAnimating = true;
            Text text = GetComponent<Text>();
            yield return StartCoroutine(FadeIn(text));
            yield return new WaitForSeconds(timeInBetween);
            yield return StartCoroutine(FadeAway(text));
            isAnimating = false;
        }

        private IEnumerator FadeAway(Text text)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                text.color = new Color(textColor.r, textColor.g, textColor.b, i);
                yield return null;
            }
        }

        private IEnumerator FadeIn(Text text)
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                text.color = new Color(textColor.r, textColor.g, textColor.b, i);
                yield return null;
            }
        }
    }
}
