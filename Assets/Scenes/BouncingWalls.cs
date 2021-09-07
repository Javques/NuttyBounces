using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingWalls : MonoBehaviour

{
    List<Collision2D> currentCol = new List<Collision2D>();
   
    // Start is called before the first frame update
    void Start()
    {

        this.GetComponent<BoxCollider2D>().sharedMaterial = new PhysicsMaterial2D()
        {
            bounciness = 1f,
            friction = 0.6f,
        };
    }

    // Update is called once per frame
    void Update()
    {

      
        //foreach(Collision2D colli in currentCol)
        //{
        //    GameObject obj = colli.gameObject;
        //    Vector3 lastVelocity = obj.GetComponent<Rigidbody2D>().velocity;
        //    var speed = lastVelocity.magnitude;
        //    var direction = Vector3.Reflect(lastVelocity.normalized, Vector3.up);
        //    obj.GetComponent<Rigidbody2D>().velocity = direction * Mathf.Max(speed, 0f);

        //}
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentCol.Add(collision);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        currentCol.Remove(collision);
    }
}
