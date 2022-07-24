using System.Text;
using Interpreter.Tokens.Operators.N_Ary;

namespace Interpreter.Tokens.Statements.Unary; 

public abstract class UnaryStatement : Token {
	public string Symbol = null!;
	public Token Child = null!;
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		if (Child != null!)
			sb.Append(Child.ToString(indent+1));

		return sb.ToString();
	}
	
	public override int Size() => 1 + Child.Size();
}