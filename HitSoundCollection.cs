using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
namespace HitSoundChanger
{
    public class HitSoundCollection
    {
        public string name;
        public string folderPath;
        public string containedSounds
        {
            get
            {
                string result = "";
                if (shortHitSoundEffects != null || longHitSoundEffects != null)
                {
                    result = "Hit";
                    if (badHitSoundEffects != null)
                        result += ", BadHit";
                }
                else if (badHitSoundEffects != null)
                    result = "BadHit";
                else
                {
                    result = "No Sounds Replaced";
                }
                return result;
            }
        }

        public AudioClip[] shortHitSoundEffects;
        public AudioClip[] longHitSoundEffects;
        public AudioClip[] badHitSoundEffects;
        public HitSoundCollection()
        {
        }
        public HitSoundCollection(string path)
        {
            folderPath = path;
            name = new DirectoryInfo(path).Name;

        }

        public IEnumerator LoadSounds()
        {
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
    }
}
