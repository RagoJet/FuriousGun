using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesFactory : MonoBehaviour{
    [SerializeField] private EnemyDescriptions enemyDescriptions;
    [SerializeField] private Player player;
    private readonly EnemyPool _enemyPool = new();
    private byte countOfList;

    private void CreateEnemy(int level, WaveStarter waveStarter){
        Enemy enemy = _enemyPool.GetEnemy(level);
        if (enemy == null){
            enemy = Instantiate(enemyDescriptions.ListOfEnemies[level].enemyPrefab);
            enemy.Ondie += _enemyPool.HideEnemy;
            enemy.Ondie += waveStarter.MinusMonster;
        }

        enemy.transform.position = this.transform.position +
                                   new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        enemy.Init(enemyDescriptions.ListOfEnemies[level], player);
        AudioPlayer.Instance.PlayCreateMonsterClip();
    }

    private void Start(){
        countOfList = (byte) enemyDescriptions.ListOfEnemies.Count;
        _enemyPool.Init(enemyDescriptions.ListOfEnemies.Count);
    }


    public void CreateMonsters(int levelOfGame, int countMonsters, WaveStarter waveStarter){
        for (int i = 0; i < countMonsters; i++){
            var levelOfEnemy = Random.Range(0, Mathf.Clamp(levelOfGame, 1, countOfList));
            CreateEnemy(levelOfEnemy, waveStarter);
        }
    }
}