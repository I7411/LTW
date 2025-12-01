USE master
GO

-- 1. KIỂM TRA VÀ XÓA DB CŨ NẾU CÓ ĐỂ TẠO MỚI SẠCH SẼ
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'QUANLYTHUVIEN')
BEGIN
    ALTER DATABASE QUANLYTHUVIEN SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE QUANLYTHUVIEN
END
GO

CREATE DATABASE QUANLYTHUVIEN
GO

USE QUANLYTHUVIEN
GO

-- =============================================
-- 2. TẠO CÁC BẢNG (TABLES)
-- =============================================

-- Tạo bảng Độc Giả
CREATE TABLE DOCGIA (
    MATHANHVIEN char(13) NOT NULL,
    TENTV nvarchar(50) NOT NULL,
    NGSINH datetime NULL,
    GIOITINH nvarchar(50) NULL,
    NGHENGHIEP nvarchar(50) NULL,
    SODIENTHOAI char(13) NULL,
    TAIKHOAN char(25) NOT NULL,
    MATKHAU char(20) NOT NULL,
    VAITRO nvarchar(50) NULL,
    DIACHI nvarchar(60) NULL,
    TENTRUONG nvarchar(60) NULL,
    KHOAHOC char(30) NULL,
    EMAIL char(30) NULL,
    GHICHU nvarchar(100) NULL,
    CONSTRAINT PK_DOCGIA PRIMARY KEY (MATHANHVIEN)
);

-- Tạo bảng Thẻ Bạn Đọc
CREATE TABLE THEBANDOC (
    MASOTHE char(10) NOT NULL,
    MATHANHVIEN char(13) NOT NULL,
    TINHTRANGTHE nvarchar(50) NOT NULL,
    HANSUDUNG nvarchar(50) NOT NULL,
    CONSTRAINT PK_THEBANDOC PRIMARY KEY (MASOTHE)
);

-- Tạo bảng Nhân Viên
CREATE TABLE NHANVIEN (
    MANV char(10) NOT NULL,
    NQL_NV char(10) NULL,
    TENNV nvarchar(50) NOT NULL,
    NGSINH datetime NULL,
    GIOITINH nvarchar(10) NULL,
    EMAIL char(30) NULL,
    SODIENTHOAI char(13) NULL,
    VAITRO nvarchar(50) NOT NULL,
    DIACHI nvarchar(60) NOT NULL,
    TAIKHOAN char(25) NOT NULL,
    MATKHAU char(20) NOT NULL,
    CONSTRAINT PK_NHANVIEN PRIMARY KEY (MANV)
);

-- Tạo bảng Tài Liệu (Đã bao gồm 3 cột mới: HINHANH, ANHIEN, LOAIBIA)
CREATE TABLE TAILIEU (
    MATAILIEU char(10) NOT NULL,
    TENSACH nvarchar(50) NOT NULL,
    NGONNGU nvarchar(50) NULL,
    TINHTRANG nvarchar(50) NULL,
    PHIMUON real NULL,
    THELOAI nvarchar(30) NULL,
    TENTACGIA nvarchar(50) NULL,
    NXB nvarchar(40) NULL,
    SOLUONG int NULL,
    HINHANH nvarchar(255) NULL,  -- Cột mới
    ANHIEN bit DEFAULT 1,         -- Cột mới
    LOAIBIA nvarchar(50) NULL,    -- Cột mới
    CONSTRAINT PK_TAILIEU PRIMARY KEY (MATAILIEU)
);

-- Tạo bảng Phiếu Mượn
CREATE TABLE PHIEUMUON (
    MAPHIEUMUON char(10) NOT NULL,
    MANV char(10) NULL,
    MASOTHE char(10) NULL,
    NGAYMUON datetime NOT NULL,
    NGAYTRA datetime NOT NULL,
    CONSTRAINT PK_PHIEUMUON PRIMARY KEY (MAPHIEUMUON)
);

