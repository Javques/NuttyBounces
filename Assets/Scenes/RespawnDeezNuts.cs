using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnDeezNuts : MonoBehaviour
{
    public int respawnTime = 3;
    public float maxSpeed = 10f;
    public static Vector2 screenBounds;
    
    // Start is called before the first frame update
    void Start()
    {
       
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); 

    }


  
    // Update is called once per frame
    void Update()
    {

        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed) this.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity.normalized * maxSpeed;

        if (this.transform.position.y+1 < screenBounds.y * -1)

            {

                Destroy(this.gameObject);
                
            }


            
        
    }
    

}
