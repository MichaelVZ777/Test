﻿using UnityEngine;
using System.Collections;

namespace UnityLib.Audio
{
    public class StaticSoundManager : SingletonMonoManager<StaticSoundManager>
    {
        public static void Play(AudioClip clip)
        {
            Play(Vector3.zero, clip);
        }

        public static void Play(Vector3 position, AudioClip clip)
        {
            var soundObject = new GameObject("Sound");
            soundObject.transform.SetParent(Instance.transform);
            soundObject.transform.position = position;
            var staticSound = soundObject.AddComponent<StaticSound>();
            staticSound.source = soundObject.AddComponent<AudioSource>();
            staticSound.source.clip = clip;
            staticSound.source.Play();
        }
    }
}