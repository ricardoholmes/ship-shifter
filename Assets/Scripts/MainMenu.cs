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

    private bool musicMuted;
    private bool sfxMuted;

    private void Awake()
    {
        audioMixer.GetFloat("musicVolume", out float musicVol);
        musicMuted = musicVol == -80;
        muteMusicButton.sprite = musicMuted ? unmuteMusicSprite : muteMusicSprite;

        audioMixer.GetFloat("sfxVolume", out float sfxVol);
        sfxMuted = sfxVol == -80;
        muteSfxButton.sprite = sfxMuted ? unmuteSfxSprite : muteSfxSprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
    }

    public void Exit()
    {
        GetComponent<AudioSource>().Play();
        Application.Quit();
    }

    public void ToggleMuteMusic()
    {
        musicMuted = !musicMuted;
        audioMixer.SetFloat("musicVolume", musicMuted ? -80 : 0);
        muteMusicButton.sprite = musicMuted ? unmuteMusicSprite : muteMusicSprite;
    }

    public void ToggleMuteSfx()
    {
        sfxMuted = !sfxMuted;
        audioMixer.SetFloat("sfxVolume", sfxMuted ? -80 : -12);
        muteSfxButton.sprite = sfxMuted ? unmuteSfxSprite : muteSfxSprite;
    }
}
