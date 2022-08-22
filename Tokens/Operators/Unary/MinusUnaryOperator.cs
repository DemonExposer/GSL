using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class MinusUnaryOperator : UnaryOperator {
	public MinusUnaryOperator() {
		Symbol = "-";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => Number.GetProperInstance(-((Number) Child.Evaluate(vars)).Num); 
}