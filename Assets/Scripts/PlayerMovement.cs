using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 8f;
    [SerializeField] private float horizontalSpeed = 6f;
    [SerializeField] private float leftLimit = -4f;
    [SerializeField] private float rightLimit = 4f;

    [SerializeField] private float deathY = -3f;
    [SerializeField] private GameObject fadeOut;
    [SerializeField] private DeathSoundPlayer deathSound;

    [SerializeField] private float jumpForce = 7f;

    private bool isDead = false;
    private bool isGrounded = true;
    private bool jumpRequest = false;

    private Rigidbody rb;
    private int prevScore = 0;

    public bool IsDead => isDead;
    public float ForwardSpeed
    {
        get => forwardSpeed;
        set => forwardSpeed = Mathf.Max(0, value);
    }
    public float HorizontalSpeed
    {
        get => horizontalSpeed;
        set => horizontalSpeed = Mathf.Max(0, value);
    }

    private void Start()
    {
        MasterInfo.score = 0;
        MasterInfo.gemCount = 0;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            jumpRequest = true;
    }

    private void FixedUpdate()
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
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // сброс вертикальной скорости
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }

        float horizontalInput = 0f;
        if (Keyboard.current.aKey.isPressed) horizontalInput = -1f;
        if (Keyboard.current.dKey.isPressed) horizontalInput = 1f;

        float newX = Mathf.Clamp(
            rb.position.x + horizontalInput * horizontalSpeed * Time.fixedDeltaTime,
            leftLimit, rightLimit
        );

        rb.MovePosition(new Vector3(newX, rb.position.y, rb.position.z + forwardSpeed * Time.fixedDeltaTime));

        if ((int)rb.position.z > prevScore)
        {
            prevScore = (int)rb.position.z;
            MasterInfo.score += 1;
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        SaveManager.SaveHighScore(MasterInfo.score);
        SaveManager.AddGems(MasterInfo.gemCount);

        deathSound?.PlayDeathSound();

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        CreateShards();

        StartCoroutine(DeathRoutine());
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

    private void CreateShards()
    {
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

                    Rigidbody shardRb = shard.AddComponent<Rigidbody>();
                    shardRb.AddExplosionForce(200f, transform.position, 2f);
                }
            }
        }
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


