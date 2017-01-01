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
                Handles.DrawLine(connection.SocketA.transform.position, connection.SocketB.transform.position);
            }

            if (connection.Initiator != null)
            {
                Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .1f);
                Handles.DrawWireDisc(connection.Initiator.transform.position, Vector3.forward, .08f);
            }
        }
    }
}

