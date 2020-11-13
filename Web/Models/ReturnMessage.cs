using Domain.Shop.Dto.CartProduct;
using Domain.Shop.Dto.Products;

namespace Web.Models
{
    public class ReturnMessage
    {
        public string Message { get; set; }
        public int State { get; set; }
        public int Count { get; set; }
        public CartProductViewModel Product { get; set; }

        public ReturnMessage()
        {
            State = 0;
        }

        public void setMessage(string message, int count, CartProductViewModel product)
        {
            State = 2;
            Count = count;
            Message = message;
            Product = product;
        }

        public void SetErrorMessage(string message)
        {
            State = -1;
            Message = message;
        }

        public void SetSuccessMessage(string message, CartProductViewModel product)
        {
            State = 1;
            Message = message;
            Product = product;
        }

        public void setMessageFirst(string message, int count, CartProductViewModel product)
        {
            State = 3;
            Count = count;
            Message = message;
            Product = product;
        }
    }
}
