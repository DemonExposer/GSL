using System.Text.RegularExpressions;

namespace Interpreter; 

public class Lexer {
	// Regex matching all valid strings, with the least complicated in the back so that e.g. == gets matched as == and not as =, =
	public static CheckedString[] Lex(string line, int lineNo) => Regex.Matches(line, "([a-zA-Z0-9]+|\\.\\.\\.|==|!=|\\|\\||&&|>=|<=|[\\^*/%+\\-=(){}#<>!,\\[\\]:.]|\"[^\"]*\")").Select(match => new CheckedString {Str = match.Value.Trim(), Line = lineNo}).ToArray();
}