using System.Text;

namespace Interpreter.Tokens.Operators.N_Ary; 

public abstract class NAryOperator : Token {
	public string Symbol = null!;
	public Token[] Children = new Token[0];
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();
		
		sb.Append(indentStr).Append(Symbol).Append('\n');
		foreach (Token t in Children)
			sb.Append(t.ToString(indent + 1));

		return sb.ToString();
	}

	public override int Size() => 1 + Children.Sum(t => t.Size());
}