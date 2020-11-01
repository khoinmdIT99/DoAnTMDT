using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Infrastructure.Common.Enums
{
    public static class CustomEnumUtility
    {
        /// To use this extantion method, the enum need to have CustomEnumAttribute with CustomEnumAttribute(true)
        public static string TextValue(this Enum myEnum)
        {
            string value = string.Empty;
            /*Check : if the myEnum is a custom enum*/
            var customEnumAttribute = (CustomEnumAttribute)myEnum
                                      .GetType()
                                      .GetCustomAttributes(typeof(CustomEnumAttribute), false)
                                      .FirstOrDefault();

            if (customEnumAttribute == null)
            {
                throw new Exception("The enum don't contain CustomEnumAttribute");
            }
            else if (customEnumAttribute.IsCustomEnum == false)
            {
                throw new Exception("The enum is not a custom enum");
            }

            /*Get the TextValueAttribute*/
            var textValueAttribute = (TextValueAttribute)myEnum
                                         .GetType().GetMember(myEnum.ToString()).Single()
                                         .GetCustomAttributes(typeof(TextValueAttribute), false)
                                         .FirstOrDefault();
            value = (textValueAttribute != null) ? textValueAttribute.Value : string.Empty;
            return value;
        }
    }
}
