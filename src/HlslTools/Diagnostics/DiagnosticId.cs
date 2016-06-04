﻿namespace HlslTools.Diagnostics
{
    public enum DiagnosticId
    {
        IllegalInputCharacter,
        UnterminatedComment,
        UnterminatedString,

        InvalidInteger,
        InvalidReal,
        InvalidOctal,
        InvalidHex,
        NumberTooLarge,
        TokenExpected,
        TokenExpectedMultipleChoices,
        TokenUnexpected,

        EndIfDirectiveExpected,
        UnexpectedDirective,
        DirectiveExpected,
        AlreadyDefined,
        BadDirectivePlacement,
        EndOfPreprocessorLineExpected,
        MissingPreprocessorFile,
        IncludeNotFound,
        NotEnoughMacroParameters,
        UnexpectedAttribute,

        InvalidExprTerm,

        NoVoidHere,
        NoVoidParameter,
        ExpressionExpected,
        TypeExpected,

        BadEmbeddedStatement,
        ConstantExpected,

        UndeclaredType,
        UndeclaredFunction,
        UndeclaredMethod,
        UndeclaredIndexer,
        UndeclaredVariable,
        UndeclaredField,
        AmbiguousInvocation,
        AmbiguousReference,
        AmbiguousType,
        AmbiguousField,
        CannotConvert,
        InvocationRequiresParenthesis,
        CannotApplyUnaryOperator,
        CannotApplyBinaryOperator,
        AmbiguousUnaryOperator,
        AmbiguousBinaryOperator,
        SymbolRedefined,
        FunctionMissingImplementation,
        LoopControlVariableConflict,
        ImplicitTruncation,
        UndeclaredNamespaceOrType,
        AmbiguousNamespaceOrType,
        UndeclaredFunctionInNamespaceOrClass
    }
}