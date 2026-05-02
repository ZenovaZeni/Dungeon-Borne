using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dungeonborn.Visual
{
    public sealed class PrototypeArenaVisualPass : MonoBehaviour
    {
        private const string CombatSandboxScene = "CombatSandbox_Prototype_0_1";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void ApplyForCombatSandbox()
        {
            if (SceneManager.GetActiveScene().name != CombatSandboxScene)
            {
                return;
            }

            if (FindFirstObjectByType<PrototypeArenaVisualPass>() != null)
            {
                return;
            }

            new GameObject("Prototype Arena Visual Pass").AddComponent<PrototypeArenaVisualPass>();
        }

        private void Awake()
        {
            ApplyLighting();
            ApplyCameraBackdrop();
            ApplyArenaContrast();
            CreateTorchPlaceholder("Torch_NorthWest", new Vector3(-7.5f, 2.2f, 10.5f));
            CreateTorchPlaceholder("Torch_NorthEast", new Vector3(7.5f, 2.2f, 10.5f));
            CreateTorchPlaceholder("Torch_SouthWest", new Vector3(-7.5f, 2.2f, -4.5f));
            CreateTorchPlaceholder("Torch_SouthEast", new Vector3(7.5f, 2.2f, -4.5f));
        }

        private static void ApplyLighting()
        {
            RenderSettings.ambientLight = new Color(0.045f, 0.05f, 0.065f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = new Color(0.035f, 0.04f, 0.055f);
            RenderSettings.fogDensity = 0.016f;

            var keyLight = GameObject.Find("Key Light");
            if (keyLight != null && keyLight.TryGetComponent<Light>(out var light))
            {
                light.color = new Color(0.72f, 0.78f, 1f);
                light.intensity = 0.55f;
                light.transform.rotation = Quaternion.Euler(58f, -35f, 0f);
            }
        }

        private static void ApplyCameraBackdrop()
        {
            if (Camera.main == null)
            {
                return;
            }

            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(0.025f, 0.027f, 0.035f);
        }

        private static void ApplyArenaContrast()
        {
            SetRendererColor("Arena_Floor", new Color(0.145f, 0.15f, 0.18f));
            SetRendererColor("North_Wall", new Color(0.075f, 0.07f, 0.085f));
            SetRendererColor("South_Wall", new Color(0.075f, 0.07f, 0.085f));
            SetRendererColor("West_Wall", new Color(0.075f, 0.07f, 0.085f));
            SetRendererColor("East_Wall", new Color(0.075f, 0.07f, 0.085f));
            SetRendererColor("Fighter_Player", new Color(0.18f, 0.48f, 1f));
            SetRendererColor("Skeleton Grunt", new Color(0.9f, 0.92f, 0.84f));
            SetRendererColor("Archer", new Color(0.35f, 0.9f, 0.46f));
            SetRendererColor("Brute", new Color(0.72f, 0.26f, 0.2f));
        }

        private static void SetRendererColor(string objectName, Color color)
        {
            var target = GameObject.Find(objectName);
            if (target == null || !target.TryGetComponent<Renderer>(out var renderer))
            {
                return;
            }

            renderer.material.color = color;
        }

        private static void CreateTorchPlaceholder(string name, Vector3 position)
        {
            if (GameObject.Find(name) != null)
            {
                return;
            }

            var torch = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            torch.name = name;
            torch.transform.position = position + Vector3.down * 0.65f;
            torch.transform.localScale = new Vector3(0.18f, 0.7f, 0.18f);
            Destroy(torch.GetComponent<Collider>());
            torch.GetComponent<Renderer>().material.color = new Color(0.18f, 0.1f, 0.05f);

            var flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flame.name = name + "_Glow";
            flame.transform.SetParent(torch.transform);
            flame.transform.localPosition = Vector3.up * 0.72f;
            flame.transform.localScale = Vector3.one * 1.6f;
            Destroy(flame.GetComponent<Collider>());
            flame.GetComponent<Renderer>().material.color = new Color(1f, 0.55f, 0.12f);

            var light = torch.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.62f, 0.22f);
            light.intensity = 1.45f;
            light.range = 6.5f;
        }
    }
}
