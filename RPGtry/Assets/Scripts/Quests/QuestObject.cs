using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    public int questNumber;
    public QuestManager questManager;
    
    public bool isItemQuest;
    public string targetItem;

    public bool isEnemyQuest;
    public string targetEnemy;
    public int numEnemyToKill;
    public int enemyKillCount;


    public string[] StartText;
    public string[] EndText;

    void Update()
    {
        if(isItemQuest){
            if(questManager.itemCollected == targetItem){
                questManager.itemCollected = null;
                EndQuest();
            }
        }
        else if(isEnemyQuest){
            for(int n = 0; n < questManager.enemyKilled.Count; n++){
                if(questManager.enemyKilled[n] == targetEnemy){
                    enemyKillCount ++;
                    if(enemyKillCount >= numEnemyToKill){
                        EndQuest();
                        break;
                    }
                }           
            }
        }
    }
    
    public void StartQuest(){
        questManager.showQuestText(StartText);
    }
    public void EndQuest(){
        questManager.showQuestText(EndText);
        questManager.questCompleted[questNumber] = true;
        gameObject.SetActive(false);
    }

}
