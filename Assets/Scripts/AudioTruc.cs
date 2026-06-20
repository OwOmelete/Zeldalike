using UnityEngine;

public class AudioTruc : MonoBehaviour
{
    public static AudioTruc Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play(string soundName)
    {
        AudioClip clip = System.Array.Find(clips, c => c.name == soundName);
        if (clip != null)
            audioSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"[AudioTruc] Son introuvable : \"{soundName}\"");
    }
}