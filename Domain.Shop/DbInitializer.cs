using Domain.Shop.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Shop
{
    public static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Province>().HasData(
                 new Province()
                 {
                     Id = "1",
                     Name = "Hà Nội"
                 },
                 new Province()
                 {
                     Id = "2",
                     Name = "Đà Nẵng"
                 },
                 new Province()
                 {
                     Id = "3",
                     Name = "Hồ Chí Minh"
                 }
              );
            modelBuilder.Entity<District>().HasData(
                new District()
                {
                    Id = "1",
                    Name = "Ba Đình",
                    ProvinceId = "1"

                },
                new District()
                {
                    Id = "2",
                    Name = "Bắc Từ Liêm",
                    ProvinceId = "1"
                },
                new District()
                {
                    Id = "3",
                    Name = "Cầu Giấy",
                    ProvinceId = "1"
                },
                new District()
                {
                    Id = "4",
                    Name = "Đống Đa",
                    ProvinceId = "1"
                },
                new District()
                {
                    Id = "5",
                    Name = "Hà Đông",
                    ProvinceId = "1"
                },
                new District()
                {
                    Id = "6",
                    Name = "Hải Châu",
                    ProvinceId = "2"
                },
                new District()
                {
                    Id = "7",
                    Name = "Cẩm Lệ",
                    ProvinceId = "2"
                },
                new District()
                {
                    Id = "8",
                    Name = "Liên Chiểu",
                    ProvinceId = "2"
                },
                new District()
                {
                    Id = "9",
                    Name = "Ngũ Hành Sơn",
                    ProvinceId = "2"
                },
                new District()
                {
                    Id = "10",
                    Name = "Sơn Trà",
                    ProvinceId = "2"
                },
                 new District()
                 {
                     Id = "11",
                     Name = "Quận 1",
                     ProvinceId = "3"
                 },
                  new District()
                  {
                      Id = "12",
                      Name = "Quận 2",
                      ProvinceId = "3"
                  },
                   new District()
                   {
                       Id = "13",
                       Name = "Quận 3",
                       ProvinceId = "3"
                   },
                    new District()
                    {
                        Id = "14",
                        Name = "Quận 4",
                        ProvinceId = "3"
                    },
                     new District()
                     {
                         Id = "15",
                         Name = "Quận 5",
                         ProvinceId = "3"
                     }
             ) ;
        }
    }
}
