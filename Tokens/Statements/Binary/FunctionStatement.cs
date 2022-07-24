using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Function;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Tokens.Statements.Binary; 

public class FunctionStatement : BinaryStatement {
	public string Name = null!;
	public FunctionArgument[] Args = null!;
	private FunctionBody body = null!;

	public FunctionStatement() {
		Symbol = "function";
	}
	
	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		Args = Left.Children.Cast<VariableToken>().ToList().Select(vt => new FunctionArgument {ArgType = typeof(Object), IsUnlimited = false, Name = vt.Name}).ToArray();
		body = new FunctionBody((MultilineStatementOperator) Right);

		if (vars[^1].Contains(Name))
			throw new InvalidOperationException(Name + " is already defined");

		Object res = new Function(Args, body);
		vars[^1][Name] = res;

		return res;
	}
}