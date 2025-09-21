using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle screenShakeToggle;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private int maxWidth;
    private int maxHeight;
    private List<(int, int)> validResolutions;
    private int currentRes;
    private bool fullscreen;
    private bool screenShake;

    void Awake()
    {
        maxHeight = Display.main.systemHeight;
        maxWidth = Display.main.systemWidth;
        Resolution[] resolutions = Screen.resolutions;
        validResolutions = new List<(int, int)> {(1280, 720), (1920, 1080), (2560, 1440), (3840, 2160) };
        foreach (Resolution r in resolutions)
        {
            if (!validResolutions.Contains((r.width, r.height))) 
            {
                validResolutions.Add((r.width, r.height));
            }
        }
        validResolutions = validResolutions.OrderBy(x => (x.Item1, x.Item2)).ToList();


        int usedIndex = -1;
        fullscreen = PlayerPrefs.GetInt("Fullscreen", -1) != 0;
        screenShake = PlayerPrefs.GetInt("ScreenShake", -1) != 0;

        if (PlayerPrefs.GetInt("Res", -1) <= -1)
        {
            for (int i = 0; i < validResolutions.Count; ++i)
            {
                if (validResolutions[i].Item1 <= maxWidth && validResolutions[i].Item2 <= maxHeight)
                {
                    usedIndex = i;
                    if (validResolutions[i].Item1 == maxWidth && validResolutions[i].Item2 == maxHeight)
                    {
                        fullscreen = true;
                        PlayerPrefs.SetInt("Fullscreen", 1);
                    }
                }
            }
            currentRes = usedIndex;
            setResText();
        }
        else
        {
            currentRes = PlayerPrefs.GetInt("Res");
            SaveResolution();
            setResText();
        }
        fullscreenToggle.isOn = fullscreen;
        screenShakeToggle.isOn = screenShake;


        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f)) * 20);
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f)) * 20);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        SFXSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f);

    }

    private void Start()
    {

        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f)) * 20);
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f)) * 20);
    }
    public bool UseScreenShake()
    {
        return screenShake;
    }

    public void BumpResolution(bool up)
    {
        if (up && currentRes + 1 < validResolutions.Count &&
            validResolutions[currentRes + 1].Item1 <= maxWidth && validResolutions[currentRes + 1].Item2 <= maxHeight)
        {
            currentRes += 1;
        }
        else if (!up && currentRes != 0)
        {
            currentRes -= 1;
        }
        setResText();
        PlayerPrefs.SetInt("Res", currentRes);
        SaveResolution();
    }

    public void SetFullscreen(bool f)
    {
        fullscreen = f;
        SaveResolution();
    }

    public void SetScreenShake(bool f)
    {
        screenShake = f;
        PlayerPrefs.SetInt("ScreenShake", f ? 1 : 0);
    }

    public void SaveResolution()
    {
        if (currentRes >= 0)
        {
            Screen.SetResolution(validResolutions[currentRes].Item1, validResolutions[currentRes].Item2, fullscreen);
        }
        if (fullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    private void setResText()
    {
        if (currentRes >= 0)
        {
            if (resText != null)
            {
                resText.text = validResolutions[currentRes].Item1 + "x" + validResolutions[currentRes].Item2;
            }
        }
        else
        {
            resText.text = Screen.width + "x" + Screen.height;
        }
    }



    void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
    }
}
