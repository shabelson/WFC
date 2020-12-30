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
    public class Prototype

    {
        public int steps;
        public Mesh geo;
        public Box boundingBox;
        public string name;
        public List<int> southMatrix;
        public List<int> northMatrix;
        public List<int> eastMatrix;
        public List<int> westMatrix;
        public List<int> upMatrix;
        public List<int> downMatrix;
        public string southHash;
        public string eastHash;
        public string northHash;
        public string westHash;
        public string upHash;
        public string downHash;
        public List<string> northNeib;
        public List<string> southNeib;
        public List<string> eastNeib;
        public List<string> westNeib;
        public List<string> upNeib;
        public List<string> downNeib;

        public Prototype() { }
        
       
        public Prototype(Mesh mesh,Box pbox,int[] pixData,int step,string hash)
        {
            geo = mesh;
            boundingBox = pbox;
            steps = step;
            name = hash;
            southMatrix= GetsouthMat(pixData);
            northMatrix= GetnorthMat(pixData);
            eastMatrix = GeteastMat(pixData);
            westMatrix = GetwestMat(pixData);
            upMatrix   = GetupMat(pixData);
            downMatrix = GetdownMat(pixData);
            northNeib = new List<string>();
            southNeib = new List<string>();
            eastNeib = new List<string>();
            westNeib = new List<string>();
            upNeib = new List<string>();
            downNeib = new List<string>();
    }

        private List<int> GetsouthMat(int[] pixData)
        {
            List<int> s = new ArraySegment<int>(pixData, 0, steps * steps).ToList(); // output: { 2, 3 }
            southHash = string.Join("", s);
            return s;
        }

        private List<int> GetnorthMat(int[] pixData)
        {


            List<int> n = new ArraySegment<int>(pixData, steps * steps * (steps-1), steps*steps).ToList(); // output: { 2, 3 }
            northHash = string.Join("", n);
            return n;
        }

        private List<int> GeteastMat(int[] pixData)
        {
            List<int> east = new List<int>();
            for (int i = 0; i < pixData.Count(); i += steps*steps) 
            {
                for (int t = 0; t < steps;t++) 
                {
                    east.Add(pixData[t + i]);
                }
            }
            eastHash = string.Join("", east);
            return east;
        }

        private List<int> GetwestMat(int[] pixData)
        {
            List<int> w = new List<int>();
            for (int i = 0; i < steps; i++)
            {
                for (int t = 0; t < steps; t++)
                {
                    w.Add(pixData[i * steps * steps + t]);
                
                }
            }
            westHash = string.Join("", w);
            return w;
        }

        private List<int> GetupMat(int[] pixData)
        {
            List<int> u = new List<int>();
            for (int i = steps; i <= steps * steps * steps; i += steps)
            {
                u.Add(pixData[i-1]);
            }
            upHash = string.Join("", u);

            return u;
            
        }

        private List<int> GetdownMat(int[] pixData)
        {
            List<int> d = new List<int>();
            for (int i = 0; i < steps * steps * steps; i += steps)
            {
                d.Add(pixData[i]);
            
            }
            downHash = string.Join("", d);
            return d;
        }
    }
}
