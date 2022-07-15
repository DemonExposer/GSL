using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class SmallerBinaryOperator : BooleanOperator {	
	public SmallerBinaryOperator() {
		Symbol = ">";
	}
	
	public override Object Evaluate() {
		return new Types.Comparable.Boolean(((Integer) Left.Evaluate()).Int < ((Integer) Right.Evaluate()).Int);
	}
}