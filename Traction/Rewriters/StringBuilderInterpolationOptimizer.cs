using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Precompiler {
    class StringBuilderInterpolationOptimizer : CSharpSyntaxRewriter
    {
        public StringBuilderInterpolationOptimizer(SemanticModel model) {
            if (model == null) throw new ArgumentNullException(nameof(model));

            this.model = model;
        }

        private readonly SemanticModel model;

        public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node) {

            //Do stuff and return some new stuff

            var memberAccess = node.Expression as MemberAccessExpressionSyntax;
            if (memberAccess != null && node.ArgumentList.Arguments.Count == 1) {
                // check if the single method argument is an interpolated string
                var interpolatedStringSyntax = node.ArgumentList.Arguments[0].Expression as InterpolatedStringExpressionSyntax;
                if (interpolatedStringSyntax != null) {
                    bool appendNewLine; // this distinguishes Append and AppendLine calls
                    if (CanRewriteSymbol(this.model.GetSymbolInfo(memberAccess), out appendNewLine)) {
                        var formatCount = 0;
                        var formatString = new StringBuilder();
                        var formatArgs = new List<ArgumentSyntax>();

                        // build the format string
                        foreach (var content in interpolatedStringSyntax.Contents) {
                            switch (content.Kind()) {
                                case SyntaxKind.InterpolatedStringText:
                                    var text = (InterpolatedStringTextSyntax)content;
                                    formatString.Append(text.TextToken.Text);
                                    break;
                                case SyntaxKind.Interpolation:
                                    var interpolation = (InterpolationSyntax)content;
                                    formatString.Append("{");
                                    formatString.Append(formatCount++);
                                    formatString.Append(interpolation.AlignmentClause);
                                    formatString.Append(interpolation.FormatClause);
                                    formatString.Append("}");

                                    // the interpolations become arguments for the AppendFormat call
                                    formatArgs.Add(SyntaxFactory.Argument(interpolation.Expression));
                                    break;
                            }
                        }
                        if (appendNewLine) {
                            formatString.AppendLine();
                        }

                        // the first parameter is the format string
                        formatArgs.Insert(0,
                          SyntaxFactory.Argument(
                            SyntaxFactory.LiteralExpression(
                              SyntaxKind.StringLiteralExpression,
                              SyntaxFactory.Literal(formatString.ToString()))));
                        return node
                          .WithExpression(memberAccess.WithName(SyntaxFactory.IdentifierName("AppendFormat")))
                          .WithArgumentList(SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(formatArgs)));
                    }
                }
            }
            return base.VisitInvocationExpression(node);
        }

        // we use the semantic model to get the type information of the method being called
        private static bool CanRewriteSymbol(SymbolInfo symbolInfo, out bool appendNewLine) {
            appendNewLine = false;
            IMethodSymbol methodSymbol = symbolInfo.Symbol as IMethodSymbol;
            if (methodSymbol == null) return false;

            switch (methodSymbol.Name) {
                case "AppendLine":
                case "Append":
                    if (methodSymbol.ContainingType.ToString() == "System.Text.StringBuilder") {
                        appendNewLine = methodSymbol.Name == "AppendLine";
                        return true;
                    }
                    break;
            }
            return false;
        }
    }
}
