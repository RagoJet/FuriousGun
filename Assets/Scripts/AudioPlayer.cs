using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    public static AudioPlayer Instance;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip bodyShot;
    [SerializeField] private AudioClip headShot;
    [SerializeField] private AudioClip startLevelSound;
    [SerializeField] private AudioClip createdMonsterClip;


    private void Awake(){
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip){
        _audioSource.PlayOneShot(clip);
    }

    public void PlayBodyShot(){
        PlayClip(bodyShot);
    }

    public void PlayHeadShot(){
        PlayClip(headShot);
    }

    public void PlayStartLevel(){
        PlayClip(startLevelSound);
    }

    public void PlayCreateMonsterClip(){
        PlayClip(createdMonsterClip);
    }
}