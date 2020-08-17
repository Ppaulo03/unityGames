using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    private QuestManager questManager;

    public int questNumber;
    public bool startQuest;
    public bool endQuest;

    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Player"){
            if(!questManager.questCompleted[questNumber]){
                if(startQuest && !questManager.quests[questNumber].gameObject.activeSelf){
                    questManager.quests[questNumber].gameObject.SetActive(true);
                    questManager.quests[questNumber].StartQuest();
                }
                if(endQuest && questManager.quests[questNumber].gameObject.activeSelf){
                    questManager.quests[questNumber].gameObject.SetActive(false);
                    questManager.quests[questNumber].EndQuest();
                }

            }
        }
    }
}
