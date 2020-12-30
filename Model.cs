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
    public class Model
    {

        Random random = new Random();
        public World slots;
        public Parts prototypes;

        public Model(List<Mesh> meshes, List<Box> pboxes, GH_Structure<GH_Integer> pixData, int steps, Box voxel, int Xs, int Ys, int Zs)
        {

            prototypes = new Parts(meshes, pboxes, pixData, steps);
            slots = new World(voxel, Xs, Ys, Zs, prototypes);
        }
        public List<Mesh> GetProtoMesh()
        {
            List<Mesh> oMesh = new List<Mesh>();
            foreach (KeyValuePair<string, Prototype> p in prototypes.allProtos) {
                oMesh.Add(p.Value.geo);
            }

            return oMesh;
        }
        void CollapseCell(int[] coords)
        {

            Cell cell = slots.GetCell(coords);
            List<string> options = cell.SuperPosition;
            string protoName = options[random.Next(options.Count - 1)];
            cell.Collapse(prototypes.allProtos[protoName]);


        }
        public bool Collapse()
        {
            List<int[]> nextCellscoords = new List<int[]>();
            List<Cell> nextCells = new List<Cell>();
            List<int> entVals = slots.entropyDict.Keys.ToList();
            int minEntVal;
            entVals.Sort();
            if (entVals[entVals.Count - 1] == 0) { return false; }
            if (entVals[0] > 0) { minEntVal = entVals[0]; }
            else { minEntVal = entVals[1]; }
            nextCellscoords = slots.entropyDict[minEntVal];

            int ind = random.Next(nextCellscoords.Count);
            int[] coords = nextCellscoords[ind];
            CollapseCell(coords);
            slots.entropyDict[minEntVal].Remove(coords);
            slots.entropyDict[0].Add(coords);

            /*
             * 1. choose cell with lowet entropy 
             * 2. collapse cells with least entropy
             * 3. propegate throug neibours (and neibours neibours) - reduce entropy
             * 
             * 
             * 
             * 
             * */         
          return false;
        }
        public void ReduceNeibsEntropy(int[] coords) 
        
        {
            Dictionary<string, List<string>> optNeibs = new Dictionary<string, List<string>>() {
                {"north",new List<string>{ } } ,
                {"south",new List<string>{ } },
                {"east",new List<string>{ } },
                {"west",new List<string>{ } },
                {"up",new List<string>{ } },
                {"down",new List<string>{ } },
            };
            
            Cell locCell = slots.GetCell(coords);
            List<string> superPosition = new List<string>();
            if (locCell.collaped == true) { superPosition.Add(locCell.state.protoName); }
            else { superPosition = locCell.SuperPosition; }
            Prototype locProto;
            foreach (string optState in superPosition) 
            {
                locProto = prototypes.allProtos[optState];
                optNeibs["north"].AddRange(locProto.northNeib);
                optNeibs["south"].AddRange(locProto.southNeib);
                optNeibs["east"].AddRange(locProto.eastNeib);
                optNeibs["west"].AddRange(locProto.westNeib);
                optNeibs["up"].AddRange(locProto.upNeib);
                optNeibs["down"].AddRange(locProto.downNeib);
            }



            int[] nCords = locCell.neibs["north"];
            //slots.cellslocCell.neibs["north"] = optNeibs["north"];
            
            
            //locCell["south"] = optNeibs["south"];
            //locCell["east"]  = optNeibs["east" ];
            ///locCell["west"]  = optNeibs["west" ];
            //locCell["up"]    = optNeibs["up"   ];
            //locCell["down"] = optNeibs["down"];
           

           




           // else(superPosition = locCell.SuperPosition)


            /*get the possible states in locCell // SolidState Name
            * gather possible neibour states
            * reduce entropy in neibeour cell by possible neibour states.
            * 
            * 
            * 
            * 
            */





        }
        public void ChainCollapse(int[] coords,int saftyDepth)
        {
            if (saftyDepth > 3) { return; }

            Cell locCell = slots.GetCell(coords);
            int cellEntVal = locCell.SuperPosition.Count;
            
            if (cellEntVal == 0) { return; }

            //if (locCell.collaped == false) { CollapseCell(coords); }
            ReduceNeibsEntropy(coords);

            foreach (string neibDir in locCell.neibs.Keys) {
                int[] neibCoords = locCell.neibs[neibDir];
                  
                    ChainCollapse(nextNeib, saftyDepth + 1);
                    //TODO - build the chain collapse and ReduceNeibEntropy
            }
        }





        public List<Mesh> GetSolution() => slots.GetMeshes();







    }

}
