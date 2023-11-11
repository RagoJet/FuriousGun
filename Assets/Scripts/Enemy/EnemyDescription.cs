using UnityEngine;


[CreateAssetMenu(fileName = "EnemyDescription", menuName = "EnemyDescription", order = 0)]
public class EnemyDescription : ScriptableObject{
    public int level;
    public int damage;
    public int maxHealth;
    public int gold;
    public Enemy enemyPrefab;
}