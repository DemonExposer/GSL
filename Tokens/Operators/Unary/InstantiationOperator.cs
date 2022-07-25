using Interpreter.Types;
using Interpreter.Types.Util;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class InstantiationOperator : UnaryOperator {
	public InstantiationOperator() {
		Symbol = "new";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Token constructorArgs = ((VariableToken) Child).Args;
		Object o = constructorArgs.Evaluate(vars);
		return ((Class) Child.Evaluate(vars)).Instantiate(o is ArgumentArray aa ? aa.Arr : new [] {o});
	}
}