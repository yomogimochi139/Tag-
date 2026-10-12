using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemView : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private List<Transform> _rootLists = new();
    private int _rootIndex = 0;
    private NavMeshAgent _agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(this._rootLists.Count > 0 && this._agent != null)
        {
            this._rootIndex++;
            this._rootIndex %= this._rootLists.Count;
            this._agent.SetDestination(this._rootLists[this._rootIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_target == null) return;
        if(!this.IsTargetVisible())
        {
            if(this._rootLists.Count > 0 && this._agent != null && this.IsStopped())
            {
                this._rootIndex++;
                this._rootIndex %= this._rootLists.Count;
                this._agent.SetDestination(this._rootLists[this._rootIndex].position);
            }
            return;
        }
        this._agent.SetDestination(_target.position);
    }

    private bool IsStopped()
    {
        if (this._agent == null) return true;
        if(!this._agent.pathPending)
        {
            if(this._agent.remainingDistance <= this._agent.stoppingDistance + 0.5f)
            {
                return true;
            }
        }
        return false;
    }
    private bool IsTargetVisible()
    {
        if(this._target == null) return false;
        Vector3 directionToTarget = this._target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        if(distanceToTarget > this._agent.stoppingDistance)
        {
            RaycastHit hit;
            if(Physics.Raycast(this.transform.position, directionToTarget.normalized, out hit, distanceToTarget))
            {
                if(hit.transform != this._target)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
