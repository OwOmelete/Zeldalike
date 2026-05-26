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

    [Tooltip("Add sounds here")]
    [SerializeField]
    private List<Sound> sounds;

    private Dictionary<string, AudioClip> soundDictionary;

    private AudioSource sfxSource;

    [SerializeField]
    [Range(0f, 1f)]
    private float sfxVolume = 1f;

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
            if (string.IsNullOrWhiteSpace(sound.id))
            {
                Debug.LogWarning("Empty sound ID.");
                continue;
            }

            if (sound.clip == null)
            {
                Debug.LogWarning($"Missing clip for {sound.id}");
                continue;
            }

            if (soundDictionary.ContainsKey(sound.id))
            {
                Debug.LogWarning($"Duplicate sound ID: {sound.id}");
                continue;
            }

            soundDictionary.Add(sound.id, sound.clip);
        }

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void Play(string id)
    {
        if (soundDictionary.TryGetValue(id, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
        else
        {
            Debug.LogWarning($"Sound '{id}' not found.");
        }
    }
}