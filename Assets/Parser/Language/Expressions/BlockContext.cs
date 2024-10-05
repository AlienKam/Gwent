

using System.Collections.Generic;

namespace Parser.Language
{

    public class BlockContext : IBlockContext
    {
        public BlockContext() { }

        public BlockContext(Dictionary<string, (VarType type, object value)> globalVar)
        {
            GlobalVar = globalVar;
        }

        public BlockContext(IEnumerable<IContextCard> targets, IContext context, IDictionary<string, (VarType type, object value)> GlobalVar)
        {
            this.targets = targets;
            this.context = context;
            this.GlobalVar = GlobalVar;
            LocalVar = new Dictionary<string, (VarType type, object value)>();
        }

        public IDictionary<string, (VarType type, object value)> LocalVar { get; private set; }

        public IDictionary<string, (VarType type, object value)> GlobalVar { get; private set; }

        public IContext context { get; private set; }

        public IEnumerable<IContextCard> targets { get; private set; }
    }
}