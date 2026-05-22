internal class EntityDataForMenu
{
    public EntityDataForMenu(IGame game, IUserSkillHandler userSkillHandler, ISkillCardDiscoverer skillCardDiscoverer)
    {
        Game = game;
        UserSkillHandler = userSkillHandler;
        SkillCardDiscoverer = skillCardDiscoverer;
    }

    internal Saver Saver { get; private set; }
    internal IGame Game { get; private set; }
    internal IUserSkillHandler UserSkillHandler { get; private set; }
    internal ISkillCardDiscoverer SkillCardDiscoverer { get; private set; }
    internal LeaderBoard LeaderBoard { get; private set; }

    internal void TakeYandexEntities(Saver saver, LeaderBoard leaderBoard)
    {
        Saver = saver;
        LeaderBoard = leaderBoard;
    }
}