using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class BossBattleManager : MonoBehaviour
{
    
    [Header("UI Containers")]
    [SerializeField] private GameObject phase3UIContainer;
    [SerializeField] private GameObject phase4UIContainer;
    [SerializeField] private GameObject phase4UIWinInstructions;
    [SerializeField] private GameObject phase4UILoseInstructions;

    [Header("End Game Events")]
    [SerializeField] UnityEvent OnVictory;
    [SerializeField] UnityEvent OnDefeat;

    [Header("Vuforia References")]
    [SerializeField] private GameObject bossTargetObject;
    [SerializeField] private Transform bossSpawnPoint;
    [SerializeField] private Transform playerTransform;

    [Header("Battle Configuration")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float baseAttackInterval = 3f;
    [SerializeField] private float baseProjectileSpeed = 2f;
    [SerializeField] private int baseHitsToWin = 5;
    [SerializeField] private int playerStartingHealth = 10;

    [Header("UI References")]
    [SerializeField] private TMP_Text bossHealthText;
    [SerializeField] private TMP_Text playerHealthText;
    [SerializeField] private Button playerAttackButton;
    [SerializeField]  private GameObject challengeButton;

    //Private Variables
    private bool isBossVisible = false;
    private bool isBattleActive = false;
    private float attackTimer;
    private float attackInterval;
    private float projectileSpeed;
    private int maxBossHealth;
    private int currentBossHealth;
    private int maxPlayerHealth;
    private int currentPlayerHealth;

    void Start()
    {
        if(bossTargetObject != null)
        {
            bossTargetObject.SetActive(false);        
        }

        if(phase4UIContainer != null)
        {
            phase4UIContainer.SetActive(false);
        }

        if(phase4UIWinInstructions != null)
        {
            phase4UIWinInstructions.SetActive(false);            
        }

        if(phase4UILoseInstructions != null)
        {
            phase4UILoseInstructions.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!isBattleActive || !isBossVisible)
        {
            return;
        }
        attackTimer -= Time.deltaTime;

        if(attackTimer <= 0)
        {
            BossShoot();
            attackTimer = attackInterval;
        }
    }

    public void PrepareBossBattle(bool playerWonPhase3)
    {
        if(playerWonPhase3)
        {
            //Standard Difficulty & display proper instructions
            attackInterval = baseAttackInterval;
            projectileSpeed = baseProjectileSpeed;
            maxBossHealth = baseHitsToWin;
            if(phase3UIContainer != null)
            {
                phase3UIContainer.SetActive(false);            
            }

            if(phase4UIWinInstructions != null)
            {
                phase4UIWinInstructions.SetActive(true);            
            }                        
        }
        else
        {
            //Hard Mode & display proper instructions
            attackInterval = baseAttackInterval / 2f;
            projectileSpeed = baseProjectileSpeed * 2f;
            maxBossHealth = baseHitsToWin * 2;
            if(phase3UIContainer != null)
            {
                phase3UIContainer.SetActive(false);            
            }

            if(phase4UILoseInstructions != null)
            {
                phase4UILoseInstructions.SetActive(true);
            }
        }

        maxPlayerHealth = playerStartingHealth;

        if(challengeButton != null)
        {
            challengeButton.SetActive(true);
        }
    }
    
    public void StartBossBattle()
    {
        //Swap UI
        if(phase4UIWinInstructions != null)
        {
            phase4UIWinInstructions.SetActive(false);
        }
        if(phase4UILoseInstructions != null)
        {
            phase4UILoseInstructions.SetActive(false);
        }
        if(phase4UIContainer != null)
        {
            phase4UIContainer.SetActive(true);
        }
        if(challengeButton != null)
        {
            challengeButton.SetActive(false);
        }

        //Enable Vuforia Object Tracking
        if(bossTargetObject != null)
        {
            bossTargetObject.SetActive(true);        
        }

        //Reset Stats
        currentBossHealth = maxBossHealth;
        currentPlayerHealth = maxPlayerHealth;
        UpdateHealthUI();

        //Activate Logic
        isBattleActive = true;
        attackTimer = attackInterval;

        //Enable Player attack button
        playerAttackButton.interactable = true;
    }

    public void OnBossFound()
    {
        if(isBattleActive)
        {
            isBossVisible = true;
            Debug.Log("Boss Found!");

            //Re-enable player input but only if there is no cooldown
            if(!IsInvoking(nameof(EnablePlayerAttack)))
            {
                if (playerAttackButton != null)
                {
                    playerAttackButton.interactable = true;
                }
            }
        }
    }

    public void OnBossLost()
    {
        isBossVisible = false;
        Debug.Log("Boss Lost!");

        //Disable player inputs
        if(playerAttackButton != null)
        {
            playerAttackButton.interactable = false;
        }
    }

    private void BossShoot()
    {
        //Spawn projectile
        GameObject bullet = Instantiate(projectilePrefab, bossSpawnPoint.position, Quaternion.identity);

        //Calculate direction towards player
        Vector3 dirToPlayer = (playerTransform.position - bossSpawnPoint.position).normalized;

        //Initialize projectile
        bullet.GetComponent<BattleProjectile>().Initialize(dirToPlayer, projectileSpeed, true, this);
    }

    public void PlayerShoot()
    {
        //Don't shoot if the boss is gone
        if(!isBossVisible)
        {
            return;
        }
        
        //Spawn bullet at camera position & offset forward
        Vector3 spawnPos = playerTransform.position + (playerTransform.forward * 0.5f);

        GameObject bullet = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        //Calculate direction towards boss
        Vector3 dirToBoss = (bossSpawnPoint.position - playerTransform.position).normalized;

        //Initialize projectile
        bullet.GetComponent<BattleProjectile>().Initialize(dirToBoss, projectileSpeed, false, this);

        //Disable button for cooldown
        playerAttackButton.interactable = false;
        Invoke(nameof(EnablePlayerAttack), attackInterval);
    }

    private void EnablePlayerAttack()
    {
        playerAttackButton.interactable = true;
    }

    public void PlayerWasHit()
    {
        currentPlayerHealth--;
        UpdateHealthUI();
        if(currentPlayerHealth <= 0)
        {
            GameOver(false);
        }
    }

    public void BossWasHit()
    {
        currentBossHealth--;
        UpdateHealthUI();
        if(currentBossHealth <= 0)
        {
            GameOver(true);
        }
    }

    private void UpdateHealthUI()
    {
        if(bossHealthText)
        {
            bossHealthText.text = $"Boss: {currentBossHealth}/{maxBossHealth}";
        }
        if(playerHealthText)
        {
            playerHealthText.text = $"Player: {currentPlayerHealth}/{maxPlayerHealth}";
        }
    }

    private void GameOver(bool playerWon)
    {
        isBattleActive = false;

        if (bossTargetObject != null)
        {
            bossTargetObject.SetActive(false);
        }
        if (phase4UIContainer != null)
        {
            phase4UIContainer.SetActive(false);
        }

        if (playerWon)
        {
            OnVictory.Invoke();
        }
        else
        {
            OnDefeat.Invoke();
        }
    }
}
