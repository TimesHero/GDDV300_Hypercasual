using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource PlayerAS;
    
    public AudioClip frogTongue;
    public AudioClip frogEat;
    public AudioClip frogMiss;
    public AudioClip frogPoisoned;
    
    public static AudioManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string clipName)
    {
        if (clipName == "frogTongue") PlayerAS.PlayOneShot(frogTongue);
        else if (clipName == "frogEat") PlayerAS.PlayOneShot(frogEat);
        else if (clipName == "frogMiss") PlayerAS.PlayOneShot(frogMiss);
        else if (clipName == "frogPoisoned") PlayerAS.PlayOneShot(frogPoisoned);
        else return;
    }
}
