using Common;

namespace TeamRNA
{
    public interface IRole
    {
        void DoAction(Player self, Pitch pitch);
    }
}