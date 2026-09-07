using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        //Check for a match with the specified name on any GameObject that collides with your GameObject
        if (collision.transform.parent && collision.transform.parent.GetComponent<Target>())
        {
            collision.transform.parent.GetComponent<Target>().onHit(transform.position.x > collision.transform.position.x);
        }
    }
}
