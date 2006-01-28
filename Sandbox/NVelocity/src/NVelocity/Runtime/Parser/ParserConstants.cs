namespace NVelocity.Runtime.Parser
{
	using System;

	/* Generated By:JJTree&JavaCC: Do not edit this line. ParserConstants.java */

	public class ParserConstants
	{
		public const int EOF = 0;
		public const int LBRACKET = 1;
		public const int RBRACKET = 2;
		public const int COMMA = 3;
		public const int DOUBLEDOT = 4;
		public const int LPAREN = 5;
		public const int RPAREN = 6;
		public const int REFMOD2_RPAREN = 7;
		public const int ESCAPE_DIRECTIVE = 8;
		public const int SET_DIRECTIVE = 9;
		public const int DOLLAR = 10;
		public const int DOLLARBANG = 11;
		public const int HASH = 15;
		public const int DOUBLE_ESCAPE = 16;
		public const int ESCAPE = 17;
		public const int TEXT = 18;
		public const int SINGLE_LINE_COMMENT = 19;
		public const int FORMAL_COMMENT = 20;
		public const int MULTI_LINE_COMMENT = 21;
		public const int WHITESPACE = 23;
		public const int STRING_LITERAL = 24;
		public const int TRUE = 25;
		public const int FALSE = 26;
		public const int NEWLINE = 27;
		public const int MINUS = 28;
		public const int PLUS = 29;
		public const int MULTIPLY = 30;
		public const int DIVIDE = 31;
		public const int MODULUS = 32;
		public const int LOGICAL_AND = 33;
		public const int LOGICAL_OR = 34;
		public const int LOGICAL_LT = 35;
		public const int LOGICAL_LE = 36;
		public const int LOGICAL_GT = 37;
		public const int LOGICAL_GE = 38;
		public const int LOGICAL_EQUALS = 39;
		public const int LOGICAL_NOT_EQUALS = 40;
		public const int LOGICAL_NOT = 41;
		public const int EQUALS = 42;
		public const int END = 43;
		public const int IF_DIRECTIVE = 44;
		public const int ELSEIF_DIRECTIVE = 45;
		public const int ELSE_DIRECTIVE = 46;
		public const int STOP_DIRECTIVE = 47;
		public const int DIGIT = 48;
		public const int NUMBER_LITERAL = 49;
		public const int LETTER = 50;
		public const int DIRECTIVE_CHAR = 51;
		public const int WORD = 52;
		public const int ALPHA_CHAR = 53;
		public const int ALPHANUM_CHAR = 54;
		public const int IDENTIFIER_CHAR = 55;
		public const int IDENTIFIER = 56;
		public const int DOT = 57;
		public const int LCURLY = 58;
		public const int RCURLY = 59;
		public const int REFERENCE_TERMINATOR = 60;
		public const int DIRECTIVE_TERMINATOR = 61;

		public const int DIRECTIVE = 0;
		public const int REFMOD2 = 1;
		public const int REFMODIFIER = 2;
		public const int DEFAULT = 3;
		public const int PRE_DIRECTIVE = 4;
		public const int REFERENCE = 5;
		public const int IN_MULTI_LINE_COMMENT = 6;
		public const int IN_FORMAL_COMMENT = 7;
		public const int IN_SINGLE_LINE_COMMENT = 8;

		public static readonly String[] TokenImage = new String[] {"<EOF>", "\"[\"", "\"]\"", "\",\"", "\"..\"", "\"(\"", "<RPAREN>", "\")\"", "<ESCAPE_DIRECTIVE>", "<SET_DIRECTIVE>", "<DOLLAR>", "<DOLLARBANG>", "\"##\"", "<token of kind 13>", "\"#*\"", "\"#\"", "\"\\\\\\\\\"", "\"\\\\\"", "<TEXT>", "<SINGLE_LINE_COMMENT>", "\"*#\"", "\"*#\"", "<token of kind 22>", "<WHITESPACE>", "<STRING_LITERAL>", "\"true\"", "\"false\"", "<NEWLINE>", "\"-\"", "\"+\"", "\"*\"", "\"/\"", "\"%\"", "\"&&\"", "\"||\"", "\"<\"", "\"<=\"", "\">\"", "\">=\"", "\"==\"", "\"!=\"", "\"!\"", "\"=\"", "<END>", "\"if\"", "\"elseif\"", "<ELSE_DIRECTIVE>", "\"stop\"", "<DIGIT>", "<NUMBER_LITERAL>", "<LETTER>", "<DIRECTIVE_CHAR>", "<WORD>", "<ALPHA_CHAR>", "<ALPHANUM_CHAR>", "<IDENTIFIER_CHAR>", "<IDENTIFIER>", "<DOT>", "\"{\"", "\"}\"", "<REFERENCE_TERMINATOR>", "<DIRECTIVE_TERMINATOR>"};
	}
}
