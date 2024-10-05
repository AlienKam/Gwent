
using Parser.Language;
using Parser.Tokenstools;

namespace Parser
{

    public abstract class BaseParser : IParser<Node, IToken>
    {
        public virtual Node Parse(IToken[] tokens) { return default; }
    }
}