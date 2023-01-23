using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController controler;
    private CapsuleCollider col;
    private Vector3 dir;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private int coins; 
    [SerializeField] private GameObject losePanel;
    [SerializeField] private Text coinsText;

    private int lineToMove = 1;
    public float lineDistance = 4;
    private  float MAX_SPEED = 110;

    void Start()
    {
        controler = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
        col = GetComponent<CapsuleCollider>();
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(SwipeController.swipeRight)
        {
            if(lineToMove < 2)
                lineToMove++;
        }

        if(SwipeController.swipeUp)
        {
            if(controler.isGrounded)
                Jump();
        }

        if(SwipeController.swipeLeft)
        {
            if (lineToMove > 0)
                lineToMove--;
        }

        if(SwipeController.swipeDown)
        {
            StartCoroutine(Slide());
        }

        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
        if(lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if(lineToMove == 2)
            targetPosition += Vector3.right *lineDistance;

        if(transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * 25 *Time.deltaTime;
        if(moveDir.sqrMagnitude < diff.sqrMagnitude)
            controler.Move(moveDir);
        else
            controler.Move(diff);
        
        }

    private void Jump()
    {
        dir.y = jumpForce;
    }
    void FixedUpdate()
    {
        dir.z = speed;
        dir.y += gravity * Time.fixedDeltaTime;
        controler.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "obstacle")
        {
            losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "arm")
        {
            coins ++;
            coinsText.text = coins.ToString();
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new  WaitForSeconds(4);
        if (speed < MAX_SPEED)
        {
            speed += 3;
            StartCoroutine(SpeedIncrease());
        }
    }

    private IEnumerator Slide()
    {
        col.center = new Vector3(0, -0.67F, 0);
        col.height = 0.68F;
        col.radius = 0.26F;

        yield return new WaitForSeconds(1);

        col.center = new Vector3(0, -0.02f, 0);
        col.height = 1.976194F;
        col.radius = 0.402793F; 
    }
}
