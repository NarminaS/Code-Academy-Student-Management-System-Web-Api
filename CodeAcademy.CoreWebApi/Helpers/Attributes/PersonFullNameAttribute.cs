using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.Helpers.Attributes
{
    public class PersonFullNameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                string fullName = (string)value;

                if (Char.IsUpper(fullName[0]))
                {
                    byte counter = 0;
                    foreach (char c in fullName)
                    {
                        if (Char.IsWhiteSpace(c) || Char.IsLetter(c))
                        {
                            counter++;
                        }
                    }

                    if (counter == fullName.Length)
                    {
                        return true;
                    }

                    ErrorMessage = "Name/Surname must contain only letters or whitespaces";
                    return false;
                }

                ErrorMessage = "Name/Surname must begin with upper";
                return false;
            }

            ErrorMessage = "Please enter some data";
            return false;
        }
    }
}
