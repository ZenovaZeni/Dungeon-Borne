using Dungeonborn.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace Dungeonborn.Core
{
    public sealed class PrototypeSandboxResetter : MonoBehaviour
    {
        private PlayerInputReader subscribedInput;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Create()
        {
            if (FindAnyObjectByType<PrototypeSandboxResetter>() != null)
            {
                return;
            }

            var resetter = new GameObject("Prototype Sandbox Resetter");
            DontDestroyOnLoad(resetter);
            resetter.AddComponent<PrototypeSandboxResetter>();
        }

        private void Update()
        {
            if (subscribedInput != null)
            {
                return;
            }

            var input = FindAnyObjectByType<PlayerInputReader>();
            if (input == null)
            {
                return;
            }

            subscribedInput = input;
            subscribedInput.ResetSandboxPressed += ResetSandbox;
        }

        private void OnDestroy()
        {
            if (subscribedInput != null)
            {
                subscribedInput.ResetSandboxPressed -= ResetSandbox;
            }
        }

        public static void ResetActiveSandbox()
        {
            var resetter = FindAnyObjectByType<PrototypeSandboxResetter>();
            if (resetter == null)
            {
                var resetterObject = new GameObject("Prototype Sandbox Resetter");
                DontDestroyOnLoad(resetterObject);
                resetter = resetterObject.AddComponent<PrototypeSandboxResetter>();
            }

            resetter.ResetSandbox();
        }

        private void ResetSandbox()
        {
            if (subscribedInput != null)
            {
                subscribedInput.ResetSandboxPressed -= ResetSandbox;
                subscribedInput = null;
            }

            Time.timeScale = 1f;
            var activeScene = SceneManager.GetActiveScene();

#if UNITY_EDITOR
            if (!string.IsNullOrWhiteSpace(activeScene.path))
            {
                EditorSceneManager.LoadSceneInPlayMode(activeScene.path, new LoadSceneParameters(LoadSceneMode.Single));
                return;
            }
#endif

            if (activeScene.buildIndex >= 0)
            {
                SceneManager.LoadScene(activeScene.buildIndex);
                return;
            }

            SceneManager.LoadScene(activeScene.name);
        }
    }
}
