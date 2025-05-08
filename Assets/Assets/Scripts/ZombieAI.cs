using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class ZombieAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    private NavMeshAgent agent;
    private Animator animator;
    private float nextAttackTime;
    private PlayerHealth playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>(); // Player'�n health component'ini al�yoruz
    }

    void Update()
    {
        //if (playerHealth.isDead) // E�er oyuncu �lm��se hi�bir �ey yapma
        //{
        //    animator.SetBool("isAttacking", false);
        //    agent.isStopped = true;
        //    return;
        //}

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange) // Oyuncuya sald�rmak i�in yak�nsa
        {
            agent.isStopped = true; // Zombi hareket etmesin
            animator.SetBool("isWalking", false); // Y�r�meyi durdur
            animator.SetBool("isAttacking", true); // Sald�r�y� ba�lat

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown; // Attack cooldown

                // Oyuncuya hasar ver
                playerHealth.TakeDamage(attackDamage);
            }
        }
        else if (distance <= detectionRange) // Oyuncu yak�nsa zombi onu takip etsin
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false); // Sald�r� durdurulsun
        }
        else // Oyuncu �ok uzaksa zombi duracak
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false); // Sald�r�y� durdur
        }
    }

    public void Die()
    {
        agent.isStopped = true; // Zombi duracak
        animator.SetBool("isDead", true); // �l�m animasyonu
        Destroy(gameObject, 5f); // �l�m animasyonu sonras� zombi yok�edilsin
����}
}