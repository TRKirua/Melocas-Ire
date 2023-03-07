using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class MyPlayer : MonoBehaviourPun,IPunObservable
{ 
    PhotonView pv;
    public float movespeed = 10;
    public float jumpforce = 800;
    private Vector3 smoothMove;
   
    public GameObject playerCamera;

    private GameObject sceneCamera;
    public SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb;
    private bool IsGrounded;

    public Transform groundCheck;
    
    public float groundCheckRadius;
    public LayerMask collisionLayers;

    public TMP_Text nameplayerr;

    void Awake()
    {

        pv = GetComponent<PhotonView>();
    }
    void Start()
    {

        if (pv.IsMine)
        {
            nameplayerr.text = PhotonNetwork.NickName;
            rb = GetComponent<Rigidbody2D>();
            sceneCamera = GameObject.Find("Main Camera");
            sceneCamera.SetActive(false);
            playerCamera.SetActive(true);
        }
        else
        {
            nameplayerr.text = pv.Owner.NickName;
        }

        if (pv.IsMine == false)
        {
            Destroy(playerCamera);
        }
    }

    void Update()
    {
       if (pv.IsMine)
        {
            ProcessInputs();
        }
       else
        {
            smoothMovement();

        }
        
    }

    private void ProcessInputs()
    {
        var move=new Vector3(Input.GetAxis("Horizontal"),0);
        transform.position += move * movespeed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sr.flipX = false;
            pv.RPC("OnDirectionChange_RIGHT",RpcTarget.All);

        }
        

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            sr.flipX = true;
            pv.RPC("OnDirectionChange_LEFT",RpcTarget.All);
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            Jump();
        }
        
    }

    [PunRPC]
    void OnDirectionChange_LEFT()
    {
        sr.flipX = false;

    }
    [PunRPC]
    void OnDirectionChange_RIGHT()
    {
        sr.flipX = true;

    }

    void Jump()
    {
        rb.AddForce(Vector2.up*jumpforce);

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (pv.IsMine)
        {

            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
            
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (pv.IsMine)
        {
            IsGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        }
    }

    private void smoothMovement()
    {
        transform.position = Vector3.Lerp(transform.position, smoothMove, Time.deltaTime * 10);

    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            
        }
        else if(stream.IsReading)
        {
           smoothMove=(Vector3) stream.ReceiveNext();
        }
        
    }
}