using UnityEngine;

    public class CharacterController : MonoBehaviour
    {
        [SerializeField] protected BallController ball;
        public bool IsMoveOnObject;
        public bool BridgeDone;
        public bool isGrounded = true;
        
        public BallController Ball => ball;
    }
