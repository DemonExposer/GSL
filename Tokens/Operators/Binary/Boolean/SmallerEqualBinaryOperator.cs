using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class SmallerEqualBinaryOperator : BooleanOperator {
	public SmallerEqualBinaryOperator() {
		Symbol = "<=";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Types.Comparable.Boolean(((Number) Left.Evaluate(vars)).Num <= ((Number) Right.Evaluate(vars)).Num);
}