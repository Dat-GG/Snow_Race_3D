using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UISettingController : MonoBehaviour
{
    [SerializeField]
    Toggle tog_Music, tog_Sound;
    [SerializeField] private Button changeNameBtn;
    [SerializeField] private Button homeBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private GameObject nameBox;
    private void Awake()
    {
        tog_Music.onValueChanged.AddListener(OnMusicChangeEvent);
        tog_Sound.onValueChanged.AddListener(OnSoundChangeEvent);
        changeNameBtn.onClick.AddListener(OnClickChangeNameBtn);
        homeBtn.onClick.AddListener(OnClickHomeBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    private void OnMusicChangeEvent(bool arg0)
    {
        MusicManager.Instance.MusicVolume = arg0 ? 1 : 0;
    }
    private void OnSoundChangeEvent(bool arg0)
    {
        MusicManager.Instance.SoundVolume = arg0 ? 1 : 0;
    }

    private void OnEnable()
    {
        tog_Music.isOn = MusicManager.Instance.MusicVolume > 0;
        tog_Sound.isOn = MusicManager.Instance.SoundVolume > 0;
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    private void OnClickExitBtn()
    {
        gameObject.SetActive(false);
    }

    private void OnClickChangeNameBtn()
    {
        gameObject.SetActive(false);
        nameBox.SetActive(true);
    }

    private void OnClickHomeBtn()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
