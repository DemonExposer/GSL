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
using Interpreter.Tokens.Operators.N_Ary;

namespace Interpreter;

public class Program {
	public static TrieDictionary<Type> bindings = new ();
	private static TrieDictionary<Object> vars = new ();
	public static TrieDictionary<int> priorities = new ();

	public static void Main(string[] args) {
		// Arithmetic
		bindings.Insert("+", typeof(PlusBinaryOperator));
		bindings.Insert("-", typeof(MinusBinaryOperator));
		bindings.Insert("*", typeof(MultiplicationBinaryOperator));
		bindings.Insert("/", typeof(DivisionBinaryOperator));
		bindings.Insert("%", typeof(ModulusBinaryOperator));
		bindings.Insert("^", typeof(PowerBinaryOperator));
		
		// Variable initialization
		bindings.Insert("decl", typeof(DeclarationOperator));
		bindings.Insert("=", typeof(AssignmentOperator));
		
		// Brackets
		bindings.Insert("(", typeof(ParenthesesOperator));
		bindings.Insert(")", typeof(ParenthesesOperator));
		bindings.Insert("{", typeof(MultiLineStatementOperator));
		bindings.Insert("}", typeof(MultiLineStatementOperator));
		
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
		bindings.Insert("on", typeof(OnStatement));

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
		priorities.Insert("%", 7);
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
			CheckedString[] lexedLine = Lexer.Lex(lines[i], i + 1);
		//	foreach (CheckedString cs in lexedLine)
		//		Console.Write("{0}, ", cs.Str);
		//	Console.WriteLine();
			lexedLine = Parser.CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;
			
			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine, new [] {vars}.ToList());

		//	Console.Write("[");
		//	foreach (Token t in tokenizedLine)
		//		Console.Write("{0}, ", t.GetType());
		//	Console.WriteLine("]");
		
			Token tree = Parser.Parse(tokenizedLine, Parser.GetTopElementIndex(tokenizedLine, 0, true), new [] {vars}.ToList(), lines, ref i, 0);

		//	Console.WriteLine(tree.ToString(0));
			tree.Evaluate();
		}
	}
}
