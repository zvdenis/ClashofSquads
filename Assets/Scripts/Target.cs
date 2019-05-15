using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    
    
    private NavMeshAgent[] navAgents;
    public Transform targetMarker;

    private void Start ()
    {
      navAgents = FindObjectsOfType(typeof(NavMeshAgent)) as NavMeshAgent[];
      
    }

    private void UpdateTargets ( Vector3 targetPosition )
    {
        if(navAgents == null) return;
            
            
        Transform CurrentRedTarget = GameObject.FindGameObjectWithTag("CurrentRedTarget").transform;
        Transform CurrentBlueTarget = GameObject.FindGameObjectWithTag("CurrentBlueTarget").transform;

            
            
      foreach(NavMeshAgent agent in navAgents)
      {

          if (agent != null && agent.isOnNavMesh)
          {
              if (agent.tag.Contains("Blue"))
              {
                  agent.destination = CurrentBlueTarget.position;
              }
              if (agent.tag.Contains("Red"))
              {
                  agent.destination = CurrentRedTarget.position;
              }
              
          }
      }
    }

    private void Update ()
    {
        if(GetInput()) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray.origin, ray.direction, out hitInfo)) 
            {
                Vector3 targetPosition = hitInfo.point;
                UpdateTargets(targetPosition);
                //if(targetPosition != null)
                //targetMarker.position = targetPosition;
                
            }
        }
    }

    private bool GetInput() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            return true;
        }
        return false;
    }
}