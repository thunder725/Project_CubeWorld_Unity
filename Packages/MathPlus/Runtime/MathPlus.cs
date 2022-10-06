using System.Collections.Generic;
using UnityEngine;

    // Homemade Mathf extention for other mathematical equations and constants
    public class MathPlus : MonoBehaviour
    {
        /// <summary> The well-known 1.618033988... value  </summary>
        public const float GoldenRatio = 1.61803399F; 

        /// <summary> Returns a three-points Bézier Curve using a, b and c by the percentage t. </summary>
        /// <param name ="a"> Starting value of the Bézier Curve </param>
        /// <param name ="b"> Middle value of the Bézier Curve </param>
        /// <param name ="c"> Ending value of the Bézier Curve </param>
        /// <param name ="t"> Value increasing from 0 to 1. When t = 0, returns a. When t = 1, returns c. </param>
        public static float BézierInterpolationThree(float a, float b, float c, float t)
        {
            // B(t) = (1-t)²P0 + 2(1-t)tP1 + t²P2 , 0 < t < 1 
            // from https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
            // t is the time ratio, capped between 0 and 1
            t = Mathf.Clamp(t, 0, 1);
            float value = MathPlus.FastSquare(1-t)*a + 2*(1-t)*t*b + MathPlus.FastSquare(t)*c;
            return value;
        }

        /// <summary> Returns a four-points Bézier Curve using a, b, c and d by the percentage t. </summary>
        /// <param name ="a"> Starting value of the Bézier Curve </param>
        /// <param name ="b"> First middle value of the Bézier Curve </param>
        /// <param name ="c"> Second middle value of the Bézier Curve </param>
        /// <param name ="d"> Ending value of the Bézier Curve </param>
        /// <param name ="t"> Value increasing from 0 to 1. When t = 0, returns a. When t = 1, returns d. </param>
        public static float BézierInterpolationFour(float a, float b, float c, float d, float t)
        {
            // B(t) = (1-t)³P0 + 3(1-t)²tP1 + 3(1-t)t²P2 + t³P3 , 0 < t < 1
            // from https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
            // t is the time ratio, capped between 0 and 1
            t = Mathf.Clamp(t, 0, 1);
            float value = MathPlus.FastCube(1-t)*a + 3*MathPlus.FastSquare(1-t)*t*b + 1*(1-t)*MathPlus.FastSquare(t)*c + FastCube(t)*d;
            return value;
        }

        /// <summary> Returns the sum of every integer from 1 to value (value strictly superior to 0) </summary>
        /// <param name ="value"> Strictly positive value to return the factorial of. </param>
        public static int Factorial(int value)
        {
            if (value <= 1) {return 1;}
            else { return value * Factorial(value - 1);}
        }

        /// <summary> Returns the value squared. </summary>
        public static float FastSquare(float value)
        { return value * value; }

        /// <summary> Returns the value to the power of three. </summary>
        public static float FastCube(float value)
        { return value * value * value; }

        /// <summary> Returns the Inverse Square Root of the value. Code taken from Quake III. Precise for the first 2 digits only. </summary>
        /// <param name ="value"> Must be between 0 and 1 only (inclusive range). </param>
        // Code from https://codegolf.stackexchange.com/questions/9027/fast-inverse-square-root
        unsafe public static float FastInverseSQRT(float value)
        {
            if (value > 1)
            {
                Debug.LogError("Trying to input a value over 1.");
                return 1;
            }
            if (value == 0)
            {return 0;}
            
            
            var n = value;
            var i = *(int*) &n / -2 + 0x5f3759df;
            var y = *(float*) &i;
            return (y * 1.5f - y * n / 2f * y * y);
        }

        /// <summary> Returns angle in degrees usable for a 2D rotation from a Vector to the target. </summary>
        /// <param name ="value"> Vector to get the angle from. </param>
        public static float AngleDegreesFromVector(UnityEngine.Vector3 value)
        {
            value = value.normalized;

            float _returned = Mathf.Rad2Deg * Mathf.Acos(value.x);
            if (value.y < 0)
            {_returned = -_returned;}
            return _returned;
        }

        
        /// <summary> Returns the average of the array. </summary>
        /// <param name ="numbersArray"> Array of integers to return the average of. </param>
        public static float Average(int[] numbersArray)
        {
            float result = 0;
            foreach (int i in numbersArray)
            {
                result += i;
            }
            result /= numbersArray.Length;
            return result;
        }

        /// <summary> Returns the average of the array. </summary>
        /// <param name ="numbersArray"> Array of floats to return the average of. </param>
        public static float Average(float[] numbersArray)
        {
            float result = 0;
            foreach (int i in numbersArray)
            {
                result += i;
            }
            result /= numbersArray.Length;
            return result;
        }

        /// <summary> Returns the average of the array. </summary>
        /// <param name ="numbersList"> List of integers to return the average of. </param>
        public static float Average(List<int> numbersList)
        {
            float result = 0;
            foreach (int i in numbersList)
            {
                result += i;
            }
            result /= numbersList.Count;
            return result;
        }

        /// <summary> Returns the average of the array. </summary>
        /// <param name ="numbersList"> List of floats to return the average of. </param>
        public static float Average(List<float> numbersList)
        {
            float result = 0;
            foreach (int i in numbersList)
            {
                result += i;
            }
            result /= numbersList.Count;
            return result;
        }

        /// <summary>
        /// Returns either -1 or +1 as a float
        /// </summary>
        /// <returns>-1 or +1 as float. Equal chance of each</returns>
        public static float MinusOrPlusOne()
        {
            float result = 0;
            result = Random.Range(0, 2) == 0 ? -1f : 1f;
            return result;
        }

        /// <summary>
        /// Converts a Celcius temperature into a Fahrenheit temperature
        /// </summary>
        /// <param name="celciusValue">The Temperature (in Celcius) to be converted.</param>
        /// <returns> The entered value, converted in Fahrenheit </returns>
        public static float CelciusToFahrenheit(float celciusValue)
        {
            return ((celciusValue * 1.8f) + 32);
        }

        /// <summary>
        /// Converts a Fahrenheit temperature into a Celcius temperature
        /// </summary>
        /// <param name="celciusValue">The Temperature (in Fahrenheit) to be converted.</param>
        /// <returns> The entered value, converted in Celcius </returns>
        public static float FahrenheitToCelcius(float fahrenheitValue)
        {
            return ((fahrenheitValue - 32) / 1.8f);
        }

        /// <summary>
        /// Returns the Vector with every value being its absolute value.
        /// </summary>
        /// <param name="_vector">The Vector to return</param>
        /// <returns>The given Vector with every parameter absolute.</returns>
        public static Vector3 AbsVector(Vector3 _vector)
        {
            _vector[0] = Mathf.Abs(_vector.x);
            _vector[1] = Mathf.Abs(_vector.y);
            _vector[2] = Mathf.Abs(_vector.z);
            return _vector;
        }

        /// <summary>
        /// Returns the Vector with every value being its absolute value.
        /// </summary>
        /// <param name="_vector">The Vector to return</param>
        /// <returns>The given Vector with every parameter absolute.</returns>
        public static Vector2 AbsVector(Vector2 _vector)
        {
            _vector[0] = Mathf.Abs(_vector.x);
            _vector[1] = Mathf.Abs(_vector.y);
            return _vector;
        }
    }
