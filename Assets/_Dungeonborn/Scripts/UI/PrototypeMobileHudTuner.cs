using UnityEngine;
using UnityEngine.UI;

namespace Dungeonborn.UI
{
    public sealed class PrototypeMobileHudTuner : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (FindAnyObjectByType<PrototypeMobileHudTuner>() != null)
            {
                return;
            }

            new GameObject("Prototype Mobile HUD Tuner").AddComponent<PrototypeMobileHudTuner>();
        }

        private void Start()
        {
            TuneStick();
            TuneButton("Attack Button", new Vector2(-264f, 84f));
            TuneButton("Dash Button", new Vector2(-160f, 84f));
            TuneButton("Cleave Button", new Vector2(-264f, 188f));
            TuneButton("Stomp Button", new Vector2(-160f, 188f));
            TuneButton("Rage Button", new Vector2(-56f, 136f));
        }

        private static void TuneStick()
        {
            var stick = GameObject.Find("Move Stick");
            if (stick == null)
            {
                return;
            }

            var rect = stick.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(128f, 128f);
            rect.sizeDelta = new Vector2(132f, 132f);

            if (stick.TryGetComponent<Image>(out var image))
            {
                image.color = new Color(1f, 1f, 1f, 0.12f);
            }
        }

        private static void TuneButton(string objectName, Vector2 anchoredPosition)
        {
            var button = GameObject.Find(objectName);
            if (button == null)
            {
                return;
            }

            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = new Vector2(78f, 78f);

            if (button.TryGetComponent<Image>(out var image))
            {
                image.color = new Color(0.08f, 0.08f, 0.1f, 0.52f);
            }
        }
    }
}
