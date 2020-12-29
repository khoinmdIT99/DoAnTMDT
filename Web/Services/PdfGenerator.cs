using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Domain.Shop.Dto.Order;
using Domain.Shop.Entities;
using Infrastructure.Web;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace Web.Services
{
    public class PdfGenerator
    {
        public byte[] CreatePdf(OrderViewModel order,List<CartProduct> cList, string email)
        {
            var html = BuildEmailContent(order,cList, email);

            byte[] bytes = null;

            StringReader sr = new StringReader(html.ToString());

            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                htmlparser.Parse(sr);
                pdfDoc.Close();

                bytes = memoryStream.ToArray();
                memoryStream.Close();
            }
            return bytes;
        }

        protected StringBuilder BuildEmailContent(OrderViewModel order,List<CartProduct> cList, string email)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<header class='clearfix'>");
            sb.Append("<h2>Hóa đơn mua hàng</h2>");
            sb.Append("<br/>");
            sb.Append("<div id='company' class='clearfix'>");
            sb.Append("<div></div>");
            sb.Append("<div></div>");
            sb.Append("<br/>");
            sb.Append("<div><a href='mailto:khoitedu99@gmail.com'>khoitedu99@gmail.com</a></div>");
            sb.Append("<br/>");
            sb.Append("</div>");
            sb.Append("<div id='project'>");
            sb.Append($"<div><span>Client: </span>{order.FullName}</div>");
            sb.Append($"<div><span>Address: </span>{order.Address}</div>");
            sb.Append($"<div><span>Date: </span>{DateTime.UtcNow.ToLocalTime().ToLongDateString()}</div>");
            sb.Append("</div>");
            sb.Append("</header>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append("<main>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th class='service'>Book</th>");
            sb.Append("<th>Price</th>");
            sb.Append("<th>Quantity</th>");
            sb.Append("<th>Total</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            long tong = 0;
            foreach (var item in cList)
            {
                sb.Append("<tr>");
                sb.Append($"<td class='service'>{item.Product.ProductName}</td>");
                sb.Append($"<td class='unit'>{CurrencyHelper.ConvertNumberToVietNamCurrency(item.Price.GetValueOrDefault().ToString())}</td>");
                sb.Append($"<td class='qty'>{item.Quantity}</td>");
                sb.Append($"<td class='total'>{CurrencyHelper.ConvertNumberToVietNamCurrency((item.Quantity*item.Price.GetValueOrDefault()).ToString())}</td>");
                sb.Append("</tr>");
                tong += (item.Quantity * item.Price.GetValueOrDefault());
            }
            sb.Append("<tr>");
            sb.Append("<br/>");
            sb.Append("<br/>");
            sb.Append($"<td><strong>Tổng cộng: {CurrencyHelper.ConvertNumberToVietNamCurrency(tong.ToString())} €</strong></td>");
            sb.Append($"<td></td>");
            sb.Append("</tr>");


            sb.Append("<tr>");
            sb.Append("<td></td>");
            sb.Append($"<td><strong></strong></td>");
            sb.Append("</tr>");
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("<div id='notices'>");

            sb.Append("</main>");
            sb.Append("<footer>");
            sb.Append("</footer>");

            return sb;
        }
    }
}
