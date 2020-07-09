using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SchemaGenerator
{
    public class GSgenerator
    {
        public async static Task<string> GenerateSchema<T>()
        {
            var obj = typeof(T).Name;
            string schema = "";
            string form = "";
            string data = "";
            schema = "{";
            form = "[";
            System.Attribute[] topattrs = System.Attribute.GetCustomAttributes(typeof(T));
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int x = 0;
            foreach (PropertyInfo prop in Props)
            {
               
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    GSchemaAttribute schemaAttr = attr as GSchemaAttribute;
                    if (schemaAttr != null)
                    {
                        string propName = prop.Name;
                        string name = schemaAttr.getName;
                        string title = schemaAttr.getTitle;
                        bool isrequired = schemaAttr.getIsRequired;
                        string regularExpression = schemaAttr.getRegularExpression;
                        string propType = schemaAttr.getType;
                        string defaultValue = schemaAttr.getDefaultValue;
                        string description = schemaAttr.getDiscription;
                        string htmlClass = schemaAttr.getHtmlClass;
                        string fieldHtmlClass = schemaAttr.getfieldHtmlClass;
                        string placeholder = schemaAttr.getPlaceHolder;
                        string activeClass = schemaAttr.getactiveClass;
                        bool message = schemaAttr.getmessage;
                        bool exclusiveMaximum = schemaAttr.getexclusiveMaximum;
                        bool exclusiveMinimum = schemaAttr.getexclusiveMinimum;
                        string EnumVal = schemaAttr.getEnumVal;
                        string minimum = schemaAttr.getminimum;
                        string maximum = schemaAttr.getMaximun;
                        string nameval = string.IsNullOrWhiteSpace(name) ? propName : name;
                        if (x == 0)
                        {
                            schema += "\"" + nameval + "\":{\"type\":\"string\"";
                            form += "{\"key\":\"" + nameval + "\"";
                        }
                        else
                        {
                            schema += ",\"" + nameval + "\":{\"type\":\"string\"";
                            form += ",{\"key\":\"" + nameval + "\"";
                        }
                        
                        if (!string.IsNullOrEmpty(title))
                        {
                            schema += ",\"title\":\"" + title + "\"";
                        }
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            schema += ",\"default\":\"" + defaultValue + "\"";
                        }
                        if (!string.IsNullOrEmpty(description))
                        {
                            schema += ",\"description\":\"" + description + "\"";
                        }
                        if (isrequired)
                        {
                            schema += ",\"required\":true";
                        }
                        if (!string.IsNullOrEmpty(EnumVal))
                        {
                            schema += ",\"enum\":" +await getEnumList.getEnumRecords(EnumVal);
                        }
                        if (!string.IsNullOrEmpty(regularExpression))
                        {
                            schema += ",\"pattern\":\"" + regularExpression+"\"";
                        }
                        if (message)
                        {
                            schema += ",\"messages\":" + await getEnumList.getVlidationMessage(nameval);
                        }
                        if (!string.IsNullOrEmpty(minimum))
                        {
                            schema += ",\"minimum\":" + minimum;
                        }
                        if (!string.IsNullOrEmpty(maximum))
                        {
                            schema += ",\"maximum\":" + maximum;
                        }
                        if (exclusiveMinimum)
                        {
                            schema += ",\"exclusiveMinimum\":" + exclusiveMinimum;
                        }
                        if (exclusiveMaximum)
                        {
                            schema += ",\"exclusiveMaximum\":" + exclusiveMaximum;
                        }

                        if (!string.IsNullOrEmpty(propType) && !propType.Equals("string"))
                        {
                            form += ",\"type\":\"" + propType + "\"";
                        }
                        
                        if (!string.IsNullOrEmpty(placeholder))
                        {
                            form += ",\"placeholder\":\"" + placeholder + "\"";
                        }
                        if (!string.IsNullOrEmpty(htmlClass))
                        {
                            form += ",\"htmlClass\":\"" + htmlClass + "\"";
                        }
                        if (!string.IsNullOrEmpty(fieldHtmlClass))
                        {
                            form += ",\"fieldHtmlClass\":\"" + fieldHtmlClass + "\"";
                        }
                        if (!string.IsNullOrEmpty(activeClass))
                        {
                            form += ",\"activeClass\":\"" + activeClass + "\"";
                        }
                        schema += "}";
                        form += "}";
                        x++;
                    }
                }
            }
            schema += "}";
            form += ",{\"type\":\"submit\",\"title\":\"Submit\"}]";
            string schemaFile = "{\"schema\":" + schema+",\"form\":"+form+"}";
            return schemaFile;
        }
    }
}
