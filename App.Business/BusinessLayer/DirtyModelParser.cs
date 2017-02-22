using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Cache;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using com.google.i18n.phonenumbers;

namespace App.Business.BusinessLayer
{
    public class DirtyModelParser : IDirtyModelParser
    {
        public string ParseName(string input)
        {
            string res = input.Trim();
            res = Regex.Replace(res, @"\s+", " ");

            var nameParts = res.Split(' ');
            return string.Join(" ",
                nameParts.Select(s => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower())));

        }

        public DateTime ParseDateOfBirth(string input)
        {
            return DateTime.Parse(input);
        }

        public string ParseEmail(string input)
        {
            var instance = new EmailAddressAttribute();
            if (!instance.IsValid(input))
                throw new ArgumentException("input");
            return input;
        }

        public string ParsePhone(string input)
        {
            var util = PhoneNumberUtil.getInstance();
            var number = util.parse(input, "RU");
            return util.format(number, PhoneNumberUtil.PhoneNumberFormat.E164);
        }
    }
}
