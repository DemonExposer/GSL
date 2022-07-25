using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Function;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter.Builtin.Functions; 

public class Print : FunctionBody {
	public Print(MultilineStatementOperator expressions) : base(expressions) { }

	public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) {
		StringBuilder sb = new StringBuilder();
		foreach (Object o in args)
			sb.Append(o.ToString()).Append(" ");
		
		Console.WriteLine(sb.ToString().Trim());
		return null!;
	}
}