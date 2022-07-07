using System.Text;

namespace Interpreter.Tokens.Operators; 

public abstract class BinaryOperator : Token {
	public string Symbol;
	public Token Left = null!, Right = null!;
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		if (Left != null)
			sb.Append(Left.ToString(indent+1));
		if (Right != null)
			sb.Append(Right.ToString(indent+1));

		return sb.ToString();
	}

	public override int Size() {
		return 1 + Left.Size() + Right.Size();
	}
}