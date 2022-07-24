using Interpreter.Types;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary; 

public class DeclarationOperator : BinaryOperator {
	public DeclarationOperator() {
		Symbol = "decl";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		if (vars[^1].Contains(((VariableToken) Left).Name))
			throw new InvalidOperationException(((VariableToken) Left).Name + " is already defined");

		Object res = new Null();
		vars[^1][((VariableToken) Left).Name] = res;

		if (Right is AssignmentOperator)
			return Right.Evaluate(vars);
		
		return res;
	}
}