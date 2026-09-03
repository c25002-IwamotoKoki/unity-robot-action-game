using UnityEngine;

namespace RobotAction.GamePlay.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _boostSpeed;

        private void Update()
        {
            float axisX = Input.GetAxisRaw("Horizontal");//TODO:InputSystem‚É‘Î‰ž‚³‚¹‚é
            float axisZ = Input.GetAxisRaw("Vertical");//TODO:InputSystem‚É‘Î‰ž‚³‚¹‚é

            if (axisX != 0)
            {
                _rigidbody.AddForce(axisX * _moveSpeed * transform.right,
                                    ForceMode.Force);
            }

            if(axisZ != 0)
            {
                _rigidbody.AddForce(axisZ * _moveSpeed * transform.forward,
                                    ForceMode.Force);
            }

            if(Input.GetKeyDown(KeyCode.LeftShift))//TODO:InputSystem‚É‘Î‰ž‚³‚¹‚é
            {
                _rigidbody.AddForce(_boostSpeed * new Vector3(axisX, 0, axisZ),
                                    ForceMode.Impulse);
            }
        }
    }
}
