using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Loki.Gettext.PortableObject
{
	/// <summary>
	/// Represents a portable format file.
	/// </summary>
	public class File
	{
		private static readonly Regex CommentRegex = new Regex(@"^#(.)?(.*)$", RegexOptions.Compiled);
		private static readonly Regex DirectiveRegex = new Regex(@"^(\w+)\s*(.+)$", RegexOptions.Compiled);
		private static readonly Regex QuotedStringRegex = new Regex(@"^'((?:[^']|(?:\\['rnt\\]))*)'$".Replace("'", "\""), RegexOptions.Compiled);
		private static readonly Regex EscapeRegex = new Regex(@"\\([""rnt\\])", RegexOptions.Compiled);

		private readonly List<Entry> _entries;

		/// <summary>
		/// Initializes a new instance of the <see cref="File"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="language">The language.</param>
		/// <param name="entries">The entries.</param>
		public File(string name, string language, IEnumerable<Entry> entries = null)
		{
			Name = name;
			Language = language;

			_entries = entries != null ? new List<Entry>(entries) : new List<Entry>();
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the language.
		/// </summary>
		public string Language { get; set; }

		/// <summary>
		/// Gets the entries.
		/// </summary>
		public List<Entry> Entries
		{
			get { return _entries; }
		}

		/// <summary>
		/// Reads the content of portable format file from the specifeid reader.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>The portable format file.</returns>
		public static File ReadFrom(TextReader reader)
		{
			var tokens = EnumerateTokens(reader);
			var entries = EnumerateEntries(tokens.GetEnumerator());

			var name = default(string);
			var language = default(string);

			entries = entries.SkipWhile((x, index) => index == 0 && ReadMetadata(x, ref name, ref language)).ToArray();

			return new File(name, language, entries);
		}

		/// <summary>
		/// Writes the content of this portable format file into the specifeid writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public void WriteTo(TextWriter writer)
		{
			WriteDirective(writer, "msgid", string.Empty);
			WriteDirective(writer, "msgstr", string.Format("{0}Project-Id-Version: {1}{0}Language: {2}{0}", writer.NewLine, Name, Language));

			writer.WriteLine();

			foreach (var entry in Entries)
			{
				Write(writer, entry);
			}
		}

		private static void Write(TextWriter output, Entry entry)
		{
			foreach (var comment in entry.Comments)
			{
				output.WriteLine("#{0}{1}", WriteCommentType(comment.Type), comment.Value);
			}

			if (entry.Context != null)
			{
				WriteDirective(output, "msgctxt", entry.Context);
			}

			if (entry.OriginalText != null)
			{
				WriteDirective(output, "msgid", entry.OriginalText);
			}

			WriteDirective(output, "msgstr", entry.TranslatedText ?? string.Empty);

			output.WriteLine();
		}

		private static void WriteDirective(TextWriter output, string directive, string value)
		{
			output.Write(directive);
			output.Write(' ');

			WriteQuoted(output, value);

			output.WriteLine();
		}

		private static void WriteQuoted(TextWriter output, string value)
		{
			output.Write('"');

			foreach (var character in value)
			{
				switch (character)
				{
					case '\r':
						output.Write("\\r");
						break;
					case '\n':
						output.Write("\\n");
						break;
					case '\t':
						output.Write("\\t");
						break;
					case '"':
						output.Write("\\\"");
						break;
					default:
						output.Write(character);
						break;
				}
			}

			output.Write('"');
		}

		private static string WriteCommentType(CommentType commentType)
		{
			switch (commentType)
			{
				case CommentType.Plain:
					return "";
				case CommentType.SourceReference:
					return ":";
				case CommentType.Flag:
					return ",";
				case CommentType.Extracted:
					return ".";
				case CommentType.Previous:
					return "|";
				default:
					throw new ArgumentOutOfRangeException("commentType");
			}
		}

		private static IEnumerable<Token> EnumerateTokens(TextReader reader)
		{
			var lineNumber = 0;
			for (var line = reader.ReadLine(); line != null; line = reader.ReadLine())
			{
				var handles = line.
					Split(new[] { '\r', '\n' }, StringSplitOptions.None).
					Select(x => new TokenHandle(lineNumber, x.Trim())).
					ToArray();

				foreach (var handle in handles)
				{
					if (string.IsNullOrEmpty(handle.Line))
					{
						yield return new EmptyLineToken(handle);

						continue;
					}

					var comment = CommentRegex.Match(handle.Line);
					if (comment.Success)
					{
						var typeGroup = comment.Groups[1];
						var valueGroup = comment.Groups[2];

						yield return new CommentToken(typeGroup.Success ? typeGroup.Value : null, valueGroup.Value, handle);

						continue;
					}

					var quotedString = QuotedStringRegex.Match(handle.Line);
					if (quotedString.Success)
					{
						var group = quotedString.Groups[1];

						yield return new QuotedStringToken(EscapeRegex.Replace(group.Value, Unescape), handle);

						continue;
					}

					var directive = DirectiveRegex.Match(handle.Line);
					if (directive.Success)
					{
						var directiveGroup = directive.Groups[1];
						var valueGroup = directive.Groups[2];

						var value = QuotedStringRegex.Match(valueGroup.Value);
						if (!value.Success)
						{
							throw new ReadException("Expected quoted string after directive", handle.LineNumber, handle.Line);
						}

						yield return new DirectiveToken(directiveGroup.Value, handle);
						yield return new QuotedStringToken(EscapeRegex.Replace(value.Groups[1].Value, Unescape), handle);

						continue;
					}

					throw new ReadException("Unexpected token", handle.LineNumber, handle.Line);
				}

				lineNumber++;
			}
		}

		private static IEnumerable<Entry> EnumerateEntries(IEnumerator<Token> enumerator)
		{
			if (!enumerator.MoveNext())
			{
				yield break;
			}

			while (true)
			{
				for (var emptyLine = enumerator.Current as EmptyLineToken; emptyLine != null; emptyLine = enumerator.Current as EmptyLineToken)
				{
					if (!enumerator.MoveNext())
					{
						yield break;
					}
				}

				var comments = new List<Comment>();

				var context = new StringBuilder();
				var originalText = new StringBuilder();
				var translatedText = new StringBuilder();

				for (var comment = enumerator.Current as CommentToken; comment != null; comment = enumerator.Current as CommentToken)
				{
					comments.Add(ReadComment(comment));

					if (enumerator.MoveNext())
					{
						continue;
					}

					yield return new Entry {Comments = comments.ToArray()};
					yield break;
				}

				var directive = enumerator.Current as DirectiveToken;
				if (directive != null && directive.Directive == "msgctxt")
				{
					enumerator = ReadDirective(enumerator, "msgctxt", context);

					directive = enumerator.Current as DirectiveToken;
				}

				if (directive != null && directive.Directive == "msgid")
				{
					enumerator = ReadDirective(enumerator, "msgid", originalText);

					directive = enumerator.Current as DirectiveToken;
				}
				else
				{
					throw new ReadException(string.Format("Expected {0} directive", "msgid"), enumerator.Current.Position.LineNumber, enumerator.Current.Position.Line);
				}

				if (directive != null && directive.Directive == "msgstr")
				{
					enumerator = ReadDirective(enumerator, "msgstr", translatedText);
				}

				yield return new Entry
					{
						Comments = comments.ToArray(),
						Context = context.ToString(),
						OriginalText = originalText.ToString(),
						TranslatedText = translatedText.ToString()
					};

				if (enumerator.Current is QuotedStringToken && !enumerator.MoveNext())
				{
					yield break;
				}
			}
		}

		private static bool ReadMetadata(Entry entry, ref string name, ref string language)
		{
			if (!string.IsNullOrEmpty(entry.OriginalText))
			{
				return false;
			}

			var metadata = entry.TranslatedText.
				Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).
				Select(x => x.Split(':')).
				Where(x => x.Length == 2).
				ToDictionary(x => x[0].Trim(), x => x[1].Trim());

			metadata.TryGetValue("Project-Id-Version", out name);
			metadata.TryGetValue("Language", out language);
			return true;
		}

		private static Comment ReadComment(CommentToken token)
		{
			CommentType type;
			string value;

			switch (token.Type)
			{
				case ":":
					type = CommentType.SourceReference;
					value = token.Content;
					break;
				case ".":
					type = CommentType.Extracted;
					value = token.Content;
					break;
				case ",":
					type = CommentType.Flag;
					value = token.Content;
					break;
				case "|":
					type = CommentType.Previous;
					value = token.Content;
					break;
				default:
					type = CommentType.Plain;
					value = token.Type + token.Content;
					break;
			}

			return new Comment { Type = type, Value = value };
		}

		private static IEnumerator<Token> ReadDirective(IEnumerator<Token> enumerator, string directive, StringBuilder context)
		{
			if (!enumerator.MoveNext() || !(enumerator.Current is QuotedStringToken))
			{
				throw new ReadException(string.Format("Expected quoted string after {0} directive", directive), enumerator.Current.Position.LineNumber, enumerator.Current.Position.Line);
			}

			for (var value = (QuotedStringToken)enumerator.Current; value != null; value = enumerator.Current as QuotedStringToken)
			{
				context.Append(value.Content);

				if (!enumerator.MoveNext())
				{
					return enumerator;
				}
			}

			return enumerator;
		}

		private static string Unescape(Match match)
		{
			if (!match.Success)
			{
				return match.Value;
			}

			var group = match.Groups[1];
			switch (group.Value)
			{
				case "r":
					return "\r";
				case "n":
					return "\n";
				case "t":
					return "\t";
				default:
					return group.Value;
			}
		}

		private struct TokenHandle
		{
			public readonly int LineNumber;
			public readonly string Line;

			public TokenHandle(int lineNumber, string line)
				: this()
			{
				LineNumber = lineNumber;
				Line = line;
			}
		}

		private abstract class Token
		{
			public readonly TokenHandle Position;

			protected Token(TokenHandle position)
			{
				Position = position;
			}
		}

		private class CommentToken : Token
		{
			public readonly string Type;
			public readonly string Content;

			public CommentToken(string type, string content, TokenHandle position)
				: base(position)
			{
				Type = type;
				Content = content;
			}
		}

		private class DirectiveToken : Token
		{
			public readonly string Directive;

			public DirectiveToken(string directive, TokenHandle position)
				: base(position)
			{
				Directive = directive;
			}
		}

		private class EmptyLineToken : Token
		{
			public EmptyLineToken(TokenHandle position)
				: base(position)
			{
			}
		}

		private class QuotedStringToken : Token
		{
			public readonly string Content;

			public QuotedStringToken(string content, TokenHandle position)
				: base(position)
			{
				Content = content;
			}
		}
	}
}