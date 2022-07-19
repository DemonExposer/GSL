using Interpreter.Tokens.Operators.N_Ary;
using TrieDictionary;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	public MultilineStatementOperator expressions = null!;

	public FunctionBody(MultilineStatementOperator expressions) {
		this.expressions = expressions;
		if (this.expressions != null!)
			this.expressions.IsPartOfFunction = true;
	}

	public virtual Object Execute(Object[] args, TrieDictionary<Object> vars, List<TrieDictionary<Object>> topScopeVars) => expressions.Evaluate(new List<TrieDictionary<Object>> {topScopeVars[0], vars});
}