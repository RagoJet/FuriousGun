using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    public static AudioPlayer Instance;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip bodyShot;
    [SerializeField] private AudioClip headShot;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip createdMonsterClip;
    [SerializeField] private AudioClip levelStartClip;
    [SerializeField] private AudioClip doorSound;


    private void Awake(){
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip){
        _audioSource.PlayOneShot(clip);
    }

    public void PlayDoorSound(){
        PlayClip(doorSound);
    }

    public void PlayStartLevelSound(){
        PlayClip(levelStartClip);
    }

    public void PlayBodyShot(){
        PlayClip(bodyShot);
    }

    public void PlayHeadShot(){
        PlayClip(headShot);
    }

    public void PlayGameOverSound(){
        PlayClip(gameOverSound);
    }

    public void PlayCreateMonsterClip(){
        PlayClip(createdMonsterClip);
    }
}