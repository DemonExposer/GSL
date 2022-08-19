using TrieDictionary;
using Interpreter.Types.Util;
using String = Interpreter.Types.Comparable.String;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary;

public class DictionaryAssignmentOperator : BinaryOperator {
	public DictionaryAssignmentOperator() {
		Symbol = ":";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) => new DictionaryEntry((String) Left.Evaluate(vars), Right.Evaluate(vars));
}

