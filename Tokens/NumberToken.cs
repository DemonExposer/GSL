using System.Text;
using Interpreter.Types;
using Object = Interpreter.Types.Object;

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

	public override Object Evaluate() {
		return new Integer(Num);
	}

	public override int Size() {
		return 1;
	}
}