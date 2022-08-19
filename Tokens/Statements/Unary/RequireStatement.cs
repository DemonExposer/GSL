using Interpreter.Builtin.Functions;
using Interpreter.Types;
using Interpreter.Types.Function;
using TrieDictionary;
using Array = Interpreter.Types.Array;
using Object = Interpreter.Types.Object;
using Boolean = Interpreter.Types.Comparable.Boolean;

namespace Interpreter.Tokens.Statements.Unary; 

public class RequireStatement : UnaryStatement {
	private List<Token> importFile = new List<Token>();
	
	public RequireStatement() {
		Symbol = "require";
	}

	public override Object Evaluate(List<TrieDictionary<Object>> vars) {
		TrieDictionary<Object> variables = new TrieDictionary<Object>();
		variables["print"] = Program.Vars["print"];
		variables["read"] = Program.Vars["read"];
		variables["false"] = Program.Vars["false"];
		variables["true"] = Program.Vars["true"];
		variables["args"] = Program.Vars["args"];
		variables["null"] = Program.Vars["null"];
		variables["File"] = Program.Vars["File"];
		variables["export"] = new Dictionary();
		
		List<TrieDictionary<Object>> variableList = new List<TrieDictionary<Object>> {variables};
		importFile.ForEach(t => t.Evaluate(variableList));
		return variables["export"];
	}

	public void ParseImportFile() {
		string[] lines = File.ReadAllLines(Child.Str + ".gsl");
		for (int i = 0; i < lines.Length; i++) {
			CheckedString[] lexedLine = Lexer.Lex(lines[i], i + 1);
			
			lexedLine = Parser.CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;
			
			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine);
			
			importFile.Add(Parser.Parse(tokenizedLine, Parser.GetTopElementIndex(tokenizedLine, 0, true), lines, ref i, 0));
		}
	}
}