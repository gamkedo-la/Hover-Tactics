using UnityEngine;

[RequireComponent(typeof(EnemyMechController))]
public class EnemyFollowWayPoints : MonoBehaviour
{

    public WayPoints wayPoints;
    public float minDistanceToTarget = 1f;
    private int currentWayPointIndex = 0;
    private EnemyMechController enemyMechController;

    private void Start()
    {
        enemyMechController = GetComponent<EnemyMechController>();
    }

    private void Update()
    {
        if(enemyMechController.IsNearTarget())
        {
            currentWayPointIndex = wayPoints.GetNextIndex(currentWayPointIndex);
            enemyMechController.SetTarget(wayPoints.GetWayPointAt(currentWayPointIndex));
        }
    }

}
