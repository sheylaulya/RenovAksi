using System.Collections.Generic;
using UnityEngine;

namespace SaveGame.Data
{
    [System.Serializable]

    public class SaveData
    {
        public Vector3 playerPosition;
        public List<QuestSaveData> quests = new();
        public bool isTutorialCompleted;
        public List<InventorySaveData> inventorySaveDatas;
    }
}

