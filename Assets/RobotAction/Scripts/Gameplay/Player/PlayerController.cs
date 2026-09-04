using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour
    {
        private const float BoostDeadZoneSqr = 0.01f;

        private PlayerInputReader _inputReader;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _boostSpeed;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
        }

        private void OnEnable()
        {
            _inputReader.OnBoost += Boost;
        }

        private void FixedUpdate()
        {
            Vector2 input = _inputReader.MoveDirection;

            if (input.x != 0)
            {
                _rigidbody.AddForce(input.x * _moveSpeed * transform.right,
                                    ForceMode.Force);
            }

            if (input.y != 0)
            {
                _rigidbody.AddForce(input.y * _moveSpeed * transform.forward,
                                    ForceMode.Force);
            }
        }

        private void OnDisable()
        {
            _inputReader.OnBoost -= Boost;
            _inputReader.Dispose();
        }

        private void Boost()
        {
            Vector2 input = _inputReader.MoveDirection;
            input = input.sqrMagnitude > BoostDeadZoneSqr ? input.normalized : input;

            _rigidbody.AddForce(_boostSpeed * new Vector3(input.x, 0, input.y),
                                ForceMode.Impulse);
        }
    }
}
