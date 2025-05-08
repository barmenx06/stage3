using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick ve Hareket Ayarlar�")]
    public VariableJoystick joystick;
    public float moveSpeed = 2.5f;  // Y�r�me h�z� - 1.5 olarak ayarland�
    public float runSpeed = 5.0f;   // Ko�ma h�z� - 2.0 olarak ayarland�
    public float runThreshold = 0.7f; // Ko�ma i�in gereken joystick e�ilim e�i�i
    public bool rotateToMoveDirection = true;

    [Header("Animasyon Parametreleri")]
    public string speedParameterName = "Speed";
    public string isAttackParameterName = "isAttack";

    private Rigidbody rb;
    private Animator animator;
    private float currentSpeed;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (rb == null)
            Debug.LogWarning("Rigidbody component eksik!");
        if (animator == null)
            Debug.LogWarning("Animator component eksik!");
    }

    void FixedUpdate()
    {
        // Joystick y�n�
        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        float inputMagnitude = direction.magnitude;

        // Joystick e�ilimi ko�ma e�i�inin �zerindeyse ko�ma modunu etkinle�tir
        isRunning = inputMagnitude > runThreshold;

        // Hareket h�z�n� belirle
        currentSpeed = isRunning ? runSpeed : moveSpeed;

        // Animator'a h�z de�erini ver (0-1 aras�nda)
        if (animator != null)
        {
            // Speed de�eri: 0 = durma, 0.5 = y�r�me, 1 = ko�ma
            float animSpeedValue = 0;

            if (inputMagnitude > 0.1f)
            {
                animSpeedValue = isRunning ? 1.0f : 0.5f;
            }

            animator.SetFloat(speedParameterName, animSpeedValue);
        }

        // Hareket uygula
        if (inputMagnitude > 0.1f)
        {
            Vector3 move = direction.normalized * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);

            // Y�ne d�nd�r
            if (rotateToMoveDirection)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 10f * Time.fixedDeltaTime);
            }
        }
    }

    // Sald�r� fonksiyonu - bunu bir UI d��mesine ba�layabilirsiniz
    public void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger(isAttackParameterName);
        }
    }

    // �l�m fonksiyonu - d��man sald�r�s� vs. i�in
    public void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Death");

            // Hareket etmeyi engelle
            this.enabled = false;
        }
    }
}