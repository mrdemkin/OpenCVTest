using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace openCVTest
{
    static class ImageAnalyzeTool // need abstract wrap
    {
        public static bool SaveImageFromMat(Mat matToSave, string name)
        {
            if (matToSave != null && name.Length != 0)
            {
                if (matToSave.SaveImage(name))
                {
                    CommonTools.WriteLog("Successfully saved");
                    return true;
                }
            }
            return false;
        }


        public static Mat LoadImage(string SOURCE_FILE_NAME)
        {
            Mat loadedImage = null;
            try
            {
                loadedImage = new Mat(SOURCE_FILE_NAME, ImreadModes.Grayscale);
            }

            catch (Exception e)
            {
                CommonTools.WriteLog(String.Format("Get some errors when loading image: {0}", e.StackTrace));
            }

            if (loadedImage != null)
                CommonTools.WriteLog("Image loaded succesfully");
            return loadedImage;
        }

        private static bool SaveMatToFile(Mat matToSave, string name)
        {
            if (matToSave != null && name.Length != 0)
            {
                matToSave.SaveImage(name);
                CommonTools.WriteLog("Successfully saved");
                return true;
            }
            return false;
        }

        public static Mat PrepareImage(Mat imageSource)
        {
            imageSource = SmoothImage(imageSource);
            imageSource = RemoveBackground(imageSource);
            return imageSource;
        }

        private static Mat SmoothImage(Mat imageToSmooth)
        {
            if (imageToSmooth != null)
            {
                imageToSmooth = imageToSmooth.MedianBlur(1);
            }
            return imageToSmooth;
        }

        private static Mat RemoveBackground(Mat imageWithBackground)
        {
            if (imageWithBackground != null)
            {
                imageWithBackground = imageWithBackground.Threshold(0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            }

            return imageWithBackground;
        }


        public static Mat DrawRectanglesWords(Mat preparedImage)
        {
            Mat resultMat = new Mat();
            preparedImage.CopyTo(resultMat);

            resultMat = DrawReactangleByContours(GetContoursForWords(resultMat), preparedImage);

            return resultMat;
        }


        public static Mat DrawRectanglesParagraphs(Mat preparedImage)
        {
            Mat resultMat = new Mat();
            preparedImage.CopyTo(resultMat);

            resultMat =  DrawReactangleByContours(GetContoursForParagraph (resultMat), preparedImage);

            return resultMat;
        }



        private static Mat DrawReactangleByContours(Point[][] contours, Mat imageToProcessing, bool fillRect = false)
        {
            Mat outputMat = new Mat();
            imageToProcessing.CopyTo(outputMat);
            int fillingParam = 1;
            Scalar _scalar = Scalar.Red;
            if (fillRect)
            {
                fillingParam = -1;
                //_scalar = Scalar.White;
            }
            Rect previousBiggestContourRect = new Rect();
            foreach (var cont in contours)
            {
                if (cont != null)
                {
                    var biggestContourRect = Cv2.BoundingRect(cont);
                    if (!previousBiggestContourRect.Contains(biggestContourRect))
                    {
                        previousBiggestContourRect = biggestContourRect;
                        Cv2.Rectangle(outputMat,
                        new Point(biggestContourRect.X, biggestContourRect.Y),
                        new Point(biggestContourRect.X + biggestContourRect.Width, biggestContourRect.Y + biggestContourRect.Height),
                        _scalar, fillingParam, LineTypes.Link8, 0);
                    }
                }
            }

            return outputMat;
        }


        private static Point[][] GetContoursForWords(Mat imageToProcesing)
        {
            Point[][] wordsContours;
            HierarchyIndex[] hierarchyIndexes;
            var kernelH = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
            Cv2.Erode(imageToProcesing, imageToProcesing, kernelH, null, 4);
            Cv2.Dilate(imageToProcesing, imageToProcesing, kernelH, null, 4);
            FindContours(imageToProcesing, out wordsContours, out hierarchyIndexes);
            Mat newOutput = new Mat();
            newOutput = CreateMaskWords(imageToProcesing);
            Point[][] contoursMask; //vector<vector<Point>> contours;
            FindContours(newOutput, out contoursMask, out hierarchyIndexes);
            return contoursMask;
        }

        private static Point[][] GetContoursForParagraph(Mat imageToProcesing)
        {

            Point[][] wordsContours;
            HierarchyIndex[] hierarchyIndexes;
            var kernelH = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 1));
            Cv2.Erode(imageToProcesing, imageToProcesing, kernelH, null, 4);
            Cv2.Dilate(imageToProcesing, imageToProcesing, kernelH, null, 4);
            FindContours(imageToProcesing, out wordsContours, out hierarchyIndexes);
            Mat newOutput = new Mat();
            newOutput = CreateMaskParagraph(imageToProcesing);
            Point[][] contoursMask; //vector<vector<Point>> contours;
            FindContours(newOutput, out contoursMask, out hierarchyIndexes);
            return contoursMask;
        }

        public static Mat CreateMaskWords(Mat imageSource)
        {
            return CreateMask(imageSource, new Size(14, 3));
        }

        public static Mat CreateMaskParagraph(Mat imageSource)
        {

            return CreateMask(imageSource, new Size(29, 20));
        }

        private static Mat CreateMask(Mat imageSource, Size structSize)
        {
            Mat newOutput = new Mat();
            if (imageSource != null)
            {
                var kernelH = Cv2.GetStructuringElement(MorphShapes.Rect, structSize);
                Cv2.MorphologyEx(imageSource, newOutput, MorphTypes.Close, kernelH);
                //var kernelV = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(12, 1));
                //Cv2.MorphologyEx(newOutput, newOutput, MorphTypes.Close, kernelV);
            }
            return newOutput;

        }

        public static void FindContours(Mat imageSource, out Point[][] contours, out HierarchyIndex[] hierarchyIndexes)
        {
            if (imageSource != null)
            {
                Cv2.Canny(imageSource, imageSource, 32, 192); // why this value?
                                                              // only external contours, without internal holes
                Cv2.FindContours(imageSource, out contours, out hierarchyIndexes, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            }
            else { contours = null; hierarchyIndexes = null; }
        }

        private static Mat CreateMask(Mat imageSource)
        {
            Mat newOutput = new Mat();
            if (imageSource != null)
            {
                var kernelH = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(14, 3));
                Cv2.MorphologyEx(imageSource, newOutput, MorphTypes.Close, kernelH);
                //var kernelV = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(12, 1));
                //Cv2.MorphologyEx(newOutput, newOutput, MorphTypes.Close, kernelV);
            }
            return newOutput;
        }
    }
}
