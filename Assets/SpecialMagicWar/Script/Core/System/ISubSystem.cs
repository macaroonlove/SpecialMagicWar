namespace SpecialMagicWar.Core
{
    public interface ISubSystem
    {
        void Initialize();
        void Deinitialize();
    }

    public interface ICoreSystem : ISubSystem { }
    public interface IBattleSystem : ISubSystem { }
}