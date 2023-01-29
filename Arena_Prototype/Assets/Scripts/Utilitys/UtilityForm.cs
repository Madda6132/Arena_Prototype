using UnityEngine;
using System;

namespace RPG {

    public static class UtilityForm
    {

        private const float TAU = 6.283185f;

        /*---Public---*/

        /*---Arc---*/
        /// <summary>
        /// Split points equally in a arc around the up vector, and towards the arcDirection. 
        /// Then return the points as rays.
        /// </summary>
        /// <param name="startPoint"> Were the arc point starts. </param>
        /// <param name="arcDirection"> The direction the arc faces </param>
        /// <param name="upDirection"> Determines  what axle the points will surround. </param>
        /// <param name="arcSize"> How wide the arc is </param>
        /// <param name="pointsAmount"> How many rays to return. </param>
        /// <returns></returns>
        public static Ray[] GetArcPointDirections(Vector3 startPoint, Vector3 arcDirection, Vector3 upDirection,
        float arcSize, int pointsAmount) {

            Matrix4x4 _Matrix = CreateMatrix(startPoint, arcDirection, upDirection);

            float anglePerPoint = 1 < pointsAmount ? ((float)arcSize / ((float)pointsAmount - 1)) : (float)arcSize / (float)pointsAmount;
            float dotProduct = GetDotProduct(_Matrix, arcDirection);

            float degOffSet = dotProduct - (arcSize / 2);

            Ray[] directions = new Ray[pointsAmount];
            
            if (1 < pointsAmount) {

                for (int i = 0; i < pointsAmount; i++) {

                    float angle = ((anglePerPoint * i) + degOffSet);

                    var x = Mathf.Cos(angle * Mathf.Deg2Rad);
                    var z = Mathf.Sin(angle * Mathf.Deg2Rad);

                    Vector3 newPoint = (new Vector3(x, 0, z));

                    directions[i] = new(_Matrix.GetPosition(), TransformPosition(_Matrix, newPoint));

                }

            } else {


                Vector3 newPoint = arcDirection;

                directions[0] = new(_Matrix.GetPosition(), newPoint);
            }

            return directions;
        }

        /*---Circle---*/
        /// <summary>
        /// Split points equally in a circle around the up vector and return them as rays.
        /// </summary>
        /// <param name="startPoint"> Were the circle center is. </param>
        /// <param name="direction"> Were the first point starts and the forward direction for points directions. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="pointsAmount"> How many rays to return. </param>
        /// <returns></returns>
        public static Ray[] GetCircleDirections(Vector3 startPoint, Vector3 direction, Vector3 upDirection,
            int pointsAmount) {

            Matrix4x4 _Matrix = CreateMatrix(startPoint, direction, upDirection);

            //float angOffset = (pointsAmount - 1) * 0.5f;
            float dotProduct = GetDotProduct(_Matrix, direction);
            float offSetAngle = dotProduct * Mathf.Deg2Rad;

            Ray[] directions = new Ray[pointsAmount];

            for (int i = 0; i < pointsAmount; i++) {

                float t = i / (float)pointsAmount;
                float angred = t * TAU;

                float x = Mathf.Cos(angred + offSetAngle);
                float z = Mathf.Sin(angred + offSetAngle);

                Vector3 point = new(x, 0, z);
                directions[i] = new(_Matrix.GetPosition(), TransformPosition(_Matrix, point));
             
            }

            return directions;
        }

        /// <summary>
        /// Split points equally in a circle around the up vector and return them as rays.
        /// </summary>
        /// <param name="startPoint"> Were the circle center is. </param>
        /// <param name="direction"> Were the first point starts and the forward direction for points directions. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="pointsAmount"> How many rays to return. </param>
        /// <returns></returns>
        public static Ray[] GetCircleDirections(Transform transform,
            int pointsAmount) => GetCircleDirections(transform.position, transform.forward, transform.up, pointsAmount);

        /// <summary>
        /// Get random points in a circle around the up vector and return its positions.
        /// </summary>
        /// <param name="startPoint"> Were the circle center is. </param>
        /// <param name="direction"> The forward direction for a Matrix4x4. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="radius"> Determines the forward direction and what axle the points will surround. </param>
        /// <param name="pointsAmount"> How many points to return. </param>
        /// <returns></returns>
        public static Vector3[] GetCircleRandomPoints(Vector3 startPoint, Vector3 direction, Vector3 upDirection,
            float radius, int pointsAmount) {

            Matrix4x4 _Matrix = CreateMatrix(startPoint, direction, upDirection);
            Vector3[] locations = new Vector3[pointsAmount];

            for (int i = 0; i < locations.Length; i++) {

                Vector2 randomPosition = (Vector3)UnityEngine.Random.insideUnitCircle;
                Vector3 newPosition = new(randomPosition.x, 0, randomPosition.y);

                locations[i] = _Matrix.MultiplyPoint(newPosition * radius);
            }
            
            return locations;
        }

        /// <summary>
        /// Get random points in a circle around the up vector and return its positions.
        /// </summary>
        /// <param name="startPoint"> Were the circle center is. </param>
        /// <param name="direction"> The forward direction for a Matrix4x4. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="radius"> Determines the forward direction and what axle the points will surround. </param>
        /// <param name="pointsAmount"> How many points to return. </param>
        /// <returns></returns>
        public static Vector3[] GetCircleRandomPoints(Transform transform, float radius, int pointsAmount) =>
            GetCircleRandomPoints(transform.position, transform.forward, transform.up, radius, pointsAmount);

