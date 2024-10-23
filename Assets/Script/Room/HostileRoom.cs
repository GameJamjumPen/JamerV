using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HostileRoom : Room
{
    public GameObject roomNumber;
    public List<List<EnemyType.Enemytype> > enemyTypes = new List<List<EnemyType.Enemytype> >();
    public int minscore;
    public int maxscore;
    public RoomType roomType;
    public Sprite _background;
    [Scene]
    public string combatScene;
    public void Awake(){
        SetEnemyWaves();
       // Debug.Log("HELOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
    }
    public override void OnPlayerAttack()
    {
        SetEnemyWaves();
        //Debug.Log("HELOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        Paper.Instance.SetScore(UnityEngine.Random.Range(minscore, maxscore));
        Paper.Instance.SetCard(GameManager.singleton.cardSOs);
        Paper.Instance.SetBackground(_background);
        Paper.Instance.enemyTypes = this.enemyTypes;
        Debug.Log("Size of enemytypes?? = " + enemyTypes.Count.ToString());
    }

    // New function to generate a wave of enemies
    private List<List<EnemyType.Enemytype>> GenerateEnemyWaves(EnemyType.Enemytype[] wave1, EnemyType.Enemytype[] wave2, EnemyType.Enemytype[] wave3)
    {
        List<List<EnemyType.Enemytype>> sth =  new List<List<EnemyType.Enemytype>> {
            new List<EnemyType.Enemytype>(wave1), // First wave
            new List<EnemyType.Enemytype>(wave2), // Second wave
            new List<EnemyType.Enemytype>(wave3)  // Third wave
        };
        Debug.Log("Size of gen" + sth.Count.ToString());
        return sth;
    }
    private List<List<EnemyType.Enemytype>> GenerateEnemyWaves(EnemyType.Enemytype[] wave1)
    {
        List<List<EnemyType.Enemytype>> sth =  new List<List<EnemyType.Enemytype>> {
            new List<EnemyType.Enemytype>(wave1), // First wave
        };
        Debug.Log("Size of gen" + sth.Count.ToString());
        return sth;
    }
    private void SetEnemyWaves()
    {
        int roomNum = int.Parse(roomNumber.name);
        Debug.Log("Room numbe is = " + roomNum.ToString());
        // Switch case for specific room numbers
        switch (roomNum)
        {
            case 1: //boss
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Prince}
                );
                break;
            case 3: //hard
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee,EnemyType.Enemytype.Bee,EnemyType.Enemytype.Bee},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee, EnemyType.Enemytype.Bee}
                );
                break;
            case 4: //easy
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish }
                );
                break;
            case 5: //medium
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Folk, EnemyType.Enemytype.Knive, EnemyType.Enemytype.Spoon },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Knive, EnemyType.Enemytype.Spoon, EnemyType.Enemytype.Spoon },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Knive, EnemyType.Enemytype.Spoon }
                );
                break;
            case 8: //Hard
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee, EnemyType.Enemytype.Bee},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee, EnemyType.Enemytype.Bee},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee, EnemyType.Enemytype.Bee}
                );
                break;
            case 17: //medium
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Spoon, EnemyType.Enemytype.Prince, EnemyType.Enemytype.Folk },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Bee, EnemyType.Enemytype.Knive },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Bee, EnemyType.Enemytype.Prince, EnemyType.Enemytype.Fish }
                );
                break;
            case 15: //easy
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish }
                );
                break;
            case 16: //easy
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish }
                );
                break;
            case 20: //easy
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish},
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish, EnemyType.Enemytype.Fish }
                );
                break;
            case 19: //medium
                enemyTypes = GenerateEnemyWaves(
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Knive, EnemyType.Enemytype.Spoon, EnemyType.Enemytype.Folk },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Knive, EnemyType.Enemytype.Spoon, EnemyType.Enemytype.Folk },
                    new EnemyType.Enemytype[] { EnemyType.Enemytype.Folk, EnemyType.Enemytype.Spoon }
                );
                break;
            default:
                Debug.Log("Bibo bruh skibidi");
                enemyTypes = new List<List<EnemyType.Enemytype>>(); // Empty list if room number doesn't match
                break;
        }
    }
}

public enum RoomType{Easy, Medium , Hard, Boss }