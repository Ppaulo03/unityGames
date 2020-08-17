using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public QuestObject[] quests;
    public bool[] questCompleted;
    public DialogManager dialogManager;

    public string itemCollected;
    public List<string> enemyKilled = new List<string>();

    void Start(){
        questCompleted = new bool[quests.Length];
    }

    void LateUpdate(){
        enemyKilled = new List<string>();
    }

    public void showQuestText(string[] questText){
        dialogManager.showBox(questText);
        dialogManager.dText.text = questText[0];
        dialogManager.currentLine = 0;
    }
    public void kill(string name){
        enemyKilled.Add(name);
    }

}
