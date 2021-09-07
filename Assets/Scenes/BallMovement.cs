using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public static int globalScore;
    public static int nutCounter;
    public float ropeLength;
    public float speed = 5f;
    GameObject leftBall;
    GameObject rightBall;
    public float BounceFactor = 100f;
    private float ballWidth;
    private float ballHeight;
    public Camera MainCamera;
    private Vector2 screenBounds;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    LineRenderer rope;
    List<GameObject> currentCollisions = new List<GameObject>();
    EdgeCollider2D edgeCollider;
    public Material matBounceable, matNotBounceable;
    public float ropeSegLen = 0.1f;
    public int segmentLength = 30;
    Vector3 points;
    Vector2[] points2 = new Vector2[30];
    public float lineWidth = 0.05f;

    public static bool bounceable = false;

    private float bounceThreshold = 2.5f; 
    // Start is called before the first frame update
    void Start()
    {
        globalScore = 0;
        nutCounter = 1;
        this.GetComponent<EdgeCollider2D>().sharedMaterial = new PhysicsMaterial2D()
        {
            bounciness = 0,
        };
        MainCamera = Camera.main;
        leftBall = GameObject.Find("LeftBall");
        rightBall = GameObject.Find("RightBall");
        leftBall.transform.position = new Vector3(-1, -3, 1);
        rightBall.transform.position = new Vector3(2, -3, 1);

        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        ballWidth = leftBall.transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        ballHeight = leftBall.transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        print(screenBounds);


        this.lineRenderer = this.GetComponent<LineRenderer>();
        Vector3 ropeStartPoint = leftBall.transform.position;

        for (int i = 0; i < segmentLength; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= ropeSegLen;
        }

        edgeCollider = this.gameObject.GetComponent<EdgeCollider2D>();
        rope = this.gameObject.GetComponent<LineRenderer>();
        getNewPositions();
        edgeCollider.points = points2;
    }

   
    // Update is called once per frame
    void Update()
    {
        
        //Left Ball Position
        float hLeft = Input.GetAxis("HorizontalLeft");
        float vLeft = Input.GetAxis("VerticalLeft");
        Vector2 posLeft = leftBall.transform.position;

        posLeft.x += hLeft *speed* Time.deltaTime;
        posLeft.y += vLeft * speed* Time.deltaTime;



        Vector3 viewPosLeft = posLeft;
        viewPosLeft.x = Mathf.Clamp(viewPosLeft.x, screenBounds.x*-1+ballWidth, screenBounds.x-ballWidth);
        viewPosLeft.y = Mathf.Clamp(viewPosLeft.y, screenBounds.y*-1+ballHeight, 0-ballHeight);
        leftBall.transform.position = viewPosLeft;






        //Right Ball Position

        float hRight = Input.GetAxis("Horizontal");
        float vRight = Input.GetAxis("Vertical");
        Vector2 posRight = rightBall.transform.position;

        posRight.x += hRight * speed * Time.deltaTime;
        posRight.y += vRight * speed * Time.deltaTime;



        Vector3 viewPosRight = posRight;
        viewPosRight.x = Mathf.Clamp(viewPosRight.x, -screenBounds.x+ballWidth, screenBounds.x-ballWidth);
        viewPosRight.y = Mathf.Clamp(viewPosRight.y, screenBounds.y *-1+ballHeight, 0-ballHeight);
        rightBall.transform.position = viewPosRight;


        ropeLength = Mathf.Sqrt(Mathf.Pow(viewPosRight.x - viewPosLeft.x,2) + Mathf.Pow(viewPosRight.y - viewPosLeft.y,2));

        bounceable = ropeLength > bounceThreshold;

        this.DrawRope();

        getNewPositions();
        edgeCollider.points = points2;

        if (bounceable)
        {
            this.GetComponent<EdgeCollider2D>().sharedMaterial.bounciness = 1.5f;
            this.GetComponent<LineRenderer>().material = matBounceable;
               
        }

        if (!bounceable)
        {
            this.GetComponent<EdgeCollider2D>().sharedMaterial.bounciness = 0;
            this.GetComponent<LineRenderer>().material = matNotBounceable;
        }
        
           
        
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, -1f);

        for (int i = 1; i < this.segmentLength; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    void getNewPositions()

    {

        for (int i = 0; i < rope.positionCount; i++)

        {

            points = transform.InverseTransformPoint(rope.GetPosition(i));
            
            points2[i] = new Vector2(points.x, points.y);

        }

    }
    private void ApplyConstraint()
    {
        //Constrant to First Point 
        RopeSegment firstSegment = this.ropeSegments[0];
        firstSegment.posNow = leftBall.transform.position; ;
        this.ropeSegments[0] = firstSegment;


        //Constrant to Second Point 
        RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
        endSegment.posNow = rightBall.transform.position;
        this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;

        for (int i = 0; i < this.segmentLength - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (dist > ropeSegLen)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < ropeSegLen)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * 0.5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * 0.5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
        }
    }
   
    private void DrawRope()
    {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentLength];
        for (int i = 0; i < this.segmentLength; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);
    }
    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    public static void nutted()
    {
        globalScore += nutCounter;
        nutCounter *= 2;
        
    }
    public static void newNut()
    {
        nutCounter = 1;
    }
    public static int getScore()
    {
        return globalScore;
    }
    void OnGUI()
    {
        GUI.Label(new Rect(0, 5, 100, 50), "CURRENT SCORE: " + globalScore.ToString());

    }
}
