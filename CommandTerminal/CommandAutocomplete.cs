using System.Collections.Generic;

namespace CommandTerminal
{
    public class CommandAutocomplete
    {
        Dictionary<string, CommandInfo> known_words = new Dictionary<string, CommandInfo>();
        List<string> buffer = new List<string>();
        public string lastInput = null;
        string[] lastResults = null;
        int lastIndex = 0;

        public void Register(CommandInfo command) {
            known_words.Add(command.name.ToLower(), command);
        }

        public (string[] all, int index) Complete(string inputText, bool backwards = false)
        {
            inputText = inputText.ToLower();

            if (inputText != lastInput)  // Calculate new completions
            {
                lastIndex = 0;
                lastInput = inputText;

                string known;

                foreach (var kv in known_words)
                {
                    known = kv.Key;

                    if (known.Contains(inputText))
                    {
                        buffer.Add(kv.Value.name);
                    }
                }

                buffer.Sort(Comparator);
                lastResults = buffer.ToArray();
                buffer.Clear();
            }
            else // Use previous completions
            {
                lastIndex += backwards ? -1 : 1;
                if (lastIndex < 0)
                {
                    lastIndex = lastResults.Length - 1;
                }
                else if (lastIndex >= lastResults.Length)
                {
                    lastIndex = 0;
                }
            }

            return (lastResults, lastIndex);
        }

        int Comparator(string x, string y)
        {
            // commands that start with the input should come first, or have input closer to beginning
            return x.ToLower().IndexOf(lastInput) - y.ToLower().IndexOf(lastInput);
        }
    }
}
