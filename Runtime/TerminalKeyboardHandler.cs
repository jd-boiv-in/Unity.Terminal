using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CommandTerminal
{
    public class TerminalKeyboardHandler : MonoBehaviour
    {
        private Terminal terminal;

        void OnEnable()
        {
            if (terminal == null) terminal = GetComponent<Terminal>();

            Terminal.Buffer = new CommandLog(terminal.BufferSize);
            Terminal.Shell = new CommandShell();
            Terminal.History = new CommandHistory();
            Terminal.Autocomplete = new CommandAutocomplete();

            // Hook Unity log events
            Application.logMessageReceivedThreaded += terminal.HandleUnityLog;
        }

        void OnDisable()
        {
            Application.logMessageReceivedThreaded -= terminal.HandleUnityLog;
        }

        void Update()
        {
            // key presses won't be registered here while console is open and the input
            // field has focus, so they're handled in Terminal.OnGUI/DrawConsole
            var numOfToggleHotkeys = terminal.ToggleHotkeys.Length;
            for (int i = 0; i < numOfToggleHotkeys; i++)
            {
                var toggleHotkey = terminal.ToggleHotkeys[i];
                
#if ENABLE_INPUT_SYSTEM
                if (Keyboard.current.backquoteKey.wasPressedThisFrame)
                {
                    bool shift = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;

                    if (!terminal.enabled)
                    {
                        terminal.enabled = true;

                        if (shift)
                        {
                            terminal.SetState(TerminalState.OpenFull);
                        }
                        else
                        {
                            terminal.SetState(TerminalState.OpenSmall);
                        }

                        terminal.initial_open = true;
                    }
                    else
                    {
                        // this is only entered when console is open and
                        // the input field has lost focus

                        if (shift)
                        {
                            terminal.ToggleState(TerminalState.OpenFull);
                        }
                        else
                        {
                            terminal.SetState(TerminalState.Close);
                        }
                    }
                }
#else
                if (Input.GetKeyDown(toggleHotkey))
                {
                    bool shift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                    if (!terminal.enabled)
                    {
                        terminal.enabled = true;

                        if (shift)
                        {
                            terminal.SetState(TerminalState.OpenFull);
                        }
                        else
                        {
                            terminal.SetState(TerminalState.OpenSmall);
                        }

                        terminal.initial_open = true;
                    }
                    else
                    {
                        // this is only entered when console is open and
                        // the input field has lost focus

                        if (shift)
                        {
                            terminal.ToggleState(TerminalState.OpenFull);
                        }
                        else
                        {
                            terminal.SetState(TerminalState.Close);
                        }
                    }
                }
#endif
            }
        }
    }
}
