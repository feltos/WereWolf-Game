using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Photon.MonoBehaviour
{
    float moveSpeed = 5f;
    float rotateSpeed = 500f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public bool UseTransformView = true;
    private Animator animator;
    private PhotonView PhotonView;
    Rigidbody body;
    public int id;
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (PhotonView.isMine)
        {
            CheckInput();
        }
        else
        {
            SmoothMove();
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (UseTransformView)
        {
            return;
        }

        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            targetPosition = (Vector3)stream.ReceiveNext();
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmoothMove()
    {
        if (UseTransformView)
        {
            return;
        }
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 500 * Time.deltaTime);
    }
    private void CheckInput()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
        transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime, 0));

        animator.SetFloat("Input", vertical);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Taunt");
        }
    }


}
