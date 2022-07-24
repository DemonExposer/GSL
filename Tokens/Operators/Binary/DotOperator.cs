using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using Interpreter.Types.Util;
using TrieDictionary;
using Array = Interpreter.Types.Array;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary; 

public class DotOperator : BinaryOperator {
	public DotOperator() {
		Symbol = ".";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		 Object res = Left.Evaluate(vars).Properties[Right.Str];

		 VariableToken properRight = (VariableToken) Right;
		 if (res is Function f) {
			 Object o = properRight.Args.Evaluate(vars);
			 return f.Execute(o is ArgumentArray aa ? aa.Arr : new [] {o}, vars);
		 }

		 if (res is Array a && properRight.Index != null!) {
			 Array arr = (Array) properRight.Index.Evaluate(vars);
			 if (arr.Arr.Count != 1 || arr.Arr[0] is not Integer)
				 throw new FormatException("Line " + Line +  ": index must be of type Integer");

			 Integer i = (Integer) arr.Arr[0];
			 Index index = i.Int >= 0 ? new Index(i.Int) : ^-i.Int; // Negative index will take nth element from the right
			 return a.Arr[index];
		 }

		 return res;
	}
}