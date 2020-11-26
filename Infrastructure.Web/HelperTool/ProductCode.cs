using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Web
{
    public static class ProductCode
    {

        static string KeySeperator = "-";
        private static readonly Random _random = new Random();
        /// <summary>
        /// The alpha chars
        /// </summary>
        private const string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// The numeric chars
        /// </summary>
        private const string NumericChars = "0123456789";
        /// <summary>
        /// The alpha numeric chars
        /// </summary>
        private const string AlphaNumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        /// <summary>
        /// The special valid email chars
        /// </summary>
        private const string SpecialValidEmailChars = "-_.";

        /// <summary>
        /// All valid chars
        /// </summary>
        private const string AllValidChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789,./?;:'*&^%$#@!~` ";
        ///<summary>
        ///<summary>
        ///Generate a product key using a random number generator with a seed value.
        ///</summary>
        private static string Generation(string chars, int length, Random random)
        {
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string CreateCodeEmail()
        {
            Random rd = new Random();
            string code = "";
            for (int i = 0; i < 6; i++)
            {
                code += rd.Next(1, 9).ToString();
            }
            return code;
        }
        public static string GenerateNewPassword()
        {
            var random = new Random();
            var length = random.Next(12, 20);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var generatedPassword = "";

            while (!generatedPassword.Any(char.IsUpper) || !generatedPassword.Any(char.IsDigit))
            {
                generatedPassword = Generation(chars, length, random);
            }

            return generatedPassword;
        }
        public static string RandomString(int maxLength, string characterSet)
        {
            var buffer = new char[maxLength];
            for (var i = 0; i < maxLength; i++)
            {
                buffer[i] = characterSet[_random.Next(characterSet.Length)];
            }

            return new string(buffer).Insert(5, KeySeperator).Insert(11, KeySeperator).Insert(17, KeySeperator).Insert(23, KeySeperator); ;
        }
        public static string GenerateKey(Int32 seedValue)
        {
            string productKey = "";
            Random keyDigit = new Random(seedValue);

            for (int x = 0; x < 25; x++)
            {
                //Decide whether the next digit will be a number or an english character.
                int generateDigit = keyDigit.Next(2);

                if (generateDigit == 0)
                {
                    var keyChar = Convert.ToChar(keyDigit.Next(65, 91));
                    productKey += keyChar;
                }
                else if (generateDigit == 1)
                {
                    var keyNum = (byte)keyDigit.Next(0, 1000);
                    productKey += keyNum;
                }
            }
            return productKey.Insert(5, KeySeperator).Insert(11, KeySeperator).Insert(17, KeySeperator).Insert(23, KeySeperator);

        }

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns>Random boolean value</returns>
        public static bool RandomBool()
        {
            return (_random.NextDouble() > 0.5);
        }
        public static int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random rdm = new Random();
            return rdm.Next(_min, _max);
        }
        /// <summary>
        /// Randoms the string.
        /// </summary>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="characterGroup">The character group.</param>
        /// <returns>System.String.</returns>
        public static string RandomString(int maxLength, RandomCharacterGroup characterGroup)
        {
            switch (characterGroup)
            {
                case RandomCharacterGroup.AlphaOnly:
                    return RandomString(maxLength, AlphaChars);
                case RandomCharacterGroup.NumericOnly:
                    return RandomString(maxLength, NumericChars);
                case RandomCharacterGroup.AlphaNumericOnly:
                    return RandomString(maxLength, AlphaNumericChars);
                default:
                    return RandomString(maxLength, AllValidChars);

            }

        }

        /// <summary>
        /// Enum RandomCharacterGroup
        /// </summary>
        public enum RandomCharacterGroup
        {
            /// <summary>
            /// The alpha only
            /// </summary>
            AlphaOnly,
            /// <summary>
            /// The numeric only
            /// </summary>
            NumericOnly,
            /// <summary>
            /// The alpha numeric only
            /// </summary>
            AlphaNumericOnly,
            /// <summary>
            /// Any character
            /// </summary>
            AnyCharacter
        }

        /// <summary>
        /// Generates a random Email address using the supplied top level domain.
        /// </summary>
        /// <param name="tld">Top Level Domain (e.g. "com", "net", "org", etc)</param>
        /// <returns>A randomly generated email address with the top level domain passed in.</returns>
        public static string RandomEmailAddress(string tld)
        {
            return string.Format("{0}@{1}.{2}", RandomString(10, RandomCharacterGroup.AlphaOnly),
                                 RandomString(15, RandomCharacterGroup.AlphaNumericOnly), tld);
        }

        /// <summary>
        /// Randoms the email address.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string RandomEmailAddress()
        {
            return
                $"{RandomString(10, RandomCharacterGroup.AlphaOnly)}@{RandomString(15, RandomCharacterGroup.AlphaNumericOnly)}.{"com"}";
        }

        /// <summary>
        /// Randoms the email address with Mozu ending..
        /// </summary>
        /// <returns>System.String.</returns>
        public static string RandomEmailAddressMozu()
        {
            return $"mozuqa+{RandomString(10, RandomCharacterGroup.AlphaOnly)}@gmail.com";
        }


        /// <summary>
        /// Randoms the day func.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <returns>Func{DateTime}.</returns>
        public static Func<DateTime> RandomDayFunc(DateTime startDate)
        {

            Random gen = new Random();
            var timeSpan = DateTime.Today - startDate;
            {
                int range = ((TimeSpan)timeSpan).Days;

                return () => startDate.AddDays(gen.Next(range));
            }
        }

        /// <summary>
        /// Randoms the int32.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int RandomInt32()
        {
            unchecked
            {
                int firstBits = _random.Next(0, 1 << 4) << 28;
                int lastBits = _random.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        /// <summary>
        /// Randoms the int32.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public static int RandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// Randoms the decimal.
        /// </summary>
        /// <param name="nonNegative">if set to <c>true</c> [non negative].</param>
        /// <returns>System.Decimal.</returns>
        public static decimal RandomDecimal(bool nonNegative)
        {
            var scale = (byte)_random.Next(29);
            return new decimal(RandomInt32(), RandomInt32(), RandomInt32(), nonNegative, scale);
        }

        /// <summary>
        /// Randoms the decimal.
        /// </summary>
        /// <param name="low">The low.</param>
        /// <param name="mid">The mid.</param>
        /// <param name="high">The high.</param>
        /// <param name="nonNegative">if set to <c>true</c> [non negative].</param>
        /// <returns>System.Decimal.</returns>
        public static decimal RandomDecimal(int low, int mid, int high, bool nonNegative)
        {
            var scale = (byte)_random.Next(29);
            return new decimal(low, mid, high, nonNegative, scale);
        }

        /// <summary>
        /// Random the decimal
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static decimal RandomDecimal(decimal min, decimal max)
        {
            decimal result = Convert.ToDecimal(_random.Next((int)(min * 100), (int)(max * 100))) / 100;
            return result;
        }



        /// <summary>
        /// Random IP address
        /// </summary>
        /// <returns></returns>
        public static string RandomIPAddress()
        {
            return RandomString(3, RandomCharacterGroup.NumericOnly) + "." +
            RandomString(3, RandomCharacterGroup.NumericOnly) + "." +
            RandomString(3, RandomCharacterGroup.NumericOnly) + "." +
            RandomString(3, RandomCharacterGroup.NumericOnly);
        }

        /// <summary>
        /// Random url
        /// </summary>
        /// <returns></returns>
        public static string RandomURL()
        {
            return "http://" + RandomString(4, RandomCharacterGroup.AlphaNumericOnly) + "/" +
                   RandomString(5, RandomCharacterGroup.AlphaNumericOnly);
        }

        /// <summary>
        /// Random UPC code
        /// </summary>
        /// <returns></returns>
        public static string RandomUPC()
        {
            string upc = "";
            int j;
            for (int i = 0; i < 12; i++)
            {
                j = _random.Next(0, 9) * 10 ^ i;
                upc += j.ToString();
            }
            return upc;
        }

        /// <summary>
        /// Random State
        /// </summary>
        /// <returns></returns>
        public static string RandomState()
        {
            string[] state =
                {
                    "AL", "AK", "AS", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FL", "GA", "GU", "HI", "ID",
                    "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MD", "MH", "MA", "MI", "FM", "MN", "MS", "MO", "MT", "NE",
                    "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC", "SD",
                    "TN", "TX", "UT", "VT", "VA", "VI", "WA", "WV", "WI", "WY"
                };

            return state[new Random().Next(0, state.Length)];
        }

        public static string RandomCompanyName()
        {
            string[] companyName = { "Idea", "Ideaa", "Aedi", "Idea Idea", "Idea Ine", "Idea Wiki", "Idea Leader", "Idea Canvas", "Idea Workshop", "Idea Horizon", "Idea Simple", "Idea Niche", "Idea Lens", "Idea Cent", "Idea Vine", "Idea Systems", "Idea Strategy", "Idea Emporium", "Ideaa", "Ideaar", "Ideamow", "Idea Next", "Idea Alliance", "Idea Technology", "Idea Crowd", "Ide Ine", "Ide Wiki", "Ide Leader", "Idea Cube", "Idea Network", "Idea Capital", "Idea Live", "Ide Niche", "Ide Lens", "Ide Cent", "Idea Venture", "Idea Affiliate", "Idea Future", "Idea Dev", "Idea", "Idear", "Idemow", "Idea Consultancy", "Idea Professionals", "Idea Topia", "Idea Strategies", "Id Ine", "Id Wiki", "Id Leader", "Mozuu", "Uzom", "Mozu Mozu", "Mozu Gam", "Mozu Number", "Mozu Giga", "Mozu Essence", "Mozu Insider", "Mozu Line", "Mozu Cube", "Mozu Ratio", "Mozu Tsar", "Mozu Tank", "Mozu Guardian", "Mozu Direct", "Mozu Lab", "Mozu Smart", "Mozuk", "Mozuwo", "Mozumoc", "Mozu Alliance", "Mozu Royal", "Mozu Launch", "Mozu Financial", "Moz Gam", "Moz Number", "Moz Giga", "Mozu Wise", "Mozu Enterprise", "Mozu Rockstar", "Mozu Crowd", "Moz Ratio", "Moz Tsar", "Moz Tank", "Mozu Consultancy", "Mozu Equinox", "Mozu Industries", "Mozu Boulevard", "Mozk", "Mozwo", "Mozmoc", "Mozu Foundry", "Mozu Vivid", "Mozu Division", "Mozu Epic", "Mo Gam", "Mo Number", "Mo Giga", "Mozu Group", "Mozu Vertical", "Mozu Business", "Mozu Lock", "Mo Ratio", "Mo Tsar", "Mo Tank", "Mozu Sage", "Mozu Dynamic", "Mozu Unlimited", "Mok", "Mowo", "Momoc", "Mozu Data", "Mozu Tracker", "Mozu Strategy", "Mozu Motion", "Sanct Mozu", "Number Mozu", "Giga Mozu", "Mozu Certified", "Mozu Vine", "Mozu Central", "Mozu Strategic", "Ratio Mozu", "Tsar Mozu", "Tank Mozu", "Mozu Fire", "Mozu Performance", "Mozu Innovation", "Mozu Arc", "Kmozu", "Womozu", "Mocmozu", "Mozu Designs", "Mozu Partners", "Mozu Vision", "Mozu Research", "Sanct Ozu", "Number Ozu", "Giga Ozu", "Mozu Quality", "Mozu Canvas", "Mozu Zone", "Mozu Synergy", "Ratio Ozu", "Tsar Ozu", "Tank Ozu", "Mozu Platinum", "Mozu Logistics", "Mozu App", "Mozu Edge", "Kozu", "Woozu", "Mocozu", "Mozu Outlet", "Mozu Theory", "Mozu Future", "Mozu Velocity", "Sanct Zu", "Number Zu", "Giga Zu", "Mozu Emporium", "Mozu Online", "Mozu Ondemand", "Mozu Strategies", "Ratio Zu", "Tsar Zu", "Tank Zu", "Mozu Bureau", "Mozu Charter", "Mozu Impact", "Mozu Fuse", "Kzu", "Wozu", "Moczu", "Mozu Target", "Mozu Network", "Mozu First", "Mozu Cyber", "Mozu Number", "Number Mozu", "Mozu Giga", "Mozu Interactive", "Mozu Hub", "Mozu Street", "Mozu Ninja", "Giga Mozu", "Mozu Ratio", "Ratio Mozu", "Mozu Quick", "Mozu Beacon", "Mozu Capital", "Mozu Tsar", "Tsar Mozu", "Mozu Tank", "Mozu Topia", "Mozu Cloud", "Mozu Bold", "Mozu Studios", "Tank Mozu", "Mozu Ine", "Mozu Ping", "Mozu Supplies", "Mozu Systems", "Mozu Ventures", "Mozu Software", "Mozu Vertical", "Mozu Mass", "Mozu Faux", "Mozu Professionals", "Mozu Rank", "Mozu Worldwide", "Mozu Centric", "Mozu Dude", "Mozuo", "Mozude", "Mozu Tactical", "Mozu Authority", "Mozu Consulting", "Mozu Work", "Mozuwon", "Moz Ine", "Moz Ping", "Mozu Foundary", "Mozu Web", "Mozu Anchor", "Mozu Zoom", "Moz Vertical", "Moz Mass", "Moz Faux", "Mozu Horizon", "Mozu Circle", "Mozu Ruby", "Mozu Labs", "Moz Dude", "Mozo", "Mozde", "Mozu Option", "Mozu Nexus", "Mozu Modern", "Mozu Live", "Mozwon", "Mo Ine", "Mo Ping", "Mozu Source", "Mozu Agent" };

            return companyName[new Random().Next(0, companyName.Length)];
        }
    }
}
