using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Simplify;

public static class CSharpAnalyzer
{
    private static readonly HashSet<SyntaxKind> DecisionKinds = new()
    {
        SyntaxKind.IfStatement,
        SyntaxKind.WhileStatement,
        SyntaxKind.ForStatement,
        SyntaxKind.ForEachStatement,
        SyntaxKind.DoStatement,
        SyntaxKind.CaseSwitchLabel,
        SyntaxKind.CasePatternSwitchLabel,
        SyntaxKind.SwitchExpressionArm,
        SyntaxKind.CatchClause,
        SyntaxKind.ConditionalExpression,
        SyntaxKind.LogicalAndExpression,
        SyntaxKind.LogicalOrExpression,
        SyntaxKind.CoalesceExpression,
    };

    public static int Complexity(string source)
    {
        var root = CSharpSyntaxTree.ParseText(source).GetRoot();
        var walker = new Walker();
        walker.Visit(root);
        return walker.Count + 1;
    }

    private sealed class Walker : CSharpSyntaxWalker
    {
        public int Count { get; private set; }

        public override void Visit(SyntaxNode? node)
        {
            if (node is not null && DecisionKinds.Contains(node.Kind())) Count++;
            base.Visit(node);
        }
    }
}
