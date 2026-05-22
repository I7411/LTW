# Library Management System

A comprehensive web-based library management system built with ASP.NET MVC and SQL Server. This system facilitates efficient management of library operations including document borrowing, membership handling, inventory management, and staff administration.

## Description

The Library Management System is designed to streamline library operations and provide a user-friendly platform for library staff and patrons. The system manages:

- **Document Management**: Track books and other library materials with detailed information including ISBN, author, publisher, and availability status
- **Member Services**: Manage library members/readers with membership cards and borrowing records
- **Borrowing Process**: Handle borrowing and returning of documents with due dates and extensions
- **Penalty Management**: Track overdue documents and issue penalties
- **Room Booking**: Allow members to reserve study rooms
- **Staff Management**: Manage library staff with role-based access control
- **Inventory Control**: Monitor document quantities and purchase new materials
- **Reporting**: Generate reports on borrowing activities and library statistics

## Features

- **User Authentication**: Role-based login system for Readers, Librarians, and Administrators
- **Member Registration**: Self-service member registration with automatic card issuance
- **Document Management**: CRUD operations for documents, categories, and inventory tracking
- **Borrowing Management**: 
  - Register document borrowing with automatic due date calculation
  - Track borrowed documents and borrowing history
  - Process document returns
  - Extend borrowing periods with request handling
  - Request pre-borrowing for documents
- **Penalty System**: Issue and track penalties for overdue documents and lost materials
- **Staff Management**: Add, update, and manage library staff with role assignments
- **Room Booking**: Allow members to book study rooms with availability checks
- **Document Purchase**: Manage new document acquisitions and stock updates
- **Document Liquidation**: Process document removal and tracking
- **Reporting**: Generate borrowing statistics and library activity reports
- **Password Management**: Support for password recovery and change functionality
- **Search Functionality**: Search documents by title, ID, publisher, or author

## Technologies Used

| Category | Technology |
|----------|-----------|
| **Programming Language** | C# |
| **Framework** | ASP.NET MVC 5 |
| **Web Technology** | Razor View Engine, HTML, CSS, JavaScript |
| **Database** | SQL Server |
| **Data Access** | Entity Framework (Code First via EDMX) |
| **Architecture** | Model-View-Controller (MVC) |
| **UI Components** | Bootstrap (implied), Custom CSS |
| **.NET Version** | .NET Framework 4.x |

## Project Structure

```
C:\LTW\
├── Controllers/                      # MVC Controllers - business logic
│   ├── AccountController.cs         # User login, registration, password recovery
│   ├── DocumentController.cs        # Document borrowing and search
│   ├── HomeController.cs            # Home page routing
│   ├── ManagementAdminController.cs # Staff and admin operations
│   ├── ManagementController.cs      # Document and inventory management
│   ├── MembershipCardController.cs  # Member card operations
│   ├── ReportController.cs          # Reporting functionality
│   ├── RequiredDocumentsController.cs # Document request handling
│   ├── SearchController.cs          # Search operations
│   ├── SellController.cs            # Document purchase operations
│   └── InfomationController.cs      # Information and profile management
│
├── Models/                          # Data models and Entity Framework context
│   ├── DOCGIA.cs                   # Reader/Member model
│   ├── NHANVIEN.cs                 # Staff member model
│   ├── TAILIEU.cs                  # Document/Book model
│   ├── PHIEUMUON.cs                # Borrowing slip model
│   ├── THEBANDOC.cs                # Membership card model
│   ├── PHIEUPHAT.cs                # Penalty slip model
│   ├── GIAHANTAILIEU.cs            # Document extension model
│   ├── PHONGHOC.cs                 # Study room model
│   ├── DATPHONG.cs                 # Room booking model
│   ├── MUATAILIEUMOI.cs            # New document purchase model
│   ├── HUIT_LIBRARY.cs             # Entity Framework context (EDMX-generated)
│   └── *ViewModels.cs              # View models for controllers
│
├── Views/                           # ASP.NET Razor view templates
│   ├── Account/                    # Login, registration, password recovery views
│   ├── Document/                   # Document borrowing and search views
│   ├── Home/                       # Home page views for different roles
│   ├── Management/                 # Document management views
│   ├── ManagementAdmin/            # Admin management views
│   ├── MembershipCard/             # Member card views
│   ├── Report/                     # Report views
│   ├── Search/                     # Search result views
│   ├── Sell/                       # Purchase and inventory views
│   ├── Shared/                     # Shared layout templates
│   ├── _ViewStart.cshtml           # Common view configuration
│   └── web.config                  # Views configuration
│
├── wwwroot/                         # Static files
│   ├── CSS/                        # Stylesheets
│   ├── Images/                     # Images and icons
│   └── JavaScript/                 # Client-side scripts
│
├── Library_Management.sln          # Visual Studio solution file
├── QUANLYTHUVIEN.sql               # Database creation and sample data script
└── README.md                        # This file
```

