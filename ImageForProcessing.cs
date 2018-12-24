using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace openCVTest
{
    abstract class ImageForProcessing
    {
        Mat sourceMat { get; set; }
        Mat preparedMat { get; set; }
        private bool _isPrepared;
        public bool isPrepared { get; private set;  }

        public abstract void InitFullProcessing();
        public Mat GetSource() { return sourceMat; }
        public Mat GetPreparedMat() { return preparedMat; }

        public void LoadImage(string pathToImage, bool forceReload = false)
        {
            if ((pathToImage.Length > 0) && sourceMat == null || forceReload)
            {
                this.sourceMat = ImageAnalyzeTool.LoadImage(pathToImage);
                isPrepared = false;
            }
        }

        public void LoadMat(Mat matToLoad, bool forceReload = false)
        {
            if ((matToLoad != null) && sourceMat == null || forceReload)
            {
                matToLoad.CopyTo(this.sourceMat);
                isPrepared = false;
            }
        }

        
        /*public void SaveImage(string fileName)
        {
            if (fileName.Length > 0 && sourceMat != null)
            {
                ImageAnalyzeTool.SaveImageFromMat(this.sourceMat, fileName);
            }
        }*/

        public abstract void SaveImage(Mat imageToSave, string fileName);

        public void PrepareImage(bool forceReprepare = false)
        {
            if (!isPrepared || forceReprepare)
            {
                preparedMat = ImageAnalyzeTool.PrepareImage(this.sourceMat);
                isPrepared = true;
            } 
        }
    }
}
