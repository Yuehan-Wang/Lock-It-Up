using UnityEngine;

public class music : MonoBehaviour
{
    public static AudioClip startSound, loseSound, winSound;
    static AudioSource audioSrc;
    public static music instance;

    private void Awake(){
        DontDestroyOnLoad(this.gameObject);
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    void Start(){
        startSound = Resources.Load<AudioClip>("startSound");
        loseSound = Resources.Load<AudioClip>("loseSound");
        winSound = Resources.Load<AudioClip>("winSound");
        audioSrc = GetComponent<AudioSource>();
    }
    public static void PlaySound(string clip){
        switch(clip){
            case "startSound":
                audioSrc.PlayOneShot(startSound);
                break;
            case "loseSound":
                audioSrc.PlayOneShot(loseSound);
                break;
            case "winSound":
                audioSrc.PlayOneShot(winSound);
                break;
        }
    }
}