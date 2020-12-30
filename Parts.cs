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
    public class Parts :IEnumerable
    {
        List<string> IDs;
        public Dictionary<string, List<string>> HasNeibsDict = new Dictionary<string, List<string>>() { 
            { "north", new List<string>() },
            { "south", new List<string>() },
            { "east", new List<string>() },
            { "west", new List<string>() },
            { "up", new List<string>() },
            { "down", new List<string>()}
        };
        
        
        
        
        public Dictionary<string, Prototype> allProtos = new Dictionary<string, Prototype>();
        public Parts(List<Mesh> meshes,List<Box> pboxes, GH_Structure<GH_Integer> pixData, int steps)
        {
            int counter = 0;
            int index = 0;

            IDs = new List<string>();
            for (int ind = 0; ind < pixData.PathCount;ind++)
            {
                if (meshes[ind].IsValid == false) {
                    //counter += 1;
                    continue; }
                //counter += 1;
                List<int> intArray = new List<int>();
                int Temp = 0;
                List<GH_Integer> branch = pixData.Branches[ind];
                foreach (GH_Integer i in branch)
                {
                    Temp = (int)i.Value; 
                    intArray.Add(Temp);
                }
                
                
                string code = string.Join("", intArray);
                IDs.Add(code);
                
               
                if (allProtos.ContainsKey(code)) continue;
                else 
                {
                    allProtos.Add(code, new Prototype(meshes[ind],pboxes[ind], intArray.ToArray(), steps, code));
                    index += 1;
                }




            }
            ConnectNeibs();
        }
        public void ConnectNeibs() 
        {
            for(int p1 = 0; p1<allProtos.Count-1;p1++) {
                for (int p2 = p1+1; p2 < allProtos.Count; p2++)
                {
                    KeyValuePair<string, Prototype> pKV1 = allProtos.ToList()[p1];
                    KeyValuePair<string, Prototype> pKV2 = allProtos.ToList()[p2];
                    if (pKV1.Value.southHash == pKV2.Value.northHash)
                    {
                        allProtos[pKV1.Key].southNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].northNeib.Add(pKV2.Key);
                    }
                    
                    else if (pKV1.Value.northHash == pKV2.Value.southHash)
                    {
                        allProtos[pKV1.Key].northNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].southNeib.Add(pKV2.Key);
                    }
                    //
                    
                    else if (pKV1.Value.eastHash == pKV2.Value.westHash)
                    {
                        allProtos[pKV1.Key].eastNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].westNeib.Add(pKV2.Key);
                    }
                    else if (pKV1.Value.westHash == pKV2.Value.eastHash)
                    {
                        allProtos[pKV1.Key].westNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].eastNeib.Add(pKV2.Key);
                    }


                    else if (pKV1.Value.upHash == pKV2.Value.downHash)
                    {
                        allProtos[pKV1.Key].upNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].downNeib.Add(pKV2.Key);
                    }
                    else if (pKV1.Value.downHash == pKV2.Value.upHash)
                    {
                        allProtos[pKV1.Key].downNeib.Add(pKV2.Key);
                        allProtos[pKV2.Key].upNeib.Add(pKV2.Key);
                    }
                    
                    else { continue; }


                }
            }
            GetNeibsDict();
        }
        public void GetNeibsDict() 
        {
            
            
            
            
            foreach (KeyValuePair<string, Prototype> p in allProtos) 
            {
                if (p.Value.eastNeib.Count > 0) { HasNeibsDict["east"].Add(p.Key); }
                if (p.Value.westNeib.Count > 0){ HasNeibsDict["west"].Add(p.Key); }
                if (p.Value.northNeib.Count > 0) { HasNeibsDict["north"].Add(p.Key); }
                if (p.Value.southNeib.Count > 0) { HasNeibsDict["south"].Add(p.Key); }
                if (p.Value.upNeib.Count > 0) { HasNeibsDict["up"].Add(p.Key); }
                if (p.Value.downNeib.Count > 0) { HasNeibsDict["down"].Add(p.Key); }
            }
        
        
        
        }
        public IEnumerator GetEnumerator()
        {
            return allProtos.GetEnumerator();

        }
    }
}
