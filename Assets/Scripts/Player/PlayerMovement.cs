using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : Photon.MonoBehaviour
{
    float moveSpeed = 5f;
    float rotateSpeed = 300f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public bool UseTransformView = true;
    private Animator animator;
    private PhotonView PhotonView;
    Rigidbody body;
    public int id;
    Camera camera;
    RaycastHit hit;
    bool voted = false;
    PlayerManagement lastVoted;
    PlayerManagement actualVoted;
    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        camera = this.GetComponentInChildren<Camera>();
        
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
        float horizontal = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, horizontal * rotateSpeed * Time.deltaTime, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Taunt");
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {         
                if (Input.GetMouseButtonDown(0))
                {
                    actualVoted = hit.collider.gameObject.GetComponent<PlayerManagement>();

                    //use on first vote;
                    if(lastVoted == null)
                    {
                        Debug.Log("no vote");
                        lastVoted = actualVoted;
                        actualVoted.UpNmbOfVotes();
                    }

                    if(lastVoted != actualVoted)
                    {
                        lastVoted.MinusNmbOfVotes();
                        actualVoted.UpNmbOfVotes();
                        lastVoted = actualVoted;
                    }
                }
            }
        }
    }


}
