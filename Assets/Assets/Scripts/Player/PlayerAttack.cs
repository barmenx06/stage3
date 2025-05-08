using UnityEngine;
using UnityEngine.UI;


public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public float attackCooldown = 1f;
    private float nextAttackTime;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerAttack();
        }
#endif
    }

    public void TriggerAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetTrigger("isAttack");  // ← Animator'daki Trigger parametresi
        }
    }
}