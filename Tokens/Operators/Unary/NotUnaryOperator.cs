using TrieDictionary;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class NotUnaryOperator : UnaryOperator {
	public NotUnaryOperator() {
		Symbol = "!";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Boolean(!((Boolean) Child.Evaluate(vars)).Bool);
}