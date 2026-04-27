using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GestorTareas.Application.Services;

public class Validator
{

    public static void ValidateString(string stringItem, string message)
    {
        Regex noValidCharacters = new Regex(@"[^a-zA-Z0-9\sáéíóúÁÉÍÓÚñÑ]");

        if (string.IsNullOrWhiteSpace(stringItem))
            throw new ArgumentException($"El {message} no puede estar vacío");

        if (noValidCharacters.IsMatch(stringItem))
            throw new FormatException($"{message} contiene caracteres no válidos.");

    }

    public static bool ValidateEnum(Type typeEnum, object item)
    {
        if (!Enum.IsDefined(typeEnum, item))
            return false;
        // throw new ArgumentException("El estado no es válido");
        return true;
    }


    public static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException("El mail no puede estar vacio.");

        string valueTrimmed = email.Trim();

        if (valueTrimmed.EndsWith("."))
            throw new ArgumentException("No es válido.");
        try
        {
           new MailAddress(valueTrimmed);

           return valueTrimmed; 
        }
        catch
        {
           throw new ArgumentException("No válido.");
        }
    }
}
