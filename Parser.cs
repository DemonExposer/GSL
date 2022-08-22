using Interpreter.Tokens;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;
using Interpreter.Tokens.Operators.Binary.Boolean;
using Interpreter.Tokens.Operators.N_Ary;
using Interpreter.Tokens.Operators.Unary;
using Interpreter.Tokens.Separators;
using Interpreter.Tokens.Statements.Binary;
using Interpreter.Tokens.Statements.Unary;

namespace Interpreter;

public class Parser {
	public static int GetTopElementIndex(Token[] line, int startIndex, bool isRightBound) {
		if (line[startIndex] is BinaryStatement or MultilineStatementOperator)
			return startIndex;

		int highestPriorityNum = -1;
		int index = -1;
		for (int i = startIndex; i < line.Length && i >= 0; i += isRightBound ? 1 : -1) {
			int numBrackets = 0;
			if (Program.OpeningBrackets.Contains(line[i].Str))
				numBrackets++;
			else if (Program.ClosingBrackets.Contains(line[i].Str))
				numBrackets--;

			while (numBrackets != 0) {
				i += isRightBound ? 1 : -1;
				if (Program.OpeningBrackets.Contains(line[i].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[i].Str))
					numBrackets--;
			}

			if (!line[i].IsDone && line[i] is BinaryOperator or UnaryStatement && Program.Priorities.Contains(line[i].Str)) {
				int priority = Program.Priorities[line[i].Str];
				if (isRightBound ? priority >= highestPriorityNum : priority > highestPriorityNum) {
					highestPriorityNum = priority;
					index = i;
				}
			}
		}

		if (index == -1)
			return startIndex;

		return index;
	}

	public static CheckedString[] CheckComment(CheckedString[] line) {
		for (int i = 0; i < line.Length; i++)
			if (line[i].Str == "#")
				return line.Take(i).ToArray();

		return line;
	}

	/**
	 * This now practically just parses everything, so maybe some refactoring is needed
	 */
	private static Token SymmetricBinaryOperatorParse(Token[] line, int i, string[] lines, ref int lineNo, int depth, bool isRightBound) {
		int startIndex = i + (isRightBound ? 1 : -1);
		int j = GetTopElementIndex(line, startIndex, isRightBound);
		if (j == startIndex && !isRightBound) {
			int numBrackets = 0;
			while (numBrackets != 0 || j >= 0 && line[j] is not BinaryOperator) {
				if (Program.OpeningBrackets.Contains(line[j].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[j].Str))
					numBrackets--;

				j--;
			}
		}

		// BinaryOperator does not exist or is already done
		if (j < 0 || line[j].IsDone)
			if (!isRightBound)
				return Parse(line, j + 1, lines, ref lineNo, depth + 1);
			else
				return Parse(line, i + 1, lines, ref lineNo, depth + 1);

		// BinaryOperator does exist and is not done, parse it
		return Parse(line, j, lines, ref lineNo, depth + 1);
	}

