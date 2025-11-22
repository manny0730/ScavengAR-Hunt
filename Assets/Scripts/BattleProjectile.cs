using UnityEngine;

public class BattleProjectile : MonoBehaviour
{
    
    private Vector3 targetDirection;
    private float speed;
    private bool isHostile; //True = Boss shot it, False = Player shot it
    private BossBattleManager manager;

    public void Initialize(Vector3 direction, float speed, bool isHostile, BossBattleManager managerRef)
    {
        this.targetDirection = direction.normalized;
        this.speed = speed;
        this.isHostile = isHostile;
        this.manager = managerRef;

        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetDirection * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHostile && other.CompareTag("Player"))
        {
            manager.PlayerWasHit();
            Destroy(gameObject);
        }
        else if(!isHostile && other.CompareTag("Boss"))
        {
            manager.BossWasHit();
            Destroy(gameObject);
        }
    }
}
