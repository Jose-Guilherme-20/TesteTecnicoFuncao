using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CpfAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null) return false;

        var cpf = value.ToString();
        cpf = Regex.Replace(cpf, "[^0-9]", "");

        if (cpf.Length != 11) return false;

        var invalids = new[] {
            "00000000000", "11111111111", "22222222222", "33333333333",
            "44444444444", "55555555555", "66666666666", "77777777777",
            "88888888888", "99999999999"
        };
        if (Array.Exists(invalids, i => i == cpf)) return false;

        for (int t = 9; t < 11; t++)
        {
            int sum = 0;
            for (int i = 0; i < t; i++)
                sum += int.Parse(cpf[i].ToString()) * (t + 1 - i);

            int digit = sum * 10 % 11;
            if (digit == 10) digit = 0;

            if (cpf[t] != digit.ToString()[0])
                return false;
        }

        return true;
    }
}