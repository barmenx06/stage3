using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Joystick ve Hareket Ayarlarý")]
    public VariableJoystick joystick;
    public float moveSpeed = 2.5f;  // Yürüme hýzý - 1.5 olarak ayarlandý
    public float runSpeed = 5.0f;   // Koþma hýzý - 2.0 olarak ayarlandý
    public float runThreshold = 0.7f; // Koþma için gereken joystick eðilim eþiði
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
        // Joystick yönü
        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        float inputMagnitude = direction.magnitude;

        // Joystick eðilimi koþma eþiðinin üzerindeyse koþma modunu etkinleþtir
        isRunning = inputMagnitude > runThreshold;

        // Hareket hýzýný belirle
        currentSpeed = isRunning ? runSpeed : moveSpeed;

        // Animator'a hýz deðerini ver (0-1 arasýnda)
        if (animator != null)
        {
            // Speed deðeri: 0 = durma, 0.5 = yürüme, 1 = koþma
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

            // Yöne döndür
            if (rotateToMoveDirection)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.rotation = Quaternion.Slerp(rb.rotation, toRotation, 10f * Time.fixedDeltaTime);
            }
        }
    }

    // Saldýrý fonksiyonu - bunu bir UI düðmesine baðlayabilirsiniz
    public void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger(isAttackParameterName);
        }
    }

    // Ölüm fonksiyonu - düþman saldýrýsý vs. için
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