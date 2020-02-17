using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WindowsInput.Native;

namespace SpeechCodingHandler
{
    public abstract class LanguageInterpreter
    {
        protected abstract string relativeFileExtension { get; }

        public virtual string RelativeFileExtension => StringHelper.GetNotEmptyValue(this.LanguageSetting?.RelativeFileExtension, this.relativeFileExtension);

        public Setting Setting { get; set; } = new Setting();

        public LanguageSetting LanguageSetting => this.Setting.LanguageSettings.FirstOrDefault(item => item.Language == this.Language);

        public abstract string Language { get; }

        public List<string> GetRelativeFileExtensions()
        {
            List<string> extensions = new List<string>();
            if (!string.IsNullOrEmpty(this.RelativeFileExtension))
            {
                extensions = this.RelativeFileExtension.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return extensions;
        }

        public string CodeTemplateFolder => Path.Combine(Utility.CurrentFolder, "CodeTemplate", this.Language);

        public string CommonCodeTemplateFolder => Path.Combine(Utility.CurrentFolder, "CodeTemplate", "Common");


        public virtual void Write(string text)
        {
            ControlWordSetting controlWordSetting = this.Setting.ControlWords.FirstOrDefault(item => !string.IsNullOrEmpty(item.Word) && item.Word == text);

            if (controlWordSetting != null)
            {
                string control = controlWordSetting.Control;

                bool useCodeTemplate = false;
                if (controlWordSetting.UseCodeTemplate)
                {
                    string extension = ".txt";

                    string templateFilePath = Path.Combine(this.CodeTemplateFolder, control + extension);

                    if (!File.Exists(templateFilePath))
                    {
                        templateFilePath = Path.Combine(this.CommonCodeTemplateFolder, control + extension);
                    }

                    if (File.Exists(templateFilePath))
                    {
                        useCodeTemplate = true;
                        text = File.ReadAllText(templateFilePath);
                    }
                }

                if (!useCodeTemplate)
                {
                    if (controlWordSetting.IsKeyboard)
                    {
                        VirtualKeyCode keyCode;
                        if (Enum.TryParse<VirtualKeyCode>(control, out keyCode))
                        {
                            KeyboardHelper.Write(keyCode);
                            return;
                        }
                    }
                    else
                    {
                        text = controlWordSetting.Control;
                    }
                }
            }

            string[] items = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            for (int i = 0; i < items.Length; i++)
            {
                string str = items[i];
                if (!string.IsNullOrEmpty(str))
                {
                    VirtualKeyCode keyCode = KeyboardHelper.Translate(str);
                    if (keyCode != VirtualKeyCode.LBUTTON)
                    {
                        KeyboardHelper.Write(keyCode);
                    }
                    else
                    {
                        KeyboardHelper.Write(str);
                    }
                }

                if ((i < items.Length - 1) || (i == items.Length - 1 && text.EndsWith(Environment.NewLine)))
                {
                    KeyboardHelper.Write(VirtualKeyCode.RETURN);
                }
            }
        }
    }
}