## Database

**Database Name**: QUANLYTHUVIEN (Database for Library Management)

### Main Tables

| Table | Purpose |
|-------|---------|
| **DOCGIA** | Library members/readers with login credentials and personal information |
| **THEBANDOC** | Membership cards with status and expiration date |
| **NHANVIEN** | Library staff members with role assignments and credentials |
| **TAILIEU** | Books and library materials with metadata (author, publisher, quantity, rental fee) |
| **PHIEUMUON** | Borrowing records linking borrower, staff, and documents |
| **CHITIETPHIEUMUON** | Details of each document in a borrowing record |
| **PHIEUPHAT** | Penalty records for overdue or damaged documents |
| **GIAHANTAILIEU** | Document borrowing period extension requests |
| **DATMUONTRUOC** | Pre-borrowing requests (reserve documents before availability) |
| **PHONGHOC** | Study room information with capacity and amenities |
| **DATPHONG** | Room booking records with date/time and status |
| **MUATAILIEUMOI** | New document purchase orders |
| **CAPNHATTHONGTIN** | Details of purchased documents in orders |
| **QUANLYTAILIEU** | Document management tracking with costs |
| **THANHLYTAILIEU** | Document liquidation/disposal records |

### Foreign Key Relationships

- THEBANDOC → DOCGIA (Membership card belongs to reader)
- PHIEUMUON → NHANVIEN, THEBANDOC (Borrowing linked to staff and member)
- CHITIETPHIEUMUON → PHIEUMUON, TAILIEU (Document details in borrowing)
- PHIEUPHAT → NHANVIEN, PHIEUMUON (Penalty issued by staff for borrowing)
- GIAHANTAILIEU → PHIEUMUON (Extension request for borrowing)
- DATPHONG → DOCGIA, PHONGHOC (Room booking by member in room)
- MUATAILIEUMOI → NHANVIEN (Purchase approved by staff)
- CAPNHATTHONGTIN → MUATAILIEUMOI, TAILIEU (Purchase details)
- NHANVIEN → NHANVIEN (Self-reference for manager hierarchy)

## How to Run

### Prerequisites
- Visual Studio 2017 or later
- .NET Framework 4.5 or higher
- SQL Server 2012 or later
- NuGet Package Manager

### Step-by-Step Setup

1. **Clone the Repository**
   ```bash
   git clone <repository-url>
   cd LTW
   ```

2. **Open Solution in Visual Studio**
   - Open `Library_Management.sln` in Visual Studio
   - Wait for NuGet packages to restore automatically (if not, right-click solution → Restore NuGet Packages)

3. **Create and Setup Database**
   - Open SQL Server Management Studio (SSMS)
   - Execute the `QUANLYTHUVIEN.sql` script to create the database and tables with sample data
   ```sql
   -- In SSMS Query Editor:
   -- Open and execute QUANLYTHUVIEN.sql
   ```

4. **Update Database Connection String** (if needed)
   - The connection string is typically configured in Entity Framework EDMX context
   - If using a different SQL Server instance or credentials:
     - Open the `HUIT_LIBRARY.edmx` file (Entity Framework model)
     - Right-click → Update Model from Database
     - Or manually update connection in the generated `*.Context.cs` file

5. **Build Solution**
   ```
   In Visual Studio:
   - Build → Build Solution (Ctrl+Shift+B)
   - Ensure no build errors occur
   ```

6. **Run Application**
   - Press `F5` or click Start (Debug)
   - The application will open in your default browser at `http://localhost:<port>/`

### Alternative: Update Database Manually

If Entity Framework migrations are used:
```
In Package Manager Console:
PM> Update-Database
```

## How to Use

