using Interpreter.Types;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class LargerBinaryOperator : BooleanOperator {
	public LargerBinaryOperator() {
		Symbol = ">";
	}
	
	public override Object Evaluate() {
		return new Types.Boolean(((Integer) Left.Evaluate()).Int > ((Integer) Right.Evaluate()).Int);
	}
}