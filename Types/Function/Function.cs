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
		
		if (args.Length != Args.Length)
			throw new InvalidOperationException("Args incorrect length: is: " + args.Length + ", should be: " +
			                                    Args.Length);

		for (int i = 0; i < args.Length; i++) {
			if (!Args[i].ArgType.IsInstanceOfType(args[i]))
				throw new InvalidOperationException("Incorrect argument type for argument " + i);
			
			vars[Args[i].Name] = args[i];
		}

		return body.Execute(args, vars);
	}

	public override string ToString() {
		return "Function";
	}

	public override string GetType() {
		return "Function";
	}
}