-- Tạo bảng Chi Tiết Phiếu Mượn
CREATE TABLE CHITIETPHIEUMUON (
    MAPHIEUMUON char(10) NOT NULL,
    MATAILIEU char(10) NOT NULL,
    PHIMUON real NOT NULL,
    SOLUONG int NOT NULL,
    CONSTRAINT PK_CHITIETPHIEUMUON PRIMARY KEY (MAPHIEUMUON, MATAILIEU)
);

-- Tạo bảng Đặt Mượn Trước
CREATE TABLE DATMUONTRUOC (
    MAMUON char(15) NOT NULL,
    NGAYDAT datetime NOT NULL,
    TRANGTHAI nvarchar(30) NULL,
    HANLAYSACH datetime NULL,
    MASOTHE char(10) NULL,
    CONSTRAINT PK_DATMUONTRUOC PRIMARY KEY (MAMUON)
);

-- Tạo bảng Chi Tiết Đặt Trước
CREATE TABLE CHITIETDATTRUOC (
    MAMUON char(15) NOT NULL,
    MATAILIEU char(10) NOT NULL,
    PHIMUON real NULL,
    SOLUONG int NULL,
    CONSTRAINT PK_CHITIETDATTRUOC PRIMARY KEY (MAMUON, MATAILIEU)
);

-- Tạo bảng Phòng Học
CREATE TABLE PHONGHOC (
    MAPHONG char(10) NOT NULL,
    TENPHONG nvarchar(50) NOT NULL,
    SUCCHUA int NOT NULL,
    TRANGBI nvarchar(25) NULL,
    TRANGTHAI nvarchar(50) NULL,
    CONSTRAINT PK_PHONGHOC PRIMARY KEY (MAPHONG)
);

-- Tạo bảng Đặt Phòng
CREATE TABLE DATPHONG (
    MASOTHE char(10) NULL,
    MADATPHONG char(10) NOT NULL,
    NGAYDAT datetime NOT NULL,
    THOIGIANBATDAU datetime NOT NULL,
    THOIGIANKETTHUC datetime NOT NULL,
    TRANGTHAI nvarchar(30) NOT NULL,
    YEUCAU nvarchar(50) NULL,
    MAPHONG char(10) NULL,
    CONSTRAINT PK_DATPHONG PRIMARY KEY (MADATPHONG)
);

-- Tạo bảng Nhận Phòng
CREATE TABLE NHANPHONG (
    MADATPHONG char(10) NOT NULL,
    MAPHONG char(10) NOT NULL,
    CONSTRAINT PK_NHANPHONG PRIMARY KEY (MADATPHONG, MAPHONG)
);

-- Tạo bảng Phiếu Phạt
CREATE TABLE PHIEUPHAT (
    MAPHIEUPHAT char(10) NOT NULL,
    MANV char(10) NOT NULL,
    MAPHIEUMUON char(10) NOT NULL,
    PHIPHAT real NOT NULL,
    LYDOPHAT nvarchar(300) NULL,
    SACHTHAYTHE nvarchar(50) NULL,
    TINHTRANGTAILIEU nvarchar(50) NOT NULL,
    LYDO nvarchar(300) NULL,
    NGTAO datetime NULL,
    NGUOILAP nvarchar(50) NULL,
    SOLAN int NULL,
    CONSTRAINT PK_PHIEUPHAT PRIMARY KEY (MAPHIEUPHAT)
);

-- Tạo bảng Gia Hạn Tài Liệu
CREATE TABLE GIAHANTAILIEU (
    MAGIAHAN char(10) NOT NULL,
    NGAYGIAHAN datetime NOT NULL,
    HANMOI datetime NOT NULL,
    TRANGTHAI nvarchar(50) NOT NULL,
    MAPHIEUMUON char(10) NULL,
    YEUCAU nvarchar(50) NULL,
    CONSTRAINT PK_GIAHANTAILIEU PRIMARY KEY (MAGIAHAN)
);

-- Tạo bảng Xử Lý Gia Hạn
CREATE TABLE XULYGIAHAN (
    MAPHIEUMUON char(10) NOT NULL,
    MAGIAHAN char(10) NOT NULL,
    CONSTRAINT PK_XULYGIAHAN PRIMARY KEY (MAPHIEUMUON, MAGIAHAN)
);

