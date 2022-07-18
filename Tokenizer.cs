using System.Data;
using System.Text.RegularExpressions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Tokens.Statements;
using Interpreter.Types.Comparable;
using TrieDictionary;
using Object = Interpreter.Types.Object;

namespace Interpreter; 

public class Tokenizer {
	public static Token[] Tokenize(CheckedString[] line, List<TrieDictionary<Object>> vars) {
		// TODO: Skip everything within curly brackets
		Token[] res = new Token[line.Length];
		
		for (int i = 0; i < line.Length; i++) {
			if (line[i].Str == "-") { // Minus is unary when it is the first token or if the previous token is an operator
				Type prevType = null!;
				try {
					prevType = Program.bindings[line[i - 1].Str];
				} catch (KeyNotFoundException) { }
				
				if (i == 0 || prevType != null!) {
					res[i] = new MinusUnaryOperator();
					res[i].Str = line[i].Str;
					continue;
				}
			}

			Type tokenType = null!;
			try {
				tokenType = Program.bindings[line[i].Str];
			} catch (KeyNotFoundException) { }
			
			if (tokenType != null!) { // Check if string is a keyword/operator
				res[i] = (Token) Activator.CreateInstance(tokenType)!; // Instantiate the corresponding class
			} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
				VariableToken vt = new VariableToken(vars);
				vt.Name = line[i].Str;
				res[i] = vt;
			} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
				NumberToken nt = new NumberToken();
				nt.Num = new Integer(Int32.Parse(line[i].Str));
				res[i] = nt;
			} else {
				throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
			}

			if (res[i] is MultilineStatementOperator mso)
				mso.Vars = vars;
			else if (res[i] is FunctionStatement fs)
				fs.Vars = vars;

			res[i].Str = line[i].Str;
			res[i].Line = line[i].Line;
		}

		return res;
	}
}