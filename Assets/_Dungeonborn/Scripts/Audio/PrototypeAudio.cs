using System;
using System.Reflection;
using UnityEngine;

namespace Dungeonborn.Audio
{
    public static class PrototypeAudio
    {
        private const int SampleRate = 22050;

        private static Type audioClipType;
        private static Type audioSourceType;
        private static Type audioListenerType;
        private static MethodInfo createClipMethod;
        private static MethodInfo setDataMethod;
        private static MethodInfo playOneShotMethod;
        private static PropertyInfo playOnAwakeProperty;
        private static PropertyInfo spatialBlendProperty;
        private static PropertyInfo volumeProperty;
        private static bool audioTypesResolved;
        private static GameObject audioSourceObject;
        private static Component audioSourceComponent;

        private static object basicSwing;
        private static object basicHit;
        private static object dash;
        private static object ability;
        private static object heavyAbility;
        private static object enemyDeath;
        private static object lootDrop;
        private static object lootPickup;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            EnsureClips();
            EnsureAudioListener();
        }

        public static void PlayBasicSwing(Vector3 position)
        {
            EnsureClips();
            Play(basicSwing, position, 0.35f);
        }

        public static void PlayBasicHit(Vector3 position)
        {
            EnsureClips();
            Play(basicHit, position, 0.45f);
        }

        public static void PlayDash(Vector3 position)
        {
            EnsureClips();
            Play(dash, position, 0.35f);
        }

        public static void PlayAbility(Vector3 position)
        {
            EnsureClips();
            Play(ability, position, 0.4f);
        }

        public static void PlayHeavyAbility(Vector3 position)
        {
            EnsureClips();
            Play(heavyAbility, position, 0.45f);
        }

        public static void PlayEnemyDeath(Vector3 position)
        {
            EnsureClips();
            Play(enemyDeath, position, 0.45f);
        }

        public static void PlayLootDrop(Vector3 position)
        {
            EnsureClips();
            Play(lootDrop, position, 0.55f);
        }

        public static void PlayLootPickup(Vector3 position)
        {
            EnsureClips();
            Play(lootPickup, position, 0.6f);
        }

        private static void Play(object clip, Vector3 position, float volume)
        {
            EnsureClips();
            EnsureAudioListener();

            if (clip != null && playOneShotMethod != null && EnsureAudioSource())
            {
                volumeProperty?.SetValue(audioSourceComponent, Mathf.Clamp01(volume * 1.8f));
                playOneShotMethod.Invoke(audioSourceComponent, new[] { clip, 1f });
            }
        }

        private static void EnsureClips()
        {
            ResolveAudioTypes();

            if (createClipMethod == null || setDataMethod == null || playOneShotMethod == null)
            {
                return;
            }

            basicSwing ??= CreateWhoosh("Prototype_BasicSwing", 220f, 95f, 0.14f, 0.8f, 11);
            basicHit ??= CreateImpact("Prototype_BasicHit", 130f, 0.13f, 0.95f, 23);
            dash ??= CreateWhoosh("Prototype_Dash", 320f, 140f, 0.18f, 0.8f, 37);
            ability ??= CreateWhoosh("Prototype_Ability", 280f, 130f, 0.22f, 0.85f, 41);
            heavyAbility ??= CreateImpact("Prototype_HeavyAbility", 85f, 0.3f, 1f, 53);
            enemyDeath ??= CreateSweep("Prototype_EnemyDeath", 180f, 60f, 0.32f, 0.8f);
            lootDrop ??= CreateSoftChime("Prototype_LootDrop", 330f, 0.24f, 0.55f);
            lootPickup ??= CreateSoftChime("Prototype_LootPickup", 420f, 0.28f, 0.65f);
        }

        private static void EnsureAudioListener()
        {
            ResolveAudioTypes();

            if (audioListenerType == null)
            {
                return;
            }

            var mainCamera = global::UnityEngine.Camera.main;
            if (mainCamera != null && mainCamera.GetComponent(audioListenerType) == null)
            {
                mainCamera.gameObject.AddComponent(audioListenerType);
            }
        }

        private static object CreateTone(string name, float frequency, float duration, float gain, Wave wave)
        {
            var sampleCount = Mathf.CeilToInt(SampleRate * duration);
            var samples = new float[sampleCount];

            for (var i = 0; i < sampleCount; i++)
            {
                var t = i / (float)SampleRate;
                var phase = t * frequency;
                var envelope = 1f - i / (float)sampleCount;
                samples[i] = GetWaveValue(phase, wave) * envelope * gain;
            }

            return CreateClip(name, samples);
        }

        private static object CreateSweep(string name, float startFrequency, float endFrequency, float duration, float gain)
        {
            var sampleCount = Mathf.CeilToInt(SampleRate * duration);
            var samples = new float[sampleCount];

            for (var i = 0; i < sampleCount; i++)
            {
                var progress = i / (float)sampleCount;
                var frequency = Mathf.Lerp(startFrequency, endFrequency, progress);
                var t = i / (float)SampleRate;
                var envelope = 1f - progress;
                samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * gain;
            }

            return CreateClip(name, samples);
        }

        private static object CreateWhoosh(string name, float startFrequency, float endFrequency, float duration, float gain, int seed)
        {
            var sampleCount = Mathf.CeilToInt(SampleRate * duration);
            var samples = new float[sampleCount];
            var filteredNoise = 0f;

