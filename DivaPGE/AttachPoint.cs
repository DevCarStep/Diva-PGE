using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ProcedureMapGenerator;

namespace DivaPGE
{
    public class AttachPoint : MonoBehaviour
    {
        public Transform point;
        public bool attached = false;

        ConnectionType connectionType;

        public AttachPoint(ConnectionType connection)
        {
            this.connectionType = connection;
        }
    }
}
