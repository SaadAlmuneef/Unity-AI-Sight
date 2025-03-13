using System;
using System.Collections.Generic;
using UnityEngine;

namespace AIPerception
{
     public class AISightSenseComponent : MonoBehaviour
{
    public AISightConfig sightConfig;
    public event Action<Transform> OnTargetSeen;
    public event Action<Transform> OnTargetLost;

    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();
    Dictionary<Transform, float> lostTargets = new Dictionary<Transform, float>();
    
    void Update()
    {
        RunAISight();
    }

    void RunAISight()
    {
        DetectTargets();
        UpdateLostTargets();
    }
    void DetectTargets()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, sightConfig.viewRadius, sightConfig.targetLayer);
        HashSet<Transform> currentlyVisible = new HashSet<Transform>();
        
        foreach (Collider col in targetsInViewRadius)
        {
            Transform target = col.transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            
            if (Vector3.Angle(transform.forward, dirToTarget) < sightConfig.viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, sightConfig.obstructionLayer))
                {
                    currentlyVisible.Add(target);
                    if (!visibleTargets.Contains(target))
                    {
                        visibleTargets.Add(target);
                        OnTargetSeen?.Invoke(target);
                        lostTargets.Remove(target); // Remove from lost targets if seen again
                    }
                }
            }
        }
        
        // Handle targets that are no longer visible
        for (int i = visibleTargets.Count - 1; i >= 0; i--)
        {
            Transform target = visibleTargets[i];
            if (!currentlyVisible.Contains(target))
            {
                visibleTargets.Remove(target);
                lostTargets[target] = Time.time + sightConfig.AgingTime;
                OnTargetLost?.Invoke(target);
            }
        }
    }
    
    void UpdateLostTargets()
    {
        List<Transform> toRemove = new List<Transform>();
        foreach (var key in lostTargets)
        {
            if (Time.time > key.Value)
            {
                toRemove.Add(key.Key);
            }
        }
        
        foreach (Transform target in toRemove)
        {
            lostTargets.Remove(target);
        }
    }
    
    public List<Transform> GetVisibleTargets()
    {
        return visibleTargets;
    }
    
    public List<Transform> GetLostTargets()
    {
        return new List<Transform>(lostTargets.Keys);
    }
    
    void OnDrawGizmos()
    {
        if (sightConfig == null) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightConfig.viewRadius);
        
        // the left edge 
        Vector3 leftBoundary = Quaternion.Euler(0, -sightConfig.viewAngle / 2, 0) * transform.forward;
        // the right edge
        Vector3 rightBoundary = Quaternion.Euler(0, sightConfig.viewAngle / 2, 0) * transform.forward;
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * sightConfig.viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * sightConfig.viewRadius);
        
        Gizmos.color = Color.green;
        foreach (Transform target in visibleTargets)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
        
        Gizmos.color = Color.red;
        foreach (Transform target in lostTargets.Keys)
        {
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}

}