            for (var i = 0; i < sampleCount; i++)
            {
                var progress = i / (float)sampleCount;
                var frequency = Mathf.Lerp(startFrequency, endFrequency, progress);
                var t = i / (float)SampleRate;
                var envelope = Mathf.Sin(progress * Mathf.PI);
                var noise = PseudoNoise(i, seed);
                filteredNoise = Mathf.Lerp(filteredNoise, noise, 0.08f);
                var tone = Mathf.Sin(2f * Mathf.PI * frequency * t) * 0.35f;
                samples[i] = (filteredNoise * 0.65f + tone) * envelope * gain;
            }

            return CreateClip(name, samples);
        }

        private static object CreateImpact(string name, float frequency, float duration, float gain, int seed)
        {
            var sampleCount = Mathf.CeilToInt(SampleRate * duration);
            var samples = new float[sampleCount];
            var filteredNoise = 0f;

            for (var i = 0; i < sampleCount; i++)
            {
                var progress = i / (float)sampleCount;
                var t = i / (float)SampleRate;
                var envelope = Mathf.Pow(1f - progress, 2.3f);
                var body = Mathf.Sin(2f * Mathf.PI * frequency * t) * 0.85f;
                filteredNoise = Mathf.Lerp(filteredNoise, PseudoNoise(i, seed), 0.12f);
                samples[i] = (body + filteredNoise * 0.25f) * envelope * gain;
            }

            return CreateClip(name, samples);
        }

        private static object CreateSoftChime(string name, float frequency, float duration, float gain)
        {
            var sampleCount = Mathf.CeilToInt(SampleRate * duration);
            var samples = new float[sampleCount];

            for (var i = 0; i < sampleCount; i++)
            {
                var progress = i / (float)sampleCount;
                var t = i / (float)SampleRate;
                var envelope = Mathf.Pow(1f - progress, 1.7f);
                var baseTone = Mathf.Sin(2f * Mathf.PI * frequency * t) * 0.7f;
                var lowTone = Mathf.Sin(2f * Mathf.PI * frequency * 0.5f * t) * 0.3f;
                samples[i] = (baseTone + lowTone) * envelope * gain;
            }

            return CreateClip(name, samples);
        }

        private static object CreateClip(string name, float[] samples)
        {
            var clip = createClipMethod.Invoke(null, new object[] { name, samples.Length, 1, SampleRate, false });
            setDataMethod.Invoke(clip, new object[] { samples, 0 });
            return clip;
        }

        private static bool EnsureAudioSource()
        {
            if (audioSourceComponent != null)
            {
                return true;
            }

            if (audioSourceType == null)
            {
                return false;
            }

            audioSourceObject = new GameObject("Prototype Audio Source");
            UnityEngine.Object.DontDestroyOnLoad(audioSourceObject);
            audioSourceComponent = audioSourceObject.AddComponent(audioSourceType);
            playOnAwakeProperty?.SetValue(audioSourceComponent, false);
            spatialBlendProperty?.SetValue(audioSourceComponent, 0f);
            volumeProperty?.SetValue(audioSourceComponent, 1f);
            return audioSourceComponent != null;
        }

        private static void ResolveAudioTypes()
        {
            if (audioTypesResolved)
            {
                return;
            }

            audioTypesResolved = true;

            audioClipType = Type.GetType("UnityEngine.AudioClip, UnityEngine.AudioModule", false);
            audioSourceType = Type.GetType("UnityEngine.AudioSource, UnityEngine.AudioModule", false);
            audioListenerType = Type.GetType("UnityEngine.AudioListener, UnityEngine.AudioModule", false);

            if (audioClipType == null || audioSourceType == null)
            {
                return;
            }

            createClipMethod = audioClipType.GetMethod(
                "Create",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new[] { typeof(string), typeof(int), typeof(int), typeof(int), typeof(bool) },
                null);
            setDataMethod = audioClipType.GetMethod(
                "SetData",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new[] { typeof(float[]), typeof(int) },
                null);
            playOneShotMethod = audioSourceType.GetMethod(
                "PlayOneShot",
                BindingFlags.Public | BindingFlags.Instance,
                null,
                new[] { audioClipType, typeof(Vector3), typeof(float) },
                null);
            if (playOneShotMethod == null)
            {
                playOneShotMethod = audioSourceType.GetMethod(
                    "PlayOneShot",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new[] { audioClipType, typeof(float) },
                    null);
            }

            playOnAwakeProperty = audioSourceType.GetProperty("playOnAwake", BindingFlags.Public | BindingFlags.Instance);
            spatialBlendProperty = audioSourceType.GetProperty("spatialBlend", BindingFlags.Public | BindingFlags.Instance);
            volumeProperty = audioSourceType.GetProperty("volume", BindingFlags.Public | BindingFlags.Instance);
        }

        private static float GetWaveValue(float phase, Wave wave)
        {
            return wave switch
            {
                Wave.Square => Mathf.Sign(Mathf.Sin(2f * Mathf.PI * phase)),
                Wave.Saw => 2f * (phase - Mathf.Floor(phase + 0.5f)),
                _ => Mathf.Sin(2f * Mathf.PI * phase)
            };
        }

        private static float PseudoNoise(int index, int seed)
        {
            var value = Mathf.Sin((index + seed) * 12.9898f) * 43758.5453f;
            return Mathf.Repeat(value, 1f) * 2f - 1f;
        }

        private enum Wave
        {
            Sine,
            Square,
            Saw
        }
    }
}
