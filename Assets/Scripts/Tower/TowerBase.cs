using UnityEngine;

/// <summary>
/// A base class for all tower types in the game, providing functionality for attacking enemies and upgrading tower stats.
/// </summary>
/// <remarks>
/// - Provides basic attack behavior with configurable attack range, cooldown, and cost.
/// - Automatically attacks the closest enemy within range when the cooldown period has passed.
/// - Can be upgraded to increase attack range and decrease attack cooldown.
/// </remarks>
public abstract class TowerBase : MonoBehaviour
{
    public Attackable AttackBehaviour => attackBehaviour;

    [SerializeField]
    private Attackable attackBehaviour;

    public bool HasUpgrade => upgradedTowerPrefab != null;
    public int AttackRange => attackRange;
    public int AttackCooldown => attackCooldown;
    public int MoneyCost => moneyCost;
    public int UpgradeCost => upgradeCost;

    [Header("Tower Settings")]
    [SerializeField]
    protected int attackRange = 10;
    [SerializeField]
    protected int attackCooldown = 2;
    [SerializeField]
    protected int moneyCost;

    [Header("Tower Upgrade Settings")]
    [SerializeField] protected TowerBase upgradedTowerPrefab;
    [SerializeField] protected int upgradeCost;
    [SerializeField] protected int upgradedRange;
    [SerializeField] protected int upgradedAttackCooldown;

    protected float lastAttackTime = float.MinValue;

    protected virtual void Update()
    {
        Transform target = FindClosestEnemy();


        if (target != null && Time.time >= lastAttackTime + attackCooldown)
        {
            attackBehaviour?.Attack(target);
            lastAttackTime = Time.time;
        }
    }

    private Transform FindClosestEnemy()
    {
        //Possibly place a layer masks so it detects only enemies.
        int enemyLayer = LayerMask.GetMask("Enemy");
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);

            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }

    public void UpgradeStats(int attackRangeAmount, int attackCooldownAmount)
    {
        attackRange += attackRangeAmount;
        attackCooldown = Mathf.Max(1, attackCooldown - attackCooldownAmount);
        Debug.Log($"Tower upgraded! New Range: {attackRange}, New Cooldown: {attackCooldown}");
    }

    public int GetTowerCost()
    {
        return moneyCost;
    }

    public TowerBase TowerUpgrade()
    {
        MoneyManager moneyManager = GameManager.GetManager<MoneyManager>();

        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not initialized.");
            return null;
        }

        if (!moneyManager.CanAfford(upgradeCost))
        {
            Debug.Log("Not enough money to upgrade.");
            return null;
        }

        moneyManager.SpendMoney(upgradeCost);

        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        TowerBase upgradedTower = Instantiate(upgradedTowerPrefab, currentPosition, currentRotation);
        upgradedTower.UpgradeStats(upgradedRange, upgradedAttackCooldown);
        Destroy(gameObject);
        return upgradedTower;
    }
}
