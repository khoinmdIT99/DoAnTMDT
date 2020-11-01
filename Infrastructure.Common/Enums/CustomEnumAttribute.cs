using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Common.Enums
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class CustomEnumAttribute : Attribute
    {
        public bool IsCustomEnum { get; set; }
        public CustomEnumAttribute(bool isCustomEnum) : this()
        {
            IsCustomEnum = isCustomEnum;
        }

        private CustomEnumAttribute()
        {
            IsCustomEnum = false;
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class TextValueAttribute : CustomEnumAttribute
    {
        public String Value { get; set; }
        public TextValueAttribute(string textValue) : this()
        {
            if (textValue == null)
            {
                throw new NullReferenceException("Null not allowed in textValue at TextValue attribute");
            }
            Value = textValue;
        }

        private TextValueAttribute() : base(true)
        {
            Value = string.Empty;
        }
    }
}