-- Tạo bảng Mua Tài Liệu Mới
CREATE TABLE MUATAILIEUMOI (
    MAMUA char(10) NOT NULL,
    MANV char(10) NOT NULL,
    NGAYMUA datetime NOT NULL,
    NHAXUATBAN nvarchar(50) NOT NULL,
    TRANGTHAI nvarchar(50) NOT NULL,
    CONSTRAINT PK_MUATAILIEUMOI PRIMARY KEY (MAMUA)
);

-- Tạo bảng Cập Nhật Thông Tin
CREATE TABLE CAPNHATTHONGTIN (
    MATAILIEU char(10) NOT NULL,
    MAMUA char(10) NOT NULL,
    SOLUONG int NOT NULL,
    CONSTRAINT PK_CAPNHATTHONGTIN PRIMARY KEY (MATAILIEU, MAMUA)
);

-- Tạo bảng Quản Lý Tài Liệu
CREATE TABLE QUANLYTAILIEU (
    MANV char(10) NOT NULL,
    MATAILIEU char(10) NOT NULL,
    CHIPHI real NOT NULL,
    CONSTRAINT PK_QUANLYTAILIEU PRIMARY KEY (MANV, MATAILIEU)
);

-- Tạo bảng Thanh Lý Tài Liệu
CREATE TABLE THANHLYTAILIEU (
    MAHOADON char(10) NOT NULL,
    MANV char(10) NOT NULL,
    NGAYTHANHLY datetime NOT NULL,
    TRANGTHAI nvarchar(50) NOT NULL,
    GHICHU nvarchar(100) NULL,
    CONSTRAINT PK_THANHLYTAILIEU PRIMARY KEY (MAHOADON)
);

-- Tạo bảng sysdiagrams
CREATE TABLE sysdiagrams (
    name nvarchar(128) NOT NULL,
    principal_id int NOT NULL,
    diagram_id int IDENTITY(1,1) NOT NULL,
    version int NULL,
    definition varbinary(max) NULL,
    CONSTRAINT PK_sysdiagrams PRIMARY KEY (diagram_id)
);


-- =============================================
-- 3. INSERT DỮ LIỆU (Đã gộp dữ liệu CŨ và MỚI)
-- =============================================

-- BẢNG ĐỘC GIẢ
INSERT INTO DOCGIA (MATHANHVIEN, TENTV, NGSINH, GIOITINH, NGHENGHIEP, SODIENTHOAI, TAIKHOAN, MATKHAU, VAITRO, DIACHI, TENTRUONG, KHOAHOC, EMAIL, GHICHU) VALUES
('TV001', N'Nguyễn Minh An', '1999-05-10', N'Nam', N'Quản trị hệ thống', '0905123456', 'minhan', '123456', N'Docgia', N'Q5, TP.HCM', N'ĐH KHTN', 'K17', 'admin@gmail.com', N'Quản trị viên'),
('TV002', N'Lê Thị Hoa', '2001-08-22', N'Nữ', N'Thủ thư thư viện', '0987987654', 'hoadephai', 'abc123', N'Docgia', N'Tân Bình, TP.HCM', N'ĐH Sư phạm', 'K15', 'thu_thu@gmail.com', N'Phụ trách mượn trả'),
('TV003', N'Trần Quốc Bảo', '2003-03-14', N'Nam', N'Sinh viên', '0977123456', 'baotran', 'pass789', N'Độc giả', N'Q10, TP.HCM', N'THPT LHP', '12A1', 'bao@gmail.com', N'Bạn đọc thường xuyên'),
('TV004', N'Trần Văn Thành', '2000-01-15', N'Nam', N'Sinh viên', '0912345678', 'thanhvan', 'pass123', N'Độc giả', N'Thủ Đức, TP.HCM', N'ĐH Bách Khoa', 'K2018', 'thanh@gmail.com', N''),
('TV005', N'Nguyễn Thị Bích', '2002-05-20', N'Nữ', N'Sinh viên', '0912345679', 'bichnguyen', 'pass123', N'Độc giả', N'Q1, TP.HCM', N'ĐH Kinh Tế', 'K45', 'bich@gmail.com', N''),
('TV006', N'Lê Hoàng Nam', '1998-12-10', N'Nam', N'Giảng viên', '0912345680', 'namhoang', 'pass123', N'Độc giả', N'Q3, TP.HCM', N'ĐH KHTN', '', 'nam.gv@gmail.com', N'Khoa CNTT'),
('TV007', N'Phạm Thu Hà', '2003-07-07', N'Nữ', N'Sinh viên', '0912345681', 'hapham', 'pass123', N'Độc giả', N'Gò Vấp, TP.HCM', N'ĐH Công Nghiệp', 'K16', 'ha@gmail.com', N''),
('TV008', N'Vũ Đức Minh', '2001-09-09', N'Nam', N'Sinh viên', '0912345682', 'minhvu', 'pass123', N'Độc giả', N'Bình Thạnh, TP.HCM', N'HUTECH', 'K19', 'minh@gmail.com', N''),
('TV009', N'Đỗ Thị Lan', '2000-03-30', N'Nữ', N'Nghiên cứu sinh', '0912345683', 'lando', 'pass123', N'Độc giả', N'Q7, TP.HCM', N'ĐH Tôn Đức Thắng', '', 'lan@gmail.com', N''),
('TV010', N'Bùi Văn Hùng', '1999-11-11', N'Nam', N'Sinh viên', '0912345684', 'hungbui', 'pass123', N'Độc giả', N'Q12, TP.HCM', N'ĐH GTVT', 'K60', 'hung@gmail.com', N'');

