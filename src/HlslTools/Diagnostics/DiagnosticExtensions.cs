﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HlslTools.Properties;
using HlslTools.Symbols;
using HlslTools.Syntax;
using HlslTools.Text;

namespace HlslTools.Diagnostics
{
    internal static class DiagnosticExtensions
    {
        public static string GetMessage(this DiagnosticId diagnosticId)
        {
            return Resources.ResourceManager.GetString(diagnosticId.ToString());
        }

        public static void Report(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, DiagnosticId diagnosticId, params object[] args)
        {
            var diagnostic = Diagnostic.Format(textSpan, diagnosticId, args);
            diagnostics.Add(diagnostic);
        }

        #region Lexer errors

        public static void ReportIllegalInputCharacter(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, char character)
        {
            diagnostics.Report(textSpan, DiagnosticId.IllegalInputCharacter, character);
        }

        public static void ReportUnterminatedComment(this ICollection<Diagnostic> diagnostics, TextSpan textSpan)
        {
            diagnostics.Report(textSpan, DiagnosticId.UnterminatedComment);
        }

        public static void ReportUnterminatedString(this ICollection<Diagnostic> diagnostics, TextSpan textSpan)
        {
            diagnostics.Report(textSpan, DiagnosticId.UnterminatedString);
        }

        public static void ReportInvalidInteger(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, string tokenText)
        {
            diagnostics.Report(textSpan, DiagnosticId.InvalidInteger, tokenText);
        }

        public static void ReportInvalidReal(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, string tokenText)
        {
            diagnostics.Report(textSpan, DiagnosticId.InvalidReal, tokenText);
        }

        public static void ReportInvalidOctal(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, string tokenText)
        {
            diagnostics.Report(textSpan, DiagnosticId.InvalidOctal, tokenText);
        }

        public static void ReportInvalidHex(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, string tokenText)
        {
            diagnostics.Report(textSpan, DiagnosticId.InvalidHex, tokenText);
        }

        public static void ReportNumberTooLarge(this ICollection<Diagnostic> diagnostics, TextSpan textSpan, string tokenText)
        {
            diagnostics.Report(textSpan, DiagnosticId.NumberTooLarge, tokenText);
        }

        #endregion

        #region Parser errors

        public static void ReportTokenExpected(this ICollection<Diagnostic> diagnostics, TextSpan span, SyntaxToken actual, SyntaxKind expected)
        {
            var actualText = actual.GetDisplayText();
            var expectedText = expected.GetDisplayText();
            diagnostics.Report(span, DiagnosticId.TokenExpected, actualText, expectedText);
        }

        public static void ReportTokenExpectedMultipleChoices(this ICollection<Diagnostic> diagnostics, TextSpan span, SyntaxToken actual, IEnumerable<SyntaxKind> expected)
        {
            var actualText = actual.GetDisplayText();
            var expectedText = string.Join(",", expected.Select(x => $"'{x.GetDisplayText()}'"));
            diagnostics.Report(span, DiagnosticId.TokenExpectedMultipleChoices, actualText, expectedText);
        }

        public static void ReportTokenUnexpected(this ICollection<Diagnostic> diagnostics, TextSpan span, SyntaxToken actual)
        {
            var actualText = actual.GetDisplayText();
            diagnostics.Report(span, DiagnosticId.TokenUnexpected, actualText);
        }

        public static void ReportNoVoidHere(this ICollection<Diagnostic> diagnostics, TextSpan textSpan)
        {
            diagnostics.Report(textSpan, DiagnosticId.NoVoidHere);
        }

        public static void ReportNoVoidParameter(this ICollection<Diagnostic> diagnostics, TextSpan textSpan)
        {
            diagnostics.Report(textSpan, DiagnosticId.NoVoidParameter);
        }

        #endregion

        #region Semantic errors

        public static void ReportUndeclaredType(this ICollection<Diagnostic> diagnostics, SyntaxNode type)
        {
            diagnostics.Report(type.GetTextSpanSafe(), DiagnosticId.UndeclaredType, type.ToStringIgnoringMacroReferences());
        }

        public static void ReportUndeclaredFunction(this ICollection<Diagnostic> diagnostics, FunctionInvocationExpressionSyntax node, IEnumerable<TypeSymbol> argumentTypes)
        {
            var name = node.Name.ToStringIgnoringMacroReferences();
            var argumentTypeList = string.Join(@", ", argumentTypes.Select(t => t.ToDisplayName()));
            diagnostics.Report(node.GetTextSpanSafe(), DiagnosticId.UndeclaredFunction, name, argumentTypeList);
        }

