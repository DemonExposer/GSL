using Interpreter.Tokens;

namespace Interpreter.Types.Function; 

public class FunctionBody {
	private Token[] expressions;

	public FunctionBody(Token[] expressions) {
		this.expressions = expressions;
	}

	public virtual Object Execute(Object[] args, IDictionary<string, Object> vars) {
		throw new NotImplementedException();
	}
}