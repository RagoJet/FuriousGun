using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesFactory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;
    [SerializeField] private Player player;
    private readonly EnemyPool _enemyPool = new();


    private void CreateEnemy(int level){
        Enemy enemy = _enemyPool.GetEnemy(level);
        if (enemy == null){
            enemy = Instantiate(enemyDescriptions.ListOfEnemies[level].enemyPrefab,
                transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)),
                Quaternion.identity);
            enemy.Ondie += _enemyPool.HideEnemy;
        }

        enemy.Init(enemyDescriptions.ListOfEnemies[level], player);
    }

    private void Start(){
        _enemyPool.Init(enemyDescriptions.ListOfEnemies.Count);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.O)){
            for (int i = 0; i < 17; i++){
                CreateEnemy(i);
            }
        }
    }
}