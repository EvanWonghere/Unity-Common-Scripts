using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.ThirdPersonControl
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        /// <summary>
        /// 控制第三人称玩家，需要使用最新版 InputSystem 
        /// </summary>
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private float rotationTime = 0.1f;
        [SerializeField] private float gravity = -9.81f;
        // [SerializeField] private float jumpForce = 100f;
        // [SerializeField] private float jumpHeight = 3f;
        // [SerializeField] private LayerMask groundLayer;
        // [SerializeField] private LayerMask groundMask;
        
        private CharacterController _characterController;
        private Camera _mainCamera;
        private float _targetRotation;

        private void Start()
        {
            _mainCamera = Camera.main;
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            // 模拟重力
            var velocity = new Vector3(0, gravity, 0);
            if (_move != Vector2.zero)
            {
                // 计算输入方向
                var inputDir = new Vector3(_move.x, 0, _move.y).normalized;
                // 计算目标旋转角度
                _targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg 
                                  + _mainCamera.transform.eulerAngles.y;
                // 计算平滑旋转
                var smoothedRotation = Mathf.SmoothDampAngle(
                    transform.eulerAngles.y, _targetRotation, ref rotationSpeed, rotationTime
                );
                // 旋转玩家
                transform.rotation = Quaternion.Euler(0f, smoothedRotation, 0f);
                
                // 计算移动方向
                var moveDir = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
                velocity += moveDir.normalized * (moveSpeed * Time.deltaTime);
            }
            
            // 移动玩家
            _characterController.Move(velocity);
        }

        private Vector2 _move;
        /// <summary>
        /// 新版 Input System 的回调函数
        /// </summary>
        /// <param name="value"></param>
        public void OnMove(InputValue value)
        {
            _move = value.Get<Vector2>();
        } 
    }
}
