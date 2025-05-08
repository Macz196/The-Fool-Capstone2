using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class LaunchProjectile : MonoBehaviour
{
   public GameObject projectile;
   public float launchVelocity = 700f;
 
   void Update()
   {
       if (Input.GetButtonDown("Fire1"))
       {
           GameObject ball = Instantiate(projectile, transform.position,  
                                                     transform.rotation);
           ball.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3 
                                                (0, launchVelocity,0));
       }
   }
}
