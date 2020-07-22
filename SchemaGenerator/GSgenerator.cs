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
            StringBuilder schema = new StringBuilder();
            StringBuilder form = new StringBuilder();
            
            schema.Append("{");
            form.Append("[");
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
                            schema.Append( "\"" + nameval + "\":{\"type\":\"string\"");
                            form.Append( "{\"key\":\"" + nameval + "\"");
                        }
                        else
                        {
                            schema.Append( ",\"" + nameval + "\":{\"type\":\"string\"");
                            form.Append( ",{\"key\":\"" + nameval + "\"");
                        }
                        
                        if (!string.IsNullOrEmpty(title))
                        {
                            schema.Append( ",\"title\":\"" + title + "\"");
                        }
                        if (!string.IsNullOrEmpty(defaultValue))
                        {
                            schema.Append(",\"default\":\"" + defaultValue + "\"");
                        }
                        if (!string.IsNullOrEmpty(description))
                        {
                            schema.Append( ",\"description\":\"" + description + "\"");
                        }
                        if (isrequired)
                        {
                            schema.Append( ",\"required\":true");
                        }
                        if (!string.IsNullOrEmpty(EnumVal))
                        {
                            schema.Append( ",\"enum\":" +await getEnumList.getEnumRecords(EnumVal));
                        }
                        if (!string.IsNullOrEmpty(regularExpression))
                        {
                            schema.Append( ",\"pattern\":\"" + regularExpression+"\"");
                        }
                        if (message)
                        {
                            schema.Append( ",\"messages\":" + await getEnumList.getVlidationMessage(nameval));
                        }
                        if (!string.IsNullOrEmpty(minimum))
                        {
                            schema.Append( ",\"minimum\":" + minimum);
                        }
                        if (!string.IsNullOrEmpty(maximum))
                        {
                            schema.Append( ",\"maximum\":" + maximum);
                        }
                        if (exclusiveMinimum)
                        {
                            schema.Append( ",\"exclusiveMinimum\":" + exclusiveMinimum);
                        }
                        if (exclusiveMaximum)
                        {
                            schema.Append( ",\"exclusiveMaximum\":" + exclusiveMaximum);
                        }

                        if (!string.IsNullOrEmpty(propType) && !propType.Equals("string"))
                        {
                            form.Append( ",\"type\":\"" + propType + "\"");
                        }
                        
                        if (!string.IsNullOrEmpty(placeholder))
                        {
                            form.Append( ",\"placeholder\":\"" + placeholder + "\"");
                        }
                        if (!string.IsNullOrEmpty(htmlClass))
                        {
                            form.Append(",\"htmlClass\":\"" + htmlClass + "\"");
                        }
                        if (!string.IsNullOrEmpty(fieldHtmlClass))
                        {
                            form.Append( ",\"fieldHtmlClass\":\"" + fieldHtmlClass + "\"");
                        }
                        if (!string.IsNullOrEmpty(activeClass))
                        {
                            form.Append(",\"activeClass\":\"" + activeClass + "\"");
                        }
                        schema.Append( "}");
                        form.Append( "}");
                        x++;
                    }
                }
            }
            schema.Append( "}");
            form.Append( ",{\"type\":\"submit\",\"title\":\"Submit\"}]");
            string schemaFile = "{\"schema\":" + schema+",\"form\":"+form+"}";
            return schemaFile;
        }
    }
}
