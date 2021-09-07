using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnTreesAndRocks : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody2D rb;
    public static string tagWalls = "Walls";
    public static string tagMain = "Main";
    public static string tagDN = "DeezNuts";
    public static Vector2 screenBounds;
    public Animator rock;
    public Animator tree;
    

    // Start is called before the first frame update
    void Start()
    {
        
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        rb = this.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = new Vector2(0, -speed);

        GameObject UpperWall = GameObject.Find("UpWall");
        Physics2D.IgnoreCollision(UpperWall.GetComponent<BoxCollider2D>(), this.GetComponent<CircleCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y+1 < screenBounds.y * -1)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tagMain)
        {
            Destroy(collision.gameObject);
            //endgame
            Destroy(this.gameObject);
            GameObject isItDestroyed = GameObject.FindGameObjectWithTag(tagDN);
            if (isItDestroyed) Destroy(isItDestroyed);
            ScoreManager.getHighScore(BallMovement.getScore());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        }
        if(collision.gameObject.tag == tagDN)
        {
            
            
            GameObject Main = GameObject.Find("Balls");
            Physics2D.IgnoreCollision(Main.GetComponent<EdgeCollider2D>(), this.GetComponent<CircleCollider2D>());
            if (this.tag == "Rock")
            {
                rock.SetBool("isDead", true);
                Destroy(this.gameObject,0.8f);
            }
            if (this.tag == "Tree")
            {
                tree.SetBool("isDead", true);
                
                Destroy(this.gameObject, 0.8f);
            }
            BallMovement.nutted();
        }

    }

   
}
