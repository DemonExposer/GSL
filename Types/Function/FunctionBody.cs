using Interpreter.Tokens.Operators.N_Ary;
using TrieDictionary;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	public MultilineStatementOperator expressions;

	public FunctionBody(MultilineStatementOperator expressions) {
		this.expressions = expressions;
	}

	public virtual Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) {
	//	expressions.Vars.Add(vars);
	//	Console.WriteLine(expressions.Vars[^1].GetKeySet()[0]);
		
		return expressions.Evaluate(topScopeVars);
	}
}