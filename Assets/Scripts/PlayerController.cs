using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controler;
    private Vector3 dir;
    [SerializeField] private int speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravity;
    [SerializeField] private GameObject losePanel;

    private int lineToMove = 1;
    public float lineDistance = 4;

    // Start is called before the first frame update
    void Start()
    {
        controler = GetComponent<CharacterController>();

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

    // Update is called once per frame
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
}
