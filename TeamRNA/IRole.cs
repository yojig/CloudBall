using Common;

namespace TeamRNA
{
    public interface IRole
    {
        PlayerType Type { get; }
        
        void DoAction();
    }
}