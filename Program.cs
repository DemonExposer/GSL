﻿using DefaultNamespace;
using Interpreter.Tokens;
using Interpreter.Tokens.Operators;
using Interpreter.Tokens.Operators.Binary;
using Interpreter.Tokens.Operators.Binary.Arithmetic;

namespace Interpreter;

using System.Text.RegularExpressions;

public class Program {
	public static IDictionary<string, Type> bindings = new Dictionary<string, Type>();
	public static IDictionary<string, object> vars = new Dictionary<string, object>();
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

		// Low number for priority means a higher priority
		priorities.Add("(", 0);
		priorities.Add("^", 1);
		priorities.Add("*", 2);
		priorities.Add("/", 2);
		priorities.Add("+", 3);
		priorities.Add("-", 3);
		priorities.Add("=", 4);
		priorities.Add("decl", 5);

		string[] lines = File.ReadAllLines(args[0]);
		for (int i = 0; i < lines.Length; i++) {
			CheckedString[] lexedLine = Regex.Matches(lines[i], "([a-zA-Z1-9]+|\\d+|[\\^*/+-=()#])").ToList().Select(match => new CheckedString {Str = match.Value.Trim(), Line = i+1}).ToArray();
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
			
			int highestPriorityNum = -1;
			int index = -1;
			for (int j = 0; j < lexedLine.Length; j++) {
				int priority;
				if (tokenizedLine[j].Str == "(") {
					int numBrackets = 1;
					while (numBrackets > 0) {
						j++;
						if (tokenizedLine[j].Str == "(")
							numBrackets++;
						else if (tokenizedLine[j].Str == ")")
							numBrackets--;
					}
				}
				if (tokenizedLine[j] is BinaryOperator && priorities.TryGetValue(tokenizedLine[j].Str, out priority)) {
					if (priority >= highestPriorityNum) {
						highestPriorityNum = priority;
						index = j;
					}
				}
			}

			if (index == -1)
				throw new FormatException("Line " + (i + 1) + " contains no expression");
			
			Token tree = Parser.Parse(tokenizedLine, index, 0);
		//	Console.WriteLine(tree.ToString(0));
			Console.WriteLine(tree.Evaluate());
		}
	}
}
