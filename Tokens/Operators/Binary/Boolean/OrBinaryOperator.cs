using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Boolean; 

public class OrBinaryOperator : BooleanOperator {
	public OrBinaryOperator() {
		Symbol = "||/or";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Types.Comparable.Boolean(((Types.Comparable.Boolean) Left.Evaluate(vars)).Bool || ((Types.Comparable.Boolean) Right.Evaluate(vars)).Bool);
}