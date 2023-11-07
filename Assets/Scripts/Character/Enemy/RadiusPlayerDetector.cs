using UnityEngine;

[RequireComponent(typeof(Enemy))]
class RadiusPlayerDetector : MonoBehaviour, IPlayerDetector
{
    Enemy enemy;
    [SerializeField] float moveTowardsThreshold;
    [SerializeField] float noticeThreshold;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        if (enemy.DistanceFromPlayer > noticeThreshold)
            enemy.ChangeMode(Enemy.ModeId.Idle);
        else if (enemy.DistanceFromPlayer > moveTowardsThreshold)
            enemy.ChangeMode(Enemy.ModeId.InRange);
        else
            enemy.ChangeMode(Enemy.ModeId.Close);
    }
}