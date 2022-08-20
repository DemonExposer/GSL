using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class SmallerBinaryOperator : BooleanOperator {	
	public SmallerBinaryOperator() {
		Symbol = ">";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Types.Comparable.Boolean(((Number) Left.Evaluate(vars)).Num < ((Number) Right.Evaluate(vars)).Num);
}