using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SocketIt
{
    /// <summary>
    /// Handles all editor gizmos for SocketIt
    /// </summary>
    public class SocketItGizmo
    {
        /// <summary>
        /// Color for gizmos representing connections between two sockets
        /// </summary>
        private static Color ConnectionColor = Color.green;

        /// <summary>
        /// Color for gizmos representing connections between two modules
        /// </summary>
        private static Color CompositionColor = Color.red;

        /// <summary>
        /// Draws gizmos for a connection between two sockets
        /// </summary>
        /// <param name="connection">Draw gizmo for this connection</param>
        public static void DrawConnection(Connection connection)
        {
            Color color = ConnectionColor;

            Handles.color = color;

            if (connection.Connector != null && connection.Connectee != null)
            {
                Vector3 startPoint = connection.Connectee.transform.position;
                Vector3 endPoint = connection.Connector.transform.position;

                float distance = Vector3.Distance(startPoint, endPoint);

                if (distance > 0.1) {
                    Handles.DrawLine(startPoint, endPoint);                   
                }
            }

            Handles.DrawWireDisc(connection.Connector.transform.position, Camera.current.transform.forward, .1f);
            Handles.DrawSolidDisc(connection.Connector.transform.position, Camera.current.transform.forward, .06f);

            Handles.DrawWireDisc(connection.Connectee.transform.position, Camera.current.transform.forward, .1f);
        }

        /// <summary>
        /// Draw a gizmo representing the snapping angle of a socket. Sockets with an angle of 360 won't draw a gizmo
        /// </summary>
        /// <param name="socket">target socket</param>
        /// <param name="angle">angle of the socket</param>
        public static void DrawSocketAngle(Socket socket, float angle)
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


        /// <summary>
        /// Draws gizmos for all connections and modules inside of a composition
        /// </summary>
        /// <param name="composition"></param>
        public static void DrawComposition(Composition composition)
        {
            if (composition.Origin == null) {
                return;
            }

            Vector3 labelPosition = composition.Origin.transform.position + Vector3.up * 2 + Vector3.right * 2;

            Handles.Label(labelPosition, composition.name);

            Vector3 startPoint = composition.Origin.transform.position;
            Handles.color = CompositionColor;
            Handles.DrawLine(labelPosition, startPoint);

            foreach (Module module in composition.Modules)
            {
                Handles.DrawWireDisc(module.transform.position, Camera.current.transform.forward, .1f);
                Handles.DrawSolidDisc(module.transform.position, Camera.current.transform.forward, .06f);

                List<Connection> connections = composition.GetConnections(module);
                Handles.color = CompositionColor;
                foreach (Connection connection in connections)
                {
                    Handles.DrawLine(module.transform.position, connection.Connector.Module == module ? connection.Connector.transform.position : connection.Connectee.transform.position);
                }
            }

            foreach(Connection connection in composition.Connections)
            {
                DrawConnection(connection);
            }
        }
    }
}

