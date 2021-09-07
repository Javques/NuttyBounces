using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployObstacles : MonoBehaviour
{

    public GameObject tree;
    public GameObject rock;
    public  GameObject deezNuts;
    public static string tagDN = "DeezNuts";
    public float respawnTime = 0.7f;
    public float rockChance = 0.6f;
    public float treeChance = 0.4f;
    public static Vector2 screenBounds;
    public static int nutCounter;
    static bool timeStarted;
    static float dnDeathTimer = 5f; 
    float timeRespawn;
    // Start is called before the first frame update
    void Start()
    {
        nutCounter = 1;
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(ObstacleWave());

        

    }
    private void spawnObstacle()
    {
        float chance = Random.value;

        
        if (chance > treeChance)
        {
            GameObject b = Instantiate(rock) as GameObject;
            b.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 2);
        }
        else if (chance < treeChance)
        {

            GameObject a = Instantiate(tree) as GameObject;

            a.transform.position = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y * 2);
        }
       

    }

    public  void spawnBack()
    {
        
        GameObject dn = Instantiate(deezNuts) as GameObject;
        dn.transform.position = new Vector2(Random.Range(-screenBounds.x+1, screenBounds.x-1), screenBounds.y - 1);
        dn.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -0);
        dn.GetComponent<Rigidbody2D>().angularVelocity = 0;
        timeStarted = false;
        BallMovement.newNut();

    }
    private void Update()
    {
        if (timeStarted)
        {
            timeRespawn += Time.deltaTime;
        }
        
        if (timeRespawn>dnDeathTimer)
        {
            spawnBack();
            timeRespawn = 0;
            timeStarted = false;
        }
        GameObject isItDestroyed = GameObject.FindGameObjectWithTag(tagDN);
        timeStarted = (isItDestroyed == null);
         
    }

    IEnumerator  ObstacleWave()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(respawnTime);
            spawnObstacle();
        }
    }

   

}
