using Interpreter.Types;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class PowerBinaryOperator : ArithmeticOperator {
	public PowerBinaryOperator() {
		Symbol = "^";
	}
	
	public override Object Evaluate() {
		return new Integer((int) Math.Pow(((Integer) Left.Evaluate()).Int, ((Integer) Right.Evaluate()).Int));
	}
}