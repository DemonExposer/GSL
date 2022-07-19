using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class AssignmentOperator : BinaryOperator {
	public AssignmentOperator() {
		Symbol = "=";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Object leftCheck = null!;
		int scopeIndex;
		for (scopeIndex = vars.Count - 1; scopeIndex >= 0; scopeIndex--) try {
			leftCheck = vars[scopeIndex][((VariableToken) Left).Name];
			break;
		} catch (KeyNotFoundException) { }
		
		if (leftCheck == null!)
			throw new KeyNotFoundException("Line " + Line + ": Variable " + ((VariableToken) Left).Name + " does not exist");

		Object res = Right.Evaluate(vars);
		vars[scopeIndex][((VariableToken) Left).Name] = res;
		
		return res;
	}
}