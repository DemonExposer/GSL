using System;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Operators.Binary;
public class ObjectAssignmentOperator : BinaryOperator {
	public ObjectAssignmentOperator() {
		Symbol = ":";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		throw new NotImplementedException();
	}
}

