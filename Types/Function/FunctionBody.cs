using Interpreter.Tokens.Operators.N_Ary;
using TrieDictionary;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	public MultilineStatementOperator expressions;

	public FunctionBody(MultilineStatementOperator expressions) {
		this.expressions = expressions;
	}

	public virtual Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => expressions.Evaluate(new List<TrieDictionary<Object>> {topScopeVars[0], vars});
}