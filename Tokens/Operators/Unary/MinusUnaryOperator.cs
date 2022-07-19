using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class MinusUnaryOperator : UnaryOperator {
	public MinusUnaryOperator() {
		Symbol = "-";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Integer(-((Integer) Child.Evaluate(vars)).Int); 
}