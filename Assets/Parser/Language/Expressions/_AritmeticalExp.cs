
using System;

namespace Parser.Language
{
    public class PlusExp : ArithmeticExp, IDoubleOperationExp<double, double>
    {
        public PlusExp(IOperationExp<double> left, IOperationExp<double> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<double> Left { get; private set; }
        public IOperationExp<double> Rigth { get; private set; }

        public override double Execute(IBlockContext blockContext)
        {
            
            return Left.Execute(blockContext) + Rigth.Execute(blockContext);
        }
    }

    public class MinusExp : ArithmeticExp, IDoubleOperationExp<double, double>
    {
        public MinusExp(IOperationExp<double> left, IOperationExp<double> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<double> Left { get; }
        public IOperationExp<double> Rigth { get; }

        public override double Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) - Rigth.Execute(blockContext);
        }
    }

    public class MultiplyExp : ArithmeticExp, IDoubleOperationExp<double, double>
    {
        public MultiplyExp(IOperationExp<double> left, IOperationExp<double> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<double> Left { get; }
        public IOperationExp<double> Rigth { get; }

        public override double Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) * Rigth.Execute(blockContext);
        }
    }

    public class DivideExp : ArithmeticExp, IDoubleOperationExp<double, double>
    {
        public DivideExp(IOperationExp<double> left, IOperationExp<double> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<double> Left { get; }
        public IOperationExp<double> Rigth { get; }

        public override double Execute(IBlockContext blockContext)
        {
            return Left.Execute(blockContext) / Rigth.Execute(blockContext);
        }
    }

    public class PowExp : ArithmeticExp, IDoubleOperationExp<double, double>
    {
        public PowExp(IOperationExp<double> left, IOperationExp<double> right)
        {
            Left = left;
            Rigth = right;
        }

        public IOperationExp<double> Left { get; }
        public IOperationExp<double> Rigth { get; }

        public override double Execute(IBlockContext blockContext)
        {
            return Math.Pow(Left.Execute(blockContext), Rigth.Execute(blockContext));
        }
    }
}