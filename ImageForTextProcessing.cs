using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace openCVTest
{
    class ImageForTextProcessing : ImageForProcessing, IImageForTextProcessing
    {
        Mat imageWithWordsBoundinRect;
        Mat imageWithParagraphBoundingRect;
        public ImageForTextProcessing(string pathToImageFile)
        {
            this.LoadImage(pathToImageFile);
        }

        public ImageForTextProcessing(Mat imageMat)
        {
            this.LoadMat(imageMat);
        }

        public override void InitFullProcessing()
        {
            PrepareImage();
            initBoundedWordsImage();
            initBoundedParagraphsImage();
        }

        public void initBoundedWordsImage()
        {
            if (!isPrepared) PrepareImage();
            this.imageWithWordsBoundinRect = ImageAnalyzeTool.DrawRectanglesWords(this.GetPreparedMat());
        }

        public void initBoundedParagraphsImage()
        {
            if(!isPrepared) PrepareImage();
            this.imageWithParagraphBoundingRect = ImageAnalyzeTool.DrawRectanglesParagraphs(this.GetPreparedMat());
        }
                
        
        public override void SaveImage(Mat imageToSave, string fileName)
        {
            {
                if (fileName.Length > 0 && imageToSave != null)
                {
                    ImageAnalyzeTool.SaveImageFromMat(imageToSave, fileName);
                }
            }
        }

        public void SaveImageWithWordsBounding(string fileName)
        {
            SaveImage(this.imageWithWordsBoundinRect, fileName);
        }

        public void SaveImageWithParagraphsBounding(string fileName)
        {
            SaveImage(this.imageWithParagraphBoundingRect, fileName);
        }
    }
}