-- BẢNG THẺ BẠN ĐỌC
INSERT INTO THEBANDOC VALUES
('THE001', 'TV001', N'Hoạt động', N'2026-12-31'),
('THE002', 'TV002', N'Hoạt động', N'2026-12-31'),
('THE003', 'TV003', N'Bị khóa', N'2025-06-01'),
('THE004', 'TV004', N'Hoạt động', N'2026-01-01'),
('THE005', 'TV005', N'Hoạt động', N'2026-06-30'),
('THE006', 'TV006', N'Hoạt động', N'2030-12-31'),
('THE007', 'TV007', N'Bị khóa', N'2025-01-01'),
('THE008', 'TV008', N'Hoạt động', N'2026-05-15'),
('THE009', 'TV009', N'Hoạt động', N'2027-01-01'),
('THE010', 'TV010', N'Hoạt động', N'2026-08-20');

-- BẢNG NHÂN VIÊN
INSERT INTO NHANVIEN VALUES
('NV001', NULL, N'Nguyễn Minh Anh', '1999-05-10', N'Nam', 'admin@gmail.com', '0905123456', N'Admin', N'Q5, TP.HCM', 'adminan', '123456'),
('NV002', 'NV001', N'Lê Thị Hoa', '2001-08-22', N'Nữ', 'thu_thu@gmail.com', '0987987654', N'Thủ thư', N'Tân Bình, TP.HCM', 'hoale', 'abc123'),
('NV003', 'NV001', N'Phạm Quốc Việt', '1990-11-09', N'Nam', 'vietpq@lib.vn', '0987654321', N'Nhân viên', N'TP.HCM', 'vietpq', '789123');

