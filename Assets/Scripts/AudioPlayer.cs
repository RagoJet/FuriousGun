using UnityEngine;

public class AudioPlayer : MonoBehaviour{
    public static AudioPlayer Instance;
    private AudioSource _audioSource;

    private void Awake(){
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip){
        _audioSource.PlayOneShot(clip);
    }
}