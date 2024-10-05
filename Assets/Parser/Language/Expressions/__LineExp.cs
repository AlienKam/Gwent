
using System;
using System.Collections.Generic;
using System.Runtime;
using static Parser.Language.CallMethods;

namespace Parser.Language
{
    public class BlockExp : Exp
    {
        private List<LineExp> lines;

        public BlockExp(List<LineExp> lines)
        {
            this.lines = lines;
        }

        public override void Execute(IBlockContext blockContext)
        {
            foreach (LineExp exp in lines)
            {
                exp.Execute(blockContext);
            }
        }
    }

    public abstract class LineExp : Exp
    {
        public override void Execute(IBlockContext blockContext) { }
    }

    public class Conditional : LineExp
    {
        public Conditional(IOperationExp<bool> boolean, BlockExp ifTrue, BlockExp ifFalse = null)
        {
            booleanExp = boolean;
            this.ifTrue = ifTrue;
            this.ifFalse = ifFalse;
        }

        IOperationExp<bool> booleanExp { get; set; }
        BlockExp ifTrue { get; set; }
        BlockExp ifFalse { get; set; }

        public override void Execute(IBlockContext blockContext)
        {
            if (booleanExp.Execute(blockContext))
            {
                ifTrue.Execute(blockContext);
            }
            else if (ifFalse != null)
            {
                ifFalse?.Execute(blockContext);
            }
        }
    }

    public class WhileExp : LineExp
    {
        public WhileExp(IOperationExp<bool> booleanExp, BlockExp block)
        {
            this.booleanExp = booleanExp;
            this.block = block;
        }

        IOperationExp<bool> booleanExp { get; set; }
        BlockExp block { get; set; }

        public override void Execute(IBlockContext blockContext)
        {
            while (booleanExp.Execute(blockContext))
            {
                block.Execute(blockContext);
            }
        }
    }

    public class ForExp : LineExp
    {
        IOperationExp<IEnumerable<IContextCard>> list;
        BlockExp block;
        string name;

        public ForExp(string name, IOperationExp<IEnumerable<IContextCard>> list, BlockExp block)
        {
            this.name = name;
            this.list = list;
            this.block = block;
        }


        public override void Execute(IBlockContext blockContext)
        {
            foreach (IContextCard contextCard in list.Execute(blockContext))
            {
                blockContext.LocalVar[name] = (VarType.Card, contextCard);
                block.Execute(blockContext);
            }
            blockContext.LocalVar.Remove(name);
        }
    }

    public class DeclarationVarExp : LineExp
    {
        public string Name { get; protected set; }
        public IOperationExp<(VarType type, object value)> ValueExp { get; protected set; }

        public DeclarationVarExp(string name, IOperationExp<(VarType type, object value)> exp)
        {
            Name = name;
            ValueExp = exp;
        }

        public override void Execute(IBlockContext blockContext)
        {
            (VarType type, object value) value = ValueExp.Execute(blockContext);
            (VarType type, object value) dicValue;
            if (blockContext.LocalVar.TryGetValue(Name, out dicValue) || blockContext.GlobalVar.TryGetValue(Name, out dicValue))
            {
                if (!dicValue.type.Equals(value.type))
                {
                    throw new ArgumentException("");
                }
                dicValue.value = value.value;
            }
            else
            {
                blockContext.LocalVar[Name] = value;
            }
        }
    }

    public class IndexDecl : DeclarationVarExp
    {
        public CallMethods callList { get; protected set; }
        public IOperationExp<double> index { get; protected set; }

        public IndexDecl(CallMethods callList, IOperationExp<double> index, string name, IOperationExp<(VarType type, object value)> valueExp) : base(name, valueExp)
        {
            this.callList = callList;
            this.index = index;
        }

        public override void Execute(IBlockContext blockContext)
        {
            List<IContextCard> list = callList.Execute(blockContext);
            var value = ValueExp.Execute(blockContext);
            switch (Name)
            {
                case "Power":
                    list[(int)index.Execute(blockContext)].Power = (double)value.value;
                    break;

                case "":
                    list[(int)index.Execute(blockContext)] = (IContextCard)value.value;
                    break;
                default:
                    throw new Exception("");
            }
        }
    }

    public class VoidMethodDef : LineExp
    {
        public CallMethods callList { get; protected set; }
        public string method { get; protected set; }

        public IOperationExp<IContextCard> exp { get; protected set; }

        public VoidMethodDef(CallMethods callList, string method, IOperationExp<IContextCard> exp)
        {
            this.callList = callList;
            this.method = method;
            this.exp = exp;
        }

        public override void Execute(IBlockContext blockContext)
        {
            var list = callList.Execute(blockContext);
            var card = exp != null ? exp.Execute(blockContext) : default;
            int player = (int)callList.player.Execute(blockContext);
            TypeList typeList = callList.method;

            switch (method)
            {
                case "Shuffle":
                    blockContext.context.Shuffle(list, typeList, player);
                    break;
                case "Push":
                    blockContext.context.Push(list, typeList, card, player);
                    break;
                case "Remove":
                    blockContext.context.Remove(list, typeList, card, player);
                    break;
                case "SendButton":
                    blockContext.context.SendButton(list, typeList, card, player);
                    break;
            }
        }
    }
}
