using Interpreter.BuiltinFunctions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Types;
using Interpreter.Types.Comparable;
using Interpreter.Types.Function;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;

namespace Interpreter;

using System.Text.RegularExpressions;

public class Program {
	public static IDictionary<string, Type> bindings = new Dictionary<string, Type>();
	public static IDictionary<string, Object> vars = new Dictionary<string, Object>();
	public static IDictionary<string, int> priorities = new Dictionary<string, int>();

	private static CheckedString[] CheckComment(CheckedString[] line) {
		for (int i = 0; i < line.Length; i++)
			if (line[i].Str == "#")
				return line.Take(i).ToArray();

		return line;
	}

	public static void Main(string[] args) {
		bindings.Add("+", typeof(PlusBinaryOperator));
		bindings.Add("-", typeof(MinusBinaryOperator));
		bindings.Add("*", typeof(MultiplicationBinaryOperator));
		bindings.Add("/", typeof(DivisionBinaryOperator));
		bindings.Add("^", typeof(PowerBinaryOperator));
		
		bindings.Add("decl", typeof(DeclarationOperator));
		bindings.Add("=", typeof(AssignmentOperator));
		
		bindings.Add("(", typeof(ParenthesesOperator));
		bindings.Add(")", typeof(ParenthesesOperator));
		
		bindings.Add("&&", typeof(AndBinaryOperator));
		bindings.Add("and", typeof(AndBinaryOperator));
		bindings.Add("||", typeof(OrBinaryOperator));
		bindings.Add("or", typeof(OrBinaryOperator));
		bindings.Add("==", typeof(EqualityBinaryOperator));
		bindings.Add("!=", typeof(InequalityBinaryOperator));
		bindings.Add(">", typeof(LargerBinaryOperator));
		bindings.Add("<", typeof(SmallerBinaryOperator));
		bindings.Add("<=", typeof(SmallerEqualBinaryOperator));
		bindings.Add(">=", typeof(LargerEqualBinaryOperator));
		bindings.Add("!", typeof(NotUnaryOperator));

		// Low number for priority means a higher priority
		priorities.Add("(", 0);
		priorities.Add("!", 1);
		priorities.Add(">", 2);
		priorities.Add("<", 2);
		priorities.Add("<=", 2);
		priorities.Add(">=", 2);
		priorities.Add("==", 3);
		priorities.Add("!=", 3);
		priorities.Add("&&", 4);
		priorities.Add("and", 4);
		priorities.Add("||", 5);
		priorities.Add("or", 5);
		priorities.Add("^", 6);
		priorities.Add("*", 7);
		priorities.Add("/", 7);
		priorities.Add("+", 8);
		priorities.Add("-", 8);
		priorities.Add("=", 9);
		priorities.Add("decl", 10);
		
		// Standard defined variables
		// TODO: Make sure print accepts an undefined number of params
		vars.Add("print", new Function(new [] {new FunctionArgument {ArgType = typeof(Object), Name = "arg"}}, new Print(null!)));
		vars.Add("false", new Boolean(false));
		vars.Add("true", new Boolean(true));

		string[] lines = File.ReadAllLines(args[0]);
		for (int i = 0; i < lines.Length; i++) {
			// Regex matching all valid strings, with the least complicated in the back so that e.g. == gets matched as == and not as =, =
			CheckedString[] lexedLine = Regex.Matches(lines[i], "([a-zA-Z0-9]+|==|!=|\\|\\||&&|>=|<=|[\\^*/+-=()#<>!])").ToList().Select(match => new CheckedString {Str = match.Value.Trim(), Line = i+1}).ToArray();
		//	foreach (CheckedString cs in lexedLine)
		//		Console.Write("{0}, ", cs.Str);
		//	Console.WriteLine();
			lexedLine = CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;

			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine);

		//	Console.Write("[");
		//	foreach (Token t in tokenizedLine)
		//		Console.Write("{0}, ", t.GetType());
		//	Console.WriteLine("]");

			Token tree = Parser.Parse(tokenizedLine, Parser.GetTopElementIndex(tokenizedLine, 0, true), 0);

		//	Console.WriteLine(tree.ToString(0));
			tree.Evaluate();
		}
	}
}
