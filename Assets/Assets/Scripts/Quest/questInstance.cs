[System.Serializable]
public class QuestInstance
{
    public QuestData data;
    public QuestState state;

    public QuestInstance(QuestData questData)
    {
        data = questData;
        state = QuestState.NotStarted;
    }
}