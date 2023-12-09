//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./Protobuf3.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="Protobuf3Parser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IProtobuf3Listener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.proto"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProto([NotNull] Protobuf3Parser.ProtoContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.proto"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProto([NotNull] Protobuf3Parser.ProtoContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.syntax"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSyntax([NotNull] Protobuf3Parser.SyntaxContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.syntax"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSyntax([NotNull] Protobuf3Parser.SyntaxContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.importStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterImportStatement([NotNull] Protobuf3Parser.ImportStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.importStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitImportStatement([NotNull] Protobuf3Parser.ImportStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.packageStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPackageStatement([NotNull] Protobuf3Parser.PackageStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.packageStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPackageStatement([NotNull] Protobuf3Parser.PackageStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.optionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOptionStatement([NotNull] Protobuf3Parser.OptionStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.optionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOptionStatement([NotNull] Protobuf3Parser.OptionStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.optionName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOptionName([NotNull] Protobuf3Parser.OptionNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.optionName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOptionName([NotNull] Protobuf3Parser.OptionNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fieldLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldLabel([NotNull] Protobuf3Parser.FieldLabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fieldLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldLabel([NotNull] Protobuf3Parser.FieldLabelContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterField([NotNull] Protobuf3Parser.FieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.field"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitField([NotNull] Protobuf3Parser.FieldContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fieldOptions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldOptions([NotNull] Protobuf3Parser.FieldOptionsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fieldOptions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldOptions([NotNull] Protobuf3Parser.FieldOptionsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fieldOption"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldOption([NotNull] Protobuf3Parser.FieldOptionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fieldOption"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldOption([NotNull] Protobuf3Parser.FieldOptionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fieldNumber"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldNumber([NotNull] Protobuf3Parser.FieldNumberContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fieldNumber"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldNumber([NotNull] Protobuf3Parser.FieldNumberContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.oneof"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOneof([NotNull] Protobuf3Parser.OneofContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.oneof"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOneof([NotNull] Protobuf3Parser.OneofContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.oneofField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOneofField([NotNull] Protobuf3Parser.OneofFieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.oneofField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOneofField([NotNull] Protobuf3Parser.OneofFieldContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.mapField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMapField([NotNull] Protobuf3Parser.MapFieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.mapField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMapField([NotNull] Protobuf3Parser.MapFieldContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.keyType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKeyType([NotNull] Protobuf3Parser.KeyTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.keyType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKeyType([NotNull] Protobuf3Parser.KeyTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.type_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType_([NotNull] Protobuf3Parser.Type_Context context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.type_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType_([NotNull] Protobuf3Parser.Type_Context context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.reserved"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReserved([NotNull] Protobuf3Parser.ReservedContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.reserved"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReserved([NotNull] Protobuf3Parser.ReservedContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.ranges"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRanges([NotNull] Protobuf3Parser.RangesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.ranges"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRanges([NotNull] Protobuf3Parser.RangesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.range_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRange_([NotNull] Protobuf3Parser.Range_Context context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.range_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRange_([NotNull] Protobuf3Parser.Range_Context context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.reservedFieldNames"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReservedFieldNames([NotNull] Protobuf3Parser.ReservedFieldNamesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.reservedFieldNames"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReservedFieldNames([NotNull] Protobuf3Parser.ReservedFieldNamesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.topLevelDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTopLevelDef([NotNull] Protobuf3Parser.TopLevelDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.topLevelDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTopLevelDef([NotNull] Protobuf3Parser.TopLevelDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumDef([NotNull] Protobuf3Parser.EnumDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumDef([NotNull] Protobuf3Parser.EnumDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumBody([NotNull] Protobuf3Parser.EnumBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumBody([NotNull] Protobuf3Parser.EnumBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumElement([NotNull] Protobuf3Parser.EnumElementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumElement([NotNull] Protobuf3Parser.EnumElementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumField([NotNull] Protobuf3Parser.EnumFieldContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumField"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumField([NotNull] Protobuf3Parser.EnumFieldContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumValueOptions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumValueOptions([NotNull] Protobuf3Parser.EnumValueOptionsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumValueOptions"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumValueOptions([NotNull] Protobuf3Parser.EnumValueOptionsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumValueOption"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumValueOption([NotNull] Protobuf3Parser.EnumValueOptionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumValueOption"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumValueOption([NotNull] Protobuf3Parser.EnumValueOptionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.messageDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageDef([NotNull] Protobuf3Parser.MessageDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.messageDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageDef([NotNull] Protobuf3Parser.MessageDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.messageBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageBody([NotNull] Protobuf3Parser.MessageBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.messageBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageBody([NotNull] Protobuf3Parser.MessageBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.messageElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageElement([NotNull] Protobuf3Parser.MessageElementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.messageElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageElement([NotNull] Protobuf3Parser.MessageElementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.extendDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExtendDef([NotNull] Protobuf3Parser.ExtendDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.extendDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExtendDef([NotNull] Protobuf3Parser.ExtendDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.serviceDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterServiceDef([NotNull] Protobuf3Parser.ServiceDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.serviceDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitServiceDef([NotNull] Protobuf3Parser.ServiceDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.serviceElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterServiceElement([NotNull] Protobuf3Parser.ServiceElementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.serviceElement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitServiceElement([NotNull] Protobuf3Parser.ServiceElementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.rpc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRpc([NotNull] Protobuf3Parser.RpcContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.rpc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRpc([NotNull] Protobuf3Parser.RpcContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstant([NotNull] Protobuf3Parser.ConstantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstant([NotNull] Protobuf3Parser.ConstantContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.blockLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBlockLit([NotNull] Protobuf3Parser.BlockLitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.blockLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBlockLit([NotNull] Protobuf3Parser.BlockLitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.emptyStatement_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEmptyStatement_([NotNull] Protobuf3Parser.EmptyStatement_Context context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.emptyStatement_"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEmptyStatement_([NotNull] Protobuf3Parser.EmptyStatement_Context context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.ident"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdent([NotNull] Protobuf3Parser.IdentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.ident"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdent([NotNull] Protobuf3Parser.IdentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fullIdent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFullIdent([NotNull] Protobuf3Parser.FullIdentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fullIdent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFullIdent([NotNull] Protobuf3Parser.FullIdentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.messageName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageName([NotNull] Protobuf3Parser.MessageNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.messageName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageName([NotNull] Protobuf3Parser.MessageNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumName([NotNull] Protobuf3Parser.EnumNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumName([NotNull] Protobuf3Parser.EnumNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.fieldName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldName([NotNull] Protobuf3Parser.FieldNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.fieldName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldName([NotNull] Protobuf3Parser.FieldNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.oneofName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOneofName([NotNull] Protobuf3Parser.OneofNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.oneofName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOneofName([NotNull] Protobuf3Parser.OneofNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.mapName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMapName([NotNull] Protobuf3Parser.MapNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.mapName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMapName([NotNull] Protobuf3Parser.MapNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.serviceName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterServiceName([NotNull] Protobuf3Parser.ServiceNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.serviceName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitServiceName([NotNull] Protobuf3Parser.ServiceNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.rpcName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterRpcName([NotNull] Protobuf3Parser.RpcNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.rpcName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitRpcName([NotNull] Protobuf3Parser.RpcNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.messageType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageType([NotNull] Protobuf3Parser.MessageTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.messageType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageType([NotNull] Protobuf3Parser.MessageTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.enumType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEnumType([NotNull] Protobuf3Parser.EnumTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.enumType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEnumType([NotNull] Protobuf3Parser.EnumTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.intLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIntLit([NotNull] Protobuf3Parser.IntLitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.intLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIntLit([NotNull] Protobuf3Parser.IntLitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.strLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStrLit([NotNull] Protobuf3Parser.StrLitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.strLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStrLit([NotNull] Protobuf3Parser.StrLitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.boolLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolLit([NotNull] Protobuf3Parser.BoolLitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.boolLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolLit([NotNull] Protobuf3Parser.BoolLitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.floatLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFloatLit([NotNull] Protobuf3Parser.FloatLitContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.floatLit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFloatLit([NotNull] Protobuf3Parser.FloatLitContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Protobuf3Parser.keywords"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterKeywords([NotNull] Protobuf3Parser.KeywordsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Protobuf3Parser.keywords"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitKeywords([NotNull] Protobuf3Parser.KeywordsContext context);
}
