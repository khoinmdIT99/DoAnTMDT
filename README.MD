Môi trường yêu cầu:
 * .NET Core SDK 3.1 https://dotnet.microsoft.com/download/dotnet-core/3.1
 * Visual studio 2019 https://visualstudio.microsoft.com/vs/
 * Entity Framework Core tools https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet


Khởi tạo database:
 * Chạy project DatabaseTools
 * Click *Init Data*
 * Nhập tên trang web, username, password, email của tài khoản quản trị
 * Bấm create để khởi tạo data mẫu

Update database
 * mở project *Domain.Application*
 * Thêm mới class vào folder *Entities*, Kế thừa class từ class *BaseEntity* để thêm các thông tin create, update
 * Thêm khai báo dbset vào *ApplicationDBContext*
 * Mở cmd tại foler */DatabaseTools*
 * Nhập lệnh *dotnet ef migrations add <tên migration>* để tạo migration
 * chạy lệnh *dotnet ef database update* để update database

Chạy ứng dụng
 * Mở cmd tại foler */Web*
 * Nhập lệnh *dotnet watch run* để khởi chạy server
 * Mở trình duyệt web nhập link *http://localhost:5000*
 * Đăng nhập bằng tài khoản *admin/123456*