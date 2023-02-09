using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class EditNameBox : MonoBehaviour
{
    public UnityAction moreActionOff;
    private UnityAction _actionHide;
    [SerializeField] private InputField inputName;
    [SerializeField] private Button btDone;
    private PlayerController _player;

    protected void Start()
    {
        inputName.text = GameSave.Cache_UserName;
        btDone.onClick.AddListener(OnClickDone);
        _player = FindObjectOfType<PlayerController>();
        inputName.onEndEdit.AddListener(val =>
        {
            // TouchScreenKeyboard.Status.Done: Keyboard disappeared when something like "Done" button in mobilekeyboard
            // TouchScreenKeyboard.Status.Canceled: Keyboard disappeared when "Back" Hardware Button Pressed in Android
            // TouchScreenKeyboard.Status.LostFocus: Keyboard disappeared when some area except keyboard and input area
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Keyboard is Disappear pressed by Return or Done mobilekeyboard button");
                if (string.IsNullOrEmpty(inputName.text)) GameSave.Cache_UserName = "Player";
                else
                {
                    GameSave.Cache_UserName = inputName.text;
                }    
                
            }
#else
            if (inputName.touchScreenKeyboard.status == TouchScreenKeyboard.Status.Done)
            {
                Debug.Log("Keyboard is Disappear pressed by Return or Done mobilekeyboard button");
                if (string.IsNullOrEmpty(inputName.text)) GameSave.Cache_UserName = "Player";
                else
                {
                    GameSave.Cache_UserName = inputName.text;
                }
            }
#endif
        });
    }

    protected void OnStart()
    {
        Init();
    }

    private void Init()
    {
        //inputName.text = DataManager.NamePlayer;
        //imgCheck.sprite = sprCheck[0];
    }
    
    public void OnValueChangeInputName()
    {
        var str = inputName.text;
        inputName.text = Regex.Replace(str, @"[^0-9a-zA-Z_]+", string.Empty);
        //var imgButtonDone = btDone.GetComponent<Image>();
        bool isValid = !string.IsNullOrEmpty(inputName.text) && str.Length >= 5 && str.Length <= 25;
        //if(imgButtonDone != null) imgButtonDone.material = isValid ? null : mtGray;
        //imgCheck.sprite = isValid ? sprCheck[0] : sprCheck[1];
    }

    private void OnClickDone()
    {
        if (!string.IsNullOrEmpty(inputName.text) && inputName.text.Length >= 1 && inputName.text.Length <= 25)
        {
            Debug.LogError("SendRequest UpdateName");
            var realName = inputName.text;
            GameSave.Cache_UserName = realName;
            _player.name.text = realName;
            gameObject.SetActive(false);
            //GameUtils.RaiseMessage(new HomeMessages.UpdateUserProfile() { DisplayName = realName});
            //UpdatePlayerName(realName);
        }
    }
}
