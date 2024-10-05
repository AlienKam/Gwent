using System;
using Parser.Language;
using Parser.Tokenstools;

namespace Parser
{

    public class AritmeticalParser : BaseParser
    {
        // BE -> TX
        // X -> ||TX | &&TX | epsilon
        // T -> FY | True | False | (BE)
        // Y -> >F | <F | ==F
        // F -> AE

        // AE -> TX
        // X -> +TX | -TX | epsilon
        // T -> FY
        // Y -> *FY | /FY | epsilon
        // F -> n | (AE)

        public override Node Parse(IToken[] tokens)
        {
            throw new NotImplementedException();
        }
    }
}