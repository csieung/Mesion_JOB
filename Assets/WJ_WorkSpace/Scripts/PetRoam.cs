using UnityEngine;
using UnityEngine.AI;

public class PetRoam : MonoBehaviour
{
    public float roamRadius = 10f; // 펫이 돌아다닐 반경
    public float roamDelay = 5f;   // 각 이동 간의 지연 시간

    private Animator animator;
    private NavMeshAgent agent;
    private Vector3 startingPosition;
    private bool isFollowingPlayer = false; // 플레이어를 바라보는지 여부

    public Transform playerTransform; // 플레이어의 Transform

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        startingPosition = transform.position;
        InvokeRepeating("Roam", 0f, roamDelay);
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            transform.LookAt(playerTransform);
        }

        // NavMeshAgent의 속도를 애니메이터의 Speed 파라미터로 설정
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void Roam()
    {
        if (!isFollowingPlayer)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
            randomDirection += startingPosition;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1))
            {
                Vector3 finalPosition = hit.position;
                agent.SetDestination(finalPosition);
            }
        }
    }

    public void SetFollowingPlayer(bool isFollowing)
    {
        isFollowingPlayer = isFollowing;
        agent.isStopped = isFollowing; // 플레이어를 따라갈 때는 NavMeshAgent를 멈춤
    }
}
