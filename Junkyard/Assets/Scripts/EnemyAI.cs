using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
	private const int PLAYER_CHASE_RANGE = 20;
	private Enemy enemy;
    private new Rigidbody rigidbody;
    private UnityEngine.AI.NavMeshAgent navAgent;

    private Transform playerTransform;

    private HealthComponent healthComponent = null;
    [SerializeField]
    private bool playerInAttackRange = false;

    private float timeOfLastHit = 0;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform = FindObjectOfType<Player>().transform;
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
	{
		if (PlayerInChaseRange)
		{
			navAgent.SetDestination(playerTransform.position);
		}

		if (playerInAttackRange && Time.time - timeOfLastHit >= 3)
		{
			healthComponent.Damage(5);
			timeOfLastHit = Time.time;
		}

		if (enemy.IsDead)
		{
			enabled = false;
			playerTransform = null;
			navAgent.enabled = false;
			rigidbody.AddForce(Random.onUnitSphere);
		}
	}

	private bool PlayerInChaseRange => (playerTransform.position - transform.position).sqrMagnitude <= PLAYER_CHASE_RANGE * PLAYER_CHASE_RANGE;

	private void OnTriggerEnter(Collider other)
    {
        if (!playerInAttackRange)
        {
            var player = other.GetComponent<Player>();
            healthComponent = other.GetComponent<HealthComponent>();

            if (player && healthComponent)
            {
                playerInAttackRange = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerInAttackRange)
        {
            var player = other.GetComponent<Player>();

            if (player)
            {
                playerInAttackRange = false;
                healthComponent = null;
            }
        }
    }
}