	/**
	 * Removes brackets and parses inside expression(s) by identifying the top operator and calling Parse
	 */
	private static Token[] BracketsParse(Token[] line, int i, string[] lines, ref int lineNo, int depth, bool isRightBound) {
		List<Token[]> arguments = new List<Token[]>();
		List<Token> subLine = new List<Token>();
		int numBrackets = 0;
		for (int j = i + (isRightBound ? 1 : -1); isRightBound ? !Program.ClosingBrackets.Contains(line[j].Str) : !Program.OpeningBrackets.Contains(line[j].Str); j += isRightBound ? 1 : -1) {
			if (Program.OpeningBrackets.Contains(line[j].Str))
				numBrackets++;
			else if (Program.ClosingBrackets.Contains(line[j].Str))
				numBrackets--;

			while (numBrackets != 0) {
				if (isRightBound)
					subLine.Add(line[j]);
				else
					subLine = subLine.Prepend(line[j]).ToList();

				j += isRightBound ? 1 : -1;

				if (Program.OpeningBrackets.Contains(line[j].Str))
					numBrackets++;
				else if (Program.ClosingBrackets.Contains(line[j].Str))
					numBrackets--;
			}

			// When a comma is found, add the current buffer as an argument and start on a new argument
			if (line[j] is CommaSeparator) {
				if (isRightBound)
					arguments.Add(subLine.ToArray());
				else
					arguments = arguments.Prepend(subLine.ToArray()).ToList();
				subLine = new List<Token>();
				continue;
			}

			if (isRightBound)
				subLine.Add(line[j]);
			else
				subLine = subLine.Prepend(line[j]).ToList();
		}

		if (subLine.Count > 0)
			if (isRightBound)
				arguments.Add(subLine.ToArray());
			else
				arguments = arguments.Prepend(subLine.ToArray()).ToList();

		// Parse each element in the brackets and put it in a new list
		List<Token> properArguments = new List<Token>();
		for (int j = 0; j < arguments.Count; j++)
			properArguments.Add(Parse(arguments[j], GetTopElementIndex(arguments[j].ToArray(), 0, isRightBound), lines, ref lineNo, depth + 1));

		return properArguments.ToArray();
	}

	private static MultilineStatementOperator CurlyBracketsParse(Token[] line, string[] lines, ref int i, Token parent, int depth) {
		List<Token> tokens = new List<Token>();
		int initialIndex = i;
		int numBrackets = 1;
		bool isFirstBracketFound = false;
		MultilineStatementOperator firstFoundBracket = null!;
		for (; i < lines.Length; i++) {
			CheckedString[] lexedLine = Lexer.Lex(lines[i], i + 1);

			lexedLine = CheckComment(lexedLine);
			if (lexedLine.Length == 0)
				continue;

			Token[] tokenizedLine = Tokenizer.Tokenize(lexedLine);

			// Find first curly bracket
			int before = i;
			bool doContinue = false;
			if (!isFirstBracketFound) {
				doContinue = true;
				for (int j = 0; j < tokenizedLine.Length; j++) {
					if (tokenizedLine[j] is MultilineStatementOperator mso) {
						isFirstBracketFound = true;
						firstFoundBracket = mso;
						tokenizedLine = new ArraySegment<Token>(tokenizedLine, j + 1, tokenizedLine.Length - (j + 1)).ToArray();
						doContinue = tokenizedLine.Length == 0; // Happens if first line of declaration ends with an opening curly bracket
						break;
					}
				}
			}

			if (doContinue) // Happens for before mentioned case and if no curly bracket is found at all
				continue;

			tokens.Add(Parse(tokenizedLine, GetTopElementIndex(tokenizedLine, 0, true), lines, ref i, depth + 1));

			if (i != before)
				continue;

			foreach (Token t in tokenizedLine) {
				if (t.Str == "}")
					numBrackets--;
				else if (t.Str == "{")
					numBrackets++;

				if (numBrackets == 0) {
					if (parent is OnStatement onStat && tokenizedLine.Length > 1 && tokenizedLine[1] is ElseStatement)
						onStat.ElseChild = (ElseStatement) Parse(tokenizedLine, 1, lines, ref i, depth + 1);

					goto FullBreak;
				}
			}
		}
		FullBreak:

		if (i >= lines.Length)
			throw new FormatException("no matched bracket for bracket on line " + (initialIndex + 1));

		firstFoundBracket.Children = tokens.ToArray();

		return firstFoundBracket;
	}

