using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : BaseObject
{
    [SerializeField] private Target target;
    [SerializeField] private float v;
    [SerializeField] private UnityAction reachAction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            float canMoveDistance = v * Time.deltaTime;
            if (distance < canMoveDistance)
            {
                transform.position = target.transform.position;
                if (reachAction != null)
                {
                    reachAction();
                    reachAction = null;
                    Destroy(gameObject);
                }
            }
            else
            {
                float x = canMoveDistance / distance;
                transform.position = transform.position + (target.transform.position - transform.position) * x;
            }
        }    
    }
    public void shoot(Target t, UnityAction reach)
    {
        target = t;
        reachAction = reach;
    }
}
