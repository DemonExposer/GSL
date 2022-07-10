using Interpreter.Types;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class PlusBinaryOperator : ArithmeticOperator {
	public PlusBinaryOperator() {
		Symbol = "+";
	}
	
	public override Object Evaluate() {
		return new Integer(((Integer) Left.Evaluate()).Int + ((Integer) Right.Evaluate()).Int);
	}
}