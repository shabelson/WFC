using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Grasshopper;
using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
namespace WFC
{
    public class Cell
    {
        public Box geo;
        public Dictionary<string, int[]> neibs = new Dictionary<string, int[]>();
        public readonly int[] name = new int[3];
        public List<string> SuperPosition = new List<string>();
        public SolidState state;
        public string stateName;
        public bool collaped = false;

        public Cell(Box box, int[] coords, int[] size, Parts protos)
        {
            int[] neibsEast =  new int[3];
            int[] neibsWest =  new int[3];
            int[] neibsNorth = new int[3];
            int[] neibsSouth = new int[3];
            int[] neibsUp =    new int[3];
            int[] neibsDown =  new int[3];

            geo = box;
            int[] name = coords;
            //neibeast
            if (coords[0] < size[0] - 1)
            {
                neibsEast =new int[] { (int)coords[0] + 1, (int)coords[1], (int)coords[2] };
                SuperPosition.AddRange(protos.HasNeibsDict["east"].ToList());
            }

            //neibwest
            if (coords[0] > 0)
            { neibsWest = new int[] {
                (int)coords[0] - 1, (int)coords[1], (int)coords[2] };
                SuperPosition.AddRange(protos.HasNeibsDict["west"].ToList());
            }
            //neibnort
            if (coords[1] < size[1] - 1)
            { neibsNorth = new int[] { (int)coords[0], (int)coords[1] + 1, (int)coords[2] };
                SuperPosition.AddRange(protos.HasNeibsDict["north"].ToList());
            }
            //nebsouth  
            if (coords[1] > 0)
            { neibsSouth=new int[] { (int)coords[0], (int)coords[1] - 1, (int)coords[2] };
                SuperPosition.AddRange(protos.HasNeibsDict["south"].ToList());
            }
            //neibup
            if (coords[2] < size[2] - 1)
            { neibsUp=new int[] { (int)coords[0], (int)coords[1], (int)coords[2] + 1 };
                SuperPosition.AddRange(protos.HasNeibsDict["up"].ToList());
            }
            //neibdown
            if (coords[2] > 0)
            { neibsDown=new int[] { (int)coords[0], (int)coords[1], (int)coords[2] - 1 };
                SuperPosition.AddRange(protos.HasNeibsDict["down"].ToList());
            }
            neibs = new Dictionary<string,int[]>(){ 
                {"east", neibsEast} ,
                {"west",neibsWest },
                {"north",neibsNorth },
                {"south",neibsSouth },
                {"up",neibsUp },
                {"down",neibsDown },
            };
        }
        public void Collapse(Prototype wave)
        {

            Point3d pt1 = wave.boundingBox.GetCorners()[0];
            Point3d pt2 = geo.GetCorners()[0];
            Vector3d vec = pt1 - pt2;
            Mesh newMesh = wave.geo.DuplicateMesh();
            newMesh.Translate(vec);

            state = new SolidState(newMesh, wave.name);
            stateName = wave.name;
            collaped = true;
            SuperPosition = new List<string>{ };
        }

    }
}
