using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

namespace Character.ThirdPersonControl
{
    [RequireComponent(typeof(Camera))]
    public class ThirdPersonCamera : MonoBehaviour
    {
        /// <summary>
        /// 控制第三人称摄像机，需要使用最新版 InputSystem 和 Cinemachine
        /// 记得将用来跟踪的 Cinemachine Virtual Camera 的 Body 修改为 3rd Person Follow 
        /// </summary>
        
        [Header("Cinematic Variables")]
        [Tooltip("相机跟随的目标")] public GameObject target;
        [Tooltip("向上移动的最大角度")] public float topAngleClamp = 70;
        [Tooltip("向下移动的最大角度")] public float bottomAngleClamp = -30;

        private Camera _mainCamera;
        private const float Threshold = 0.1f;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private void Start()
        {
            _mainCamera = Camera.main;
            Debug.Assert(_mainCamera != null, nameof(_mainCamera) + " != null");
            _cinemachineTargetYaw = _mainCamera.transform.eulerAngles.y;
        }

        private void Update()
        {
            if (_look.sqrMagnitude >= Threshold)
            {
                _cinemachineTargetYaw += _look.x;
                _cinemachineTargetPitch += _look.y;
            }
            
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomAngleClamp, topAngleClamp);
            
            target.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360) angle += 360;
            if (angle > 360) angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

        private Vector2 _look;
        /// <summary>
        /// 新版 InputSystem 的回调函数
        /// </summary>
        /// <param name="value"></param>
        public void OnLook(InputValue value)
        {
            _look = value.Get<Vector2>();
        }
    }
}
