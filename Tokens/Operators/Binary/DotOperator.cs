using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using Interpreter.Types.Util;
using Interpreter.Types;
using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Array = Interpreter.Types.Array;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary; 

public class DotOperator : BinaryOperator {
	public DotOperator() {
		Symbol = ".";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Object leftObj = Left.Evaluate(vars);
		Object res = leftObj.Properties.Contains(Right.Str) ? leftObj.Properties[Right.Str] : new Null();

		VariableToken properRight = (VariableToken) Right;
		if (res is Function f) {
			Object o = properRight.Args.Evaluate(vars);
			vars[^1]["this"] = leftObj;
			return f.Execute(o is ArgumentArray aa ? aa.Arr : new [] {o}, vars);
		}
		
		if (res is not Class && properRight.Args != null!)
			throw new FormatException("Tried to call a non-function");

		if (res is Array a && properRight.Index != null!) {
			Array arr = (Array) properRight.Index.Evaluate(vars);
			if (arr.Arr.Count != 1 || arr.Arr[0] is not Integer)
				throw new FormatException("Line " + Line +  ": index must be of type Integer");

			Integer i = (Integer) arr.Arr[0];
			Index index = (int) i.Num.Num >= 0 ? new Index((int) i.Num.Num) : ^-(int) i.Num.Num; // Negative index will take nth element from the right
			return a.Arr[index];
		}
		
		if (properRight.Index != null!)
			throw new FormatException("Tried to index a non-array");

		return res;
	}
}