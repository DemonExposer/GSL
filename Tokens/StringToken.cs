using System.Text;
using TrieDictionary;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Tokens; 

public class StringToken : Token {
	public String StrVal = null!;

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(StrVal.ToString()).Append('\n');

		return sb.ToString();
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		StrVal.Escape();
		return StrVal;
	}

	public override int Size() => 1;
}