-- BẢNG TÀI LIỆU
INSERT INTO TAILIEU (MATAILIEU, TENSACH, NGONNGU, TINHTRANG, PHIMUON, THELOAI, TENTACGIA, NXB, SOLUONG, HINHANH, ANHIEN, LOAIBIA) 
VALUES
('S001', N'Lập trình Python cơ bản', N'Tiếng Việt', N'Còn', 5000, N'Công nghệ', N'Nguyễn Văn Cường', N'NXB Trẻ', 10, 'python.jpg', 1, N'Bìa mềm'),
('S002', N'Cấu trúc dữ liệu & Giải thuật', N'Tiếng Việt', N'Còn', 6000, N'Tin học', N'Lê Minh Quân', N'NXB Giáo Dục', 5, 'ctdl.jpg', 1, N'Bìa mềm'),
('S003', N'Trí tuệ nhân tạo', N'Tiếng Anh', N'Hết', 8000, N'AI', N'John McCarthy', N'Oxford Press', 0, 'ai.jpg', 1, N'Bìa cứng'),
('S004', N'Clean Code', N'Tiếng Anh', N'Còn', 10000, N'Công nghệ', N'Robert C. Martin', N'Prentice Hall', 5, 'cleancode.jpg', 1, N'Bìa cứng'),
('S005', N'Đắc Nhân Tâm', N'Tiếng Việt', N'Còn', 3000, N'Kỹ năng sống', N'Dale Carnegie', N'NXB Tổng Hợp', 20, 'dacnhantam.jpg', 1, N'Bìa mềm'),
('S006', N'Nhà Giả Kim', N'Tiếng Việt', N'Còn', 3000, N'Văn học', N'Paulo Coelho', N'NXB Văn Học', 15, 'nhagiakim.jpg', 1, N'Bìa mềm'),
('S007', N'SQL Server 2019', N'Tiếng Anh', N'Còn', 8000, N'Tin học', N'Microsoft Press', N'Microsoft', 3, 'sql2019.jpg', 1, N'Bìa cứng'),
('S008', N'Kinh tế vĩ mô', N'Tiếng Việt', N'Còn', 4000, N'Kinh tế', N'Nhiều tác giả', N'NXB Kinh Tế', 50, 'kinhtevimo.jpg', 1, N'Bìa mềm'),
('S009', N'Sherlock Holmes', N'Tiếng Việt', N'Còn', 5000, N'Trinh thám', N'Conan Doyle', N'NXB Văn Học', 8, 'sherlock.jpg', 1, N'Bìa cứng'),
('S010', N'Thiết kế Web với HTML5', N'Tiếng Việt', N'Còn', 6000, N'Công nghệ', N'Nguyễn Anh Tuấn', N'NXB Giáo Dục', 10, 'html5.jpg', 1, N'Bìa mềm'),
('S011', N'Lược sử loài người', N'Tiếng Việt', N'Hết', 7000, N'Lịch sử', N'Yuval Noah Harari', N'NXB Tri Thức', 0, 'sapiens.jpg', 1, N'Bìa mềm'),
('S012', N'Mạng máy tính căn bản', N'Tiếng Việt', N'Còn', 5500, N'Tin học', N'Lê Hoa', N'NXB Bách Khoa', 12, 'mangmaytinh.jpg', 1, N'Bìa mềm'),
('S013', N'Harry Potter 1', N'Tiếng Anh', N'Còn', 9000, N'Văn học', N'J.K. Rowling', N'Bloomsbury', 7, 'hp1.jpg', 1, N'Bìa cứng');

-- BẢNG PHIẾU MƯỢN
INSERT INTO PHIEUMUON (MAPHIEUMUON, MANV, MASOTHE, NGAYMUON, NGAYTRA) VALUES
('PM001', 'NV002', 'THE001', '2025-11-01', '2025-11-15'),
('PM002', 'NV003', 'THE002', '2025-11-05', '2025-11-20'),
('PM003', 'NV002', 'THE004', '2025-11-20', '2025-11-27'),
('PM004', 'NV003', 'THE005', '2025-11-21', '2025-12-05'),
('PM005', 'NV002', 'THE006', '2025-11-22', '2025-11-29'),
('PM006', 'NV002', 'THE008', '2025-11-23', '2025-11-30'),
('PM007', 'NV003', 'THE009', '2025-11-24', '2025-12-08'),
('PM008', 'NV002', 'THE001', '2025-11-25', '2025-12-01');

-- BẢNG CHI TIẾT PHIẾU MƯỢN
INSERT INTO CHITIETPHIEUMUON (MAPHIEUMUON, MATAILIEU, PHIMUON, SOLUONG) VALUES
('PM001', 'S001', 5000, 1),
('PM001', 'S002', 6000, 1),
('PM002', 'S001', 5000, 1),
('PM003', 'S004', 10000, 1),
('PM003', 'S007', 8000, 1),
('PM004', 'S005', 3000, 1),
('PM005', 'S010', 6000, 1),
('PM005', 'S012', 5500, 1),
('PM006', 'S006', 3000, 1),
('PM007', 'S009', 5000, 1),
('PM007', 'S013', 9000, 1),
('PM008', 'S008', 4000, 2);

