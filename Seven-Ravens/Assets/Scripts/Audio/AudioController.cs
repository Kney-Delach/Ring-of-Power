using System.Collections;
using UnityEngine;

// controls audio playback for a single sound
public class AudioController : MonoBehaviour
{
    // reference the sfx clip to play 
    [SerializeField]
    private string _soundName;

    // refernece to the audio manager
    private AudioManager _audioManager;

    [SerializeField]
    private bool _isSceneMusic = false;

    private void Awake()
    {
        _audioManager = Object.FindObjectOfType<AudioManager>();       
    }
    
    private void Start()
    {
        if(_isSceneMusic)
            PlaySceneMusic();
    }
    // play single shot sfx
    public void PlaySfx()
    {
        if (_audioManager == null)
            return;
        _audioManager.PlaySound(_soundName);    
    }

    public void StopSound()
    {
        if(_audioManager == null)
            return; 
        _audioManager.StopSound(_soundName);
    }

    private void PlaySceneMusic()
    {
        if(_audioManager == null)
            return; 
        _audioManager.SwitchScene(_soundName);
    }
}
