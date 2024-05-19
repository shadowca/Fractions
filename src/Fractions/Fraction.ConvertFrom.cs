﻿using System;
using System.Globalization;
using System.Numerics;
using Fractions.Extensions;
using Fractions.Properties;
#if NET
using System.Buffers.Binary;
#endif

namespace Fractions;

public readonly partial struct Fraction {

   

    /// <summary>
    ///     Attempts to parse a ReadOnlySpan of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, string is parsed using the
    ///     <see cref="NumberStyles.Number" /> style and the thread current culture.
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     Here are some examples of how to use the
    ///     <see cref="TryParse(string, out Fraction)" /> method:
    ///     <example>
    ///         <code>
    /// Fraction.TryParse("3/4", out Fraction fraction);
    /// </code>
    ///         This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///         denominator of 4.
    ///         <code>
    /// Fraction.TryParse("1.25", out Fraction fraction);
    /// </code>
    ///         This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///         denominator of 4.
    ///         <code>
    /// Fraction.TryParse("1.23e-2", out Fraction fraction);
    /// </code>
    ///         This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and
    ///         a denominator of 10000.
    ///     </example>
    /// </remarks>
    public static bool TryParse(string value, out Fraction fraction) {
        return TryParse(value, NumberStyles.Number, null, true, out fraction);
    }

    /// <summary>
    ///     Attempts to parse a string of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, the parsing behavior is influenced by the
    ///     <paramref name="numberStyles" /> and <paramref name="formatProvider" /> parameters.
    /// </param>
    /// <param name="numberStyles">
    ///     A bitwise combination of number styles permitted in the input string. This is relevant when the string
    ///     is not in the numerator/denominator format. For instance, <see cref="NumberStyles.Float" /> allows decimal
    ///     points and scientific notation.
    /// </param>
    /// <param name="formatProvider">
    ///     An <see cref="IFormatProvider" /> that supplies culture-specific information used to parse the input string.
    ///     This is relevant when the string is not in the numerator/denominator format. For example,
    ///     <c>new CultureInfo("en-US")</c> for US English culture.
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The <paramref name="numberStyles" /> parameter allows you to specify which number styles are allowed in the input
    ///     string while the <paramref name="formatProvider" /> parameter provides culture-specific formatting information.
    ///     <para>
    ///         Here are some examples of how to use the
    ///         <see cref="TryParse(string, NumberStyles, IFormatProvider, out Fraction)" /> method:
    ///         <example>
    ///             <code>
    /// Fraction.TryParse("3/4", NumberStyles.Any, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.25", NumberStyles.Number, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.23e-2", NumberStyles.Float, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and
    ///             a denominator of 10000.
    ///         </example>
    ///     </para>
    /// </remarks>
    public static bool TryParse(string value, NumberStyles numberStyles, IFormatProvider formatProvider, out Fraction fraction) {
        return TryParse(value, numberStyles, formatProvider, true, out fraction);
    }

#if NET
    /// <summary>
    ///     Attempts to parse a string of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, the parsing behavior is influenced by the
    ///     <paramref name="numberStyles" /> and <paramref name="formatProvider" /> parameters.
    /// </param>
    /// <param name="numberStyles">
    ///     A bitwise combination of number styles permitted in the input string. This is relevant when the string
    ///     is not in the numerator/denominator format. For instance, <see cref="NumberStyles.Float" /> allows decimal
    ///     points and scientific notation.
    /// </param>
    /// <param name="formatProvider">
    ///     An <see cref="IFormatProvider" /> that supplies culture-specific information used to parse the input string.
    ///     This is relevant when the string is not in the numerator/denominator format. For example,
    ///     <c>new CultureInfo("en-US")</c> for US English culture.
    /// </param>
    /// <param name="normalize">
    ///     A boolean value indicating whether the parsed fraction should be reduced to its simplest form.
    ///     For example, if true, "4/8" will be reduced to "1/2".
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The <paramref name="numberStyles" /> parameter allows you to specify which number styles are allowed in the input
    ///     string while the <paramref name="formatProvider" /> parameter provides culture-specific formatting information.
    ///     <para>
    ///         Here are some examples of how to use the
    ///         <see cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider, bool, out Fraction)" /> method:
    ///         <example>
    ///             <code>
    /// Fraction.TryParse("3/4", NumberStyles.Any, new CultureInfo("en-US"), true, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.25", NumberStyles.Number, new CultureInfo("en-US"), true, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1,234.56", NumberStyles.Number, new CultureInfo("en-US"), false, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1,234.56" into a <see cref="Fraction" /> object with a numerator of 12345
    ///             and a
    ///             denominator of 100.
    ///             <code>
    /// Fraction.TryParse("1.23e-2", NumberStyles.Float, new CultureInfo("en-US"), false, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and
    ///             a
    ///             denominator of 10000.
    ///         </example>
    ///     </para>
    /// </remarks>
    public static bool TryParse(string value, NumberStyles numberStyles, IFormatProvider formatProvider, bool normalize, out Fraction fraction) {
        return TryParse(value.AsSpan(), numberStyles, formatProvider, normalize, out fraction);
    }

    /// <summary>
    ///     Attempts to parse a ReadOnlySpan of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, the parsing behavior is influenced by the
    ///     <paramref name="numberStyles" /> and <paramref name="formatProvider" /> parameters.
    /// </param>
    /// <param name="numberStyles">
    ///     A bitwise combination of number styles permitted in the input string. This is relevant when the string
    ///     is not in the numerator/denominator format. For instance, <see cref="NumberStyles.Float" /> allows decimal
    ///     points and scientific notation.
    /// </param>
    /// <param name="formatProvider">
    ///     An <see cref="IFormatProvider" /> that supplies culture-specific information used to parse the input string.
    ///     This is relevant when the string is not in the numerator/denominator format. For example,
    ///     <c>new CultureInfo("en-US")</c> for US English culture.
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The <paramref name="numberStyles" /> parameter allows you to specify which number styles are allowed in the input
    ///     string while the <paramref name="formatProvider" /> parameter provides culture-specific formatting information.
    ///     <para>
    ///         Here are some examples of how to use the
    ///         <see cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider, out Fraction)" /> method:
    ///         <example>
    ///             <code>
    /// Fraction.TryParse("3/4", NumberStyles.Any, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.25", NumberStyles.Number, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.23e-2", NumberStyles.Float, new CultureInfo("en-US"), out Fraction fraction);
    /// </code>
    ///             This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and
    ///             a denominator of 10000.
    ///         </example>
    ///     </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> value, NumberStyles numberStyles, IFormatProvider formatProvider, out Fraction fraction) {
        return TryParse(value, numberStyles, formatProvider, true, out fraction);
    }

    /// <summary>
    ///     Attempts to parse a ReadOnlySpan of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, the parsing behavior is influenced by the
    ///     <paramref name="numberStyles" /> and <paramref name="formatProvider" /> parameters.
    /// </param>
    /// <param name="numberStyles">
    ///     A bitwise combination of number styles permitted in the input string. This is relevant when the string
    ///     is not in the numerator/denominator format. For instance, <see cref="NumberStyles.Float" /> allows decimal
    ///     points and scientific notation.
    /// </param>
    /// <param name="formatProvider">
    ///     An <see cref="IFormatProvider" /> that supplies culture-specific information used to parse the input string.
    ///     This is relevant when the string is not in the numerator/denominator format. For example,
    ///     <c>new CultureInfo("en-US")</c> for US English culture.
    /// </param>
    /// <param name="normalize">
    ///     A boolean value indicating whether the parsed fraction should be reduced to its simplest form.
    ///     For example, if true, "4/8" will be reduced to "1/2".
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The <paramref name="numberStyles" /> parameter allows you to specify which number styles are allowed in the input
    ///     string while the <paramref name="formatProvider" /> parameter provides culture-specific formatting information.
    ///     <para>
    ///         Here are some examples of how to use the
    ///         <see cref="TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider, bool, out Fraction)" /> method:
    ///         <example>
    ///             <code>
    /// Fraction.TryParse("3/4", NumberStyles.Any, new CultureInfo("en-US"), true, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1.25", NumberStyles.Number, new CultureInfo("en-US"), true, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///             denominator of 4.
    ///             <code>
    /// Fraction.TryParse("1,234.56", NumberStyles.Number, new CultureInfo("en-US"), false, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1,234.56" into a <see cref="Fraction" /> object with a numerator of 12345
    ///             and a
    ///             denominator of 100.
    ///             <code>
    /// Fraction.TryParse("1.23e-2", NumberStyles.Float, new CultureInfo("en-US"), false, out Fraction fraction);
    ///         </code>
    ///             This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and
    ///             a
    ///             denominator of 10000.
    ///         </example>
    ///     </para>
    /// </remarks>
    public static bool TryParse(ReadOnlySpan<char> value, NumberStyles numberStyles, IFormatProvider formatProvider, bool normalize, out Fraction fraction) {
        if (value.IsEmpty) {
            return CannotParse(out fraction);
        }

        if (value.Length == 1) {
            if (!BigInteger.TryParse(value, numberStyles, formatProvider, out var singleDigit)) {
                return CannotParse(out fraction);
            }

            fraction = new Fraction(singleDigit);
            return true;
        }

        var ranges = new Span<Range>(new Range[2]);
        var nbRangesFilled = value.Split(ranges, '/');

        if (nbRangesFilled == 2) {
            var numeratorValue = value[ranges[0]];
            var denominatorValue = value[ranges[1]];

            var withoutDecimalPoint = numberStyles & ~NumberStyles.AllowDecimalPoint;
            if (!BigInteger.TryParse(
                    numeratorValue,
                    withoutDecimalPoint,
                    formatProvider,
                    out var numerator)
                || !BigInteger.TryParse(
                    denominatorValue,
                    withoutDecimalPoint,
                    formatProvider,
                    out var denominator)) {
                return CannotParse(out fraction);
            }

            fraction = new Fraction(numerator, denominator, normalize);
            return true;
        }

        // parsing a number using to the selected NumberStyles: e.g. " $ 12345.1234321e-4- " should result in -1.23451234321 with NumberStyles.Any
        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
        // check for any of the special symbols (these cannot be combined with anything else)
        if (value.SequenceEqual(numberFormatInfo.NaNSymbol.AsSpan())) {
            fraction = NaN;
            return true;
        }

        if (value.SequenceEqual(numberFormatInfo.PositiveInfinitySymbol.AsSpan())) {
            fraction = PositiveInfinity;
            return true;
        }

        if (value.SequenceEqual(numberFormatInfo.NegativeInfinitySymbol.AsSpan())) {
            fraction = NegativeInfinity;
            return true;
        }

        var currencyAllowed = (numberStyles & NumberStyles.AllowCurrencySymbol) != 0;
        var currencyDetected = false; // there "special" rules regarding the white-spaces after a leading currency symbol is detected
        ReadOnlySpan<char> currencySymbol;
        if (currencyAllowed) {
            currencySymbol = numberFormatInfo.CurrencySymbol.AsSpan();
            if (currencySymbol.IsEmpty) {
                currencyAllowed = false;
                numberStyles &= ~NumberStyles.AllowCurrencySymbol; // no currency symbol available
            }
        } else {
            currencySymbol = [];
        }

        ReadOnlySpan<char> decimalSeparator; // note: decimal.TryParse relies on the CurrencySymbol (when currency is detected)
        var decimalsAllowed = (numberStyles & NumberStyles.AllowDecimalPoint) != 0;
        if (decimalsAllowed) {
            decimalSeparator = numberFormatInfo.NumberDecimalSeparator.AsSpan();
            if (decimalSeparator.IsEmpty) {
                decimalsAllowed = false;
                numberStyles &= ~NumberStyles.AllowDecimalPoint; // no decimal separator available
            }
        } else {
            decimalSeparator = [];
        }

        var startIndex = 0;
        var endIndex = value.Length;
        var isNegative = false;

        // examine the leading characters
        do {
            var character = value[startIndex];
            if (char.IsDigit(character)) {
                break;
            }

            if (char.IsWhiteSpace(character)) {
                if ((numberStyles & NumberStyles.AllowLeadingWhite) == 0) {
                    return CannotParse(out fraction);
                }

                startIndex++;
                continue;
            }

            if (character == '(') {
                if ((numberStyles & NumberStyles.AllowParentheses) == 0) {
                    return CannotParse(out fraction); // not allowed
                }

                if (startIndex == endIndex - 1) {
                    return CannotParse(out fraction); // not enough characters
                }

                startIndex++; // consume the current character
                isNegative = true; // the closing parenthesis will be validated in the backwards iteration

                if (currencyDetected) {
                    // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                    numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                    // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                    currencyDetected = true;
                    startIndex += currencySymbol.Length;
                    numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);
                } else {
                    // if next character is a white space the format should be rejected
                    numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    break;
                }

                continue;
            }

            if ((numberStyles & NumberStyles.AllowLeadingSign) != 0) {
                if (numberFormatInfo.NegativeSign.AsSpan() is { IsEmpty: false } negativeSign && startsWith(value, negativeSign, startIndex)) {
                    isNegative = true;
                    startIndex += negativeSign.Length;
                    if (currencyDetected) {
                        // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowHexSpecifier);
                    } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                        // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                        currencyDetected = true;
                        startIndex += currencySymbol.Length;
                        numberStyles &=
                            ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol |
                              NumberStyles.AllowHexSpecifier);
                    } else {
                        // if next character is a white space the format should be rejected
                        numberStyles &=
                            ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowLeadingWhite |
                              NumberStyles.AllowHexSpecifier);
                        break;
                    }

                    continue;
                }

                if (numberFormatInfo.PositiveSign.AsSpan() is { IsEmpty: false } positiveSign && startsWith(value, positiveSign, startIndex)) {
                    isNegative = false;
                    startIndex += positiveSign.Length;
                    if (currencyDetected) {
                        // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowHexSpecifier);
                    } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                        // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                        currencyDetected = true;
                        startIndex += currencySymbol.Length;
                        numberStyles &=
                            ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol |
                              NumberStyles.AllowHexSpecifier);
                    } else {
                        // if next character is a white space the format should be rejected
                        numberStyles &=
                            ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowLeadingWhite |
                              NumberStyles.AllowHexSpecifier);
                        break;
                    }

                    continue;
                }
            }

            if (currencyAllowed && !currencyDetected && startsWith(value, currencySymbol, startIndex)) {
                // there can be no more currency symbols but there could be more white-spaces (we skip and continue)
                currencyDetected = true;
                numberStyles &= ~NumberStyles.AllowCurrencySymbol;
                startIndex += currencySymbol.Length;
                continue;
            }

            if (decimalsAllowed && startsWith(value, decimalSeparator, startIndex)) {
                break; // decimal string with no leading zeros
            }

            // this is either an expected hex string or an invalid format
            return (numberStyles & NumberStyles.AllowHexSpecifier) != 0
                ? TryParseInteger(value[startIndex..endIndex], numberStyles & ~NumberStyles.AllowTrailingSign, formatProvider, isNegative, out fraction)
                : CannotParse(out fraction); // unexpected character
        } while (startIndex < endIndex);

        if (startIndex >= endIndex) {
            return CannotParse(out fraction);
        }

        if (isNegative) {
            numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign);
        } else {
            numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowParentheses);
        }

        // examine the trailing characters
        do {
            var character = value[endIndex - 1];
            if (char.IsDigit(character)) {
                break;
            }

            if (char.IsWhiteSpace(character)) {
                if ((numberStyles & NumberStyles.AllowTrailingWhite) == 0) {
                    return CannotParse(out fraction);
                }

                endIndex--;
                continue;
            }

            if (character == ')') {
                if ((numberStyles & NumberStyles.AllowParentheses) == 0) {
                    return CannotParse(out fraction); // not allowed
                }

                numberStyles &= ~(NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol);
                endIndex--;
                continue;
            }

            if ((numberStyles & NumberStyles.AllowTrailingSign) != 0) {
                if (numberFormatInfo.NegativeSign.AsSpan() is { IsEmpty: false } negativeSign && endsWith(value, negativeSign, endIndex)) {
                    isNegative = true;
                    numberStyles &= ~(NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    endIndex -= negativeSign.Length;
                    continue;
                }

                if (numberFormatInfo.PositiveSign.AsSpan() is { IsEmpty: false } positiveSign && endsWith(value, positiveSign, endIndex)) {
                    isNegative = false;
                    numberStyles &= ~(NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    endIndex -= positiveSign.Length;
                    continue;
                }
            }

            if (currencyAllowed && !currencyDetected && endsWith(value, currencySymbol, endIndex)) {
                // there can be no more currency symbols but there could be more white-spaces (we skip and continue)
                currencyDetected = true;
                numberStyles &= ~NumberStyles.AllowCurrencySymbol;
                endIndex -= currencySymbol.Length;
                continue;
            }

            if (decimalsAllowed && endsWith(value, decimalSeparator, endIndex)) {
                break;
            }

            // this is either an expected hex string or an invalid format
            return (numberStyles & NumberStyles.AllowHexSpecifier) != 0
                ? TryParseInteger(value[startIndex..endIndex], numberStyles & ~NumberStyles.AllowTrailingSign, formatProvider, isNegative, out fraction)
                : CannotParse(out fraction); // unexpected character
        } while (startIndex < endIndex);

        if (startIndex >= endIndex) {
            return CannotParse(out fraction); // not enough characters
        }

        if (isNegative && (numberStyles & NumberStyles.AllowParentheses) != 0) {
            return CannotParse(out fraction); // failed to find a closing parentheses
        }

        numberStyles &= ~(NumberStyles.AllowTrailingWhite | NumberStyles.AllowTrailingSign | NumberStyles.AllowCurrencySymbol);
        // at this point value[startIndex, endIndex] should correspond to the number without the sign (or the format is invalid)
        var unsignedValue = value[startIndex..endIndex];

        if (unsignedValue.Length == 1) {
            // this can only be a single digit (integer)
            return TryParseInteger(unsignedValue, numberStyles, formatProvider, isNegative, out fraction);
        }

        if ((numberStyles & NumberStyles.AllowExponent) != 0) {
            return TryParseWithExponent(unsignedValue, numberStyles, numberFormatInfo, isNegative, normalize, out fraction);
        }

        return decimalsAllowed
            ? TryParseDecimalNumber(unsignedValue, numberStyles, numberFormatInfo, isNegative, normalize, out fraction)
            : TryParseInteger(unsignedValue, numberStyles, formatProvider, isNegative, out fraction);

        static bool startsWith(ReadOnlySpan<char> fractionString, ReadOnlySpan<char> testString, int startIndex) {
            return fractionString.Slice(startIndex, testString.Length).SequenceEqual(testString);
        }

        static bool endsWith(ReadOnlySpan<char> fractionString, ReadOnlySpan<char> testString, int endIndex) {
            return fractionString.Slice(endIndex - testString.Length, testString.Length).SequenceEqual(testString);
        }
    }

    private static bool TryParseWithExponent(ReadOnlySpan<char> value, NumberStyles parseNumberStyles, NumberFormatInfo numberFormatInfo, bool isNegative, bool reduceTerms,
        out Fraction fraction) {
        // 1. try to find the exponent character (extracting the left and right sides)
        parseNumberStyles &= ~NumberStyles.AllowExponent;
        var ranges = new Span<Range>(new Range[2]);
        var nbRangesFilled = value.SplitAny(ranges, ['e', 'E']);

        if (nbRangesFilled != 2) {
            // no exponent found
            return (parseNumberStyles & NumberStyles.AllowDecimalPoint) != 0
                ? TryParseDecimalNumber(value, parseNumberStyles, numberFormatInfo, isNegative, reduceTerms, out fraction)
                : TryParseInteger(value, parseNumberStyles, numberFormatInfo, isNegative, out fraction);
        }

        // 2. try to parse the exponent (w.r.t. the scientific notation format)
        var exponentSpan = value[ranges[1]];
        if (!int.TryParse(exponentSpan, NumberStyles.AllowLeadingSign | NumberStyles.Integer, numberFormatInfo, out var exponent)) {
            return CannotParse(out fraction);
        }

        // 3. try to parse the coefficient (w.r.t. the decimal separator allowance)
        var coefficientSpan = value[ranges[0]];
        if (coefficientSpan.IsEmpty) {
            return CannotParse(out fraction); // nothing in the coefficient
        }

        if (coefficientSpan.Length == 1 || (parseNumberStyles & NumberStyles.AllowDecimalPoint) == 0) {
            if (!TryParseInteger(coefficientSpan, parseNumberStyles, numberFormatInfo, isNegative, out fraction)) {
                return false;
            }
        } else {
            if (!TryParseDecimalNumber(coefficientSpan, parseNumberStyles, numberFormatInfo, isNegative, reduceTerms, out fraction)) {
                return false;
            }
        }

        // 4. multiply the coefficient by 10 to the power of the exponent
        fraction *= Pow(TEN, exponent);
        return true;
    }

    private static bool TryParseDecimalNumber(ReadOnlySpan<char> value, NumberStyles parseNumberStyles, NumberFormatInfo numberFormatInfo, bool isNegative, bool reduceTerms,
        out Fraction fraction) {
        // 1. try to find the decimal separator (extracting the left and right sides)
        var ranges = new Span<Range>(new Range[2]);
        var nbRangesFilled = value.Split(ranges, numberFormatInfo.NumberDecimalSeparator);

        if (nbRangesFilled != 2) {
            return TryParseInteger(value, parseNumberStyles, numberFormatInfo, isNegative, out fraction);
        }

        var integerSpan = value[ranges[0]];
        var fractionalSpan = value[ranges[1]];

        // 2. validate the format of the string after the radix
        if (integerSpan.IsEmpty && fractionalSpan.IsEmpty) {
            // after excluding the sign, the input was reduced to just the decimal separator (with nothing on either sides)
            return CannotParse(out fraction);
        }

        if (fractionalSpan.IsEmpty) {
            // after excluding the sign, the input was reduced to just an integer part (e.g. "1 234.")
            return TryParseInteger(integerSpan, parseNumberStyles, numberFormatInfo, isNegative, out fraction);
        }

        if ((parseNumberStyles & NumberStyles.AllowThousands) != 0) {
            if (fractionalSpan.Contains(numberFormatInfo.NumberGroupSeparator.AsSpan(), StringComparison.Ordinal)) {
                return CannotParse(out fraction); // number group separator detected in the fractional part (e.g. "1.2 34")
            }
        }

        // 3. extract the value of the string corresponding to the number without the decimal separator: "0.123 " should return "0123"
        var integerString = string.Concat(integerSpan, fractionalSpan);
        if (!BigInteger.TryParse(integerString, parseNumberStyles, numberFormatInfo, out var numerator)) {
            return CannotParse(out fraction);
        }

        if (numerator.IsZero && reduceTerms) {
            fraction = Zero;
            return true;
        }

        if (isNegative) {
            numerator = -numerator;
        }

        var nbDecimals = fractionalSpan.Length;
        if (nbDecimals == 0) {
            fraction = new Fraction(numerator);
            return true;
        }

        // 4. construct the fractional part using the corresponding decimal power for the denominator
        var denominator = BigInteger.Pow(TEN, nbDecimals);
        fraction = reduceTerms ? ReduceSigned(numerator, denominator) : new Fraction(true, numerator, denominator);
        return true;
    }

    private static bool TryParseInteger(ReadOnlySpan<char> value, NumberStyles parseNumberStyles, IFormatProvider formatProvider, bool isNegative, out Fraction fraction) {
        if (!BigInteger.TryParse(value, parseNumberStyles, formatProvider, out var bigInteger)) {
            return CannotParse(out fraction);
        }

        fraction = new Fraction(isNegative ? -bigInteger : bigInteger);
        return true;
    }

#else
    /// <summary>
    ///     Attempts to parse a string of characters into a fraction.
    /// </summary>
    /// <param name="value">
    ///     The input string to parse. The numerator and denominator must be separated by a '/' (slash) character.
    ///     For example, "3/4". If the string is not in this format, the parsing behavior is influenced by the
    ///     <paramref name="numberStyles" /> and <paramref name="formatProvider" /> parameters.
    /// </param>
    /// <param name="numberStyles">
    ///     A bitwise combination of number styles permitted in the input string. This is relevant when the string
    ///     is not in the numerator/denominator format. For instance, <see cref="NumberStyles.Float" /> allows decimal
    ///     points and scientific notation.
    /// </param>
    /// <param name="formatProvider">
    ///     An <see cref="IFormatProvider" /> that supplies culture-specific information used to parse the input string.
    ///     This is relevant when the string is not in the numerator/denominator format. For example,
    ///     <c>new CultureInfo("en-US")</c> for US English culture.
    /// </param>
    /// <param name="normalize">
    ///     A boolean value indicating whether the parsed fraction should be reduced to its simplest form.
    ///     For example, if true, "4/8" will be reduced to "1/2".
    /// </param>
    /// <param name="fraction">
    ///     When this method returns, contains the parsed fraction if the operation was successful; otherwise,
    ///     it contains an invalid fraction.
    /// </param>
    /// <returns>
    ///     <c>true</c> if the input string is well-formed and could be parsed into a fraction; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    ///     The <paramref name="numberStyles" /> parameter allows you to specify which number styles are allowed in the input
    ///     string.
    ///     For example, <see cref="NumberStyles.Float" /> allows decimal points and scientific notation.
    ///     The <paramref name="formatProvider" /> parameter provides culture-specific formatting information.
    ///     For example, you can use <c>new CultureInfo("en-US")</c> for US English culture.
    ///     Here are some examples of how to use the <see cref="TryParse(string,out Fraction)" /> method:
    ///     <example>
    ///         <code>
    /// Fraction.TryParse("3/4", NumberStyles.Any, new CultureInfo("en-US"), true, out Fraction fraction);
    /// </code>
    ///         This example parses the string "3/4" into a <see cref="Fraction" /> object with a numerator of 3 and a
    ///         denominator of 4.
    ///         <code>
    /// Fraction.TryParse("1.25", NumberStyles.Number, new CultureInfo("en-US"), true, out Fraction fraction);
    /// </code>
    ///         This example parses the string "1.25" into a <see cref="Fraction" /> object with a numerator of 5 and a
    ///         denominator of 4.
    ///         <code>
    /// Fraction.TryParse("1.23e-2", NumberStyles.Float, new CultureInfo("en-US"), true, out Fraction fraction);
    /// </code>
    ///         This example parses the string "1.23e-2" into a <see cref="Fraction" /> object with a numerator of 123 and a
    ///         denominator of 10000.
    ///     </example>
    /// </remarks>
    public static bool TryParse(string value, NumberStyles numberStyles, IFormatProvider formatProvider, bool normalize, out Fraction fraction) {
        if (string.IsNullOrWhiteSpace(value)) {
            return CannotParse(out fraction);
        }

        if (value.Length == 1) {
            if (!BigInteger.TryParse(value, numberStyles, formatProvider, out var singleDigit)) {
                return CannotParse(out fraction);
            }

            fraction = new Fraction(singleDigit);
            return true;
        }

        var components = value.Split('/');

        if (components.Length >= 2) {
            var numeratorString = components[0];
            var denominatorString = components[1];

            var withoutDecimalPoint = numberStyles & ~NumberStyles.AllowDecimalPoint;
            if (!BigInteger.TryParse(
                    numeratorString,
                    withoutDecimalPoint,
                    formatProvider,
                    out var numerator)
                || !BigInteger.TryParse(
                    denominatorString,
                    withoutDecimalPoint,
                    formatProvider,
                    out var denominator)) {
                return CannotParse(out fraction);
            }

            fraction = new Fraction(numerator, denominator, normalize);
            return true;
        }

        // parsing a number using to the selected NumberStyles: e.g. " $ 12345.1234321e-4- " should result in -1.23451234321 with NumberStyles.Any
        var numberFormatInfo = NumberFormatInfo.GetInstance(formatProvider);
        // check for any of the special symbols (these cannot be combined with anything else)
        if (value == numberFormatInfo.NaNSymbol) {
            fraction = NaN;
            return true;
        }

        if (value == numberFormatInfo.PositiveInfinitySymbol) {
            fraction = PositiveInfinity;
            return true;
        }

        if (value == numberFormatInfo.NegativeInfinitySymbol) {
            fraction = NegativeInfinity;
            return true;
        }

        var currencyAllowed = (numberStyles & NumberStyles.AllowCurrencySymbol) != 0;
        var currencyDetected = false; // there "special" rules regarding the white-spaces after a leading currency symbol is detected
        var currencySymbol = numberFormatInfo.CurrencySymbol;
        if (currencyAllowed && string.IsNullOrEmpty(currencySymbol)) {
            numberStyles &= ~NumberStyles.AllowCurrencySymbol; // no currency symbol available
            currencyAllowed = false;
        }

        var decimalSeparator = numberFormatInfo.NumberDecimalSeparator; // note: decimal.TryParse relies on the CurrencySymbol (when currency is detected)
        var decimalsAllowed = (numberStyles & NumberStyles.AllowDecimalPoint) != 0;
        if (decimalsAllowed && string.IsNullOrEmpty(decimalSeparator)) {
            numberStyles &= ~NumberStyles.AllowDecimalPoint; // no decimal separator available
            decimalsAllowed = false;
        }

        var startIndex = 0;
        var endIndex = value.Length;
        var isNegative = false;

        // examine the leading characters
        do {
            var character = value[startIndex];
            if (char.IsDigit(character)) {
                break;
            }

            if (char.IsWhiteSpace(character)) {
                if ((numberStyles & NumberStyles.AllowLeadingWhite) == 0) {
                    return CannotParse(out fraction);
                }

                startIndex++;
                continue;
            }

            if (character == '(') {
                if ((numberStyles & NumberStyles.AllowParentheses) == 0) {
                    return CannotParse(out fraction); // not allowed
                }

                if (startIndex == endIndex - 1) {
                    return CannotParse(out fraction); // not enough characters
                }

                startIndex++; // consume the current character
                isNegative = true; // the closing parenthesis will be validated in the backwards iteration

                if (currencyDetected) {
                    // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                    numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                    // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                    currencyDetected = true;
                    startIndex += currencySymbol.Length;
                    numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);
                } else {
                    // if next character is a white space the format should be rejected
                    numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    break;
                }

                continue;
            }

            if ((numberStyles & NumberStyles.AllowLeadingSign) != 0) {
                if (numberFormatInfo.NegativeSign is { Length: > 0 } negativeSign && startsWith(value, negativeSign, startIndex)) {
                    isNegative = true;
                    startIndex += negativeSign.Length;
                    if (currencyDetected) {
                        // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowHexSpecifier);
                    } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                        // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                        currencyDetected = true;
                        startIndex += currencySymbol.Length;
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol |
                                          NumberStyles.AllowHexSpecifier);
                    } else {
                        // if next character is a white space the format should be rejected
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowLeadingWhite |
                                          NumberStyles.AllowHexSpecifier);
                        break;
                    }

                    continue;
                }

                if (numberFormatInfo.PositiveSign is { Length: > 0 } positiveSign && startsWith(value, positiveSign, startIndex)) {
                    isNegative = false;
                    startIndex += positiveSign.Length;
                    if (currencyDetected) {
                        // any number of white-spaces are allowed following a leading currency symbol (but no other signs)
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowHexSpecifier);
                    } else if (currencyAllowed && startsWith(value, currencySymbol, startIndex)) {
                        // there can be no more currency symbols but there could be more white-spaces (we skip and continue) 
                        currencyDetected = true;
                        startIndex += currencySymbol.Length;
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol |
                                          NumberStyles.AllowHexSpecifier);
                    } else {
                        // if next character is a white space the format should be rejected
                        numberStyles &= ~(NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign | NumberStyles.AllowParentheses | NumberStyles.AllowLeadingWhite |
                                          NumberStyles.AllowHexSpecifier);
                        break;
                    }

                    continue;
                }
            }

            if (currencyAllowed && !currencyDetected && startsWith(value, currencySymbol, startIndex)) {
                // there can be no more currency symbols but there could be more white-spaces (we skip and continue)
                currencyDetected = true;
                numberStyles &= ~NumberStyles.AllowCurrencySymbol;
                startIndex += currencySymbol.Length;
                continue;
            }

            if (decimalsAllowed && startsWith(value, decimalSeparator, startIndex)) {
                break; // decimal string with no leading zeros
            }

            // this is either an expected hex string or an invalid format
            return (numberStyles & NumberStyles.AllowHexSpecifier) != 0
                ? TryParseInteger(value, numberStyles & ~NumberStyles.AllowTrailingSign, formatProvider, startIndex, endIndex, isNegative, out fraction)
                : CannotParse(out fraction); // unexpected character
        } while (startIndex < endIndex);

        if (startIndex >= endIndex) {
            return CannotParse(out fraction);
        }

        if (isNegative) {
            numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign);
        } else {
            numberStyles &= ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowParentheses);
        }

        // examine the trailing characters
        do {
            var character = value[endIndex - 1];
            if (char.IsDigit(character)) {
                break;
            }

            if (char.IsWhiteSpace(character)) {
                if ((numberStyles & NumberStyles.AllowTrailingWhite) == 0) {
                    return CannotParse(out fraction);
                }

                endIndex--;
                continue;
            }

            if (character == ')') {
                if ((numberStyles & NumberStyles.AllowParentheses) == 0) {
                    return CannotParse(out fraction); // not allowed
                }

                numberStyles &= ~(NumberStyles.AllowParentheses | NumberStyles.AllowCurrencySymbol);
                endIndex--;
                continue;
            }

            if ((numberStyles & NumberStyles.AllowTrailingSign) != 0) {
                if (numberFormatInfo.NegativeSign is { Length: > 0 } negativeSign && endsWith(value, negativeSign, endIndex)) {
                    isNegative = true;
                    numberStyles &= ~(NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    endIndex -= negativeSign.Length;
                    continue;
                }

                if (numberFormatInfo.PositiveSign is { Length: > 0 } positiveSign && endsWith(value, positiveSign, endIndex)) {
                    isNegative = false;
                    numberStyles &= ~(NumberStyles.AllowTrailingSign | NumberStyles.AllowHexSpecifier);
                    endIndex -= positiveSign.Length;
                    continue;
                }
            }

            if (currencyAllowed && !currencyDetected && endsWith(value, currencySymbol, endIndex)) {
                // there can be no more currency symbols but there could be more white-spaces (we skip and continue)
                currencyDetected = true;
                numberStyles &= ~NumberStyles.AllowCurrencySymbol;
                endIndex -= currencySymbol.Length;
                continue;
            }

            if (decimalsAllowed && endsWith(value, decimalSeparator, endIndex)) {
                break;
            }

            // this is either an expected hex string or an invalid format
            return (numberStyles & NumberStyles.AllowHexSpecifier) != 0
                ? TryParseInteger(value, numberStyles & ~NumberStyles.AllowTrailingSign, formatProvider, startIndex, endIndex, isNegative, out fraction)
                : CannotParse(out fraction); // unexpected character
        } while (startIndex < endIndex);

        if (startIndex >= endIndex) {
            return CannotParse(out fraction); // not enough characters
        }

        if (isNegative && (numberStyles & NumberStyles.AllowParentheses) != 0) {
            return CannotParse(out fraction); // failed to find a closing parentheses
        }

        numberStyles &= ~(NumberStyles.AllowTrailingWhite | NumberStyles.AllowTrailingSign | NumberStyles.AllowCurrencySymbol);
        // at this point value[startIndex, endIndex] should correspond to the number without the sign (or the format is invalid)

        if (startIndex == endIndex - 1) {
            // this can only be a single digit (integer)
            return TryParseInteger(value, numberStyles, formatProvider, startIndex, endIndex, isNegative, out fraction);
        }

        if ((numberStyles & NumberStyles.AllowExponent) != 0) {
            return TryParseWithExponent(value, numberStyles, numberFormatInfo, startIndex, endIndex, isNegative, normalize, out fraction);
        }

        return decimalsAllowed
            ? TryParseDecimalNumber(value, numberStyles, numberFormatInfo, startIndex, endIndex, isNegative, normalize, out fraction)
            : TryParseInteger(value, numberStyles, formatProvider, startIndex, endIndex, isNegative, out fraction);

        static bool startsWith(string fractionString, string testString, int startIndex) {
            if (fractionString.Length - startIndex < testString.Length) {
                return false;
            }

            for (var i = 0; i < testString.Length; i++) {
                if (fractionString[i + startIndex] != testString[i]) {
                    return false;
                }
            }

            return true;
        }

        static bool endsWith(string fractionString, string testString, int endIndex) {
            return startsWith(fractionString, testString, endIndex - testString.Length);
        }
    }

    private static bool TryParseWithExponent(string valueString, NumberStyles parseNumberStyles, NumberFormatInfo numberFormatInfo,
        int startIndex, int endIndex, bool isNegative, bool reduceTerms, out Fraction fraction) {
        // 1. try to find the exponent character index
        parseNumberStyles &= ~NumberStyles.AllowExponent;
        var exponentIndex = valueString.IndexOfAny(['e', 'E'], startIndex + 1, endIndex - startIndex - 1);
        if (exponentIndex == -1) {
            // no exponent found
            return (parseNumberStyles & NumberStyles.AllowDecimalPoint) != 0
                ? TryParseDecimalNumber(valueString, parseNumberStyles, numberFormatInfo, startIndex, endIndex, isNegative, reduceTerms, out fraction)
                : TryParseInteger(valueString, parseNumberStyles, numberFormatInfo, startIndex, endIndex, isNegative, out fraction);
        }

        if (startIndex == exponentIndex) {
            return CannotParse(out fraction); // no characters on the left of the exponent symbol
        }

        // 2. try to parse the exponent (w.r.t. the scientific notation format)
        var exponentString = valueString.Substring(exponentIndex + 1, endIndex - exponentIndex - 1);
        if (!int.TryParse(exponentString, NumberStyles.AllowLeadingSign | NumberStyles.Integer, numberFormatInfo, out var exponent)) {
            return CannotParse(out fraction);
        }

        // 3. try to parse the coefficient (w.r.t. the decimal separator allowance)
        if (startIndex == endIndex - 1 || (parseNumberStyles & NumberStyles.AllowDecimalPoint) == 0) {
            if (!TryParseInteger(valueString, parseNumberStyles, numberFormatInfo, startIndex, exponentIndex, isNegative, out fraction)) {
                return false;
            }
        } else {
            if (!TryParseDecimalNumber(valueString, parseNumberStyles, numberFormatInfo, startIndex, exponentIndex, isNegative, reduceTerms, out fraction)) {
                return false;
            }
        }

        // 4. multiply the coefficient by 10 to the power of the exponent
        fraction *= Pow(TEN, exponent);
        return true;
    }

    private static bool TryParseDecimalNumber(string valueString, NumberStyles parseNumberStyles, NumberFormatInfo numberFormatInfo,
        int startIndex, int endIndex, bool isNegative, bool reduceTerms, out Fraction fraction) {
        // 1. find the position of the decimal separator (if any)
        var decimalSeparatorIndex = valueString.IndexOf(numberFormatInfo.NumberDecimalSeparator, startIndex, endIndex - startIndex, StringComparison.Ordinal);
        if (decimalSeparatorIndex == -1) {
            return TryParseInteger(valueString, parseNumberStyles, numberFormatInfo, startIndex, endIndex, isNegative, out fraction);
        }

        // 2. validate the format of the string after the radix
        var decimalSeparatorLength = numberFormatInfo.NumberDecimalSeparator.Length;
        if (startIndex + decimalSeparatorLength == endIndex) {
            // after excluding the sign, the input was reduced to just the decimal separator (with nothing on either sides)
            return CannotParse(out fraction);
        }

        if ((parseNumberStyles & NumberStyles.AllowThousands) != 0) {
            if (valueString.IndexOf(numberFormatInfo.NumberGroupSeparator, decimalSeparatorIndex + decimalSeparatorLength, StringComparison.Ordinal) != -1) {
                return CannotParse(out fraction);
            }
        }

        // 3. extract the value of the string corresponding to the number without the decimal separator: " 0.123 " should return "0123"
        var integerString = string.Concat(
            valueString.Substring(startIndex, decimalSeparatorIndex - startIndex),
            valueString.Substring(decimalSeparatorIndex + decimalSeparatorLength, endIndex - decimalSeparatorIndex - decimalSeparatorLength));

        if (!BigInteger.TryParse(integerString, parseNumberStyles, numberFormatInfo, out var numerator)) {
            return CannotParse(out fraction);
        }

        if (numerator.IsZero && reduceTerms) {
            fraction = Zero;
            return true;
        }

        if (isNegative) {
            numerator = -numerator;
        }

        var nbDecimals = endIndex - decimalSeparatorIndex - decimalSeparatorLength;
        if (nbDecimals == 0) {
            fraction = new Fraction(numerator);
            return true;
        }

        // 4. construct the fractional part using the corresponding decimal power for the denominator
        var denominator = BigInteger.Pow(TEN, nbDecimals);
        fraction = reduceTerms ? ReduceSigned(numerator, denominator) : new Fraction(true, numerator, denominator);
        return true;
    }

    private static bool TryParseInteger(string valueString, NumberStyles parseNumberStyles, IFormatProvider formatProvider,
        int startIndex, int endIndex, bool isNegative, out Fraction fraction) {
        if (!BigInteger.TryParse(valueString.Substring(startIndex, endIndex - startIndex), parseNumberStyles, formatProvider, out var bigInteger)) {
            return CannotParse(out fraction);
        }

        fraction = new Fraction(isNegative ? -bigInteger : bigInteger);
        return true;
    }

