using Interpreter.Types.Comparable;
using Interpreter.Types.Comparable.Numbers;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary.Arithmetic; 

public class ModulusBinaryOperator : ArithmeticOperator {
	public ModulusBinaryOperator() {
		Symbol = "%";
	}

	// Because C# modulus doesn't actually do modulus. It does for positive numbers, but for negative numbers it pretty much just guesses
	private static int ProperModulus(int a, int b) {
		int div = a / b;
		if (div > a && div * b != a)
			div--;

		return a - div*b;
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new Integer(ProperModulus(((Integer) Left.Evaluate(vars)).Int, ((Integer) Right.Evaluate(vars)).Int));
}