-- BẢNG ĐẶT MƯỢN TRƯỚC
INSERT INTO DATMUONTRUOC VALUES
('DM001', '2025-10-20', N'Chờ lấy sách', '2025-10-25', 'THE003'),
('DM002', '2025-10-28', N'Đã hủy', NULL, 'THE001');

-- BẢNG CHI TIẾT ĐẶT TRƯỚC
INSERT INTO CHITIETDATTRUOC VALUES
('DM001', 'S003', 8000, 1),
('DM002', 'S002', 6000, 1);

-- BẢNG PHÒNG HỌC
INSERT INTO PHONGHOC (MAPHONG, TENPHONG, SUCCHUA, TRANGBI, TRANGTHAI) VALUES
('P001', N'Phòng đọc 1', 50, N'Máy lạnh', N'Hoạt động'),
('P002', N'Phòng đọc 2', 40, N'Máy chiếu', N'Bảo trì'),
('P003', N'Phòng Lab 1', 30, N'30 Máy tính', N'Hoạt động'),
('P004', N'Phòng Thảo luận', 10, N'Bảng trắng', N'Hoạt động');

-- BẢNG ĐẶT PHÒNG
INSERT INTO DATPHONG (MASOTHE, MADATPHONG, NGAYDAT, THOIGIANBATDAU, THOIGIANKETTHUC, TRANGTHAI, YEUCAU, MAPHONG) VALUES
('THE001', 'DP001', '2025-11-10', '2025-11-12 08:00', '2025-11-12 11:00', N'Đã duyệt', N'Mượn học nhóm', 'P001'),
('THE002', 'DP002', '2025-11-11', '2025-11-15 13:00', '2025-11-15 17:00', N'Chờ duyệt', N'Hội thảo nhỏ', 'P002'),
('THE004', 'DP003', '2025-11-25', '2025-11-26 08:00', '2025-11-26 10:00', N'Đã duyệt', N'Thực hành code', 'P003'),
('THE006', 'DP004', '2025-11-26', '2025-11-27 14:00', '2025-11-27 16:00', N'Chờ duyệt', N'Họp nhóm nghiên cứu', 'P004');

-- BẢNG NHẬN PHÒNG
INSERT INTO NHANPHONG VALUES
('DP001', 'P001'),
('DP002', 'P002'),
('DP003', 'P003');

-- BẢNG PHIẾU PHẠT
INSERT INTO PHIEUPHAT (MAPHIEUPHAT, MANV, MAPHIEUMUON, PHIPHAT, LYDOPHAT, SACHTHAYTHE, TINHTRANGTAILIEU, LYDO, NGTAO, NGUOILAP, SOLAN) VALUES
('PP001', 'NV003', 'PM001', 15000, N'Trễ hạn', N'', N'Hư nhẹ', N'Độc giả trả trễ 3 ngày', '2025-11-20', N'Mai', 1),
('PP002', 'NV002', 'PM002', 50000, N'Mất sách', N'Thay thế bằng bản khác', N'Mất hoàn toàn', N'Độc giả làm mất sách', '2025-11-22', N'Hùng', 1),
('PP003', 'NV002', 'PM003', 20000, N'Làm rách bìa', N'', N'Hư hỏng nhẹ', N'Độc giả làm rách trang bìa', '2025-11-27', N'Hoa', 1);

-- BẢNG GIA HẠN TÀI LIỆU
INSERT INTO GIAHANTAILIEU VALUES
('GH001', '2025-11-14', '2025-11-30', N'Đã duyệt', 'PM001', N'Cần thêm thời gian đọc'),
('GH002', '2025-11-18', '2025-12-01', N'Chờ duyệt', 'PM002', N'Muốn gia hạn do bận');

-- BẢNG XỬ LÝ GIA HẠN
INSERT INTO XULYGIAHAN VALUES
('PM001', 'GH001'),
('PM002', 'GH002');

