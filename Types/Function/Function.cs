using TrieDictionary;

namespace Interpreter.Types.Function; 

public class Function : Object {
	private TrieDictionary<Object> vars = null!;
	public FunctionArgument[] Args;
	private FunctionBody body;

	public Function(FunctionArgument[] args, FunctionBody body) {
		Args = args;
		this.body = body;
	}

	public Object Execute(Object[] args) {
		vars = new TrieDictionary<Object>();
		
		if (!Args[^1].IsUnlimited && args.Length != Args.Length)
			throw new InvalidOperationException("Args incorrect length: is: " + args.Length + ", should be: " +
			                                    Args.Length);

		for (int i = 0; i < args.Length; i++) {
			if (i >= Args.Length && !Args[^1].ArgType.IsInstanceOfType(args[i]) || !Args[i].ArgType.IsInstanceOfType(args[i]))
				throw new InvalidOperationException("Incorrect argument type for argument " + i);

			if (i >= Args.Length - 1 && Args[^1].IsUnlimited) 
			//	vars[Args[i].Name] = someArray; // TODO: implement array type to make unlimited args an array type
				throw new NotImplementedException("unlimited parameters are not yet implemented");
			else
				vars[Args[i].Name] = args[i];
		}

		return body.Execute(args, vars);
	}

	public override string ToString() => "Function";

	public override string GetType() => "Function";
}

