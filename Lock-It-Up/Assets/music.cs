using UnityEngine;

public class music : MonoBehaviour
{
    public static AudioClip startSound, deathSound, winSound;
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
        audioSrc = GetComponent<AudioSource>();
    }
    public static void PlaySound(string clip){
        switch(clip){
            case "startSound":
                audioSrc.PlayOneShot(startSound);
                break;
        }
    }
}