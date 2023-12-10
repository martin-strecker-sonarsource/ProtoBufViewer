using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Protobuf3Parser;

namespace ProtoBuf.Logic
{
    public static class ProtoParser
    {
        public static ProtoContext ParseFile(string fileName)
        {
            var tokenStream = new BufferedTokenStream(new Protobuf3Lexer(CharStreams.fromPath(fileName)));
            var parser = new Protobuf3Parser(tokenStream);
            var proto = parser.proto();
            return proto;
        }
    }
}
