using System.Collections;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class StateMachine
    {
        private readonly EnemyBase _owner;
        private readonly BlackBoard _blackBoard;
        private StateBase _currentState;
        private WaitForSeconds _thinkIntetrvalWait;

        public StateMachine(EnemyBase owner,BlackBoard blackBoard)
        {
            _owner = owner;
            _blackBoard = blackBoard;
            _thinkIntetrvalWait = new WaitForSeconds(_blackBoard.ThinkInterval);
        }

        public void Initialize(StateBase initialState)
        {
            _currentState = initialState;
            _currentState?.Enter();

            _blackBoard.IsThinking = true;
            _owner.StartCoroutine(ThinkFlow());
        }

        public void ChangeState(StateBase newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public void Tick()
        {
            _currentState?.OnTick();
        }

        private IEnumerator ThinkFlow()
        {
            while(_blackBoard.IsThinking)
            {
                _currentState?.Think();
                yield return _thinkIntetrvalWait;
            }

            yield break;
        }
    }
}
