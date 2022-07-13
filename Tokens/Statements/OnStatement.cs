using System.Text;
using Interpreter.Tokens.Operators.Unary;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements; 

public class OnStatement : Token {
	public string Symbol = "on";
	public ParenthesesOperator Left = null!;
	public Token Right = null!;

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		if (Left != null!)
			sb.Append(Left.ToString(indent+1));
		if (Right != null!)
			sb.Append(Right.ToString(indent+1));

		return sb.ToString();
	}

	public override Object Evaluate() {
		if (((Boolean) Left.Evaluate()).Bool)
			return Right.Evaluate();

		return null!;
	}

	public override int Size() {
		return 1 + Left.Size() + Right.Size();
	}
}