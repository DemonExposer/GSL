using TrieDictionary;

namespace Interpreter.Types.Function; 

public class Function : Object {
	public FunctionArgument[] Args;
	private FunctionBody body;

	public Function(FunctionArgument[] args, FunctionBody body) {
		Args = args;
		this.body = body;
	}

	public Object Execute(Object[] args, List<TrieDictionary<Object>> topScopeVars) {
		TrieDictionary<Object> vars = new TrieDictionary<Object>();

		if (args.Length == 0 && Args.Length == 0)
			return body.Execute(args, vars, topScopeVars);
		
		if (!Args[^1].IsUnlimited && args.Length != Args.Length)
			throw new InvalidOperationException("Args incorrect length: is: " + args.Length + ", should be: " +
			                                    Args.Length);

		for (int i = 0; i < args.Length; i++) {
			// Check if Args are same type (for an unlimited argument check the last argument of the function
			//                              as that is the only place where unlimited args are allowed)
			if (i >= Args.Length && !Args[^1].ArgType.IsInstanceOfType(args[i]) || i < Args.Length && !Args[i].ArgType.IsInstanceOfType(args[i]))
				throw new InvalidOperationException("Incorrect argument type for argument " + i);

			if (i >= Args.Length - 1 && Args[^1].IsUnlimited) 
				vars[Args[^1].Name] = new Array(args);
			else
				vars[Args[i].Name] = args[i];
		}

		return body.Execute(args, vars, topScopeVars);
	}

	public override string ToString() => "Function";

	public override string GetType() => "Function";
}

