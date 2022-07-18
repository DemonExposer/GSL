using Interpreter.Tokens.Operators.N_Ary;
using TrieDictionary;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	public MultilineStatementOperator expressions;

	public FunctionBody(MultilineStatementOperator expressions) {
		this.expressions = expressions;
	}

	public virtual Object Execute(Object[] args, TrieDictionary<Object> vars) {
		vars.GetKeySet().ToList().ForEach(key => expressions.Vars[^1].Insert(key, vars[key]));
		
		return expressions.Evaluate();
	}
}