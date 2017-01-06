using UnityEngine;
using UnityEditor;

namespace SocketIt
{
    public class SocketItGizmo
    {
        private static Color ConnectionColor = Color.green;
        private static Color NodeColor = Color.red;

        public static void DrawConnection(Connection connection)
        {
            Color color = ConnectionColor;
            if(connection.SocketA.Module.GetComponent<NodeModule>() != null && connection.SocketB.Module.GetComponent<NodeModule>() != null)
            {
                color = NodeColor;
            }

            Handles.color = color;

            if (connection.SocketA != null && connection.SocketB != null)
            {
                Vector3 startPoint = connection.SocketA.transform.position;
                Vector3 endPoint = connection.SocketB.transform.position;

                float handleLength = Vector3.Distance(startPoint, endPoint) / 2;

                Vector3 startTangent = connection.SocketA.transform.position + connection.SocketA.transform.forward * handleLength;
                Vector3 endTangent = connection.SocketB.transform.position + connection.SocketB.transform.forward * handleLength;

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, color, null, 5);

                Bezier bezier = new Bezier(startPoint, endPoint, startTangent, endTangent);


                DrawArrow(bezier.GetPointAtTime(0.5f), bezier.GetPointAtTime(0.55f));
            }

            Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .1f);
            Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .08f);
            

            if(connection.Initiator == connection.SocketA)
            {
                Handles.DrawSolidDisc(connection.SocketB.transform.position, Vector3.forward, .05f);
            }
            else
            {
                Handles.DrawSolidDisc(connection.SocketA.transform.position, Vector3.forward, .05f);
            }


        }

        public static void DrawSocketAngle(SnapSocket socket, float angle)
        {
            if(angle == 180)
            {
                return;
            }
            Color color = Color.green;
            color.a = 0.05f;
            Handles.color = color;

            Handles.DrawSolidArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, angle, 0.5f);
            Handles.DrawSolidArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, -angle, 0.5f);
            color.a = 1f;
            Handles.color = color;

            Handles.DrawWireArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, angle, 0.5f);
            Handles.DrawWireArc(socket.transform.position, Camera.current.transform.forward, socket.transform.forward, -angle, 0.5f);
        }

        private static void DrawArrow(Vector3 position, Vector3 target)
        {
            Handles.color = Color.green;
            Handles.DrawSolidDisc(position, Vector3.forward, .05f * HandleUtility.GetHandleSize(position));
            Handles.DrawLine(position, target);
        }
    }
}

