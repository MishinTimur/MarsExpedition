using System;

namespace App.Business.BusinessLayer
{
    public interface IDirtyModelParser
    {
        DateTime ParseDateOfBirth(string input);
        string ParseEmail(string input);
        string ParseName(string input);
        string ParsePhone(string input);
    }
}