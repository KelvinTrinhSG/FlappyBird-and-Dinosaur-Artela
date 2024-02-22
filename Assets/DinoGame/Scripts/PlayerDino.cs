using UnityEngine;

public class PlayerDino : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;
    public float gravity = 9.81f * 2f;
    public float jumpforce = 8f;
    private void Awake()
    {
        character = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        direction = Vector3.zero;
    }
    private void Update()
    {
        direction += Vector3.down * gravity * Time.deltaTime;
        if (character.isGrounded)
        {
            direction = Vector3.down;
            if (Input.GetButton("Jump"))
            {
                direction = Vector3.up * jumpforce;
            }
        }
        character.Move(direction * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            GameManagerDino.Instance.GameOver();
        }
    }
}
