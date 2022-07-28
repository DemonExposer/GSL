using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Types.Function;
using TrieDictionary;
using Object = Interpreter.Types.Object;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter.Builtin.Functions; 

public class Read : FunctionBody {
	public Read(MultilineStatementOperator expressions) : base(expressions) { }

	public override Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => new String(Console.ReadLine()!);
}