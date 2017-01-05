using UnityEngine;
using UnityEditor;

namespace SocketIt
{
    public class SocketItGizmo
    {
        private static Color SocketItGizmoColor = Color.green;

        public static void DrawConnection(Connection connection)
        {
            Handles.color = SocketItGizmoColor;

            if (connection.SocketA != null && connection.SocketB != null)
            {
                Vector3 startPoint = connection.SocketA.transform.position;
                Vector3 endPoint = connection.SocketB.transform.position;

                float handleLength = Vector3.Distance(startPoint, endPoint) / 2;

                Vector3 startTangent = connection.SocketA.transform.position + connection.SocketA.transform.forward * handleLength;
                Vector3 endTangent = connection.SocketB.transform.position + connection.SocketB.transform.forward * handleLength;

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, SocketItGizmoColor, null, 5);
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
    }
}