        public static void ReportUndeclaredNumericConstructor(this ICollection<Diagnostic> diagnostics, NumericConstructorInvocationExpressionSyntax node, IEnumerable<TypeSymbol> argumentTypes)
        {
            var name = node.Type.ToStringIgnoringMacroReferences();
            var argumentTypeList = string.Join(@", ", argumentTypes.Select(t => t.ToDisplayName()));
            diagnostics.Report(node.GetTextSpanSafe(), DiagnosticId.UndeclaredFunction, name, argumentTypeList);
        }

        public static void ReportUndeclaredMethod(this ICollection<Diagnostic> diagnostics, MethodInvocationExpressionSyntax node, TypeSymbol declaringType, IEnumerable<TypeSymbol> argumentTypes)
        {
            var name = node.Name.ValueText;
            var declaringTypeName = declaringType.ToDisplayName();
            var argumentTypeNames = string.Join(@", ", argumentTypes.Select(t => t.ToDisplayName()));
            diagnostics.Report(node.GetTextSpanRoot(), DiagnosticId.UndeclaredMethod, declaringTypeName, name, argumentTypeNames);
        }

        public static void ReportUndeclaredFunctionInNamespaceOrClass(this ICollection<Diagnostic> diagnostics, QualifiedDeclarationNameSyntax name)
        {
            var declaringTypeName = name.Left.ToStringIgnoringMacroReferences();
            diagnostics.Report(name.GetTextSpanSafe(), DiagnosticId.UndeclaredFunctionInNamespaceOrClass, declaringTypeName, name.GetUnqualifiedName().Name.Text);
        }

        public static void ReportUndeclaredIndexer(this ICollection<Diagnostic> diagnostics, ElementAccessExpressionSyntax node, TypeSymbol declaringType, IEnumerable<TypeSymbol> argumentTypes)
        {
            var declaringTypeName = declaringType.ToDisplayName();
            var argumentTypeNames = string.Join(@", ", argumentTypes.Select(t => t.ToDisplayName()));
            diagnostics.Report(node.GetTextSpanRoot(), DiagnosticId.UndeclaredIndexer, declaringTypeName, argumentTypeNames);
        }

        public static void ReportVariableNotDeclared(this ICollection<Diagnostic> diagnostics, SyntaxToken name)
        {
            diagnostics.Report(name.Span, DiagnosticId.UndeclaredVariable, name.ValueText);
        }

        public static void ReportUndeclaredField(this ICollection<Diagnostic> diagnostics, FieldAccessExpressionSyntax node, TypeSymbol type)
        {
            var typeName = type.ToDisplayName();
            var propertyName = node.Name.ValueText;
            diagnostics.Report(node.GetTextSpanSafe(), DiagnosticId.UndeclaredField, typeName, propertyName);
        }

        public static void ReportUndeclaredNamespaceOrType(this ICollection<Diagnostic> diagnostics, QualifiedDeclarationNameSyntax node)
        {
            var typeName = node.Left.ToStringIgnoringMacroReferences();
            diagnostics.Report(node.GetTextSpanSafe(), DiagnosticId.UndeclaredNamespaceOrType, typeName);
        }

        public static void ReportAmbiguousInvocation(this ICollection<Diagnostic> diagnostics, TextSpan span, InvocableSymbol symbol1, InvocableSymbol symbol2, IReadOnlyList<TypeSymbol> argumentTypes)
        {
            if (argumentTypes.Count > 0)
            {
                var displayTypes = string.Join(@", ", argumentTypes.Select(t => t.ToDisplayName()));
                diagnostics.Report(span, DiagnosticId.AmbiguousInvocation, symbol1, symbol2, displayTypes);
            }
            else
            {
                var message = string.Format(CultureInfo.CurrentCulture, "Invocation is ambiguous between '{0}' and '{1}'.", symbol1, symbol2);
                var diagnostic = new Diagnostic(span, DiagnosticId.AmbiguousInvocation, message);
                diagnostics.Add(diagnostic);
            }
        }

        public static void ReportAmbiguousField(this ICollection<Diagnostic> diagnostics, SyntaxToken name)
        {
            diagnostics.Report(name.Span, DiagnosticId.AmbiguousField, name.ValueText);
        }

        public static void ReportCannotConvert(this ICollection<Diagnostic> diagnostics, TextSpan span, TypeSymbol sourceType, TypeSymbol targetType)
        {
            var sourceTypeName = sourceType.ToDisplayName();
            var targetTypeName = targetType.ToDisplayName();
            diagnostics.Report(span, DiagnosticId.CannotConvert, sourceTypeName, targetTypeName);
        }