### Login Page
- Navigate to `/Account/Login`
- Select role: **Độc giả** (Reader), **Thủ thư** (Librarian), or **Admin**
- Enter credentials from sample data below
- Click "ĐĂNG NHẬP" (Login)

### Reader (Độc Giả) Workflow
1. **Browse & Search Documents**
   - Navigate to Document page
   - Search for books by title, ID, or publisher
   - View document details including availability

2. **Borrow Documents**
   - Select document and enter quantity
   - Submit borrowing request
   - System automatically generates due date

3. **View Borrowed Items**
   - Check borrowing history and current borrowed documents
   - View due dates

4. **Request Extensions**
   - Submit extension request for borrowed documents
   - Approve/reject by librarian

5. **Reserve Study Room**
   - Book available study rooms
   - Check room availability and amenities

6. **View Penalties**
   - Check any penalties issued (overdue or damaged items)

### Librarian (Thủ Thư) Workflow
1. **Accept/Process Borrowing Requests**
   - Review borrowing requests from readers
   - Process and finalize borrowing

2. **Process Returns**
   - Record returned documents
   - Update document quantities

3. **Issue Penalties**
   - Record overdue documents
   - Create penalty slips for damaged or lost items

4. **Approve Extensions**
   - Review extension requests
   - Approve or reject based on policy

5. **Manage Rooms**
   - View room bookings
   - Approve/reject booking requests

6. **Generate Reports**
   - View borrowing statistics
   - Monitor library activity

### Admin Workflow
1. **Staff Management**
   - Add new staff members
   - Assign roles (Librarian, Staff)
   - Update staff information
   - Remove staff members

2. **Document Management**
   - Add new documents to inventory
   - Update document information (quantity, rental fee)
   - Manage document categories

3. **Inventory & Purchasing**
   - Create purchase orders for new documents
   - Track stock levels
   - Update document quantities after purchases

4. **Document Liquidation**
   - Mark documents for removal/disposal
   - Track liquidation records

5. **System Administration**
   - View all system reports
   - Monitor system activities

## Deployment

### Requirements for Target Machine
- Windows 7 or later / Windows Server 2008 or later
- .NET Framework 4.5 or later installed
- SQL Server 2012 or later (local or remote instance)
- Internet Information Services (IIS) 7.5 or later (for web server deployment)

### Deployment Steps

1. **Build Release Version**
   ```
   In Visual Studio:
   - Build → Configuration Manager
   - Select "Release" configuration
   - Build → Build Solution
   ```

