
using System;

namespace Parser.Language
{
    public abstract class CompareExp<T> : BooleanExp, IDoubleOperationExp<bool, T>
    {
        public CompareExp(IOperationExp<T> left, IOperationExp<T> rigth)
        {
            Left = left;
            Rigth = rigth;
        }

        public IOperationExp<T> Left { get; protected set; }
        public IOperationExp<T> Rigth { get; protected set; }
    }

    public class EqualsExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public EqualsExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) == 0;
        }
    }

    public class DistExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public DistExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) != 0;
        }
    }

    public class LessExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public LessExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) < 0;
        }
    }

    public class LessEqualExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public LessEqualExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) <= 0;
        }
    }

    public class GreaterExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public GreaterExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) > 0;
        }
    }

    public class GreaterEqualExp<T> : CompareExp<T> where T: IComparable<T>
    {
        public GreaterEqualExp(IOperationExp<T> left, IOperationExp<T> rigth) : base(left, rigth) { }

        public override bool Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext).CompareTo(Rigth.Execute(blockContext)) >= 0;
        }
    }
}