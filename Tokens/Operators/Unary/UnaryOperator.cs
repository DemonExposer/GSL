using System.Text;

namespace Interpreter.Tokens.Operators.Unary; 

public abstract class UnaryOperator : Token {
	public string Symbol;
	public Token Child = null!;
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		if (Child != null)
			sb.Append(Child.ToString(indent + 1));

		return sb.ToString();
	}
	
	public override int Size() {
		return 1 + Child.Size();
	}
}