#endif

    /// <summary>
    ///     Sets the out parameter to the default value of Fraction and returns false.
    ///     This method is used when the parsing of a string to a Fraction fails.
    /// </summary>
    /// <param name="fraction">The Fraction object that will be set to its default value.</param>
    /// <returns>Always returns false.</returns>
    private static bool CannotParse(out Fraction fraction) {
        fraction = default;
        return false;
    }

   

    /// <inheritdoc cref="FromDecimal(decimal, bool)"/>>
    public static Fraction FromDecimal(decimal value) {
        return FromDecimal(value, reduceTerms: true);
    }

    /// <summary>
    ///     Converts a decimal value in a fraction. The value will not be rounded.
    /// </summary>
    /// <param name="value">A decimal value.</param>
    /// <param name="reduceTerms">
    ///     Indicates whether the terms of the fraction should be reduced by their greatest common
    ///     denominator.
    /// </param>
    /// <returns>A fraction.</returns>
    public static Fraction FromDecimal(decimal value, bool reduceTerms) {
        if (reduceTerms) {
            switch (value) {
                case decimal.Zero:
                    return Zero;
                case decimal.One:
                    return One;
                case decimal.MinusOne:
                    return MinusOne;
            }
        }
#if NET
        Span<int> bits = stackalloc int[4];
        decimal.GetBits(value, bits);
        Span<byte> buffer = stackalloc byte[16];
        // Assume BitConverter.IsLittleEndian = true
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(0, 4), bits[0]);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(4, 4), bits[1]);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(8, 4), bits[2]);
        BinaryPrimitives.WriteInt32LittleEndian(buffer.Slice(12, 4), bits[3]);
        var exp = buffer[14];
        var positiveSign = (buffer[15] & 0x80) == 0;
        // Pass false to the isBigEndian parameter
        var numerator = new BigInteger(buffer.Slice(0, 13), false, false);
#else
        var bits = decimal.GetBits(value);
        var low = BitConverter.GetBytes(bits[0]);
        var middle = BitConverter.GetBytes(bits[1]);
        var high = BitConverter.GetBytes(bits[2]);
        var scale = BitConverter.GetBytes(bits[3]);

        var exp = scale[2];
        var positiveSign = (scale[3] & 0x80) == 0;

        // value = 0x00,high,middle,low / 10^exp
        var numerator = new BigInteger([
            low[0], low[1], low[2], low[3],
            middle[0], middle[1], middle[2], middle[3],
            high[0], high[1], high[2], high[3],
            0x00
        ]);
#endif

        if (!positiveSign) {
            numerator = -numerator;
        }

        var denominator = BigInteger.Pow(TEN, exp);

        return reduceTerms ? ReduceSigned(numerator, denominator) : new Fraction(true, numerator, denominator);
    }
}
