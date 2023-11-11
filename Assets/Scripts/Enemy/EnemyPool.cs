using System.Collections.Generic;

public class EnemyPool{
    readonly Dictionary<int, Queue<Enemy>> _enemyDictionary = new();

    public void Init(int countOfEnemies){
        for (int i = 0; i < countOfEnemies; i++){
            _enemyDictionary[i] = new Queue<Enemy>();
        }
    }

    public void HideEnemy(Enemy enemy){
        Queue<Enemy> queue = _enemyDictionary[enemy.GetLevel()];
        queue.Enqueue(enemy);
        enemy.gameObject.SetActive(false);
    }

    public Enemy GetEnemy(int level){
        if (_enemyDictionary[level].TryDequeue(out var enemy)){
            enemy.gameObject.SetActive(true);
        }

        return enemy;
    }
}