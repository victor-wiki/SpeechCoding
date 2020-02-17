using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpeechCodingHandler
{
    public class LanguageInterpreterHelper
    {
        public const string CommonLanguage = "Common";

        public static LanguageInterpreter GetInterpreter(string language)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeArray = assembly.ExportedTypes;

            var types = (from type in typeArray
                         where type.IsSubclassOf(typeof(LanguageInterpreter))
                         select type).ToList();

            foreach (var type in types)
            {
                LanguageInterpreter interpreter = (LanguageInterpreter)Activator.CreateInstance(type);

                if (interpreter.Language == language)
                {
                    return interpreter;
                }
            }

            return null;
        }        
    }
}
