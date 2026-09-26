using System.Collections;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    public class StateMachine
    {
        private readonly EnemyBase _owner;
        private readonly Blackboard _blackboard;
        private StateBase _currentState;
        private WaitForSeconds _periodicTickWait;

        public StateMachine(EnemyBase owner,Blackboard blackBoard)
        {
            _owner = owner;
            _blackboard = blackBoard;
            _periodicTickWait = new WaitForSeconds(_blackboard.ThinkInterval);
        }

        public void Initialize(StateBase initialState)
        {
            _currentState = initialState;
            _currentState?.Enter();

            _blackboard.IsPeriodicTickActive = true;
            _owner.StartCoroutine(PeriodicTickRoutine());
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

        private IEnumerator PeriodicTickRoutine()
        {
            while(_blackboard.IsPeriodicTickActive)
            {
                _currentState?.OnPeriodicTick();
                yield return _periodicTickWait;
            }

            yield break;
        }
    }
}
