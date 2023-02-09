using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform cameraTarget;
    public float sSpeed = 10.0f;
    public Vector3 dist;
    public Vector3 winDist;
    public Vector3 slideDist;
    public Transform lookTarget;
    private PlayerController _player;
    private EnemyController[] _enemy;
    private bool _isLose;
    private GameObject _target;
    private Transform _myTransform;
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectsOfType<EnemyController>();
        cameraTarget = _player.transform;
        lookTarget = _player.camLookAt.transform;
        _target = GameObject.FindWithTag("ScorePos");
        _myTransform = transform;
    }

    private void LateUpdate() 
    {
        if (_player.isSlide)
        {
            var dPos = cameraTarget.position + slideDist;
            var sPos = Vector3.Lerp(_myTransform.position, dPos, sSpeed * Time.deltaTime);
            _myTransform.position = sPos;
            _myTransform.LookAt(lookTarget.position);
        }
        else
        {
            var dPos = cameraTarget.position + dist;
            var sPos = Vector3.Lerp(_myTransform.position, dPos, sSpeed * Time.deltaTime);
            _myTransform.position = sPos;
            _myTransform.LookAt(lookTarget.position);
        }

        switch (GameManager.Instance._state)
        {
            case GameManager.GameStates.Win:
                cameraTarget = _player.Ball.transform;
                lookTarget = _player.Ball.transform;
                dist = winDist;
                break;
            case GameManager.GameStates.Lose:
            {
                if (!_isLose)
                {
                    _isLose = true;
                    sSpeed = sSpeed / 7f;
                    for (int i = 0; i < _enemy.Length; i++)
                    {
                        if (_enemy[i].isDance)
                        {
                            _target = _enemy[i].gameObject;
                        }
                    }
                }
                cameraTarget = _target.transform;
                lookTarget = _target.transform;
                break;
            }
        }
    }
}
