using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using IPA;
using IPALogger = IPA.Logging.Logger;
using Harmony;
using UnityEngine.Networking;
namespace HitSoundChanger
{
    public class Plugin : IBeatSaberPlugin
    {
        public static AudioClip[] shortHitSoundEffects;
        public static AudioClip[] longHitSoundEffects;
        public static AudioClip[] badHitSoundEffects;

        public void OnApplicationStart()
        {
            var harmony = HarmonyInstance.Create("com.kyle1413.BeatSaber.HitSoundChanger");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

        }

        public void Init(object thisIsNull, IPALogger pluginLogger)
        {

            Utilities.Logging.Log = pluginLogger;
        }

        public IEnumerator LoadAudio()
        {
            Utilities.Logging.Log.Notice("Attempting to load Audio files");
            var folderPath = Environment.CurrentDirectory + "/UserData/HitSoundChanger";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            if (shortHitSoundEffects == null || longHitSoundEffects == null)
                if (File.Exists(folderPath + "/HitSound.ogg"))
                {
                    string url1 = "file:///" + folderPath + "/HitSound.ogg";
                    UnityWebRequest www1 = UnityWebRequestMultimedia.GetAudioClip(url1, AudioType.OGGVORBIS);
                    AudioClip hitAudio = null;
                    yield return www1.SendWebRequest();

                    if (www1.isNetworkError)
                        Utilities.Logging.Log.Notice("Failed to load HitSound audio: " + www1.error);
                    else
                        hitAudio = DownloadHandlerAudioClip.GetContent(www1);
                    if (hitAudio != null)
                    {
                        shortHitSoundEffects = new AudioClip[] { hitAudio };
                        longHitSoundEffects = new AudioClip[] { hitAudio };
                    }
                }
            if (badHitSoundEffects == null)
                if (File.Exists(folderPath + "/BadHitSound.ogg"))
                {
                    string url2 = "file:///" + folderPath + "/BadHitSound.ogg";
                    UnityWebRequest www2 = UnityWebRequestMultimedia.GetAudioClip(url2, AudioType.OGGVORBIS);
                    AudioClip badHitAudio = null;
                    yield return www2.SendWebRequest();

                    if (www2.isNetworkError)
                        Utilities.Logging.Log.Notice("Failed to load HitSound audio: " + www2.error);
                    else
                        badHitAudio = DownloadHandlerAudioClip.GetContent(www2);
                    if (badHitAudio != null)
                    {
                        badHitSoundEffects = new AudioClip[] { badHitAudio };
                    }
                }


        }
        public void OnApplicationQuit()
        {

        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {

        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuViewControllers")
                SharedCoroutineStarter.instance.StartCoroutine(LoadAudio());
        }

        public void OnUpdate()
        {

        }

        public void OnFixedUpdate()
        {

        }

    }
}
