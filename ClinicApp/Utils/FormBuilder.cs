using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils
{
    public static class FormBuilder
    {
        public static T Build<T>() where T : new()
        {
            // Boxing один раз дозволяє працювати також зі структурами.
            object obj = new T();

            PropertyInfo[] properties = typeof(T).GetProperties(
                BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // GetSetMethod() повертає тільки публічний setter.
                if (property.GetSetMethod() == null ||
                    property.GetIndexParameters().Length != 0)
                {
                    continue;
                }

                List<string> hints = new List<string>();

                RequiredAttribute? required =
                    property.GetCustomAttribute<RequiredAttribute>();

                MaxLengthAttribute? maxLength =
                    property.GetCustomAttribute<MaxLengthAttribute>();

                MinValueAttribute? minValue =
                    property.GetCustomAttribute<MinValueAttribute>();

                if (required != null)
                {
                    hints.Add("обов'язкове");
                }

                if (maxLength != null)
                {
                    hints.Add($"макс. {maxLength.Length}");
                }

                if (minValue != null)
                {
                    hints.Add($"мін. {minValue.Min}");
                }

                string prompt = property.Name;

                if (hints.Count > 0)
                {
                    prompt += $" ({string.Join(", ", hints)})";
                }

                while (true)
                {
                    Console.Write($"{prompt}: ");

                    string? input = Console.ReadLine();

                    if (input == null)
                    {
                        throw new OperationCanceledException(
                            "Введення завершено. Створення об'єкта скасовано.");
                    }

                    object? converted;

                    try
                    {
                        Type targetType =
                            Nullable.GetUnderlyingType(property.PropertyType)
                            ?? property.PropertyType;

                        bool isNullable =
                            Nullable.GetUnderlyingType(property.PropertyType)
                            != null;

                        if (isNullable && string.IsNullOrWhiteSpace(input))
                        {
                            converted = null;
                        }
                        else if (targetType == typeof(string))
                        {
                            converted = input;
                        }
                        else if (targetType.IsEnum)
                        {
                            converted = Enum.Parse(
                                targetType,
                                input,
                                ignoreCase: true);

                            if (!Enum.IsDefined(targetType, converted))
                            {
                                throw new ArgumentException(
                                    "Невідоме значення переліку.");
                            }
                        }
                        else
                        {
                            converted = Convert.ChangeType(
                                input,
                                targetType,
                                CultureInfo.CurrentCulture);
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine(
                            "Некоректне значення, спробуйте ще раз.");
                        continue;
                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine(
                            "Значення завелике або замале, спробуйте ще раз.");
                        continue;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine(
                            "Некоректне значення, спробуйте ще раз.");
                        continue;
                    }
                    catch (InvalidCastException)
                    {
                        throw new NotSupportedException(
                            $"Тип властивості {property.Name} " +
                            $"({property.PropertyType.Name}) не підтримується.");
                    }

                    property.SetValue(obj, converted);
                    break;
                }
            }

            return (T)obj;
        }
    }
}