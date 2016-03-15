using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System;
using Amazon.DynamoDBv2.Model;
using SynapsedServerLibrary.Tables.Defines;

namespace SynapsedServerLibrary.Model
{
    public class DataObject : Base.BaseObject
    {
        List<string> PropertiesToExclude = new List<string>()
        {
            "ThisTableDefinition"
        };

        private TableDefinitions _ThisTableDefinition;

        [System.Web.Script.Serialization.ScriptIgnore]
        public TableDefinitions ThisTableDefinition
        {
            get
            {
                return _ThisTableDefinition;
            }
            set
            {
                _ThisTableDefinition = value;
            }
        }

        private int _DataVersion = Defines.Global.DataVersionCurrent;
        public int DataVersion
        {
            get
            {
                return _DataVersion;
            }
            set
            {
                _DataVersion = value;
            }
        }

        private int _ModifiedByEntityId;
        [System.Web.Script.Serialization.ScriptIgnore]
        public int ModifiedByEntityId
        {
            get
            {
                return _ModifiedByEntityId;
            }
            set
            {
                _ModifiedByEntityId = value;
            }
        }

        private string _TimestampModified;
        public string TimestampModified
        {
            get
            {
                return _TimestampModified;
            }
            set
            {
                _TimestampModified = value;
            }
        }

        private string _TimestampCreation;
        public string TimestampCreation
        {
            get
            {
                return _TimestampCreation;
            }
            set
            {
                _TimestampCreation = value;
            }
        }
        //public void SetTimestampCreation(string Timestamp)
        //{
        //    this.TimestampCreation = Timestamp;
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("---Base Class---");
            sb.AppendLine("DataVersion:\t\t" + this.DataVersion);
            sb.AppendLine("ModifiedByEntityId:\t" + this.ModifiedByEntityId);
            sb.AppendLine("TimestampCreation:\t" + this.TimestampCreation);
            sb.AppendLine("TimestampModified:\t" + this.TimestampModified);
            return sb.ToString();
        }



        /// <summary>
        /// Uses reflection to convert a Amazon DynamoDBv2 Item to fill as many properties as possible
        /// </summary>
        /// <param name="Item">Amazon DynamoDBv2 Item</param>
        /// <param name="UnprocessedProperties"></param>
        /// <returns></returns>
        public string LoadFromItem(Dictionary<string, AttributeValue> Item, out Dictionary<string, AttributeValue> UnprocessedProperties)
        {
            StringBuilder Output = new StringBuilder();
            UnprocessedProperties = new Dictionary<string, AttributeValue>();

            Dictionary<string, PropertyInfo> PropertyDictionary = GetProperties();

            foreach (string Key in Item.Keys)
            {
                if (PropertyDictionary.ContainsKey(Key) == false)
                {
                    UnprocessedProperties.Add(Key, Item[Key]);
                    Output.AppendLine("Unable to inherently process " + Key + " of unrecognized type and value " + Item[Key]);
                    continue;
                }
                switch (PropertyDictionary[Key].PropertyType.ToString())
                {
                    case "System.String":
                        PropertyDictionary[Key].DeclaringType.GetProperty(Key).SetValue(this, Item[Key].S);
                        Output.AppendLine("Assigned: " + Key + " => " + PropertyDictionary[Key].GetValue(this));
                        break;

                    case "System.Boolean":
                        PropertyDictionary[Key].DeclaringType.GetProperty(Key).SetValue(this, Item[Key].BOOL);
                        Output.AppendLine("Assigned: " + Key + " => " + PropertyDictionary[Key].GetValue(this));
                        break;

                    case "System.Int32":
                        PropertyDictionary[Key].DeclaringType.GetProperty(Key).SetValue(this, Int32.Parse(Item[Key].N));
                        Output.AppendLine("Assigned: " + Key + " => " + PropertyDictionary[Key].GetValue(this));
                        break;

                    case "System.Collections.Generic.List`1[System.String]":
                        PropertyDictionary[Key].DeclaringType.GetProperty(Key).SetValue(this, Item[Key].SS);
                        Output.AppendLine("Assigned: " + Key);
                        List<string> RetrievedListString = (List<string>)PropertyDictionary[Key].GetValue(this);
                        foreach (string Element in RetrievedListString)
                        {
                            Output.AppendLine(" => " + Element);
                        }
                        break;

                    case "System.Collections.Generic.List`1[System.Int32]":
                        List<string> ReceivedListIntAsString = (List<string>)Item[Key].NS;
                        List<Int32> ConvertedListStringToInt = new
                            List<Int32>();
                        foreach (string IntAsString in ReceivedListIntAsString)
                        {
                            ConvertedListStringToInt.Add(Int32.Parse(IntAsString));
                        }
                        PropertyDictionary[Key].DeclaringType.GetProperty(Key).SetValue(this, ConvertedListStringToInt);
                        Output.AppendLine("Assigned: " + Key);
                        List<Int32> RetrievedListInt = (List<Int32>)PropertyDictionary[Key].GetValue(this);
                        foreach (Int32 Element in RetrievedListInt)
                        {
                            Output.AppendLine(" => " + Element);
                        }
                        break;

                    // If all else fails, return in UnprocessedProperties
                    default:
                        UnprocessedProperties.Add(Key, Item[Key]);
                        Output.AppendLine("Unable to inherently process " + Key + " of type " + PropertyDictionary[Key].PropertyType.ToString());
                        break;
                }
            }
            return Output.ToString();
        }

        /// <summary>
        /// Uses reflection to convert any object to an Item for upload
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, AttributeValue> ConvertToItem()
        {
            Dictionary<string, AttributeValue> Item = new Dictionary<string, AttributeValue>();

            Dictionary<string, PropertyInfo> PropertyDictionary = GetProperties();
            string Value = "";
            foreach (string Key in PropertyDictionary.Keys)
            {
                // Skip over properties we should excude
                if (PropertiesToExclude.Contains(Key) == true)
                {
                    Utilities.Debug.WriteLine("excluding: " + Key);
                    continue;
                }
                switch (PropertyDictionary[Key].PropertyType.ToString())
                {
                    case "System.String":
                        Value = PropertyDictionary[Key].GetValue(this) + "";
                        // Skip a value if empty
                        if (Value.Length > 0)
                        {
                            Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { S = Value });
                        }
                        break;

                    case "System.Boolean":
                        Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { BOOL = (bool)PropertyDictionary[Key].GetValue(this) });
                        break;

                    case "System.Int32":

                        Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { N = PropertyDictionary[Key].GetValue(this).ToString() });
                        break;

                    case "System.Collections.Generic.List`1[System.String]":
                        List<string> ListString = (List<string>)PropertyDictionary[Key].GetValue(this);
                        // Don't add this if empty
                        if (ListString.Count == 0) { break; }
                        Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { SS = ListString });
                        break;

                    case "System.Collections.Generic.List`1[System.Int32]":
                        List<string> ConvertListIntToListString = new List<string>();
                        List<Int32> ListInt = (List<Int32>)PropertyDictionary[Key].GetValue(this);
                        if (ListInt.Count == 0) { break; }
                        foreach (Int32 integer in ListInt)
                        {
                            ConvertListIntToListString.Add(integer.ToString());
                        }
                        Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { NS = ConvertListIntToListString });
                        break;

                    // If all else fails, include as string
                    default:
                        Item.Add(PropertyDictionary[Key].Name, new AttributeValue() { S = PropertyDictionary[Key].GetValue(this).ToString() });
                        break;
                }

            }
            return Item;
        }
    }
}
