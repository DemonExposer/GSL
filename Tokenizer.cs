using System.Data;
using System.Text.RegularExpressions;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Types.Comparable;
using String = Interpreter.Types.Comparable.String;

namespace Interpreter; 

public class Tokenizer {
	public static Token[] Tokenize(CheckedString[] line) {
		Token[] res = new Token[line.Length];
		
		for (int i = 0; i < line.Length; i++) {
			if (line[i].Str == "-") { // Minus is unary when it is the first token or if the previous token is an operator
				Type prevType = null!;
				try {
					prevType = Program.Bindings[line[i - 1].Str];
				} catch (KeyNotFoundException) { }
				
				if (i == 0 || prevType != null!) {
					res[i] = new MinusUnaryOperator();
					res[i].Str = line[i].Str;
					continue;
				}
			}

			Type tokenType = null!;
			try {
				tokenType = Program.Bindings[line[i].Str];
			} catch (KeyNotFoundException) { }
			
			if (tokenType != null!) { // Check if string is a keyword/operator
				res[i] = (Token) Activator.CreateInstance(tokenType)!; // Instantiate the corresponding class
			} else if (Regex.Matches(line[i].Str, "^[a-zA-Z]\\w*$").Count == 1) {
				VariableToken vt = new VariableToken();
				vt.Name = line[i].Str;
				res[i] = vt;
			} else if (Regex.Matches(line[i].Str, "(\\s|^)-?\\d+(\\s|$)").Count == 1) {
				NumberToken nt = new NumberToken();
				nt.Num = new Integer(Int32.Parse(line[i].Str));
				res[i] = nt;
			} else if (Regex.Matches(line[i].Str, "\".*\"").Count == 1) {
				StringToken st = new StringToken();
				st.StrVal = new String(line[i].Str.Substring(1, line[i].Str.Length-2));
				res[i] = st;
			} else {
				throw new InvalidExpressionException("Line " + line[i].Line + ": " + line[i].Str + " is not a valid expression");
			}

			res[i].Str = line[i].Str;
			res[i].Line = line[i].Line;
		}

		return res;
	}
}