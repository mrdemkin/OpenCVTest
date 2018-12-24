using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCVTest
{
    interface IImageForTextProcessing
    {
        //Point[][] GetContoursForParagraph(Mat imageToProcesing);
        void initBoundedWordsImage();
        void initBoundedParagraphsImage();
        void SaveImageWithWordsBounding(string fileName);
        void SaveImageWithParagraphsBounding(string fileName);
    }
}
