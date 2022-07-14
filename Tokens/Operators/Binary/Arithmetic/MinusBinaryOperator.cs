using Interpreter.Types;
using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class MinusBinaryOperator : ArithmeticOperator {
	public MinusBinaryOperator() {
		Symbol = "-";
	}
	
	public override Object Evaluate() => new Integer(((Integer) Left.Evaluate()).Int - ((Integer) Right.Evaluate()).Int);
}