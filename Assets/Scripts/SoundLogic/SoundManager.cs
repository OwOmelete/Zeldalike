using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string id;
        public AudioClip clip;
    }

    [Tooltip("Add sound.mp3 or whatever and give it a name")]
    public List<Sound> sounds;

    private Dictionary<string, AudioClip> soundDictionary;

    private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioClip>();

        foreach (var sound in sounds)
        {
            soundDictionary[sound.id] = sound.clip;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(string id)
    {
        if (soundDictionary.TryGetValue(id, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{id}' not found.");
        }
    }
}