        public static void ReportAmbiguousName(this ICollection<Diagnostic> diagnostics, SyntaxToken name, IReadOnlyList<Symbol> candidates)
        {
            var symbol1 = candidates[0];
            var symbol2 = candidates[1];
            diagnostics.Report(name.Span, DiagnosticId.AmbiguousReference, name.ValueText, symbol1.Name, symbol2.Name);
        }

        public static void ReportAmbiguousType(this ICollection<Diagnostic> diagnostics, SyntaxToken name, IReadOnlyList<Symbol> candidates)
        {
            var symbol1 = candidates[0];
            var symbol2 = candidates[1];
            diagnostics.Report(name.Span, DiagnosticId.AmbiguousType, name.ValueText, symbol1.Name, symbol2.Name);
        }

        public static void ReportAmbiguousNamespaceOrType(this ICollection<Diagnostic> diagnostics, QualifiedDeclarationNameSyntax syntax, IReadOnlyList<Symbol> candidates)
        {
            var symbol1 = candidates[0];
            var symbol2 = candidates[1];
            diagnostics.Report(syntax.GetTextSpanSafe(), DiagnosticId.AmbiguousNamespaceOrType, syntax.ToStringIgnoringMacroReferences(), symbol1.Name, symbol2.Name);
        }

        public static void ReportInvocationRequiresParenthesis(this ICollection<Diagnostic> diagnostics, SyntaxToken name)
        {
            diagnostics.Report(name.Span, DiagnosticId.InvocationRequiresParenthesis, name.ValueText);
        }

        public static void ReportCannotApplyBinaryOperator(this ICollection<Diagnostic> diagnostics, SyntaxToken operatorToken, TypeSymbol leftType, TypeSymbol rightType)
        {
            var operatorName = operatorToken.Text;
            var leftTypeName = leftType.ToDisplayName();
            var rightTypeName = rightType.ToDisplayName();
            diagnostics.Report(operatorToken.Span, DiagnosticId.CannotApplyBinaryOperator, operatorName, leftTypeName, rightTypeName);
        }

        public static void ReportAmbiguousBinaryOperator(this ICollection<Diagnostic> diagnostics, SyntaxToken operatorToken, TypeSymbol leftType, TypeSymbol rightType)
        {
            var operatorName = operatorToken.Text;
            var leftTypeName = leftType.ToDisplayName();
            var rightTypeName = rightType.ToDisplayName();
            diagnostics.Report(operatorToken.Span, DiagnosticId.AmbiguousBinaryOperator, operatorName, leftTypeName, rightTypeName);
        }

        public static void ReportCannotApplyUnaryOperator(this ICollection<Diagnostic> diagnostics, SyntaxToken operatorToken, TypeSymbol type)
        {
            var operatorName = operatorToken.Text;
            var typeName = type.ToDisplayName();
            diagnostics.Report(operatorToken.Span, DiagnosticId.CannotApplyUnaryOperator, operatorName, typeName);
        }

        public static void ReportAmbiguousUnaryOperator(this ICollection<Diagnostic> diagnostics, SyntaxToken operatorToken, TypeSymbol type)
        {
            var operatorName = operatorToken.Text;
            var typeName = type.ToDisplayName();
            diagnostics.Report(operatorToken.Span, DiagnosticId.AmbiguousUnaryOperator, operatorName, typeName);
        }

        public static void ReportFunctionMissingImplementation(this ICollection<Diagnostic> diagnostics, FunctionInvocationExpressionSyntax syntax)
        {
            diagnostics.Report(syntax.Name.GetTextSpanSafe(), DiagnosticId.FunctionMissingImplementation, syntax.Name.ToStringIgnoringMacroReferences());
        }

        public static void ReportMethodMissingImplementation(this ICollection<Diagnostic> diagnostics, MethodInvocationExpressionSyntax syntax)
        {
            diagnostics.Report(syntax.Name.Span, DiagnosticId.FunctionMissingImplementation, syntax.Name.Text);
        }

        public static void ReportSymbolRedefined(this ICollection<Diagnostic> diagnostics, TextSpan span, Symbol symbol)
        {
            diagnostics.Report(span, DiagnosticId.SymbolRedefined, symbol.Name);
        }

        public static void ReportLoopControlVariableConflict(this ICollection<Diagnostic> diagnostics, VariableDeclaratorSyntax syntax)
        {
            diagnostics.Report(syntax.Identifier.Span, DiagnosticId.LoopControlVariableConflict, syntax.Identifier.Text);
        }

        public static void ReportImplicitTruncation(this ICollection<Diagnostic> diagnostics, TextSpan span, TypeSymbol sourceType, TypeSymbol destinationType)
        {
            diagnostics.Report(span, DiagnosticId.ImplicitTruncation, sourceType.Name, destinationType.Name);
        }

        #endregion
    }
}