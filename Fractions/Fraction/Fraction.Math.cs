﻿using System;
using System.Diagnostics.Contracts;
using System.Numerics;

namespace Fractions {
    public partial struct Fraction {
        /// <summary>
        /// Calculates the remainder of the division with the fraction's value and the supplied <paramref name="divisor"/> (% operator).
        /// </summary>
        /// <param name="divisor">Divisor</param>
        /// <returns>The remainder (left over)</returns>
        public Fraction Remainder(Fraction divisor) {
            if (divisor.IsZero) {
                throw new DivideByZeroException();
            }
            if (IsZero) {
                return _zero;
            }

            var gcd = BigInteger.GreatestCommonDivisor(_denominator, divisor.Denominator);

            var this_multiplier = BigInteger.Divide(_denominator, gcd);
            var other_multiplier = BigInteger.Divide(divisor.Denominator, gcd);

            var least_common_multiple = BigInteger.Multiply(this_multiplier, divisor.Denominator);

            var a = BigInteger.Multiply(_numerator, other_multiplier);
            var b = BigInteger.Multiply(divisor.Numerator, this_multiplier);

            var remainder = BigInteger.Remainder(a, b);

            return new Fraction(remainder, least_common_multiple);
        }

        /// <summary>
        /// Adds the fraction's value with <paramref name="summand"/>.
        /// </summary>
        /// <param name="summand">Summand</param>
        /// <returns>The result as summation.</returns>
        [Pure]
        public Fraction Add(Fraction summand) {
            if (_denominator == summand.Denominator) {
                return new Fraction(BigInteger.Add(_numerator, summand.Numerator), _denominator, true);
            }

            if (IsZero) {
                // 0 + b = b
                return summand;
            }

            if (summand.IsZero) {
                // a + 0 = a
                return this;
            }

            var gcd = BigInteger.GreatestCommonDivisor(_denominator, summand.Denominator);

            var this_multiplier = BigInteger.Divide(_denominator, gcd);
            var other_multiplier = BigInteger.Divide(summand.Denominator, gcd);

            var least_common_multiple = BigInteger.Multiply(this_multiplier, summand.Denominator);

            var calculated_numerator = BigInteger.Add(
                BigInteger.Multiply(_numerator, other_multiplier),
                BigInteger.Multiply(summand.Numerator, this_multiplier)
                );

            return new Fraction(calculated_numerator, least_common_multiple, true);
        }

        /// <summary>
        /// Subtracts the fraction's value (minuend) with <paramref name="subtrahend"/>.
        /// </summary>
        /// <param name="subtrahend">Subtrahend.</param>
        /// <returns>The result as difference.</returns>
        [Pure]
        public Fraction Subtract(Fraction subtrahend) {
            return Add(subtrahend.Invert());
        }

        /// <summary>
        /// Inverts the fraction. Has the same result as multiplying it by -1.
        /// </summary>
        /// <returns>The inverted fraction.</returns>
        [Pure]
        public Fraction Invert() {
            if (IsZero) {
                return _zero;
            }
            return new Fraction(BigInteger.Negate(_numerator), _denominator, _state);
        }

        /// <summary>
        /// Multiply the fraction's value by <paramref name="factor"/>.
        /// </summary>
        /// <param name="factor">Factor</param>
        /// <returns>The result as product.</returns>
        [Pure]
        public Fraction Multiply(Fraction factor) {
            return new Fraction(
                (_numerator * factor._numerator),
                (_denominator * factor._denominator),
                true);
        }

        /// <summary>
        /// Divides the fraction's value by <paramref name="divisor"/>.
        /// </summary>
        /// <param name="divisor">Divisor</param>
        /// <returns>The result as quotient.</returns>
        [Pure]
        public Fraction Divide(Fraction divisor) {
            if (divisor.IsZero) {
                throw new DivideByZeroException(string.Format(Exceptions.DivideByZero, this));
            }

            return new Fraction(
                (_numerator * divisor._denominator),
                (_denominator * divisor._numerator),
                true);
        }

        /// <summary>
        /// Returns this as reduced/simplified fraction. The fraction's sign will be normalized.
        /// </summary>
        /// <returns>A reduced and normalized fraction.</returns>
        [Pure]
        public Fraction Reduce() {
            return (_state == FractionState.IsNormalized)
                ? this
                : GetReducedFraction(_numerator, _denominator);
        }

        /// <summary>
        /// Gets the absolute value of a <see cref="Fraction"/> object.
        /// </summary>
        /// <returns>The absolute value.</returns>
        [Pure]
        public Fraction Abs() {
            return Abs(this);
        }

        /// <summary>
        /// Gets the absolute value of a <see cref="Fraction"/> object.
        /// </summary>
        /// <param name="fraction">The fraction.</param>
        /// <returns>The absolute value.</returns>
        [Pure]
        public static Fraction Abs(Fraction fraction) {
            return new Fraction(BigInteger.Abs(fraction.Numerator), BigInteger.Abs(fraction.Denominator), fraction.State);
        }

        /// <summary>
        /// Returns a reduced and normalized fraction.
        /// </summary>
        /// <param name="numerator">Numerator</param>
        /// <param name="denominator">Denominator</param>
        /// <returns>A reduced and normalized fraction</returns>
        [Pure]
        public static Fraction GetReducedFraction(BigInteger numerator, BigInteger denominator) {
            if (numerator.IsZero || denominator.IsZero) {
                return Zero;
            }

            if (denominator.Sign == -1) {
                // Denominator must not be negative after normalization
                numerator = BigInteger.Negate(numerator);
                denominator = BigInteger.Negate(denominator);
            }

            var gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            if (!gcd.IsOne && !gcd.IsZero) {
                return new Fraction(BigInteger.Divide(numerator, gcd), BigInteger.Divide(denominator, gcd),
                    FractionState.IsNormalized);
            }

            return new Fraction(numerator, denominator, FractionState.IsNormalized);
        }

        /// <summary>
        /// Returns a fraction raised to the specified power.
        /// </summary>
        /// <param name="base">base to be raised to a power</param>
        /// <param name="exponent">A number that specifies a power (exponent)</param>
        /// <returns>The fraction <paramref name="base"/> raised to the power <paramref name="exponent"/>.</returns>
        [Pure]
        public static Fraction Pow(Fraction @base, int exponent) {
            return (exponent < 0)
                ? Pow(new Fraction(@base._denominator, @base._numerator), -exponent)
                : new Fraction(BigInteger.Pow(@base._numerator, exponent), BigInteger.Pow(@base._denominator, exponent));
        }
    }
}