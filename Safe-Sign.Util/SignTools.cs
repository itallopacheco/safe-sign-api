using iText.Layout;
using iText.Barcodes;
using iText.Kernel.Pdf;
using iText.Kernel.Font;
using iText.Kernel.Colors;
using iText.Svg.Converter;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

using Path = System.IO.Path;
using Rectangle = iText.Kernel.Geom.Rectangle;

using Safe_Sign.DTO.Person;
using Safe_Sign.DTO.Document;
using Safe_Sign.DTO.Signature;
using Safe_Sign.DAL.Models;

namespace Safe_Sign.Util
{
    public static class SignTools
    {
        // Colors
        private static readonly DeviceCmyk blackColor = new(0, 0, 0, 1);
        private static readonly DeviceCmyk grayColor = new(0, 0, 0, 37);

        // Fonts
        private static readonly PdfFont robotoLight = PdfFontFactory.CreateFont(Path.GetFullPath("../Safe-Sign.Util/Fonts/Roboto/Roboto-Light.ttf"));
        private static readonly PdfFont robotoRegular = PdfFontFactory.CreateFont(Path.GetFullPath("../Safe-Sign.Util/Fonts/Roboto/Roboto-Regular.ttf"));
        private static readonly PdfFont robotoBold = PdfFontFactory.CreateFont(Path.GetFullPath("../Safe-Sign.Util/Fonts/Roboto/Roboto-Bold.ttf"));
        private static readonly PdfFont robotoItalic = PdfFontFactory.CreateFont(Path.GetFullPath("../Safe-Sign.Util/Fonts/Roboto/Roboto-Italic.ttf"));
        private static readonly PdfFont robotoBlackItalic = PdfFontFactory.CreateFont(Path.GetFullPath("../Safe-Sign.Util/Fonts/Roboto/Roboto-BlackItalic.ttf"));

        private static List<int> SearchTextInPDFFile(string pdfPath, string searchText)
        {
            PdfReader pdfRead = new(pdfPath);
            PdfDocument pdfDocument = new(pdfRead);
            List<int> pages = new();

            for (int page = 1; page <= pdfDocument.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                var findingsInPage = 0;
                string currentPageText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(page), strategy);
                var lastIndex = 0;
                do
                {
                    currentPageText = currentPageText.Substring(lastIndex + (lastIndex != 0 ? searchText.Length : 0));
                    lastIndex = currentPageText.IndexOf(searchText);
                    findingsInPage = lastIndex != -1 ? ++findingsInPage : findingsInPage;

                } while (lastIndex != -1);
                
                pages.Add(findingsInPage);
            }

            pdfRead.Close();

            return pages;
        }

        private static void SignPageFooter(PdfDocument pdfDocument, int pageNumber, DocumentDTO documentDTO)
        {
            Rectangle pageSize; PdfCanvas canvas;
            PdfPage page = pdfDocument.GetPage(pageNumber);
            pageSize = page.GetPageSize();
            canvas = new PdfCanvas(page);

            // Insert footer line
            canvas.SetStrokeColor(grayColor)
                .SetLineWidth(.8f)
                .MoveTo(pageSize.GetWidth() / 9, 25)
                .LineTo(pageSize.GetWidth() / 9 * 8, 25)
                .Stroke();

            // Insert footer text
            canvas.BeginText()
                .MoveText(pageSize.GetWidth() / 9, 18)
                .SetFontAndSize(robotoLight, 7)
                .ShowText("Documento assinado digitalmente através do SafeSign.")
                .EndText();

            canvas.BeginText()
                .MoveText(pageSize.GetWidth() / 9, 10)
                .SetFontAndSize(robotoRegular, 7)
                .ShowText($"Verifique a validade com o identificador: {documentDTO.KeyHash} ")
                .SetFontAndSize(robotoItalic, 7)
                .EndText();
        }
        /// <summary>
        /// Sign footer of documents coming from SGP
        /// </summary>
        /// <param name="pdfDocument"></param>
        /// <param name="pageNumber"></param>
        /// <param name="document"></param>
        private static void SignPageFooter(PdfDocument pdfDocument, int pageNumber, DAL.Models.Document document)
        {
            Rectangle pageSize; PdfCanvas canvas;
            PdfPage page = pdfDocument.GetPage(pageNumber);
            pageSize = page.GetPageSize();
            canvas = new PdfCanvas(page);

            // Insert footer line
            canvas.SetStrokeColor(grayColor)
                .SetLineWidth(.8f)
                .MoveTo(pageSize.GetWidth() / 9, 25)
                .LineTo(pageSize.GetWidth() / 9 * 8, 25)
                .Stroke();

            // Insert footer text
            canvas.BeginText()
                .MoveText(pageSize.GetWidth() / 9, 18)
                .SetFontAndSize(robotoLight, 7)
                .ShowText("Documento assinado digitalmente através do SafeSign.")
                .EndText();

            canvas.BeginText()
                .MoveText(pageSize.GetWidth() / 9, 10)
                .SetFontAndSize(robotoRegular, 7)
                .ShowText($"Verifique a validade com o identificador: {document.KeyHash} ")
                .SetFontAndSize(robotoItalic, 7)
                .EndText();
        }

