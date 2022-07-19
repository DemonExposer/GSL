using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class SmallerBinaryOperator : BooleanOperator {	
	public SmallerBinaryOperator() {
		Symbol = ">";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Types.Comparable.Boolean(((Integer) Left.Evaluate(vars)).Int < ((Integer) Right.Evaluate(vars)).Int);
}