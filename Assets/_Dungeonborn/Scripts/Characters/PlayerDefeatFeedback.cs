using Dungeonborn.Combat;
using Dungeonborn.Input;
using UnityEngine;

namespace Dungeonborn.Characters
{
    [RequireComponent(typeof(Damageable))]
    public sealed class PlayerDefeatFeedback : MonoBehaviour
    {
        [SerializeField] private Color defeatedColor = new Color(0.25f, 0.02f, 0.02f);

        private Damageable damageable;
        private PlayerMotor motor;
        private PlayerCombatController combat;
        private Renderer[] renderers;
        private bool defeated;
        private float pulseTimer;
        private GUIStyle messageStyle;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AttachToPlayer()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.GetComponent<PlayerDefeatFeedback>() == null)
            {
                player.AddComponent<PlayerDefeatFeedback>();
            }
        }

        private void Awake()
        {
            damageable = GetComponent<Damageable>();
            motor = GetComponent<PlayerMotor>();
            combat = GetComponent<PlayerCombatController>();
            renderers = GetComponentsInChildren<Renderer>();
            damageable.Died.AddListener(HandleDefeated);
        }

        private void OnDestroy()
        {
            if (damageable != null)
            {
                damageable.Died.RemoveListener(HandleDefeated);
            }
        }

        private void Update()
        {
            if (!defeated)
            {
                return;
            }

            pulseTimer += Time.deltaTime * 5f;
            var pulse = 1f + Mathf.Sin(pulseTimer) * 0.04f;
            transform.localScale = new Vector3(pulse, 0.45f, pulse);
        }

        private void OnGUI()
        {
            if (!defeated)
            {
                return;
            }

            EnsureStyle();
            const float width = 520f;
            const float height = 80f;
            var rect = new Rect((Screen.width - width) * 0.5f, Screen.height * 0.18f, width, height);
            GUI.Box(rect, string.Empty);
            GUI.Label(rect, "DEFEATED - Press R to reset sandbox", messageStyle);
        }

        private void HandleDefeated()
        {
            if (defeated)
            {
                return;
            }

            defeated = true;
            if (motor != null)
            {
                motor.enabled = false;
            }

            if (combat != null)
            {
                combat.enabled = false;
            }

            foreach (var targetRenderer in renderers)
            {
                if (targetRenderer != null)
                {
                    targetRenderer.material.color = defeatedColor;
                }
            }

            SpawnDefeatMarker();
        }

        private void SpawnDefeatMarker()
        {
            var marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            marker.name = "Playtest_PlayerDefeated_Marker";
            Destroy(marker.GetComponent<Collider>());
            marker.transform.position = transform.position + Vector3.up * 0.05f;
            marker.transform.localScale = new Vector3(2.2f, 0.04f, 2.2f);
            marker.GetComponent<Renderer>().material.color = new Color(0.8f, 0.02f, 0.02f);
            Destroy(marker, 2f);
        }

        private void EnsureStyle()
        {
            if (messageStyle != null)
            {
                return;
            }

            messageStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 28,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
        }
    }
}
