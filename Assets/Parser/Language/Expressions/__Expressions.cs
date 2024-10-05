
namespace Parser.Language
{
    public abstract class Exp
    {
        public Exp() { }

        public abstract void Execute(IBlockContext blockContext);
    }
}