	private static DictionaryAssignmentOperator[] SimpleObjectParse(Token[] line, string[] lines, ref int i, int startIndex, int depth) {
		int initialIndex = i;
		int numBrackets = 0;
		List<Token[]> properLines = new List<Token[]>();
		List<Token> subLine = new List<Token>();
		for (; i < lines.Length; i++) {
			Token[] tokenizedLine;
			int j = 0;
			if (i == initialIndex) {
				tokenizedLine = line;
				j = startIndex;
			} else {
				CheckedString[] lexedLine = Lexer.Lex(lines[i], i + 1);

				lexedLine = CheckComment(lexedLine);
				if (lexedLine.Length == 0)
					continue;

				tokenizedLine = Tokenizer.Tokenize(lexedLine);
			}

			for (; j < tokenizedLine.Length; j++) {
				Token t = tokenizedLine[j];
				if (t.Str == "}")
					numBrackets--;
				else if (t.Str == "{") {
					if (numBrackets == 0) { // Exclude opening bracket from line
						numBrackets++;
						continue;
					}
					numBrackets++;
				}

				if (t is CommaSeparator && numBrackets == 1) {
					properLines.Add(subLine.ToArray());
					subLine = new List<Token>();
					continue;
				}

				if (numBrackets == 0) {
					properLines.Add(subLine.ToArray());
					goto FullBreak;
				}

				subLine.Add(t);
			}
		}
		FullBreak:

		List<DictionaryAssignmentOperator> objectBody = new List<DictionaryAssignmentOperator>();
		foreach (Token[] properLine in properLines) {
			// TODO: fix the ConcatenationOperator thing, because right here it's not performing concatenation
			if (properLine[0] is not StringToken || properLine[1] is not ConcatenationOperator)
				throw new FormatException("simple objects should only consist of declarations");

			Token[] parseLine = new ArraySegment<Token>(properLine, 2, properLine.Length - 2).ToArray();
			DictionaryAssignmentOperator objAssOp = new DictionaryAssignmentOperator();
			objAssOp.Left = properLine[0];
			objAssOp.Right = Parse(parseLine, GetTopElementIndex(parseLine, 0, true), lines, ref i, depth + 1);
			objectBody.Add(objAssOp);
		}

		if (i >= lines.Length)
			throw new FormatException("no matched bracket for bracket on line " + (initialIndex + 1));

		return objectBody.ToArray();
	}