        private static Image QrCode(PdfDocument pdfDoc, PdfPage pdfPage, PdfCanvas pdfCanvas, float marginDistance, DocumentDTO documentDTO)
        {
            marginDistance -= 100;
            Rectangle pageSize = pdfPage.GetPageSize();
            Rectangle rect = new((pageSize.GetWidth()/9) +10, marginDistance, pageSize.GetHeight()/10, pageSize.GetHeight()/10);
            Rectangle textSection = new((pageSize.GetWidth() / 9) + 120, marginDistance, (pageSize.GetWidth() / 2) +25  , pageSize.GetHeight() / 10);
            pdfCanvas.SaveState()   
                .SetFillColor(ColorConstants.WHITE)
                .Rectangle(rect)
                .Fill()
                .RestoreState();

            BarcodeQRCode barcodeQRCode = new(Environment.GetEnvironmentVariable("SAFE_SIGN_QRCODEURL"));
            PdfFormXObject pdfFormXObject = barcodeQRCode.CreateFormXObject(ColorConstants.BLACK, pdfDoc);
            Image qrCodeImage = new Image(pdfFormXObject)
                                    .SetWidth(rect.GetWidth() )
                                    .SetHeight(rect.GetHeight() );

            Canvas qrCanvas = new(pdfCanvas, rect);
            qrCanvas.Add(qrCodeImage);
            qrCanvas.Close();

            Text textVerifyAuthenticity = new Text($"Verifique a autenticidade em: " ) 
               .SetFont(robotoRegular)
               .SetFontSize(11)
               .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text textLinkSafeSign = new Text($"{Environment.GetEnvironmentVariable("SAFE_SIGN_URL")}, ")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text textInform = new Text("informando o código identificador:  ")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text verificationCode = new Text($"{documentDTO.KeyHash}")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            
            Paragraph paragraph = new Paragraph().Add(textVerifyAuthenticity).Add(textLinkSafeSign).Add(textInform).Add(verificationCode);
            Canvas TextCanvas = new(pdfCanvas, textSection );
            TextCanvas.Add(paragraph);

            return qrCodeImage;
        }
        /// <summary>
        /// Insert QR Code in a document coming from SGP
        /// </summary>
        /// <param name="pdfDoc"></param>
        /// <param name="pdfPage"></param>
        /// <param name="pdfCanvas"></param>
        /// <param name="marginDistance"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private static Image QrCode(PdfDocument pdfDoc, PdfPage pdfPage, PdfCanvas pdfCanvas, float marginDistance, DAL.Models.Document document)
        {
            marginDistance -= 100;
            Rectangle pageSize = pdfPage.GetPageSize();
            Rectangle rect = new((pageSize.GetWidth() / 9) + 10, marginDistance, pageSize.GetHeight() / 10, pageSize.GetHeight() / 10);
            Rectangle textSection = new((pageSize.GetWidth() / 9) + 120, marginDistance, (pageSize.GetWidth() / 2) + 25, pageSize.GetHeight() / 10);
            pdfCanvas.SaveState()
                .SetFillColor(ColorConstants.WHITE)
                .Rectangle(rect)
                .Fill()
                .RestoreState();

            BarcodeQRCode barcodeQRCode = new(Environment.GetEnvironmentVariable("SAFE_SIGN_QRCODEURL"));
            PdfFormXObject pdfFormXObject = barcodeQRCode.CreateFormXObject(ColorConstants.BLACK, pdfDoc);
            Image qrCodeImage = new Image(pdfFormXObject)
                                    .SetWidth(rect.GetWidth())
                                    .SetHeight(rect.GetHeight());

            Canvas qrCanvas = new(pdfCanvas, rect);
            qrCanvas.Add(qrCodeImage);
            qrCanvas.Close();

            Text textVerifyAuthenticity = new Text($"Verifique a autenticidade em: ")
               .SetFont(robotoRegular)
               .SetFontSize(11)
               .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text textLinkSafeSign = new Text($"{Environment.GetEnvironmentVariable("SAFE_SIGN_URL")}, ")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text textInform = new Text("informando o código identificador:  ")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text verificationCode = new Text($"{document.KeyHash}")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);

