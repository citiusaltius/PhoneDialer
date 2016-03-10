using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapsedServerLibrary.Utilities
{
    public class Debug
    {
        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static string PadToLength(string StringToPad, int DesiredLength)
        {
            return PadToLength(StringToPad, DesiredLength, ' ');
        }

        public static string PadToLength(string StringToPad, int DesiredLength, char DesiredCharacter)
        {
            StringBuilder PaddedString = new StringBuilder();
            PaddedString.Append(StringToPad);
            if (StringToPad.Length < DesiredLength)
            {
                for (int i = StringToPad.Length; i < DesiredLength; i++)
                {
                    PaddedString.Append(DesiredCharacter);
                }
            }
            else
            {
                PaddedString.Append("\t");
            }
            return PaddedString.ToString();
        }

        public static string IndentBySpaces(string StringsToIndent, int SpacesToIndent)
        {
            StringBuilder IndentedStrings = new StringBuilder();
            string[] Lines = StringsToIndent.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string Indent = "";
            // Add indent
            for (int i = 0; i < SpacesToIndent; i++)
            {
                Indent += " ";
            }
            // Indent each line
            foreach (string Line in Lines)
            {
                IndentedStrings.AppendLine(Indent + Line);
            }
            return IndentedStrings.ToString();
        }


        public static void SetNewSynchronizationContext()
        {
            System.Threading.SynchronizationContext.SetSynchronizationContext(new System.Threading.SynchronizationContext());
        }

        public static string PrintJsonClass(SimpleJSON.JSONClass Class)
        {
            StringBuilder PrintedClass = new StringBuilder();
            if (Class.Value.Length > 0)
            {
                PrintedClass.AppendLine(PadToLength("Class Value:", 25) + Class.Value);
            }
            foreach (string Key in (Class).GetChildKeys())
            {
                // PrintedClass.AppendLine("---" + Class.GetType().ToString() + "---");
                if (Class[Key].GetType() == typeof(SimpleJSON.JSONClass))
                {
                    PrintedClass.AppendLine(IndentBySpaces(
                            "---Class: " + Debug.PadToLength(Key + " (" + Class[Key].AsObject.Count + "):", 18)
                            + Class[Key].Value + "---"
                        , 3));

                    PrintedClass.AppendLine(IndentBySpaces(
                        PrintJsonClass((SimpleJSON.JSONClass)Class[Key])
                        , 3)
                        );
                }
                else if (Class[Key].GetType() == typeof(SimpleJSON.JSONArray))
                {
                    PrintedClass.AppendLine("---Array: " + Key + "---");
                    PrintedClass.AppendLine(IndentBySpaces(
                        PrintJsonArray(Class[Key].AsArray)
                        , 3));
                    PrintedClass.AppendLine("---Array: " + Key + "---");
                }
                else if (Class[Key].GetType() == typeof(SimpleJSON.JSONData))
                {
                    PrintedClass.AppendLine(IndentBySpaces(
                            Debug.PadToLength(Key + ":", 25)
                            + PrintJsonData((SimpleJSON.JSONData)Class[Key])
                            , 3));
                }
            }
            return PrintedClass.ToString();
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string PrintJsonArray(SimpleJSON.JSONArray Array)
        {
            StringBuilder PrintedArray = new StringBuilder();
            if (Array.Value.Length > 0)
            {
                PrintedArray.AppendLine(PadToLength("Array Value:", 25) + Array.Value);
            }
            for (int i = 0; i < Array.Count; i++)
            {
                PrintedArray.AppendLine(IndentBySpaces(
                        PrintJsonNode(Array[i])
                    , 3));
            }
            return PrintedArray.ToString();
        }

        public static string PrintJsonData(SimpleJSON.JSONData Data)
        {
            return Data.Value;
        }

        public static string PrintJsonNode(SimpleJSON.JSONNode Node)
        {
            if (Node.GetType() == typeof(SimpleJSON.JSONClass))
            {
                return PrintJsonClass(Node.AsObject);
            }
            else if (Node.GetType() == typeof(SimpleJSON.JSONArray))
            {
                return PrintJsonArray(Node.AsArray);
            }
            else if (Node.GetType() == typeof(SimpleJSON.JSONData))
            {
                return PrintJsonData((SimpleJSON.JSONData)Node);
            }
            return "Node not processed: " + Node.Value;
        }
    }
}
