using System.Text;
using Interpreter.Types;
using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
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
	public bool IsUnlimited = false; // Only for function declarations

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
		if (IsUnlimited)
			sb.Append(indentSb.Append('\t').Append("...").Append('\n'));

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

		if (res is Function f && Args != null!) {
			Object o = Args.Evaluate(vars);
			return f.Execute(o is ArgumentArray aa ? aa.Arr : new [] {o}, vars);
		}
		
		if (res is not Class && Args != null!)
			throw new FormatException("Tried to call a non-function");

		if (res is Array a && Index != null!) {
			Array arr = (Array) Index.Evaluate(vars);
			if (arr.Arr.Count != 1 || arr.Arr[0] is not Integer)
				throw new FormatException("Line " + Line +  ": index must be of type Integer");

			Integer i = (Integer) arr.Arr[0];
			Index index = (int) i.Num.Num >= 0 ? new Index((int) i.Num.Num) : ^-(int) i.Num.Num; // Negative index will take nth element from the right
			return a.Arr[index];
		}

		if (Index != null!)
			throw new FormatException("Tried to index a non-array");

		return res;
	}

	public override int Size() => 1;
}