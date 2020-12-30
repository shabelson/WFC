using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
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
// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace WFC
{
    public class WFCComponent : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public WFCComponent()
          : base("WFC Build Model", "modeler",
              "test1",
              "SHA", "Model")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("meshes", "meshalach", "los meshelitos", GH_ParamAccess.list);
            pManager.AddBoxParameter("PBoxes", "PrototypeBoxes", "boxalach", GH_ParamAccess.list);
            pManager.AddIntegerParameter("pixelData", "pxd", "numbers", GH_ParamAccess.tree);
            pManager.AddIntegerParameter("steps", "steps", "number of pixData in every direction", GH_ParamAccess.item);
            pManager.AddBoxParameter("voxel", "voxel", "box Geo the size of the voxel that the data was gathered", GH_ParamAccess.item);
            pManager.AddIntegerParameter("worldX", "Xs", "num of vox in x", GH_ParamAccess.item);
            pManager.AddIntegerParameter("worldY", "Ys", "num of vox in Y", GH_ParamAccess.item);
            pManager.AddIntegerParameter("worldZ", "Zs", "num of vox in z", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Run", "run", "run", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Reset", "reset", "reset", GH_ParamAccess.item);



        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {


            pManager.AddBoxParameter("oBox", "oBox", "oBox", GH_ParamAccess.list);
            pManager.AddMeshParameter("oMesh", "oMesh", "om", GH_ParamAccess.list);
            pManager.AddMeshParameter("ProtoMesh", "oMesh", "om", GH_ParamAccess.list);
            pManager.AddPointParameter("opts", "opts", "opts", GH_ParamAccess.list);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> meshes = new List<Mesh>();
            List<Box> pboxes = new List<Box>();
            GH_Structure<Grasshopper.Kernel.Types.GH_Integer> pixData = new GH_Structure<Grasshopper.Kernel.Types.GH_Integer>();
            int steps = 0;
            Box voxel = new Box() ;
            int Xs = 0;
            int Ys = 0;
            int Zs = 0;
            bool Run = false;
            bool Reset = false;
            List<bool> enter = new List<bool>
            {
                DA.GetDataList("meshes", meshes),
                DA.GetDataTree(name: "pixelData", out pixData),
                DA.GetData("steps", ref steps),
                DA.GetData("voxel", ref voxel),
                DA.GetData("worldX", ref Xs),
                DA.GetData("worldY", ref Ys),
                DA.GetData("worldZ", ref Zs),
                DA.GetData("Reset",ref Reset),
                DA.GetDataList("PBoxes",pboxes)
            };
            DA.GetData("Run", ref Run);
            foreach (bool Bool in enter) 
            {
                if (Bool == false) DA.AbortComponentSolution();
            }

            if (Reset == true) 
                {
                model = new Model(meshes, pboxes, pixData, steps, voxel, Xs, Ys, Zs);
                
                
                
                
                
                List<Mesh> oMesh = model.GetProtoMesh();
                DA.SetDataList("ProtoMesh", oMesh);


                 }
            if (Run == true) 
            {

                model.Collapse();
                DA.SetDataList("oMesh",model.GetSolution());



            }
            else
            { model = null; }

        }
        public Model model;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("5b5870ed-ea05-4f9f-8cdd-6a91b715c516"); }
        }
    }
}
