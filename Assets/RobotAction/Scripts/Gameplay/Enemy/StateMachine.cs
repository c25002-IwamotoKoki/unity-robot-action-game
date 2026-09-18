using System.Collections;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class StateMachine
    {
        private readonly EnemyBase _owner;
        private readonly Blackboard _blackboard;
        private StateBase _currentState;
        private WaitForSeconds _thinkIntetrvalWait;

        public StateMachine(EnemyBase owner,Blackboard blackBoard)
        {
            _owner = owner;
            _blackboard = blackBoard;
            _thinkIntetrvalWait = new WaitForSeconds(_blackboard.ThinkInterval);
        }

        public void Initialize(StateBase initialState)
        {
            _currentState = initialState;
            _currentState?.Enter();

            _blackboard.IsThinking = true;
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
            while(_blackboard.IsThinking)
            {
                _currentState?.Think();
                yield return _thinkIntetrvalWait;
            }

            yield break;
        }
    }
}
