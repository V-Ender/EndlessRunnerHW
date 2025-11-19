using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float forwardSpeed = 8f;     
    public float horizontalSpeed = 6f;
    public float dashDistance = 1f;  
    public float leftLimit = -4f;       
    public float rightLimit = 4f;       
    
    public float deathY = -3f; 
    public GameObject fadeOut; 
    private bool isDead = false;
    public DeathSoundPlayer deathSound;

    public float jumpForce = 7f;  
    private bool isGrounded = true;
    private bool jumpRequest = false;

    private Rigidbody rb;
    private int prev_score = 0;

    void Start()
    {
        MasterInfo.score = 0;
        MasterInfo.gemCount = 0;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpRequest = true;
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (rb.position.y < deathY)
        {
            Die();
            return;
        }

        CheckGrounded();
        if (jumpRequest && isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }

        Vector3 forwardVelocity = Vector3.forward * forwardSpeed;

        float horizontalInput = 0f;
        if (Keyboard.current.aKey.isPressed) horizontalInput = -1f;
        if (Keyboard.current.dKey.isPressed) horizontalInput = 1f;

        float newX = Mathf.Clamp(rb.position.x + horizontalInput * horizontalSpeed * Time.fixedDeltaTime,
                                 leftLimit, rightLimit);

        rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z + forwardSpeed * Time.fixedDeltaTime));
        if ((int)rb.position.z > prev_score) {
            prev_score = (int)rb.position.z;
            MasterInfo.score += 1;
        }
    }

    void Dash(float distance)
    {
        float newX = Mathf.Clamp(rb.position.x + distance, leftLimit, rightLimit);
        rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z));
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        SaveManager.SaveHighScore(MasterInfo.score);
        SaveManager.AddGems(MasterInfo.gemCount);

        if (deathSound != null) deathSound.PlayDeathSound();

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        int rows = 3;
        float spacing = 0.2f;
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                for (int z = 0; z < rows; z++)
                {
                    Vector3 pos = transform.position + new Vector3(
                        (x - 1) * spacing,
                        (y - 1) * spacing,
                        (z - 1) * spacing
                    );

                    GameObject shard = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    shard.transform.position = pos;
                    shard.transform.localScale = Vector3.one * 0.2f;

                    Rigidbody rb = shard.AddComponent<Rigidbody>();
                    rb.AddExplosionForce(200f, transform.position, 2f);
                }
            }
        }
        StartCoroutine(DeathRoutine());
    }

    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (fadeOut != null)
            fadeOut.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(0);
    }
}


