using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrlTest : MonoBehaviour
{
    public static PlayerCtrlTest Instance;

    public GameObject player;

    public float moveSpeed;

    public Rigidbody rb;

    public GameObject moveBlockParent;

    public GameObject movePrevBlock;

    public List<GameObject> moveBlockList = new List<GameObject>();

    public GameObject StackHolder;

    public GameObject movePrevBlock2;

    public bool CantTouch = false;

    public GameObject BlockBackWallFloor2;

    public GameObject BlockBackWallFloor3;

    public GameObject BallHolder;

    public GameObject BallPrev;

    public List<GameObject> BallList = new List<GameObject>();

    public bool CamMovePos1 = false;

    public bool CamMovePos2 = false;

    public bool CamMovePos3 = false;

    public Transform CannonPos;

    public Transform CurrentPlayerPos;

    public List<GameObject> listTriggerMoveToCannon;

    public Transform GunBarrel;

    public bool ShootBall = false;

    public GameObject BulletHolder;

    public int ballCount = 0;

    public GameObject BallPrefabs;

    public bool CantPickupBall = false;

    public bool isShooting = false;

    public bool barrelMove = false;

    public bool isFinish = false;

    public Vector3 startTouch;

    public Vector3 endTouch;

    public Vector3 currentSwipe;

    public bool isNextRound = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        BallList.Add(BallPrev);
    }

    void Update()
    {
        MoveCharacter();

        if (ShootBall == true)
        {
            StartCoroutine("spawnBall");
            StartCoroutine("NextRound");
        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(3f);

        isNextRound = true;
    }

    IEnumerator spawnBall()
    {
        if (ballCount >= 30)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        Instantiate(BallPrefabs, BulletHolder.transform.position, transform.rotation);

        ballCount++;

        foreach (GameObject ball in BallList)
        {
            Destroy(ball);
        }
    }

   

    public void MoveCharacter() // move with Swipe
    {
        if(CantTouch == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startTouch = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            }
          
            if (Input.GetMouseButtonUp(0))
            {
                endTouch = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

                currentSwipe = new Vector3(endTouch.x - startTouch.x, endTouch.y - startTouch.y);

                currentSwipe.Normalize();

                if(currentSwipe.y > 0f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    rb.velocity = Vector3.forward.normalized * moveSpeed * Time.deltaTime;
                    Debug.Log("Up Swipe");
                }
                else if (currentSwipe.y < 0f && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    rb.velocity = -Vector3.forward.normalized * moveSpeed * Time.deltaTime;
                    Debug.Log("Down Swipe");
                }
                else if (currentSwipe.x < 0f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    rb.velocity = -Vector3.right.normalized * moveSpeed * Time.deltaTime;
                    Debug.Log("Left Swipe");
                }
                else if (currentSwipe.x > 0f && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    rb.velocity = Vector3.right.normalized * moveSpeed * Time.deltaTime;
                    Debug.Log("Right Swipe");
                }
            }
        }
    }

    public void PickMoveBlock(GameObject moveBlockObject)
    {
        moveBlockObject.transform.SetParent(moveBlockParent.transform); // di chuyển moveObject thành object con của stackHolder

        moveBlockList.Add(moveBlockObject); // thêm moveObject vào list để tiện quản lý

        Vector3 pos = movePrevBlock.transform.localPosition; //Vị trí của moveBlock đầu tiên

        pos.y -= 0.15f; // khi nhặt moveBlock sẽ dịch chuyển xuống 0.15f so với cái trước đấy

        moveBlockObject.transform.localPosition = pos;

        Vector3 CharacterPos = transform.localPosition;

        CharacterPos.y += 0.15f;

        transform.localPosition = CharacterPos;

        movePrevBlock = moveBlockObject;

        movePrevBlock.GetComponent<BoxCollider>().isTrigger = false;
    }

    public void PickupBall(GameObject Ball)
    {
        BallList.Add(BallPrev);

        Ball.transform.SetParent(BallHolder.transform);

        BallList.Add(Ball);

        Vector3 pos = BallPrev.transform.localPosition;

        pos.y += 0.16f;

        Ball.transform.localPosition = pos;

        BallPrev = Ball;

        Ball.gameObject.layer = 10;

        ballCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Trigger Cant Touch"))
        {
            CantTouch = true;
        }

        if (other.gameObject.CompareTag("Check Point"))
        {
            CamMovePos1 = true;
        }

        if (other.gameObject.CompareTag("Check Point 2"))
        {
            CamMovePos1 = false;
            CamMovePos2 = true;
        }

        if(other.gameObject.CompareTag("Trigger Move To Cannon"))
        {
            CamMovePos1 = false;
            CamMovePos2 = false;
            CamMovePos3 = true;
        }

        if (other.gameObject.CompareTag("Ball"))
        {
            if (CantPickupBall == false)
            {
                PickupBall(other.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check Point"))
        {
            CantTouch = true;

            StartCoroutine(MoveNextFloor());

            StartCoroutine(BlockBack());

            movePrevBlock = GetComponent<PlayerCtrlTest>().movePrevBlock2.gameObject;
        }

        if (other.gameObject.CompareTag("Check Point 2"))
        {
            CantTouch = true;

            BallHolder.gameObject.SetActive(true);

            BallPrev.gameObject.SetActive(true);

            StartCoroutine(MoveNextFloor2());

            StartCoroutine(BlockBack2());

            movePrevBlock = GetComponent<PlayerCtrlTest>().movePrevBlock2.gameObject;
        }

        if (other.gameObject.CompareTag("Trigger Move To Cannon"))
        {
            CantTouch = true;

            CantPickupBall = true;

            rb.velocity = Vector3.zero;

            BallHolder.GetComponent<MeshRenderer>().enabled = false;

            StartCoroutine(MoveToCannon());
        }

        if (other.gameObject.CompareTag("Pos Fire Cannon"))
        {

            listTriggerMoveToCannon[0].gameObject.GetComponent<BoxCollider>().isTrigger = false;

            listTriggerMoveToCannon[1].gameObject.GetComponent<BoxCollider>().isTrigger = false;

            listTriggerMoveToCannon[2].gameObject.GetComponent<BoxCollider>().isTrigger = false;

            listTriggerMoveToCannon[3].gameObject.GetComponent<BoxCollider>().isTrigger = false;

            rb.isKinematic = true;

            isShooting = true;

            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

            foreach (GameObject ball in BallList)
            {

                ball.GetComponent<Collider>().isTrigger = false;

                ball.AddComponent<Rigidbody>();

                ball.GetComponent<Rigidbody>().mass = 5f;
            }

            StartCoroutine(ShootingTheBall());

        }
    }
    IEnumerator ShootingTheBall()
    {
        yield return new WaitForSeconds(2f);

        ShootBall = true;

        barrelMove = true;
    }

    IEnumerator MoveToCannon()
    {
        yield return new WaitForSeconds(2f);

        player.transform.position = Vector3.MoveTowards(CurrentPlayerPos.transform.position, CannonPos.transform.position, 100f);
    }

    IEnumerator MoveNextFloor2()
    {
        yield return new WaitForSeconds(1f);

        CantTouch = false;

        rb.velocity = Vector3.forward * 2000f * Time.deltaTime;
    }

    IEnumerator MoveNextFloor()
    {
        yield return new WaitForSeconds(1f);

        CantTouch = false;

        movePrevBlock2.gameObject.SetActive(true);

        rb.velocity = Vector3.forward * moveSpeed * Time.deltaTime;
    }

    IEnumerator BlockBack()
    {
        yield return new WaitForSeconds(3.0f);

        BlockBackWallFloor2.gameObject.SetActive(true);
    }

    IEnumerator BlockBack2()
    {
        yield return new WaitForSeconds(2.8f);

        BlockBackWallFloor3.gameObject.SetActive(true);
    }

}
