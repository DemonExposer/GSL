using Interpreter.BuiltinFunctions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Tokens.Statements;
using Interpreter.Types.Function;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;
using TrieDictionary;
using System.Text.RegularExpressions;

namespace Interpreter;

public class Program {
	public static TrieDictionary<Type> bindings = new ();
	public static TrieDictionary<Object> vars = new ();
	public static TrieDictionary<int> priorities = new ();

	private static CheckedString[] CheckComment(CheckedString[] line) {
		for (int i = 0; i < line.Length; i++)
			if (line[i].Str == "#")
				return line.Take(i).ToArray();

		return line;
	}

	public static void Main(string[] args) {
		// Arithmetic
		bindings.Insert("+", typeof(PlusBinaryOperator));
		bindings.Insert("-", typeof(MinusBinaryOperator));
		bindings.Insert("*", typeof(MultiplicationBinaryOperator));
		bindings.Insert("/", typeof(DivisionBinaryOperator));
		bindings.Insert("^", typeof(PowerBinaryOperator));
		
		// Variable initialization
		bindings.Insert("decl", typeof(DeclarationOperator));
		bindings.Insert("=", typeof(AssignmentOperator));
		
		// Brackets
		bindings.Insert("(", typeof(ParenthesesOperator));
		bindings.Insert(")", typeof(ParenthesesOperator));
		
		// Boolean logic
		bindings.Insert("&&", typeof(AndBinaryOperator));
		bindings.Insert("and", typeof(AndBinaryOperator));
		bindings.Insert("||", typeof(OrBinaryOperator));
		bindings.Insert("or", typeof(OrBinaryOperator));
		bindings.Insert("==", typeof(EqualityBinaryOperator));
		bindings.Insert("!=", typeof(InequalityBinaryOperator));
		bindings.Insert(">", typeof(LargerBinaryOperator));
		bindings.Insert("<", typeof(SmallerBinaryOperator));
		bindings.Insert("<=", typeof(SmallerEqualBinaryOperator));
		bindings.Insert(">=", typeof(LargerEqualBinaryOperator));
		bindings.Insert("!", typeof(NotUnaryOperator));
		
		// Statements
		bindings.Insert("if", typeof(IfStatement));

		// Low number for priority means a higher priority
		priorities.Insert("(", 0);
		priorities.Insert("!", 1);
		priorities.Insert(">", 2);
		priorities.Insert("<", 2);
		priorities.Insert("<=", 2);
		priorities.Insert(">=", 2);
		priorities.Insert("==", 3);
		priorities.Insert("!=", 3);
		priorities.Insert("&&", 4);
		priorities.Insert("and", 4);
		priorities.Insert("||", 5);
		priorities.Insert("or", 5);
		priorities.Insert("^", 6);
		priorities.Insert("*", 7);
		priorities.Insert("/", 7);
		priorities.Insert("+", 8);
		priorities.Insert("-", 8);
		priorities.Insert("=", 9);
		priorities.Insert("decl", 10);
		
		// Standard defined variables
		// TODO: Make sure print accepts an undefined number of params
		vars.Insert("print", new Function(new [] {new FunctionArgument {ArgType = typeof(Object), Name = "arg"}}, new Print(null!)));
		vars.Insert("false", new Boolean(false));
		vars.Insert("true", new Boolean(true));

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
