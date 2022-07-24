using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Unary; 

public class UnlimitedArgumentOperator : UnaryOperator {
	public UnlimitedArgumentOperator() {
		Symbol = "...";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => null!;
}