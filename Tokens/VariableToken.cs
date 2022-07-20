using System.Text;
using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using Interpreter.Types.Util;
using Object = Interpreter.Types.Object;
using TrieDictionary;
using Array = Interpreter.Types.Array;

namespace Interpreter.Tokens;

public class VariableToken : Token {
	public string Name = null!;
	public Token Args = null!;
	public Token Index = null!;

	public override string ToString(int indent) {
		StringBuilder sb = new StringBuilder();
		
		StringBuilder indentSb = new StringBuilder();
		for (int i = 0; i < indent; i++)
			indentSb.Append('\t');
		string indentStr = indentSb.ToString();

		sb.Append(indentStr).Append(Name).Append('\n');

		if (Args != null)
			sb.Append(Args.ToString(indent + 1));
		if (Index != null)
			sb.Append(Index.ToString(indent + 1));

		return sb.ToString();
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Object res = null!;

		for (int i = vars.Count - 1; i >= 0; i--) try {
			res = vars[i][Name];
			break;
		} catch (KeyNotFoundException) { }
		
		if (res == null!)
			throw new KeyNotFoundException("Line " + Line + ": Variable " + Name + " does not exist");

		if (res is Function f) {
			Object o = Args.Evaluate(vars);
			return f.Execute(o is ArgumentArray aa ? aa.Arr : new [] {o}, vars);
		}

		if (res is Array a && Index != null!) {
			Array arr = (Array) Index.Evaluate(vars);
			if (arr.Arr.Count != 1 || arr.Arr[0] is not Integer)
				throw new FormatException("Line " + Line +  ": index must be of type Integer");

			Integer i = (Integer) arr.Arr[0];
			Index index = i.Int >= 0 ? new Index(i.Int) : ^-i.Int; // Negative index will take nth element from the right
			return a.Arr[index];
		}

		return res;
	}

	public override int Size() => 1;
}