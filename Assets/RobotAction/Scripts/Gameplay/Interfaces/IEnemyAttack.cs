using RobotAction.Gameplay.Enemy;

namespace RobotAction.Gameplay.Interfaces
{
    public interface IEnemyAttack
    {
        public void Attack(in EnemyAttackContext context);
    }
}