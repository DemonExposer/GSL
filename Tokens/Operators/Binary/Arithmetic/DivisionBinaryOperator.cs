using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class DivisionBinaryOperator : ArithmeticOperator {
	public DivisionBinaryOperator() {
		Symbol = "/";
	}
	
	public override Object Evaluate() => new Integer(((Integer) Left.Evaluate()).Int / ((Integer) Right.Evaluate()).Int);
}