-- BẢNG MUA TÀI LIỆU MỚI
INSERT INTO MUATAILIEUMOI VALUES
('MM001', 'NV001', '2025-10-01', N'NXB Trẻ', N'Đã nhập kho'),
('MM002', 'NV002', '2025-09-15', N'NXB Giáo Dục', N'Đang giao hàng');

-- BẢNG CẬP NHẬT THÔNG TIN
INSERT INTO CAPNHATTHONGTIN VALUES
('S001', 'MM001', 5),
('S003', 'MM002', 2);

-- BẢNG QUẢN LÝ TÀI LIỆU
INSERT INTO QUANLYTAILIEU VALUES
('NV001', 'S001', 500000),
('NV002', 'S002', 400000);

-- BẢNG THANH LÝ TÀI LIỆU
INSERT INTO THANHLYTAILIEU VALUES
('TL001', 'NV003', '2025-10-10', N'Đã thanh lý', N'Sách cũ, hỏng'),
('TL002', 'NV002', '2025-11-01', N'Chờ phê duyệt', N'Sách lỗi in');

-- =============================================
-- 4. TẠO RÀNG BUỘC KHÓA NGOẠI (FOREIGN KEYS)
-- =============================================

-- Liên kết Thẻ Bạn Đọc -> Độc Giả
ALTER TABLE THEBANDOC ADD CONSTRAINT FK_THEBANDO_SOHUU_DOCGIA FOREIGN KEY (MATHANHVIEN) REFERENCES DOCGIA (MATHANHVIEN);
-- Liên kết Phiếu Mượn -> Nhân Viên
ALTER TABLE PHIEUMUON ADD CONSTRAINT FK_PHIEUMUO_LAPPHIEUM_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV);
-- Liên kết Phiếu Mượn -> Thẻ Bạn Đọc
ALTER TABLE PHIEUMUON ADD CONSTRAINT FK_PHIEUMUO_MUONTAILI_THEBANDO FOREIGN KEY (MASOTHE) REFERENCES THEBANDOC (MASOTHE);
-- Liên kết Chi Tiết Phiếu Mượn -> Phiếu Mượn
ALTER TABLE CHITIETPHIEUMUON ADD CONSTRAINT FK_CHITIETP_CHITIETPH_PHIEUMUO FOREIGN KEY (MAPHIEUMUON) REFERENCES PHIEUMUON (MAPHIEUMUON);
-- Liên kết Chi Tiết Phiếu Mượn -> Tài Liệu
ALTER TABLE CHITIETPHIEUMUON ADD CONSTRAINT FK_CHITIETP_CHITIETPH_TAILIEU FOREIGN KEY (MATAILIEU) REFERENCES TAILIEU (MATAILIEU);
-- Liên kết Đặt Mượn Trước -> Thẻ Bạn Đọc
ALTER TABLE DATMUONTRUOC ADD CONSTRAINT FK_DATMUONTRUOC_THEBANDOC FOREIGN KEY (MASOTHE) REFERENCES THEBANDOC (MASOTHE);
-- Liên kết Chi Tiết Đặt Trước -> Đặt Mượn Trước
ALTER TABLE CHITIETDATTRUOC ADD CONSTRAINT FK_CHITIETD_CHITIETDA_DATMUONT FOREIGN KEY (MAMUON) REFERENCES DATMUONTRUOC (MAMUON);
-- Liên kết Chi Tiết Đặt Trước -> Tài Liệu
ALTER TABLE CHITIETDATTRUOC ADD CONSTRAINT FK_CHITIETD_CHITIETDA_TAILIEU FOREIGN KEY (MATAILIEU) REFERENCES TAILIEU (MATAILIEU);
-- Liên kết Đặt Phòng -> Thẻ Bạn Đọc
ALTER TABLE DATPHONG ADD CONSTRAINT FK_DATPHONG_THEBANDOC FOREIGN KEY (MASOTHE) REFERENCES THEBANDOC (MASOTHE);
-- Liên kết Đặt Phòng -> Phòng Học
ALTER TABLE DATPHONG ADD CONSTRAINT FK_DATPHONG_PHONGHOC FOREIGN KEY (MAPHONG) REFERENCES PHONGHOC (MAPHONG);
-- Liên kết Nhận Phòng -> Đặt Phòng
ALTER TABLE NHANPHONG ADD CONSTRAINT FK_NHANPHON_NHANPHONG_DATPHONG FOREIGN KEY (MADATPHONG) REFERENCES DATPHONG (MADATPHONG);
-- Liên kết Nhận Phòng -> Phòng Học
ALTER TABLE NHANPHONG ADD CONSTRAINT FK_NHANPHON_NHANPHONG_PHONGHOC FOREIGN KEY (MAPHONG) REFERENCES PHONGHOC (MAPHONG);
-- Liên kết Phiếu Phạt -> Nhân Viên
ALTER TABLE PHIEUPHAT ADD CONSTRAINT FK_PHIEUPHA_LAPPHIEUP_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV);
-- Liên kết Phiếu Phạt -> Phiếu Mượn
ALTER TABLE PHIEUPHAT ADD CONSTRAINT FK_PHIEUPHA_XULYPHAT_PHIEUMUO FOREIGN KEY (MAPHIEUMUON) REFERENCES PHIEUMUON (MAPHIEUMUON);
-- Liên kết Gia Hạn Tài Liệu -> Phiếu Mượn
ALTER TABLE GIAHANTAILIEU ADD CONSTRAINT FK_GIAHANTAILIEU_PHIEUMUON1 FOREIGN KEY (MAPHIEUMUON) REFERENCES PHIEUMUON (MAPHIEUMUON);
-- Liên kết Xử Lý Gia Hạn -> Phiếu Mượn
ALTER TABLE XULYGIAHAN ADD CONSTRAINT FK_XULYGIAH_XULYGIAHA_PHIEUMUO FOREIGN KEY (MAPHIEUMUON) REFERENCES PHIEUMUON (MAPHIEUMUON);
-- Liên kết Xử Lý Gia Hạn -> Gia Hạn Tài Liệu
ALTER TABLE XULYGIAHAN ADD CONSTRAINT FK_XULYGIAH_XULYGIAHA_GIAHANTA FOREIGN KEY (MAGIAHAN) REFERENCES GIAHANTAILIEU (MAGIAHAN);
-- Liên kết Mua Tài Liệu Mới -> Nhân Viên
ALTER TABLE MUATAILIEUMOI ADD CONSTRAINT FK_MUATAILI_DUYETMUA_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV);
-- Liên kết Cập Nhật Thông Tin -> Mua Tài Liệu Mới
ALTER TABLE CAPNHATTHONGTIN ADD CONSTRAINT FK_CAPNHATT_CAPNHATTH_MUATAILI FOREIGN KEY (MAMUA) REFERENCES MUATAILIEUMOI (MAMUA);
-- Liên kết Cập Nhật Thông Tin -> Tài Liệu
ALTER TABLE CAPNHATTHONGTIN ADD CONSTRAINT FK_CAPNHATT_CAPNHATTH_TAILIEU FOREIGN KEY (MATAILIEU) REFERENCES TAILIEU (MATAILIEU);
-- Liên kết Quản Lý Tài Liệu -> Nhân Viên
ALTER TABLE QUANLYTAILIEU ADD CONSTRAINT FK_QUANLYTA_QUANLYTAI_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV);
-- Liên kết Quản Lý Tài Liệu -> Tài Liệu
ALTER TABLE QUANLYTAILIEU ADD CONSTRAINT FK_QUANLYTA_QUANLYTAI_TAILIEU FOREIGN KEY (MATAILIEU) REFERENCES TAILIEU (MATAILIEU);
-- Liên kết Thanh Lý Tài Liệu -> Nhân Viên
ALTER TABLE THANHLYTAILIEU ADD CONSTRAINT FK_THANHLYT_LAP_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN (MANV);
-- Liên kết Nhân Viên -> Nhân Viên (Quản lý)
ALTER TABLE NHANVIEN ADD CONSTRAINT FK_NHANVIEN____UOC_QU_NHANVIEN FOREIGN KEY (NQL_NV) REFERENCES NHANVIEN (MANV);

GO
