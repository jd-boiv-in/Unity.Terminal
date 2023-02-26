using System.Collections.Generic;

namespace CommandTerminal
{
    public class CommandAutocomplete
    {
        Dictionary<string, CommandInfo> known_words = new Dictionary<string, CommandInfo>();
        List<string> buffer = new List<string>();

        public void Register(CommandInfo command) {
            known_words.Add(command.name.ToLower(), command);
        }

        public string[] Complete(ref string text, ref int format_width) {
            string partial_word = EatLastWord(ref text).ToLower();
            string known;

            foreach (var kv in known_words)
            {
                known = kv.Key;

                if (known.Contains(partial_word)) {
                    buffer.Add(kv.Value.name);

                    if (known.Length > format_width) {
                        format_width = known.Length;
                    }
                }
            }

            string[] completions = buffer.ToArray();
            buffer.Clear();

            text += PartialWord(completions);
            return completions;
        }

        string EatLastWord(ref string text) {
            int last_space = text.LastIndexOf(' ');
            string result = text.Substring(last_space + 1);

            text = text.Substring(0, last_space + 1); // Remaining (keep space)
            return result;
        }

        string PartialWord(string[] words) {
            if (words.Length == 0) {
                return "";
            }

            string first_match = words[0];
            int partial_length = first_match.Length;

            if (words.Length == 1) {
                return first_match;
            }

            foreach (string word in words) {
                if (partial_length > word.Length) {
                    partial_length = word.Length;
                }

                for (int i = 0; i < partial_length; i++) {
                    if (word[i] != first_match[i]) {
                        partial_length = i;
                    }
                }
            }
            return first_match.Substring(0, partial_length);
        }
    }
}