	public static Token Parse(Token[] line, int i, string[] lines, ref int lineNo, int depth) {
		Token t = line[i];

		switch (t) {
			// Check which lowest level class (i.e. most abstract), which can be parsed uniformly, the object is an instance of 
			case ArithmeticOperator or BooleanOperator or ConcatenationOperator or DotOperator: // BUG: DotOperator parsing incorrect: !file.exists() gets parsed as (!file).exists() instead of !(file.exists())
				line[i].IsDone = true;

				// Parse only the appropriate section (i.e. Left should only parse to the left and Right only to the right, that's what the array slicing does)
				((BinaryOperator) t).Left = SymmetricBinaryOperatorParse(line.Take(i + 1).ToArray(), i, lines, ref lineNo, depth + 1, false);
				((BinaryOperator) t).Right = SymmetricBinaryOperatorParse(new ArraySegment<Token>(line, i, line.Length - i).ToArray(), 0, lines, ref lineNo, depth + 1, true);
				break;
			case DeclarationOperator decOp: {
				decOp.Left = Parse(line, i + 1, lines, ref lineNo, depth + 1);

				if (i + 2 < line.Length) // Only Parse right hand side if it exists
					decOp.Right = Parse(line, i + 2, lines, ref lineNo, depth + 1);
				break;
			}
			case AssignmentOperator assOp: {
				assOp.IsDone = true;

				int numBrackets = 0;
				int j;
				// Gets the first variable token to the right of the nearest operator
				for (j = i; j >= 0 && line[j + 1] is not VariableToken || j == i; j--) {
					if (Program.OpeningBrackets.Contains(line[j].Str))
						numBrackets++;
					else if (Program.ClosingBrackets.Contains(line[j].Str))
						numBrackets--;

					while (numBrackets != 0) {
						j--;

						if (Program.OpeningBrackets.Contains(line[j].Str))
							numBrackets++;
						else if (Program.ClosingBrackets.Contains(line[j].Str))
							numBrackets--;
					}
				}

				if (j >= 0 && line[j] is DotOperator)
					j--;

				assOp.Left = Parse(line, j + 1, lines, ref lineNo, depth + 1);
				Token[] subLine = new ArraySegment<Token>(line, i, line.Length - i).ToArray();
				assOp.Right = Parse(subLine, GetTopElementIndex(subLine, 1, true), lines, ref lineNo, depth + 1);
				break;
			}
			case ParenthesesOperator parOp:
				parOp.Children = BracketsParse(line, i, lines, ref lineNo, depth + 1, Program.OpeningBrackets.Contains(line[i].Str));
				break;
			case SquareBracketOperator sqOp:
				sqOp.Children = BracketsParse(line, i, lines, ref lineNo, depth + 1, Program.OpeningBrackets.Contains(line[i].Str));
				break;
			case UnaryOperator unOp:
				unOp.Child = Parse(line, i + 1, lines, ref lineNo, depth + 1);
				break;
			case VariableToken vt: {
				if (i + 1 < line.Length)
					switch (line[i+1]) {
						case ParenthesesOperator:
							vt.Args = Parse(line, i + 1, lines, ref lineNo, depth + 1);
							break;
						case SquareBracketOperator:
							vt.Index = Parse(line, i + 1, lines, ref lineNo, depth + 1);
							break;
						case UnlimitedArgumentOperator:
							vt.IsUnlimited = true;
							break;
					}
				break;
			}
			case BinaryStatement statement: {
				int addition = 1;
				if (t is FunctionStatement fs) {
					addition = 2;
					fs.Name = line[i + 1].Str;
				}

				Token left = Parse(line, i + addition, lines, ref lineNo, depth + 1);
				if (left is not ParenthesesOperator po)
					throw new FormatException("statement condition/parameter declaration on line " + left.Line + " is missing parentheses");

				statement.Left = po;

				int numBrackets = 0;
				int j = i+1;
				do {
					if (Program.ClosingBrackets.Contains(line[j].Str))
						numBrackets--;
					else if (Program.OpeningBrackets.Contains(line[j].Str))
						numBrackets++;

					j++;
				} while (numBrackets != 0);

				statement.Right = CurlyBracketsParse(line, lines, ref lineNo, statement, depth + 1);
				break;
			}
			case RequireStatement reqStat:
				reqStat.Child = Parse(line, i + 1, lines, ref lineNo, depth + 1);
				reqStat.ParseImportFile();
				break;
			case ElseStatement or ClassStatement: {
				if (t is ClassStatement classStat) {
					classStat.Name = line[i + 1].Str;
					if (line[i + 2] is InheritsStatement)
						classStat.Parents = (InheritsStatement) Parse(line, i + 2, lines, ref lineNo, depth + 1);
				}

				Token child = CurlyBracketsParse(line, lines, ref lineNo, t, depth + 1);
				if (child is not MultilineStatementOperator mso)
					throw new FormatException("statement argument on line " + child.Line + " needs curly brackets");

				((UnaryStatement) t).Child = mso;
				break;
			}
			case UnaryStatement unStat: {
				Token[] subLine = new ArraySegment<Token>(line, i+1, line.Length - (i+1)).ToArray();
				unStat.Child = Parse(subLine, GetTopElementIndex(subLine, 0, true), lines, ref lineNo, depth + 1);
				break;
			}
			case MultilineStatementOperator mso: {
				if (mso.Str == "}")
					break;

				// If for some reason the Token[] is changed within mso and an element is replaced by a type
				// which is not a subclass or superclass of DictionaryAssignmentOperator, the program will error, but that should never happen
				mso.Children = SimpleObjectParse(line, lines, ref lineNo, i, depth + 1);
				mso.IsDictionary = true;
				break;
			}
		}

		t.Line = line[i].Line;

		line[i].IsDone = true;

		return t;
	}
}