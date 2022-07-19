using Interpreter.Types.Comparable;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class SmallerEqualBinaryOperator : BooleanOperator {
	public SmallerEqualBinaryOperator() {
		Symbol = "<=";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Types.Comparable.Boolean(((Integer) Left.Evaluate(vars)).Int <= ((Integer) Right.Evaluate(vars)).Int);
}