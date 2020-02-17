using CsvHelper;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using WindowsInput;
using WindowsInput.Native;
using System;

namespace SpeechCodingHandler
{
    public static class KeyboardHelper
    {
        private static readonly string mappingFileName = "KeyboardMapping.csv";
        public static InputSimulator Simulator = new InputSimulator();

        public static List<KeyboardMapping> KeyboardMappings { get; private set; } = new List<KeyboardMapping>();

        static KeyboardHelper()
        {
            GetMappings();
        }

        static void GetMappings()
        {
            if (File.Exists(mappingFileName))
            {
                CsvReader reader = new CsvReader(new StringReader(File.ReadAllText(mappingFileName)), CultureInfo.CurrentCulture);
                reader.Configuration.HasHeaderRecord = false;

                while(reader.Read())
                {
                    KeyboardMappings.Add(new KeyboardMapping() { KeyCode = reader.GetField(0), Text = reader.GetField(1) });                        
                }              
            }
        }

        public static IKeyboardSimulator Write(this InputSimulator simulator, string text)
        {
            return simulator.Keyboard.TextEntry(text);
        }

        public static IKeyboardSimulator Write(this InputSimulator simulator, VirtualKeyCode code)
        {
            return simulator.Keyboard.KeyPress(code);
        }

        public static IKeyboardSimulator Write(this IKeyboardSimulator simulator, string text)
        {
            return simulator.TextEntry(text);
        }

        public static IKeyboardSimulator Write(this IKeyboardSimulator simulator, VirtualKeyCode code)
        {
            return simulator.KeyPress(code);
        }

        public static IKeyboardSimulator Write(string text)
        {
            return Simulator.Write(text);
        }

        public static IKeyboardSimulator Write(VirtualKeyCode code)
        {
            return Simulator.Write(code);
        }

        public static VirtualKeyCode Translate(string text)
        {
            VirtualKeyCode code = VirtualKeyCode.LBUTTON;

            KeyboardMapping mapping = KeyboardMappings.FirstOrDefault(item => item.Text == text);

            if (mapping != null)
            {
                if (Enum.TryParse(mapping.KeyCode, out code))
                {
                    return code;
                }
            }
            return code;
        }

        public static bool IsKeyboard(string text)
        {
            return Enum.TryParse(text, out VirtualKeyCode code);
        }
    }   
}
