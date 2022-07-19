using System.Text;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Function;
using Object = Interpreter.Types.Object;
using TrieDictionary;

namespace Interpreter.BuiltinFunctions; 

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