2. **Publish Application** (for IIS)
   ```
   - Right-click project → Publish
   - Choose publishing target (IIS, FTP, or file system)
   - Configure settings
   - Publish
   ```

   **Or manually copy files:**
   - Copy output from `bin\Release` folder
   - Copy all files including `Views\`, `wwwroot\`, `Web.config`, etc.

3. **Deploy Database**
   - Execute `QUANLYTHUVIEN.sql` on target SQL Server
   - Ensure SQL Server is running and accessible

4. **Configure Connection String**
   - Update SQL Server connection string in application config:
     - For web apps: modify connection string in `Web.config` in the root or App.config
     - For local connections: typically configured in Entity Framework context

5. **Setup IIS** (if using IIS)
   ```
   - Create new Application Pool (.NET Framework v4.0.30319)
   - Create new website/application pointing to published folder
   - Configure application pool identity (application pool or specific service account)
   - Ensure SQL Server is accessible from IIS server
   ```

6. **Test Application**
   - Navigate to application URL
   - Verify login functionality
   - Test basic workflows

### Important Notes for Deployment
- Ensure SQL Server TCP/IP protocol is enabled
- If using remote SQL Server, configure firewall rules
- SQL Server authentication or Windows authentication should be configured
- Test application on target machine before production deployment
- Backup original database before running on production server

## Default Accounts

Sample data includes the following test accounts created in `QUANLYTHUVIEN.sql`:

### Readers (Độc Giả)
| Username | Password | Role |
|----------|----------|------|
| minhan | 123456 | Reader |
| hoadephai | abc123 | Reader |
| baotran | pass789 | Reader |

### Staff (Nhân Viên)
| Username | Password | Role |
|----------|----------|------|
| adminan | 123456 | Admin |
| hoale | abc123 | Librarian |
| vietpq | 789123 | Staff |

### Default New Staff Password
When creating new staff members through the admin panel, the default password is: **123**

**⚠️ Security Warning**: These are sample credentials for development/testing only. Before deploying to production:
- Change all default passwords
- Implement secure password generation
- Enforce strong password policies
- Never commit real credentials to version control

## What I Learned

1. **ASP.NET MVC Architecture**: Understanding the separation of concerns between Models, Views, and Controllers for maintainable web applications

2. **Entity Framework & ORM Concepts**: Working with Object-Relational Mapping for database abstraction and CRUD operations without writing raw SQL

3. **Relational Database Design**: Designing normalized databases with appropriate foreign key relationships, constraints, and referential integrity

4. **Role-Based Access Control (RBAC)**: Implementing authentication and authorization systems with different user roles and permissions using session management

5. **Web Forms & Data Binding**: Building dynamic web forms with proper model binding, validation, and data transfer between client and server

6. **SQL Server & T-SQL**: Creating complex databases, managing transactions, implementing business logic through stored procedures and constraints

7. **CRUD Operations & Business Logic**: Implementing complete data management workflows including validation, error handling, and state management

## Future Improvements

1. **Security Enhancements**
   - Implement password hashing (bcrypt/PBKDF2) instead of plain text storage
   - Add CSRF token validation
   - Implement parameterized queries throughout (prevent SQL injection)
   - Add rate limiting for login attempts
   - Implement account lockout after failed login attempts

2. **Data Validation & Error Handling**
   - Add comprehensive input validation on both client and server side
   - Implement global exception handling middleware
   - Add user-friendly error messages with proper logging
   - Implement data type validation

3. **Code Quality Improvements**
   - Move connection strings to configuration files (Web.config or appsettings)
   - Implement dependency injection for better testability
   - Add unit tests for controllers and business logic
   - Implement repository pattern for data access layer
   - Add proper logging framework (NLog, Serilog)

4. **Performance Optimization**
   - Add pagination for large data sets
   - Implement caching for frequently accessed data
   - Optimize database queries with proper indexing
   - Add lazy loading where appropriate
   - Minimize JavaScript and CSS

5. **UI/UX Improvements**
   - Implement responsive design for mobile devices
   - Add modern UI framework (Bootstrap 5, Tailwind CSS)
   - Improve accessibility (WCAG compliance)
   - Add real-time notifications
   - Create better admin dashboard

6. **Feature Enhancements**
   - Add email notifications for borrowing reminders
   - Implement document rating and review system
   - Add wishlist functionality
   - Implement SMS notifications for due dates
   - Create API endpoint for mobile app integration

7. **Testing & Quality Assurance**
   - Add automated unit tests using xUnit or NUnit
   - Add integration tests for database operations
   - Implement end-to-end testing with Selenium
   - Add load testing for performance validation

8. **Documentation & Maintenance**
   - Add API documentation (Swagger/OpenAPI)
   - Create user manual in Vietnamese and English
   - Add architecture documentation and diagrams
   - Document database schema in more detail

9. **Infrastructure**
   - Containerize application with Docker
   - Implement CI/CD pipeline (GitHub Actions, Azure DevOps)
   - Add database backup automation
   - Implement monitoring and alerting

10. **Scalability**
    - Consider moving to ASP.NET Core for better performance
    - Implement microservices architecture if needed
    - Add message queue for asynchronous operations
    - Implement caching layer (Redis)

## Suggested .gitignore Additions

To prevent committing unnecessary or sensitive files to version control, add the following to your `.gitignore`:

```
# Visual Studio directories
bin/
obj/
.vs/
*.user
*.suo
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/

# NuGet
*.nupkg
*.snupkg
packages/
.nuget/

# Cache directories
.cache/
.npm/

# IDE
.vscode/
.idea/

# Environment files
.env
.env.local

# Dependencies
node_modules/

# OS files
.DS_Store
Thumbs.db

# Sensitive data
appsettings.*.json
*.ConnectionStrings.json

# Build output
artifacts/
```

## Author

Huỳnh Thanh Minh Tâm

## License

This project is created for educational purposes. It was developed as a capstone/academic project for learning ASP.NET MVC and database management.

## Contact & Support

For questions or issues regarding this project, please refer to the project documentation or contact the author.

---

**Last Updated**: 2026

**Repository**: Library Management System (QUANLYTHUVIEN)
