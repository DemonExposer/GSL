using Interpreter.Builtin.Functions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Types.Function;
using Boolean = Interpreter.Types.Comparable.Boolean;
using Object = Interpreter.Types.Object;
using TrieDictionary;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Tokens.Separators;
using Interpreter.Tokens.Statements.Binary;
using Interpreter.Tokens.Statements.Unary;
using Interpreter.Types;
using Array = Interpreter.Types.Array;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter;

public class Program {
	public static TrieDictionary<Type> Bindings = new ();
	public static TrieDictionary<int> Priorities = new ();
	public static List<string> OpeningBrackets = new ();
	public static List<string> ClosingBrackets = new ();
	public static TrieDictionary<Object> Vars = new ();

	public static void Main(string[] args) {
		// Arithmetic
		Bindings.Insert("+", typeof(PlusBinaryOperator));
		Bindings.Insert("-", typeof(MinusBinaryOperator));
		Bindings.Insert("*", typeof(MultiplicationBinaryOperator));
		Bindings.Insert("/", typeof(DivisionBinaryOperator));
		Bindings.Insert("%", typeof(ModulusBinaryOperator));
		Bindings.Insert("^", typeof(PowerBinaryOperator));
		
		// Variable initialization
		Bindings.Insert("decl", typeof(DeclarationOperator));
		Bindings.Insert("=", typeof(AssignmentOperator));
		
		// Brackets
		Bindings.Insert("(", typeof(ParenthesesOperator));
		Bindings.Insert(")", typeof(ParenthesesOperator));
		Bindings.Insert("{", typeof(MultilineStatementOperator));
		Bindings.Insert("}", typeof(MultilineStatementOperator));
		Bindings.Insert("[", typeof(SquareBracketOperator));
		Bindings.Insert("]", typeof(SquareBracketOperator));
		
		OpeningBrackets.Add("(");
		OpeningBrackets.Add("[");
		
		ClosingBrackets.Add(")");
		ClosingBrackets.Add("]");
		
		// Boolean logic
		Bindings.Insert("&&", typeof(AndBinaryOperator));
		Bindings.Insert("and", typeof(AndBinaryOperator));
		Bindings.Insert("||", typeof(OrBinaryOperator));
		Bindings.Insert("or", typeof(OrBinaryOperator));
		Bindings.Insert("==", typeof(EqualityBinaryOperator));
		Bindings.Insert("!=", typeof(InequalityBinaryOperator));
		Bindings.Insert(">", typeof(LargerBinaryOperator));
		Bindings.Insert("<", typeof(SmallerBinaryOperator));
		Bindings.Insert("<=", typeof(SmallerEqualBinaryOperator));
		Bindings.Insert(">=", typeof(LargerEqualBinaryOperator));
		Bindings.Insert("!", typeof(NotOperator));
		
		// Statements
		Bindings.Insert("on", typeof(OnStatement));
		Bindings.Insert("else", typeof(ElseStatement));
		Bindings.Insert("while", typeof(WhileLoop));
		Bindings.Insert("function", typeof(FunctionStatement));
		Bindings.Insert("inherits", typeof(InheritsStatement));
		Bindings.Insert("return", typeof(ReturnStatement));
		Bindings.Insert("class", typeof(ClassStatement));
		Bindings.Insert("require", typeof(RequireStatement));
		
		// Separators
		Bindings.Insert(",", typeof(CommaSeparator));
		
		// Misc
		Bindings.Insert(":", typeof(ConcatenationOperator));
		Bindings.Insert(".", typeof(DotOperator));
		Bindings.Insert("...", typeof(UnlimitedArgumentOperator));
		Bindings.Insert("new", typeof(InstantiationOperator));

		// Low number for priority means a higher priority
		Priorities.Insert("(", 0);
		Priorities.Insert("[", 0);
		Priorities.Insert(".", 0);
		Priorities.Insert("!", 1);
		Priorities.Insert("^", 2);
		Priorities.Insert("*", 3);
		Priorities.Insert("/", 3);
		Priorities.Insert("%", 3);
		Priorities.Insert("+", 4);
		Priorities.Insert("-", 4);
		Priorities.Insert(">", 5);
		Priorities.Insert("<", 5);
		Priorities.Insert("<=", 5);
		Priorities.Insert(">=", 5);
		Priorities.Insert("==", 6);
		Priorities.Insert("!=", 6);
		Priorities.Insert("&&", 7);
		Priorities.Insert("and", 7);
		Priorities.Insert("||", 8);
		Priorities.Insert("or", 8);
		Priorities.Insert(":", 9);
		Priorities.Insert("=", 10);
		Priorities.Insert("decl", 11);
		
		// Standard defined variables
		Vars.Insert("print", new Function(new [] {new FunctionArgument {ArgType = typeof(Object), Name = "args", IsUnlimited = true}}, new Print(null!)));
		Vars.Insert("read", new Function(new FunctionArgument[0], new Read(null!)));
		Vars.Insert("false", new Boolean(false));
		Vars.Insert("true", new Boolean(true));
		Vars.Insert("args", new Array(new ArraySegment<string>(args, 1, args.Length-1).Select(s => new String(s))));
		Vars.Insert("null", new Null());
		Vars.Insert("File", new Builtin.Classes.File());

		string[] lines = File.ReadAllLines(args[0]);
		for (int i = 0; i < lines.Length; i++) {
			CheckedString[] lexedLine = Lexer.Lex(lines[i], i + 1);
		//	foreach (CheckedString cs in lexedLine)
		//		Console.Write("{0}, ", cs.Str);
		//	Console.WriteLine();
			lexedLine = Parser.CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;
			
			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine);

		//	Console.Write("[");
		//	foreach (Token t in tokenizedLine)
		//		Console.Write("{0}, ", t.Str);
		//	Console.WriteLine("\u0008\u0008]");
		
			Token tree = Parser.Parse(tokenizedLine, Parser.GetTopElementIndex(tokenizedLine, 0, true), lines, ref i, 0);

		//	Console.WriteLine(tree.ToString(0));
			tree.Evaluate(new List<TrieDictionary<Object>> {Vars});
		}
	}
}