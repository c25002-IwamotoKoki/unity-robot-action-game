namespace RobotAction.Gameplay.Enemy
{
    public interface IEnemyAttack
    {
        public void Attack(in EnemyAttackContext context);
    }
}