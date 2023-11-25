using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesFactory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;
    [SerializeField] private Player player;
    private readonly EnemyPool _enemyPool = new();
    private byte countOfList;

    private void CreateEnemy(int levelOfUnit, int levelOfGame){
        Enemy enemy = _enemyPool.GetEnemy(levelOfUnit);
        if (enemy == null){
            enemy = Instantiate(enemyDescriptions.ListOfEnemies[levelOfUnit].enemyPrefab,
                transform.position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-19f, 4f)),
                Quaternion.identity);
            enemy.Ondie += _enemyPool.HideEnemy;
            enemy.Ondie += WaveStarter.Instance.MinusMonster;
        }
        else{
            enemy.transform.position = this.transform.position +
                                       new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-19f, 4f));
        }

        enemy.Init(enemyDescriptions.ListOfEnemies[levelOfUnit], player, levelOfGame);
        AudioPlayer.Instance.PlayCreateMonsterClip();
    }

    private void Start(){
        countOfList = (byte) enemyDescriptions.ListOfEnemies.Count;
        _enemyPool.Init(enemyDescriptions.ListOfEnemies.Count);
    }


    public void CreateMonsters(int levelOfGame, int countMonsters){
        for (int i = 0; i < countMonsters; i++){
            var levelOfEnemy = Random.Range(0, Mathf.Clamp(levelOfGame, 1, countOfList));
            CreateEnemy(levelOfEnemy, levelOfGame);
        }
    }
}