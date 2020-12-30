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
    public class World
    {
        public  Cell[,,] cells;
        readonly int xs;
        readonly int ys;
        readonly int zs;
        public Dictionary<int, List<int[]>> entropyDict;
        public World(Box voxel,int x, int y, int z,Parts protos)
        {
            
            xs = x;
            ys = y;
            zs = z;
            this.cells = new Cell[xs, ys, zs];

            
            Point3d bPt = voxel.PointAt(0, 0, 0);
            Point3d ePt = voxel.PointAt(1, 1, 1);
            Vector3d vec = ePt - bPt;
            Vector3d vecX = new Vector3d(vec.X, 0, 0);
            Vector3d vecY = new Vector3d(0, vec.Y, 0);
            Vector3d vecZ = new Vector3d(0, 0, vec.Z);
            for (int u = 0; u < xs; u++)
            {

                for (int v = 0; v < ys; v++)
                {
                    for (int w = 0; w < zs; w++)
                    {

                        Vector3d locVec = u * vecX + v * vecY + w * vecZ;
                        Box locBox = new Box(voxel);
                        var trans = Rhino.Geometry.Transform.Translation(locVec);
                        locBox.Transform(trans);
                        this.cells[u, v, w] = new Cell(locBox, new int[] { u, v, w }, new int[] { xs, ys, zs }, protos);
                    }
                } 
            }
            entropyInit();
        }
        public void entropyInit() 
        {

            entropyDict = new Dictionary<int, List<int[]>>();
            for (int u = 0; u < xs; u++)
            {
                for (int v = 0; v < ys; v++)
                {
                    for (int w = 0; w < zs; w++)
                    {
                        if (entropyDict.ContainsKey(cells[u, v, w].SuperPosition.Count))
                        {
                            entropyDict[cells[u, v, w].SuperPosition.Count].Add(new int[] { u, v, w });
                        }
                        else { entropyDict.Add(cells[u, v, w].SuperPosition.Count, new List<int[]>() { new int[] { u, v, w } }); }
                    }
                }

            }
            if (entropyDict.ContainsKey(0) == false)
            { entropyDict.Add(0, new List<int[]>()); }

        }
        public void checkEntropy(List<int[]> changedCells)
        
        {
            throw new NotImplementedException();
        }
        public Cell GetCell(int[] coords) 
        {
            return cells[coords[0], coords[1], coords[2]];
        
        }


        public void Propegate() { }
        public void CollapseCell(int[] coords, Prototype wave) { }

        public List<Mesh> GetMeshes()
        {
            List<Mesh> oMesh = new List<Mesh>();
            for (int u = 0; u < xs; u++)
            {
                for (int v = 0; v < ys; v++)
                {
                    for (int w = 0; w < zs; w++)
                    {
                        if (cells[u, v, w].collaped == true) { oMesh.Add(cells[u, v, w].state.geo);}
                    }
                }
            }
            return oMesh;
         }       
    }
}
