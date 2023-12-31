 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    static AudioManager current;
    [Header("��������")]
    public AudioClip ambientclip;
    public AudioClip musicclip;
    [Header("FX��Ч")]
    public AudioClip deathFXClip;
    public AudioClip orbFXClip;
    public AudioClip doorFXClip;
    public AudioClip winClip;
    public AudioClip startLevelClip;
    [Header("Robbie")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip jumpVoiceClip;
    public AudioClip deathClip;
    public AudioClip deathVoiceClip;
    public AudioClip orbVoiceClip;

    public AudioMixerGroup ambientGroup, musicGrounp, FXGroup, playerGroup, voiceGroup;
        
        private AudioSource ambientSource;
        private AudioSource musicSource;
        private AudioSource fxSource;
        private AudioSource playerSource;
        private AudioSource voiceSource;
    private void Awake()
    {
        if (current != null) { Destroy(gameObject); }
        else current = this;
        DontDestroyOnLoad(gameObject); 

        ambientSource=gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        musicSource.outputAudioMixerGroup = musicGrounp;
        fxSource.outputAudioMixerGroup = FXGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        StartLevelAudio();
    }
    public static void PlayDoorOpenAudio()
    {
        current.fxSource.clip = current.doorFXClip;
        current.fxSource.PlayDelayed(1f);
    }

    void StartLevelAudio()
    {
        current.ambientSource.clip = current.ambientclip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        current.musicSource.clip = current.musicclip;
        current.musicSource.loop = true;
        current.musicSource.Play();

        current.fxSource.clip = current.startLevelClip;
        current.fxSource.Play();

    }
    public static void PlayerWonAudio()
    {
        current.fxSource.clip = current.winClip;
        current.fxSource.Play();
        current.playerSource.Stop();
    }
    public static void PlayFootstepAudio()
    {
        int index=Random.Range(0,current.walkStepClips.Length);
        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayCrouchFootstepAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);
        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }
    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();
        current.voiceSource.clip = current.jumpVoiceClip;
        current.voiceSource.Play();
    }
    public static void PlayDeathAudio()
    {
        current.playerSource.clip = current.deathClip;
        current.playerSource.Play();
        current.voiceSource.clip = current.deathVoiceClip;
        current.voiceSource.Play();
        current.fxSource.clip = current.deathFXClip;
        current.fxSource.Play();
    }
    public static void PlayOrbAudio()
    {
        current.fxSource.clip = current.orbFXClip;
        current.fxSource.Play(); 
        current.voiceSource.clip = current.orbVoiceClip;
        current.voiceSource.Play();
    }
}
