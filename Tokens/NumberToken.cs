using System.Text;

namespace Interpreter.Tokens; 

public class NumberToken : Token {
	public int Num;
	
	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(Num).Append('\n');

		return sb.ToString();
	}

	public override int Evaluate() {
		return Num;
	}

	public override int Size() {
		return 1;
	}
}