using UnityEngine;

[RequireComponent(typeof(EnemyMechController))]
public class EnemyFollowWayPoints : MonoBehaviour
{
    public string targetTag = "Player";
    public float targetDetectDistance = 30.0f;
    [Space]
    public WayPoints wayPoints;
    public float minDistanceToTarget = 1f;

    private int currentWayPointIndex = 0;
    private EnemyMechController enemyMechController;
    private GameObject[] targetObjects;
    private int targetStatus = -1;

    private void Start()
    {
        enemyMechController = GetComponent<EnemyMechController>();
        targetObjects = GameObject.FindGameObjectsWithTag(targetTag);
    }

    private void Update()
    {
        bool gotTarget = false;
        for(int i = 0; i < targetObjects.Length; i++)
        {
            if(Vector3.Distance(
                targetObjects[i].transform.position,
                wayPoints.GetWayPointAt(currentWayPointIndex).position)
            <= targetDetectDistance)
            {
                enemyMechController.SetTarget(targetObjects[i].transform);
                targetStatus = 1;
                gotTarget = true;
                break;
            }
        }
        if(targetStatus == 1 && !gotTarget) targetStatus = -1;

        if(targetStatus != 1)
        {
            if(targetStatus == -1)
            {
                enemyMechController.SetTarget(wayPoints.GetWayPointAt(currentWayPointIndex));
                targetStatus = 0;
            }
            else if(enemyMechController.IsNearTarget())
            {
                currentWayPointIndex = wayPoints.GetNextIndex(currentWayPointIndex);
                enemyMechController.SetTarget(wayPoints.GetWayPointAt(currentWayPointIndex));
            }
        }
    }

}
