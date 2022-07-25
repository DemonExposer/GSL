using Interpreter.Types;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class InstantiationOperator : UnaryOperator {
	public InstantiationOperator() {
		Symbol = "new";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Token constructorArgs = ((VariableToken) Child).Args;
		return ((Class) Child.Evaluate(vars)).Instantiate();
	}
}