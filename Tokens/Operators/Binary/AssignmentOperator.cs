using Interpreter.Types.Comparable.Numbers;
using Object = Interpreter.Types.Object;
using TrieDictionary;
using Array = Interpreter.Types.Array;

namespace Interpreter.Tokens.Operators.Binary; 

public class AssignmentOperator : BinaryOperator {
	public AssignmentOperator() {
		Symbol = "=";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		if (Left is DotOperator dotOp) {
			// TODO: check whether property exists and do further checks for indexers
			Object o = dotOp.Left.Evaluate(vars);
			return o.Properties[dotOp.Right.Str] = Right.Evaluate(vars);
		}
		
		Object leftCheck = null!;
		int scopeIndex;
		for (scopeIndex = vars.Count - 1; scopeIndex >= 0; scopeIndex--) try {
			leftCheck = vars[scopeIndex][((VariableToken) Left).Name];
			break;
		} catch (KeyNotFoundException) { }
		
		if (leftCheck == null!)
			throw new KeyNotFoundException("Line " + Line + ": Variable " + ((VariableToken) Left).Name + " does not exist");

		Object res = Right.Evaluate(vars);
		if (((VariableToken) Left).Index != null!) { // If the left element is indexed, resolve that index and assign to it
			Array arr = (Array) ((VariableToken) Left).Index.Evaluate(vars);
			if (arr.Arr.Count == 0) {
				((Array) vars[scopeIndex][((VariableToken) Left).Name]).Arr.Add(res);
				return res;
			}

			if (arr.Arr.Count != 1 || arr.Arr[0] is not Integer)
				throw new FormatException("Line " + Line +  ": index must be of type Integer");

			Integer i = (Integer) arr.Arr[0];
			Index index = (int) i.Num.Num >= 0 ? new Index((int) i.Num.Num) : ^-(int) i.Num.Num;
			((Array) vars[scopeIndex][((VariableToken) Left).Name]).Arr[index] = res;
		} else {
			vars[scopeIndex][((VariableToken) Left).Name] = res;
		}

		return res;
	}
}