REM Generate the Antlr runtime files with the AST and visitor
REM .\Protobuf3.g4 was downloaded from
REM https://github.com/antlr/grammars-v4/blob/master/protobuf3/Protobuf3.g4

java -cp antlr-4.13.1-complete.jar org.antlr.v4.Tool .\Protobuf3.g4 -Dlanguage=CSharp -o .. -listener -visitor -package ProtoBuf.Antlr