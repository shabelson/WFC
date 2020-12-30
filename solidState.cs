using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper;
namespace WFC
{
    public struct SolidState
    {
        public Mesh geo;
        public string protoName;

        public SolidState(Mesh mesh,string pName)
        {
            geo = mesh;
            protoName = pName;

        }          
        
        
        
    }

}
