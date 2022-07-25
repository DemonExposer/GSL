using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Unary; 

public class ClassStatement : UnaryStatement {
	public string Name = null!;

	public ClassStatement() {
		Symbol = "class";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		List<TrieDictionary<Object>> classProperties = new List<TrieDictionary<Object>>(new [] {new TrieDictionary<Object>()});
		((MultilineStatementOperator) Child).Evaluate(classProperties);

		return vars[^1][Name] = new Class {Name = Name, ClassProperties = classProperties[0]};
	}
}