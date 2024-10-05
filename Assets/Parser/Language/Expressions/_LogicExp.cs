
namespace Parser.Language
{

    public abstract class DoubleLogicExp : IOperationExp<bool>, IDoubleOperationExp<bool, bool>
    {
        public DoubleLogicExp(IOperationExp<bool> left, IOperationExp<bool> rigth)
        {
            Left = left;
            Rigth = rigth;
        }

        public IOperationExp<bool> Left { get; protected set; }
        public IOperationExp<bool> Rigth { get; protected set; }

        public abstract bool Execute(IBlockContext blockContext);
    }

    public class AndExp : DoubleLogicExp
    {
        public AndExp(IOperationExp<bool> left, IOperationExp<bool> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) && Rigth.Execute(blockContext);
        }
    }

    public class OrExp : DoubleLogicExp
    {
        public OrExp(IOperationExp<bool> left, IOperationExp<bool> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) || Rigth.Execute(blockContext);
        }
    }

    public abstract class SimpleLogicExp : IOperationExp<bool>, ISimpleOperationExp<bool, bool>
    {
        protected IOperationExp<bool> operation;

        protected SimpleLogicExp(IOperationExp<bool> operation)
        {
            this.operation = operation;
        }

        public IOperationExp<bool> Operation => operation;

        public abstract bool Execute(IBlockContext blockContext);
    }

    public class NotExp : SimpleLogicExp
    {
        public NotExp(IOperationExp<bool> operation) : base(operation) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return !Operation.Execute(blockContext);
        }
    }
}