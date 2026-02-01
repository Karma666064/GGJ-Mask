using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public enum CodeOST
    {
        menu,
    }

    public enum CodeSFX
    {
        draw,
        hurt,
        shield,
        heal
    }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(CodeOST.menu);
    }

    [SerializeField] private AudioSource[] sources;
    [SerializeField] private AudioClip[] clipsOST;
    [SerializeField] private AudioClip[] clipsSFX;

    public void PlayMusic(CodeOST _code)
    {
        sources[0].clip = clipsOST[(int)_code];
        sources[0].Play();
    }

    public void PlaySFX(CodeSFX _code)
    {
        sources[0].PlayOneShot(clipsSFX[(int)_code]);
    }
}
