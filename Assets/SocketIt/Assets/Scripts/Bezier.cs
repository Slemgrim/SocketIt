using UnityEngine;

namespace SocketIt
{
    public class Bezier
    {
        //vars to store our control points
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Vector3 startTangent;
        public Vector3 endTangent;

        // Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point
        public Bezier(Vector3 startPoint, Vector3 endPoint, Vector3 startTangent, Vector3 endTangent)
        {
            this.startPoint = startPoint;
            this.startTangent = startTangent;
            this.endTangent = endTangent;
            this.endPoint = endPoint;
        }

        // 0.0 >= t <= 1.0 In here be dragons and magic
        public Vector3 GetPointAtTime(float t)
        {
            float u = 1f - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector3 p = uuu * startPoint; //first term
            p += 3 * uu * t * startTangent; //second term
            p += 3 * u * tt * endTangent; //third term
            p += ttt * endPoint; //fourth term

            return p;

        }
    }
}
