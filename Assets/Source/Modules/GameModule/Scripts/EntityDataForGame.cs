public class EntityDataForGame
{
    internal RuneBoard RuneBoard { get; private set; }
    internal UserSkillPerformer UserSkillPerformer { get; private set; }
    internal UserSkillScreen UserSkillScreen { get; private set; }
    internal ISaver Saver { get; private set; }
    internal FinalGameHandler FinalGameHandler { get; private set; }
    internal TaskFactory TaskFactory { get; private set; }

    internal void TakeModules(RuneBoard runeBoard, UserSkillPerformer userSkillPerformer)
    {
        RuneBoard = runeBoard;
        UserSkillPerformer = userSkillPerformer;
    }

    internal void Take(ISaver saver, FinalGameHandler finalGameHandler, UserSkillScreen userSkillScreen, TaskFactory taskFactory)
    {
        Saver = saver;
        FinalGameHandler = finalGameHandler;
        UserSkillScreen = userSkillScreen;
        TaskFactory = taskFactory;
    }
}