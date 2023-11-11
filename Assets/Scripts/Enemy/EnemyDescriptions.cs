using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDescriptions", menuName = "EnemyDescriptions", order = 0)]
public class EnemyDescriptions : ScriptableObject{
    [SerializeField] private List<EnemyDescription> listOfEnemies;

    public List<EnemyDescription> ListOfEnemies => listOfEnemies;
}