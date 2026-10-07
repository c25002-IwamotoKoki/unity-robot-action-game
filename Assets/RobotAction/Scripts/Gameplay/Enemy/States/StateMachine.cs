namespace RobotAction.Gameplay.Enemy
{
    public class StateMachine
    {
        private readonly EnemyBase _owner;
        private readonly Blackboard _blackboard;
        private StateBase _currentState;
        private float _periodicTickTimer;

        public StateMachine(EnemyBase owner,Blackboard blackBoard)
        {
            _owner = owner;
            _blackboard = blackBoard;
        }

        public void Initialize(StateBase initialState)
        {
            _currentState = initialState;
            _currentState?.Enter();
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

        public void PeriodicTick(float deltaTime)
        {
            _periodicTickTimer += deltaTime;

            if(_periodicTickTimer >= _blackboard.PeriodicTickInterval)
            {
                _periodicTickTimer = 0;
                _currentState?.OnPeriodicTick();
            }
        }
    }
}
