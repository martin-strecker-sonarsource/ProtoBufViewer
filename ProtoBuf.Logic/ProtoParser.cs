﻿using Antlr4.Runtime;
using ProtoBuf.Antlr;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBuf.Logic
{
    public static class ProtoParser
    {
        public static ProtoContext ParseFile(string fileName) =>
            ParseFile(new FileStream(fileName, FileMode.Open, FileAccess.Read));

        public static ProtoContext ParseFile(Stream stream)
        {
            var tokenStream = new BufferedTokenStream(new Protobuf3Lexer(CharStreams.fromStream(stream)));
            var parser = new Protobuf3Parser(tokenStream);
            var proto = parser.proto();
            return proto;
        }
    }
}
