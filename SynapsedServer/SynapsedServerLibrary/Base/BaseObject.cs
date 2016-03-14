using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace SynapsedServerLibrary.Base
{
    public class BaseObject
    {
        public string AsJson()
        {
            System.Web.Script.Serialization.JavaScriptSerializer JsonSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return JsonSerializer.Serialize(this);
        }

        public string PrintProperties()
        {
            return PrintProperties(false);
        }

        public Dictionary<string, PropertyInfo> GetProperties()
        {
            PropertyInfo[] Properties;
            Properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Dictionary<string, PropertyInfo> PropertyDictionary = new Dictionary<string, PropertyInfo>();
            // Load PropertyDictionary for this type
            foreach (PropertyInfo Property in Properties)
            {

                // If it doesn't exist
                if (PropertyDictionary.ContainsKey(Property.Name) == false)
                {
                    PropertyDictionary.Add(Property.Name, Property);
                }
                else
                {
                    //Debug.WriteLine("Type:      " + Property.PropertyType.ToString());
                    //Debug.WriteLine("Base Type: " + Property.PropertyType.BaseType.ToString());
                    //Debug.WriteLine("Current:   " + PropertyDictionary[Property.Name].PropertyType.ToString());

                    // If this Property is not a base class of the existing, don't set it and continue
                    if (Property.PropertyType.BaseType != typeof(Base.BaseObject)
                        || Property.PropertyType.BaseType == PropertyDictionary[Property.Name].PropertyType)
                    {
                        //Debug.WriteLine("Replaced");
                        PropertyDictionary[Property.Name] = Property;
                    }
                    //Debug.WriteLine("Not updated");
                    // Only get here if Property is therefore not a Base for the known Property
                    continue;
                }
            }
            return PropertyDictionary;
        }


        public string PrintProperties(bool InHtml)
        {
            StringBuilder Output = new StringBuilder();

            Dictionary<string, System.Reflection.PropertyInfo> PropertyDictionary = GetProperties();
            if (InHtml == true) { Output.AppendLine("<table>"); }
            foreach (PropertyInfo Property in PropertyDictionary.Values)
            {
                if (InHtml == true)
                {
                    Output.AppendLine("<tr><td align='right'>");
                    string PropertyTypeString = Property.PropertyType.ToString();
                    if (PropertyTypeString.Length > 30)
                    {
                        for (int i = 0; i < PropertyTypeString.Length; i += 30)
                        {
                            if (i + 30 < PropertyTypeString.Length)
                            {
                                Output.AppendLine(PropertyTypeString.Substring(i, 30) + "<br/>");
                            }
                            else
                            {
                                Output.AppendLine(PropertyTypeString.Substring(i, PropertyTypeString.Length - i) + "<br/>");
                            }

                        }
                    }
                    else
                    {
                        Output.AppendLine("" + Property.PropertyType.ToString() + "");
                    }

                }
                else
                {
                    Output.AppendLine("\t[" + Utilities.Debug.PadToLength(Property.PropertyType.ToString(), 50, '-') + "]");
                }

                if (InHtml == true) { Output.AppendLine("</td><td align='right'>"); }
                Output.Append(Utilities.Debug.PadToLength(Property.Name, 30));
                if (InHtml == true) { Output.AppendLine("</td><td>"); }
                // Customise printout based on property types
                switch (Property.PropertyType.ToString())
                {
                    case "System.Collections.Generic.List`1[System.String]":
                        List<string> ValuesToPrint = (List<string>)Property.GetValue(this);
                        // if(ValuesToPrint == null) { break; }
                        Output.AppendLine(ValuesToPrint.Count + " entries");
                        foreach (string ValueToPrint in ValuesToPrint)
                        {
                            if (InHtml == true) { Output.AppendLine("<br/>"); }
                            Output.AppendLine("=> " + ValueToPrint);
                        }
                        break;

                    default:
                        Output.AppendLine("" + Property.GetValue(this));
                        break;
                }

                if (InHtml == true) { Output.AppendLine("</td></tr>"); }
            }
            if (InHtml == true) { Output.AppendLine("</table>"); }
            return Output.ToString();
        }

    }
}
