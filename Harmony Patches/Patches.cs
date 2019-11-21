using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harmony;
using UnityEngine;
namespace HitSoundChanger.Harmony_Patches
{
    [HarmonyPatch(typeof(NoteCutSoundEffect))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    class BadCutSoundPatch
    {
        public static void Prefix(ref AudioClip[] ____badCutSoundEffectAudioClips)
        {
            if (Plugin.badHitSoundEffects == null) return;
            ____badCutSoundEffectAudioClips = Plugin.badHitSoundEffects;

        }
    }

    [HarmonyPatch(typeof(NoteCutSoundEffectManager))]
    [HarmonyPatch("Start", MethodType.Normal)]
    class HitSoundsPatch
    {
        public static void Prefix(ref AudioClip[] ____longCutEffectsAudioClips, ref AudioClip[] ____shortCutEffectsAudioClips)
        {
            if (Plugin.longHitSoundEffects == null || Plugin.shortHitSoundEffects == null) return;
            ____longCutEffectsAudioClips = Plugin.longHitSoundEffects;
            ____shortCutEffectsAudioClips = Plugin.shortHitSoundEffects;

        }
    }
}
