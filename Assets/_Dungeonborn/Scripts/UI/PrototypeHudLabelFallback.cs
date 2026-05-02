using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dungeonborn.UI
{
    public sealed class PrototypeHudLabelFallback : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AddFallbackLabels()
        {
            AddFallbackLabel("Attack Button", "ATK");
            AddFallbackLabel("Dash Button", "DASH");
            AddFallbackLabel("Cleave Button", "CLV");
            AddFallbackLabel("Stomp Button", "STP");
            AddFallbackLabel("Rage Button", "RAGE");
        }

        private static void AddFallbackLabel(string buttonName, string text)
        {
            var button = GameObject.Find(buttonName);
            if (button == null || button.transform.Find("Prototype Label") != null)
            {
                return;
            }

            var tmpLabel = button.GetComponentInChildren<TextMeshProUGUI>();
            if (tmpLabel != null && tmpLabel.font != null)
            {
                return;
            }

            var labelObject = new GameObject("Prototype Label");
            labelObject.transform.SetParent(button.transform, false);

            var rect = labelObject.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var label = labelObject.AddComponent<Text>();
            label.text = text;
            label.alignment = TextAnchor.MiddleCenter;
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf") ??
                         Resources.GetBuiltinResource<Font>("Arial.ttf");
            label.fontSize = 22;
            label.fontStyle = FontStyle.Bold;
            label.color = Color.white;
            label.raycastTarget = false;
        }
    }
}
