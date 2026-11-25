using System;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class ModelValidator
    {
        public static ValidationResult Validate(object obj)
        {
            ValidationResult result = new ValidationResult();

            if (obj == null)
            {
                result.AddError("Object cannot be null.");
                return result;
            }

            Type type = obj.GetType();

            PropertyInfo[] properties = type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // Індексатори та властивості без публічного getter пропускаємо.
                if (property.GetIndexParameters().Length != 0 ||
                    property.GetGetMethod() == null)
                {
                    continue;
                }

                object? value = property.GetValue(obj);

                RequiredAttribute? required =
                    property.GetCustomAttribute<RequiredAttribute>();

                MaxLengthAttribute? maxLength =
                    property.GetCustomAttribute<MaxLengthAttribute>();

                MinValueAttribute? minValue =
                    property.GetCustomAttribute<MinValueAttribute>();

                if (required != null)
                {
                    if (value == null ||
                        value is string text &&
                        string.IsNullOrWhiteSpace(text))
                    {
                        result.AddError(
                            $"{property.Name}: {required.ErrorMessage}");
                    }
                }

                if (maxLength != null &&
                    value is string stringValue &&
                    stringValue.Length > maxLength.Length)
                {
                    string message = maxLength.ErrorMessage;

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        message =
                            $"Maximum length is {maxLength.Length} characters.";
                    }

                    result.AddError($"{property.Name}: {message}");
                }

                if (minValue != null && value != null)
                {
                    try
                    {
                        double number = Convert.ToDouble(value);

                        if (number < minValue.Min)
                        {
                            string message = minValue.ErrorMessage;

                            if (string.IsNullOrWhiteSpace(message))
                            {
                                message =
                                    $"Value must be at least {minValue.Min}.";
                            }

                            result.AddError($"{property.Name}: {message}");
                        }
                    }
                    catch (FormatException)
                    {
                        result.AddError(
                            $"{property.Name}: Value must be numeric.");
                    }
                    catch (InvalidCastException)
                    {
                        result.AddError(
                            $"{property.Name}: Value must be numeric.");
                    }
                    catch (OverflowException)
                    {
                        result.AddError(
                            $"{property.Name}: Numeric value is too large.");
                    }
                }
            }

            return result;
        }

        public static void PrintInfo(Type type)
        {
            Console.WriteLine(type.Name);

            PropertyInfo[] properties = type.GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                Console.Write($"  {property.Name,-16}");

                foreach (Attribute attribute in property.GetCustomAttributes())
                {
                    string attributeName = attribute.GetType().Name;

                    if (attributeName.EndsWith("Attribute"))
                    {
                        attributeName = attributeName.Substring(
                            0,
                            attributeName.Length - "Attribute".Length);
                    }

                    Console.Write($" [{attributeName}]");
                }

                Console.WriteLine();
            }
        }
    }
}