            Paragraph paragraph = new Paragraph().Add(textVerifyAuthenticity).Add(textLinkSafeSign).Add(textInform).Add(verificationCode);
            Canvas TextCanvas = new(pdfCanvas, textSection);
            TextCanvas.Add(paragraph);

            return qrCodeImage;
        }

        private static void InsertSignatures(PdfDocument pdfDoc, PdfCanvas pdfCanvas, Rectangle pageSize, float marginDistance, SignatureDTO signature, PersonDTO person)
        {
            string signIconSVG = "<svg width=\"21\" height=\"19\" viewBox=\"0 0 21 19\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\r\n    <path d=\"M7.75081 18.85C9.53081 18.15 9.14081 16.22 8.24081 15C7.35081 13.75 6.12081 12.89 4.88081 12.06C4.00081 11.5 3.19081 10.8 2.54081 10C2.26081 9.67 1.69081 9.06 2.27081 8.94C2.86081 8.82 3.88081 9.4 4.40081 9.62C5.31081 10 6.21081 10.44 7.05081 10.96L8.06081 9.26C6.50081 8.23 4.50081 7.32 2.64081 7.05C1.58081 6.89 0.46081 7.11 0.10081 8.26C-0.21919 9.25 0.29081 10.25 0.87081 11.03C2.24081 12.86 4.37081 13.74 5.96081 15.32C6.30081 15.65 6.71081 16.04 6.91081 16.5C7.12081 16.94 7.07081 16.97 6.60081 16.97C5.36081 16.97 3.81081 16 2.80081 15.36L1.79081 17.06C3.32081 18 5.88081 19.47 7.75081 18.85ZM16.9608 5.33L11.2908 11H9.00081V8.71L14.6708 3.03L16.9608 5.33ZM20.3608 4.55C20.3508 4.85 20.0408 5.16 19.7208 5.47L17.2008 8L16.3308 7.13L18.9308 4.54L18.3408 3.95L17.6708 4.62L15.3808 2.33L17.5308 0.18C17.7708 -0.06 18.1608 -0.06 18.3908 0.18L19.8208 1.61C20.0608 1.83 20.0608 2.23 19.8208 2.47C19.6108 2.68 19.4108 2.88 19.4108 3.08C19.3908 3.28 19.5908 3.5 19.7908 3.67C20.0808 3.97 20.3708 4.25 20.3608 4.55Z\" fill=\"black\"/>\r\n    </svg>\r\n    ";

            // Insert all sections dynamically
            Rectangle signatureBlockRectangle = new((pageSize.GetWidth() / 9), marginDistance - 70, (pageSize.GetWidth() / 9) * 7, pageSize.GetHeight() / 13);
            Rectangle signatureTextRectangle = new((pageSize.GetWidth() / 9 + 35), marginDistance - 70, (pageSize.GetWidth() / 9) * 7 - 39, pageSize.GetHeight() / 13);
            Rectangle rectangleSVG = new(pageSize.GetWidth() / 9 + 12, marginDistance - 48, 21, 19);

            // Apply color to signature block rectangle
            pdfCanvas.SaveState()
                .SetFillColor(ColorConstants.LIGHT_GRAY)
                .Rectangle(signatureBlockRectangle)
                .Fill()
                .RestoreState();

            pdfCanvas.SaveState()
                 .SetStrokeColor(ColorConstants.BLACK)
                 .SetLineWidth(.8f)
                 .MoveTo(((pageSize.GetWidth() / 9) +10), marginDistance - 75)
                 .LineTo((pageSize.GetWidth() / 9 * 8) -10 , marginDistance - 75)
                 .Stroke()
                 .RestoreState();

            // Convert SVG icon to PDF object
            PdfFormXObject signImage = SvgConverter.ConvertToXObject(signIconSVG, pdfDoc);

            // Create an image, a new canvas
            Image signImageSvg = new(signImage);
            Canvas imageCanvas = new(pdfCanvas, rectangleSVG);
            
            // Add the created image (SVG icone) to canvas
            imageCanvas.Add(signImageSvg);
            imageCanvas.Close();

            Text teste = new Text("Documento assinado digitalmente via credenciais de usuário e senha. SafeSign - SGP")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);

            string fullname = person.FullName.ToUpper();
            Text teste2 = new Text($"Assinador por: {fullname} em {signature.SignatureDate}, ")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text test3 = new Text("conforme horário oficial de Brasília.")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);

            Paragraph paragraph = new Paragraph().Add(teste).Add(teste2).Add(test3);
            Canvas TextCanvas = new(pdfCanvas, signatureTextRectangle);
            TextCanvas.Add(paragraph);
        }
        /// <summary>
        /// Insert signatures in a document coming from SGP
        /// </summary>
        /// <param name="pdfDoc"></param>
        /// <param name="pdfCanvas"></param>
        /// <param name="pageSize"></param>
        /// <param name="marginDistance"></param>
        /// <param name="signature"></param>
        /// <param name="person"></param>
        private static void InsertSignatures(PdfDocument pdfDoc, PdfCanvas pdfCanvas, Rectangle pageSize, float marginDistance, Signature signature, Person person)
        {
            string signIconSVG = "<svg width=\"21\" height=\"19\" viewBox=\"0 0 21 19\" fill=\"none\" xmlns=\"http://www.w3.org/2000/svg\">\r\n    <path d=\"M7.75081 18.85C9.53081 18.15 9.14081 16.22 8.24081 15C7.35081 13.75 6.12081 12.89 4.88081 12.06C4.00081 11.5 3.19081 10.8 2.54081 10C2.26081 9.67 1.69081 9.06 2.27081 8.94C2.86081 8.82 3.88081 9.4 4.40081 9.62C5.31081 10 6.21081 10.44 7.05081 10.96L8.06081 9.26C6.50081 8.23 4.50081 7.32 2.64081 7.05C1.58081 6.89 0.46081 7.11 0.10081 8.26C-0.21919 9.25 0.29081 10.25 0.87081 11.03C2.24081 12.86 4.37081 13.74 5.96081 15.32C6.30081 15.65 6.71081 16.04 6.91081 16.5C7.12081 16.94 7.07081 16.97 6.60081 16.97C5.36081 16.97 3.81081 16 2.80081 15.36L1.79081 17.06C3.32081 18 5.88081 19.47 7.75081 18.85ZM16.9608 5.33L11.2908 11H9.00081V8.71L14.6708 3.03L16.9608 5.33ZM20.3608 4.55C20.3508 4.85 20.0408 5.16 19.7208 5.47L17.2008 8L16.3308 7.13L18.9308 4.54L18.3408 3.95L17.6708 4.62L15.3808 2.33L17.5308 0.18C17.7708 -0.06 18.1608 -0.06 18.3908 0.18L19.8208 1.61C20.0608 1.83 20.0608 2.23 19.8208 2.47C19.6108 2.68 19.4108 2.88 19.4108 3.08C19.3908 3.28 19.5908 3.5 19.7908 3.67C20.0808 3.97 20.3708 4.25 20.3608 4.55Z\" fill=\"black\"/>\r\n    </svg>\r\n    ";

            // Insert all sections dynamically
            Rectangle signatureBlockRectangle = new((pageSize.GetWidth() / 9), marginDistance - 70, (pageSize.GetWidth() / 9) * 7, pageSize.GetHeight() / 13);
            Rectangle signatureTextRectangle = new((pageSize.GetWidth() / 9 + 35), marginDistance - 70, (pageSize.GetWidth() / 9) * 7 - 39, pageSize.GetHeight() / 13);
            Rectangle rectangleSVG = new(pageSize.GetWidth() / 9 + 12, marginDistance - 48, 21, 19);

            // Apply color to signature block rectangle
            pdfCanvas.SaveState()
                .SetFillColor(ColorConstants.LIGHT_GRAY)
                .Rectangle(signatureBlockRectangle)
                .Fill()
                .RestoreState();

            pdfCanvas.SaveState()
                 .SetStrokeColor(ColorConstants.BLACK)
                 .SetLineWidth(.8f)
                 .MoveTo(((pageSize.GetWidth() / 9) + 10), marginDistance - 75)
                 .LineTo((pageSize.GetWidth() / 9 * 8) - 10, marginDistance - 75)
                 .Stroke()
                 .RestoreState();

            // Convert SVG icon to PDF object
            PdfFormXObject signImage = SvgConverter.ConvertToXObject(signIconSVG, pdfDoc);

            // Create an image, a new canvas
            Image signImageSvg = new(signImage);
            Canvas imageCanvas = new(pdfCanvas, rectangleSVG);

            // Add the created image (SVG icone) to canvas
            imageCanvas.Add(signImageSvg);
            imageCanvas.Close();

            Text teste = new Text("Documento assinado digitalmente via credenciais de usuário e senha. SafeSign - SGP")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);

            string fullname = person.FullName.ToUpper();
            Text teste2 = new Text($"Assinador por: {fullname} em {signature.SignatureDate}, ")
                .SetFont(robotoBold)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);
            Text test3 = new Text("conforme horário oficial de Brasília.")
                .SetFont(robotoRegular)
                .SetFontSize(11)
                .SetTextAlignment(TextAlignment.JUSTIFIED_ALL);

            Paragraph paragraph = new Paragraph().Add(teste).Add(teste2).Add(test3);
            Canvas TextCanvas = new(pdfCanvas, signatureTextRectangle);
            TextCanvas.Add(paragraph);
        }

        private static void SignLastPage(PdfDocument pdfDoc, int signaturesNumber, IList<SignatureDTO> signatures, IList<PersonDTO> persons, DocumentDTO documentDTO)
        {
            PdfPage page = pdfDoc.AddNewPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            // Insert last page title
            Rectangle titleRectangle = new(pageSize.GetWidth() / 9, 750, 595 - (pageSize.GetWidth() / 9) * 2, 36);
            pdfCanvas.SaveState()
               .SetFillColor(ColorConstants.LIGHT_GRAY)
               .Rectangle(titleRectangle)
               .Fill()
               .RestoreState();
            // Insert last page
            pdfCanvas.BeginText()
                .SetColor(blackColor, false)
                .MoveText(pageSize.GetWidth() / 2 - 130, pageSize.GetHeight() -80)
                .SetFontAndSize(robotoLight, 16)
                .ShowText("Documento Assinado Digitalmente")
                .EndText();


            // Define default margin distance between sections
            float marginDistance = ((pageSize.GetHeight() / 6) * 5);

            // Create the section of all signatures

            float rectangleYsize;

            switch (signaturesNumber)
            {
                case 1:
                    rectangleYsize = (float)-(signaturesNumber * 4 * pageSize.GetHeight() / 13);
                    break;

                case 2:
                    rectangleYsize = (float)-(signaturesNumber * 2.5 * pageSize.GetHeight() / 13);
                    break;

                case 3:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;

                case 4:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;

                default:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;
            }

            Rectangle signaturesRectangle = new(pageSize.GetWidth() / 9, (pageSize.GetHeight() / 6) * 5, 595 - (pageSize.GetWidth() / 9) * 2, rectangleYsize);
            pdfCanvas.SaveState()
             .SetFillColor(ColorConstants.LIGHT_GRAY)
             .Rectangle(signaturesRectangle)
             .Fill()
             .RestoreState();

            // Insert all signatures sections
            for (int i = 0; i < signaturesNumber; i++)
            {
                SignatureDTO signature = signatures[i];
                PersonDTO person = persons[i];

                InsertSignatures(pdfDoc, pdfCanvas, pageSize, marginDistance, signature, person);

                marginDistance -= 80;
            }

            // Insert QR Code section
            QrCode(pdfDoc, page, pdfCanvas, marginDistance, documentDTO);
        }
        /// <summary>
        /// Sign last page of documents coming from SGP
        /// </summary>
        /// <param name="pdfDoc"></param>
        /// <param name="signaturesNumber"></param>
        /// <param name="signatures"></param>
        /// <param name="persons"></param>
        /// <param name="document"></param>
        private static void SignLastPage(PdfDocument pdfDoc, int signaturesNumber, IList<Signature> signatures, IList<Person> persons, DAL.Models.Document document)
        {
            PdfPage page = pdfDoc.AddNewPage();
            Rectangle pageSize = page.GetPageSize();
            PdfCanvas pdfCanvas = new(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

            // Insert last page title
            Rectangle titleRectangle = new(pageSize.GetWidth() / 9, 750, 595 - (pageSize.GetWidth() / 9) * 2, 36);
            pdfCanvas.SaveState()
               .SetFillColor(ColorConstants.LIGHT_GRAY)
               .Rectangle(titleRectangle)
               .Fill()
               .RestoreState();
            // Insert last page
            pdfCanvas.BeginText()
                .SetColor(blackColor, false)
                .MoveText(pageSize.GetWidth() / 2 - 130, pageSize.GetHeight() - 80)
                .SetFontAndSize(robotoLight, 16)
                .ShowText("Documento Assinado Digitalmente")
                .EndText();


            // Define default margin distance between sections
            float marginDistance = ((pageSize.GetHeight() / 6) * 5);

            // Create the section of all signatures

            float rectangleYsize;

            switch (signaturesNumber)
            {
                case 1:
                    rectangleYsize = (float)-(signaturesNumber * 4 * pageSize.GetHeight() / 13);
                    break;

                case 2:
                    rectangleYsize = (float)-(signaturesNumber * 2.5 * pageSize.GetHeight() / 13);
                    break;

                case 3:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;

                case 4:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;

                default:
                    rectangleYsize = (float)-(signaturesNumber * 2 * pageSize.GetHeight() / 13);
                    break;
            }

            Rectangle signaturesRectangle = new(pageSize.GetWidth() / 9, (pageSize.GetHeight() / 6) * 5, 595 - (pageSize.GetWidth() / 9) * 2, rectangleYsize);
            pdfCanvas.SaveState()
             .SetFillColor(ColorConstants.LIGHT_GRAY)
             .Rectangle(signaturesRectangle)
             .Fill()
             .RestoreState();

            // Insert all signatures sections
            for (int i = 0; i < signaturesNumber; i++)
            {
                Signature signature = signatures[i];
                Person person = persons[i];

                InsertSignatures(pdfDoc, pdfCanvas, pageSize, marginDistance, signature, person);

                marginDistance -= 80;
            }

            // Insert QR Code section
            QrCode(pdfDoc, page, pdfCanvas, marginDistance, document);
        }

        public static void SignPDF(string originalPDFPath, string targetPDFPath, int signaturesNumber, DocumentDTO documentDTO, IList<SignatureDTO> signatures, IList<PersonDTO> persons)
        {
            PdfDocument pdfDocument = new(new PdfReader(Path.GetFullPath(originalPDFPath)), new PdfWriter(Path.GetFullPath(targetPDFPath)));
            Rectangle pageSize; PdfCanvas canvas;

            int pagesNumber = pdfDocument.GetNumberOfPages();
            for (int i = 1; i <= pagesNumber; i++)
            {
                // Insert footer signature
                SignPageFooter(pdfDocument, i, documentDTO);

                // Add new page with all sinatures detailed
                if (i == pagesNumber) SignLastPage(pdfDocument, signaturesNumber, signatures, persons, documentDTO);
            }

            pdfDocument.Close();
        }
        /// <summary>
        /// Sign documents coming from SGP
        /// </summary>
        /// <param name="originalPDFPath"></param>
        /// <param name="targetPDFPath"></param>
        /// <param name="signaturesNumber"></param>
        /// <param name="documentDTO"></param>
        /// <param name="signatures"></param>
        /// <param name="persons"></param>
        /// <param name="comingFromSGP"></param>
        public static void SignPDF(string originalPDFPath, string targetPDFPath, int signaturesNumber, DAL.Models.Document document, IList<Signature> signatures, IList<Person> persons, bool comingFromSGP = true)
        {
            PdfDocument pdfDocument = new(new PdfReader(Path.GetFullPath(originalPDFPath)), new PdfWriter(Path.GetFullPath(targetPDFPath)));
            Rectangle pageSize; PdfCanvas canvas;

            int pagesNumber = pdfDocument.GetNumberOfPages();
            for (int i = 1; i <= pagesNumber; i++)
            {
                // Insert footer signature
                SignPageFooter(pdfDocument, i, document);

                // Add new page with all sinatures detailed
                if (i == pagesNumber) SignLastPage(pdfDocument, signaturesNumber, signatures, persons, document);
            }

            pdfDocument.Close();
        }
    }

}

