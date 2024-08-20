using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Image muteMusicButton;
    public Sprite muteMusicSprite;
    public Sprite unmuteMusicSprite;

    public Image muteSfxButton;
    public Sprite muteSfxSprite;
    public Sprite unmuteSfxSprite;

    AudioSource audioSource;

    private bool musicMuted;
    private bool sfxMuted;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioMixer.GetFloat("musicVolume", out float musicVol);
        musicMuted = musicVol == -80;
        muteMusicButton.sprite = musicMuted ? unmuteMusicSprite : muteMusicSprite;

        audioMixer.GetFloat("sfxVolume", out float sfxVol);
        sfxMuted = sfxVol == -80;
        muteSfxButton.sprite = sfxMuted ? unmuteSfxSprite : muteSfxSprite;
    }

    public void Play()
    {
        audioSource.Play();
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Exit()
    {
        audioSource.Play();
        Application.Quit();
    }

    public void ToggleMuteMusic()
    {
        audioSource.Play();
        musicMuted = !musicMuted;
        audioMixer.SetFloat("musicVolume", musicMuted ? -80 : 0);
        muteMusicButton.sprite = musicMuted ? unmuteMusicSprite : muteMusicSprite;
    }

    public void ToggleMuteSfx()
    {
        audioSource.Play();
        sfxMuted = !sfxMuted;
        audioMixer.SetFloat("sfxVolume", sfxMuted ? -80 : -12);
        muteSfxButton.sprite = sfxMuted ? unmuteSfxSprite : muteSfxSprite;
    }
}
