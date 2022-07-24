using Interpreter.Types;
using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	public DeclarationOperator() {
		Symbol = "decl";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		try {
			vars[^1].Get(((VariableToken) Left).Name);
			throw new InvalidOperationException(((VariableToken) Left).Name + " is already defined");
		} catch (KeyNotFoundException) { }
		
		Object res = new Null();
		vars[^1][((VariableToken) Left).Name] = res;

		if (Right is AssignmentOperator)
			return Right.Evaluate(vars);
		
		return res;
	}
}