        /*---Square---*/
        /// <summary>
        /// Get random points in a square, from the center of the width of shape and towards the forward direction
        /// and return its positions.
        /// </summary>
        /// <param name="startPoint"> Were the square's width center starts. </param>
        /// <param name="direction"> The forward direction for points directions. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="shapeDimensions"> Determines the square dimensions X = Width, Y = Length </param>
        /// <param name="pointsAmount"> How many points to return. </param>
        /// <returns></returns>
        public static Vector3[] GetSquareRandomPointsTowardsDirection(Vector3 startPoint, Vector3 direction,
            Vector3 upDirection, Vector2 shapeDimensions, int pointsAmount) {

            return GetBoxRandomPointsTowardsDirection(startPoint, direction, upDirection, 
            new(shapeDimensions.x, 0f, shapeDimensions.y), pointsAmount);
        }

        public static Vector3[] GetSquarePointsTowardsDirection(Vector3 startPoint, Vector3 direction,
                Vector3 upDirection, Vector2 shapeDimensions, int pointsAmount) {

            //Divide equally
            //Divide by int x + 1
            //Divide by int y + 1
            //How do I divide pointsAmount equally or close
            //Square root pointAmount for square

            return new Vector3[pointsAmount];
            
        }

        /*---Box---*/
        /// <summary>
        /// Get random points in a square, from the center of the width of shape and towards the forward direction
        /// and return its positions.
        /// </summary>
        /// <param name="startPoint"> Were the square's width center starts. </param>
        /// <param name="direction"> The forward direction for points directions. </param>
        /// <param name="upDirection"> Determines what axle the points will surround. </param>
        /// <param name="shapeDimensions"> Determines the Box dimensions X = Width, Y = Height Z = Length </param>
        /// <param name="pointsAmount"> How many points to return. </param>
        /// <returns></returns>
        public static Vector3[] GetBoxRandomPointsTowardsDirection(Vector3 startPoint, Vector3 direction,
                Vector3 upDirection, Vector3 shapeDimensions, int pointsAmount) {

            //Offset the startPosition to center 
                Vector3 _XAxle = Vector3.Cross(upDirection, direction).normalized;
                Vector3 offSet = _XAxle * 0.5f * shapeDimensions.x;

                //Matrix used to position points
                Matrix4x4 _Matrix = CreateMatrix(startPoint - offSet, direction, upDirection);

                Vector3[] randomPositions = GetRandomPoints(pointsAmount);

                Vector3[] newLocations = new Vector3[pointsAmount];

                for (int i = 0; i < newLocations.Length; i++) {

                    Vector3 newPosition = Vector3.Scale(randomPositions[i], shapeDimensions);

                    newLocations[i] = _Matrix.MultiplyPoint(newPosition);
                }

                return newLocations;
            }

        /// <summary>
        /// Each point's value is between 0.00 - 1.00
        /// </summary>
        /// <param name="pointsAmount"> Get the amounts of points </param>
        /// <returns></returns>
        private static Vector3[] GetRandomPoints(int pointsAmount) {

            Vector3[] locations = new Vector3[pointsAmount];

            Func<float> RandomNum = () => {
                return UnityEngine.Random.Range(0f, 101f) * 0.01f;
            };

            for (int i = 0; i < locations.Length; i++) {

                float x = RandomNum();
                float y = RandomNum();
                float z = RandomNum();

                locations[i] = new Vector3(x, y, z);
            }

            return locations;
        }

        //Divide equally
        //Layer by layer: 
        // *-*-*    -*-*-    *-*-*
        // *-*-* -> -*-*- -> *-*-*
        // *-*-*    -*-*-    *-*-*

        /*---Other---*/
        public static Matrix4x4 CreateMatrix(Vector3 startPoint, Vector3 direction, Vector3 upDirection) {


            //Transform should change to direction as forward and another for Up vector

            Vector3 _XAxle = Vector3.Cross(upDirection, direction).normalized;
            Vector3 _YAxle = Vector3.Cross(direction, _XAxle).normalized;
            Vector3 _ZAxle = Vector3.Cross(_XAxle, _YAxle).normalized;

            Vector4 _X = new(_XAxle.x, _XAxle.y, _XAxle.z);
            Vector4 _Y = new(_YAxle.x, _YAxle.y, _YAxle.z);
            Vector4 _Z = new(_ZAxle.x, _ZAxle.y, _ZAxle.z);
            Vector4 Pos = new(startPoint.x, startPoint.y, startPoint.z, 1);

            return new(_X, _Y, _Z, Pos);
        }

        /*---Private---*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        private static Vector3 TransformPosition(Matrix4x4 matrix, Vector3 dir) => matrix.MultiplyVector(dir);
        
        
        /// <summary>
        /// Dot product has a failsafe for NaN value.
        /// Returns the Dot product.
        /// Usually used for offset for circle.
        /// </summary> 
        private static float GetDotProduct(Matrix4x4 _Matrix, Vector3 direction) {
            float dotProduct = Mathf.Acos(Vector3.Dot(_Matrix.MultiplyVector(Vector3.right), direction)) * Mathf.Rad2Deg;
            //Dot product cant handle 90 degrees 
            if (float.IsNaN(dotProduct)) {
                dotProduct = Mathf.Acos(Vector3.Dot(Vector3.right, (direction + new Vector3(0.001f, 0, 0)).normalized)) * Mathf.Rad2Deg;
            }
            return dotProduct;
        }
    }
}