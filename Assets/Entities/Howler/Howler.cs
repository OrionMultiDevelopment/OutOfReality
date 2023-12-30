using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Howler : MonoBehaviour
{
    public Transform player;
    public float walkSpeed = 2.0f;
    public float runSpeed = 5.0f;
    public float alertDistance = 10f;
    public Animator animator;
    public AudioSource chaseAudio;
    public Camera playerCamera;
    private Vector3 originalCameraPosition;
    public float jumpscareCameraDistance = 1f;
    private bool isRunning = false;
    public Transform monsterHead;
    public float jumpscareCameraOffset = 1.5f;

    void Start()
    {
        originalCameraPosition = playerCamera.transform.localPosition;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= alertDistance)
        {
            if (!isRunning)
            {
                isRunning = true;
                chaseAudio.Play();
                animator.SetBool("IsWalking", true);
            }
            
            RunTowardsPlayer();
        }
        else
        {
            if (isRunning)
            {
                isRunning = false;
                chaseAudio.Stop();
                animator.SetBool("IsWalking", false);
            }
        }

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

   void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(TriggerJumpscare());
        }
    }

    IEnumerator TriggerJumpscare()
    {
        player.GetComponent<CharacterController>().enabled = false;
        this.enabled = false;

        Vector3 jumpscarePosition = playerCamera.transform.position - playerCamera.transform.forward * jumpscareCameraOffset;
        playerCamera.transform.position = jumpscarePosition;
        playerCamera.transform.LookAt(monsterHead);

        animator.SetTrigger("Jumpscare");
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void RunTowardsPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (!Physics.Raycast(transform.position, directionToPlayer, 1f))
        {
            transform.position += directionToPlayer * runSpeed * Time.deltaTime;
        }
    }
}
