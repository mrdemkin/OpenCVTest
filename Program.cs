using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace openCVTest
{
    class Program
    {
        private const string SOURCE_FILE_NAME = "source.tif";
        // private const string SOURCE_FILE_NAME = "anotherTest.png";
        // private const string SOURCE_FILE_NAME = "anotherTest2.jpg";
        // private const string SOURCE_FILE_NAME = "anotherTest3.jpg";
        // private const string SOURCE_FILE_NAME = "anotherTest4.jpg";
        // private const string SOURCE_FILE_NAME = "anotherTest5.jpg";

        static void Main(string[] args)
        {
            ImageForTextProcessing newImageWithText = new ImageForTextProcessing(SOURCE_FILE_NAME);
            newImageWithText.PrepareImage();
            newImageWithText.initBoundedWordsImage();
            newImageWithText.initBoundedParagraphsImage();
            newImageWithText.SaveImageWithWordsBounding("image-with-words.png");
            newImageWithText.SaveImageWithParagraphsBounding("image-with-paragraphs.png");
            CommonTools.WaitUser();
        }
    }

}
