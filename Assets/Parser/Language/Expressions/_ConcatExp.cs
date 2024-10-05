
using System.Diagnostics.Contracts;

namespace Parser.Language
{

    public class ConcatExp : StringExp, IDoubleOperationExp<string, string>
    {
        public ConcatExp(IOperationExp<string> left, IOperationExp<string> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<string> Left { get; private set; }

        public IOperationExp<string> Rigth { get; private set; }

        public override string Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) + Rigth.Execute(blockContext);
        }
    }

    public class ConcatExp2 : StringExp, IDoubleOperationExp<string, string>
    {
        public ConcatExp2(IOperationExp<string> left, IOperationExp<string> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<string> Left { get; private set; }

        public IOperationExp<string> Rigth { get; private set; }

        public override string Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) + " " + Rigth.Execute(blockContext);
        }
    }
}