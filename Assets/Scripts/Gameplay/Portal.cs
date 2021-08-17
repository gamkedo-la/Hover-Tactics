using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private float teleportPauseDelay = 0.05f;
    private float teleportPauseTimer = 0.0f;
    static List<Portal> portals = null;

    private GameObject portalUser = null;

    static public void ClearPortals()
    {
        portals.Clear();
        portals = null;
    }

    void Start()
    {
        if(portals == null) portals = new List<Portal>();
        portals.Add(this);
    }

    void Update()
    {
        if(teleportPauseTimer > 0.0f)
            teleportPauseTimer -= Time.deltaTime;
        else
            teleportPauseTimer = 0.0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if(portalUser == null && teleportPauseTimer <= 0.0f)
        {
            portalUser = other.gameObject;

            int jumpToPortal = UnityEngine.Random.Range(0, portals.Count);
            if(portals[jumpToPortal] == this)
            {
                if(jumpToPortal >= 1) jumpToPortal--;
                else if(jumpToPortal + 1 < portals.Count) jumpToPortal++;
            }
            
            Vector3 position = other.transform.position;
            position.x = portals[jumpToPortal].transform.position.x;
            position.z = portals[jumpToPortal].transform.position.z;
            other.transform.position = position;

            portals[jumpToPortal].teleportPauseTimer = teleportPauseDelay;
            portals[jumpToPortal].portalUser = portalUser;

            teleportPauseTimer = teleportPauseDelay;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject == portalUser)
        {
            portalUser = null;
        }
    }
}
