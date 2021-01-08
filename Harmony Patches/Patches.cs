using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace HitSoundChanger.Harmony_Patches
{
    [HarmonyPatch(typeof(NoteCutSoundEffect))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    class BadCutSoundPatch
    {
        public static void Prefix(ref AudioClip[] ____badCutSoundEffectAudioClips)
        {
            if (Plugin.originalBadSounds == null)
            {
                Plugin.originalBadSounds = new List<AudioClip>();
                Plugin.originalBadSounds.AddRange(____badCutSoundEffectAudioClips);
            }

            if (Plugin.currentHitSound.badHitSoundEffects == null)
            {
                ____badCutSoundEffectAudioClips = Plugin.originalBadSounds.ToArray();
            }
            else
            {
            ____badCutSoundEffectAudioClips = Plugin.currentHitSound.badHitSoundEffects;
            }


        }
    }

    [HarmonyPatch(typeof(NoteCutSoundEffectManager))]
    [HarmonyPatch("Start", MethodType.Normal)]
    class HitSoundsPatch
    {
        public static void Prefix(ref AudioClip[] ____longCutEffectsAudioClips, ref AudioClip[] ____shortCutEffectsAudioClips)
        {
            if (Plugin.originalLongSounds == null)
            {
                Plugin.originalLongSounds = new List<AudioClip>();
                Plugin.originalLongSounds.AddRange(____longCutEffectsAudioClips);
            }

            if (Plugin.originalShortSounds == null)
            {
                Plugin.originalShortSounds = new List<AudioClip>();
                Plugin.originalShortSounds.AddRange(____shortCutEffectsAudioClips);
            }

            if (Plugin.currentHitSound.longHitSoundEffects == null || Plugin.currentHitSound.shortHitSoundEffects == null)
            {
                ____longCutEffectsAudioClips = Plugin.originalLongSounds.ToArray();
                ____shortCutEffectsAudioClips = Plugin.originalShortSounds.ToArray();
            }
            else
            {
            ____longCutEffectsAudioClips = Plugin.currentHitSound.longHitSoundEffects;
            ____shortCutEffectsAudioClips = Plugin.currentHitSound.shortHitSoundEffects;
            }


        }
    }

    [HarmonyPatch(typeof(GameNoteController))]
    [HarmonyPatch("NoteDidPassMissedMarker", MethodType.Normal)]
    class MissSoundsPatch 
    {
        static AudioSource[] audioSources;
        static float volumeMultiplier = 0.9f;

        public static void Postfix() 
        {
            if (Plugin.currentHitSound.missSoundEffect != null) 
            {
                if (audioSources == null) 
                {
                    audioSources = new AudioSource[64];
                    for (int i = 0; i < audioSources.Length; i++) 
                    {
                        audioSources[i] = new GameObject().AddComponent<AudioSource>();
                        audioSources[i].clip = Plugin.currentHitSound.missSoundEffect;
                        audioSources[i].volume = volumeMultiplier;
                        audioSources[i].gameObject.name = $"MissSoundEffect {i}";
                        UnityEngine.Object.DontDestroyOnLoad(audioSources[i]);
                    }
                }
                audioSources.First(x => !x.isPlaying).Play();
            }


        }
    }
}
