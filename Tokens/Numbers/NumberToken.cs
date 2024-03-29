using System.Text;
using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Numbers; 

public class NumberToken : Token {
	public Number Num = null!;

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(Num.ToString()).Append('\n');

		return sb.ToString();
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Num;

	public override int Size() => 1;
}