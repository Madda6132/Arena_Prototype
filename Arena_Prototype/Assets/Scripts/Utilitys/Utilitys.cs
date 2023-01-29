using System.Collections.Generic;
using UnityEngine; 
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using System; 
using System.Linq;

namespace RPG {

    public static class Utilitys {

        public const float TRAVEL_SPEED = 10;

        public static async Task TimeScaleChange(float timeScaleTarget, float changeSpeed = 1f) {

            timeScaleTarget = Mathf.Max(timeScaleTarget, 0f);
            float modifier = Time.timeScale < timeScaleTarget ? 1 : -1;
            modifier *= MathF.Abs(changeSpeed);

            if (!timeScaleTarget.AlmostEqual(Time.timeScale)) {
                float timeScale = Time.timeScale;

                while (!timeScale.AlmostEqual(timeScaleTarget)) {

                    timeScale += Time.fixedUnscaledDeltaTime * modifier;
                    if (modifier < 0) {
                        timeScale = Mathf.Max(timeScale, timeScaleTarget);
                    } else {
                        timeScale = Mathf.Min(timeScale, timeScaleTarget);
                    }

                    Time.timeScale = timeScale;
                    await Task.Yield();
                }
            }

            await Task.Yield();
        }

        /// <summary>
        /// Fade all ILayoutElement in fadeGameObject
        /// </summary>
        /// <param name="fadeTarget"> How much to fade to 0 = Invisible / 1 = Opaque </param>
        /// <param name="fadeSpeed"> Seconds it takes to fade </param>
        /// <param name="fadeGameObject"> GameObject with ILayoutElement components to fade </param>
        /// <returns></returns>
        public static async Task FadeUI(float fadeTarget, float fadeSpeed, GameObject fadeGameObject) {

            List<ILayoutElement> layoutElements = new List<ILayoutElement>();
            foreach (var item in fadeGameObject.GetComponents<ILayoutElement>()) {

                layoutElements.Add(item);
            }

            foreach (var item in fadeGameObject.GetComponentsInChildren<ILayoutElement>()) {

                layoutElements.Add(item);
            }

            layoutElements = layoutElements.Where(x => CheckCastColor(x)).ToList();
            await FadeUI(fadeTarget, fadeSpeed, layoutElements.ToArray());
        }

        /// <summary>
        /// Fade all ILayoutElement
        /// </summary>
        /// <param name="fadeTarget"> How much to fade to 0 = Invisible / 1 = Opaque </param>
        /// <param name="fadeSpeed"> Seconds it takes to fade </param>
        /// <param name="fadeElement"> ILayoutElement to fade components </param>
        /// <returns></returns>
        public static async Task FadeUI(float fadeTarget, float fadeSpeed, params ILayoutElement[] fadeElement) {

            Func<float, float, Color, float> fadeColor = (fadetarget, speed, color) => {

                float fadeAmount = 0;
                float modifier = fadeTarget < color.a ? -1 : 1;
                modifier *= 1 / speed;
                fadeAmount = color.a + (Time.fixedDeltaTime * modifier);
                if (modifier < 0) {
                    fadeAmount = Mathf.Max(fadeAmount, fadeTarget);
                } else {
                    fadeAmount = Mathf.Min(fadeAmount, fadeTarget);
                }

                return fadeAmount;
            };

            fadeSpeed = MathF.Abs(fadeSpeed);

            bool prossessing = true;
            Color color = new Color();
            while (prossessing) {

                bool isFinished = true;
                foreach (var item in fadeElement) {

                    switch (item) {
                        case Image:
                            Image image = (Image)item;
                            color = image.color;
                            if (color.a.AlmostEqual(fadeTarget)) break;
                            image.color = new Color(color.r, color.g, color.b, fadeColor(fadeTarget, fadeSpeed, color));

                            break;
                        case TextMeshProUGUI:
                            TextMeshProUGUI text = (TextMeshProUGUI)item;
                            color = text.color;
                            if (color.a.AlmostEqual(fadeTarget)) break;
                            text.color = new Color(color.r, color.g, color.b, fadeColor(fadeTarget, fadeSpeed, color));

                            break;
                        default:
                            Debug.Log("Unacceptable cast of " + nameof(item.GetType));
                            break;

                    }

                    if (isFinished)
                        isFinished = color.a.AlmostEqual(fadeTarget);
                }

                prossessing = !isFinished;
                await Task.Yield();
            }

        }

        public static T Random<T>(this List<T> list) {

            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static bool AlmostEqual(this float x, float y) => Math.Abs(x - y) < 0.001f;

        public static bool CheckEnumValues<T>(this T requirements, T values) where T : Enum {

            bool hasValues = true;
            foreach (Enum value in Enum.GetValues(requirements.GetType())) {

                if (requirements.HasFlag(value)) {

                    hasValues = values.HasFlag(value);

                    if (!hasValues) break;
                }
            }

            return hasValues;
        }

        /*---Private---*/

        private static bool CheckCastColor(ILayoutElement element) {

            switch (element) {
                case Image:
                case TextMeshProUGUI:
                    return true;
                default:
                    return false;
            }

        }
    }

}
