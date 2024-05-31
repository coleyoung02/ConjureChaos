using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] private Toggle fullscreenToggle;

    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_SFX = "SFXVolume";

    private int maxWidth;
    private int maxHeight;
    private int[,] validResolutions;
    private int currentRes;
    private bool fullscreen;
    void Awake()
    {
        maxHeight = Display.main.systemHeight;
        maxWidth = Display.main.systemWidth;
        validResolutions = new int[7, 2] { { 1024, 576 }, { 1280, 720 }, { 1408, 792 }, { 1664, 936 }, { 1920, 1080 }, { 2560, 1440 }, { 3840, 2160 } };
        int usedIndex = -1;
        fullscreen = PlayerPrefs.GetInt("Fullscreen", -1) != 0;

        if (PlayerPrefs.GetInt("Res", -1) <= -1)
        {
            for (int i = 0; i < validResolutions.GetLength(0); ++i)
            {
                if (validResolutions[i, 0] <= maxWidth && validResolutions[i, 1] <= maxHeight)
                {
                    usedIndex = i;
                    if (validResolutions[i, 0] == maxWidth && validResolutions[i, 1] == maxHeight)
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

    public void BumpResolution(bool up)
    {
        Debug.Log(currentRes + 1);
        Debug.Log(validResolutions.GetLength(0));
        if (up && currentRes + 1 < validResolutions.GetLength(0) &&
            validResolutions[currentRes + 1, 0] <= maxWidth && validResolutions[currentRes + 1, 1] <= maxHeight)
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

    public void SaveResolution()
    {
        if (currentRes >= 0)
        {
            Screen.SetResolution(validResolutions[currentRes, 0], validResolutions[currentRes, 1], fullscreen);
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
                resText.text = validResolutions[currentRes, 0] + "x" + validResolutions[currentRes, 1];
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
        Debug.Log("mixer set to " + Mathf.Log10(musicSlider.value) * 20);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(AudioManager.SFX_KEY, SFXSlider.value);
    }
}
