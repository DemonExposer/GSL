using Interpreter.Tokens;
using Interpreter.Tokens.Operators.N_Ary;
using TrieDictionary;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	private MultilineStatementOperator expressions;

	public FunctionBody(MultilineStatementOperator expressions) {
		this.expressions = expressions;
	}

	public virtual Object Execute(Object[] args, TrieDictionary<Object> vars) {
		throw new NotImplementedException();
	}
}