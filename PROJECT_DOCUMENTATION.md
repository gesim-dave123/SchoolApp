# SchoolApp - Comprehensive Project Documentation

## Table of Contents
1. [Project Overview](#project-overview)
2. [Technology Stack](#technology-stack)
3. [Project Architecture](#project-architecture)
4. [Project Structure](#project-structure)
5. [Database Schema](#database-schema)
6. [Core Models](#core-models)
7. [Controllers & Business Logic](#controllers--business-logic)
8. [Authentication & Authorization](#authentication--authorization)
9. [Views & User Interface](#views--user-interface)
10. [Installation & Setup](#installation--setup)
11. [Running the Application](#running-the-application)
12. [Features Overview](#features-overview)
13. [API Endpoints](#api-endpoints)
14. [Configuration Files](#configuration-files)

---

## Project Overview

**SchoolApp** is a web-based school management system built with ASP.NET Core 10.0 and Entity Framework Core 10. The application provides functionality for:

- **User Management**: Create, read, update, and delete users with role-based access control
- **Transaction Management**: Track financial transactions with categorization
- **Activity Logging**: Maintain an audit trail of all administrative actions
- **Dashboard**: View analytics and statistics about users and transactions
- **Authentication**: Secure cookie-based authentication system

**Target Users**: School administrators and staff for managing users and financial transactions

**Default Admin Account**:
- Email: `admin@school.com`
- Password: `Admin123`

---

## Technology Stack

| Layer | Technology |
|-------|-----------|
| **Framework** | ASP.NET Core 10.0 |
| **ORM** | Entity Framework Core 10.0.5 |
| **Database** | SQLite (`schoolapp.db`) |
| **Authentication** | Cookie-based Authentication |
| **Frontend** | Razor Views with Bootstrap |
| **Styling** | Bootstrap 5, Custom CSS |
| **Build System** | .NET CLI / MSBuild |

**Key NuGet Packages**:
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.5" />
```

---

## Project Architecture

### Architectural Pattern: MVC (Model-View-Controller)

```
┌─────────────────────────────────────────────────────────┐
│                    Client Browser                        │
└────────────────────┬────────────────────────────────────┘
                     │ HTTP Requests/Responses
                     ▼
┌─────────────────────────────────────────────────────────┐
│              ASP.NET Core Application                    │
├─────────────────────────────────────────────────────────┤
│  Controllers (Routing & Logic)                          │
│  ├─ HomeController (Landing page)                       │
│  ├─ AuthController (Login/Register/Logout)              │
│  ├─ UsersController (User CRUD operations)              │
│  ├─ TransactionsController (Transaction CRUD)           │
│  ├─ DashboardController (Analytics)                     │
│  └─ ActivityLogsController (Audit trail)                │
├─────────────────────────────────────────────────────────┤
│  Models (Data Structures)                               │
│  ├─ UserModel                                           │
│  ├─ TransactionModel                                    │
│  ├─ ActivityLogModel                                    │
│  └─ LoginModel                                          │
├─────────────────────────────────────────────────────────┤
│  Data Layer (Entity Framework Core)                     │
│  └─ AppDbContext (Database context)                     │
└────────────────┬───────────────────────────────────────┘
                 │
                 ▼
     ┌──────────────────────┐
     │   SQLite Database    │
     │   (schoolapp.db)     │
     └──────────────────────┘
```

### Layered Architecture
1. **Presentation Layer**: Razor Views & Controllers handling HTTP requests
2. **Application Layer**: Controllers containing business logic
3. **Data Access Layer**: Entity Framework Core with AppDbContext
4. **Database Layer**: SQLite relational database

---

## Project Structure

```
SchoolApp/
├── Program.cs                          # Application entry point & configuration
├── SchoolApp.csproj                    # Project file with dependencies
├── SchoolApp.slnx                      # Solution file
├── appsettings.json                    # Configuration (production)
├── appsettings.Development.json        # Configuration (development)
│
├── Models/                              # Data Models (C# classes)
│   ├── UserModel.cs                    # User entity with validation
│   ├── TransactionModel.cs             # Transaction entity
│   ├── ActivityLogModel.cs             # Activity log entity
│   ├── LoginModel.cs                   # Login form model
│   └── ErrorViewModel.cs               # Error display model
│
├── Controllers/                         # Request handlers & business logic
│   ├── HomeController.cs               # Landing page
│   ├── AuthController.cs               # Authentication (login/register/logout)
│   ├── UsersController.cs              # User management (CRUD)
│   ├── TransactionsController.cs       # Transaction management (CRUD)
│   ├── DashboardController.cs          # Dashboard analytics
│   └── ActivityLogsController.cs       # Activity audit trail
│
├── Data/                                # Data persistence
│   └── AppDbContext.cs                 # EF Core database context
│
├── Views/                               # Razor templates
│   ├── Shared/
│   │   ├── _Layout.cshtml              # Master layout
│   │   ├── _Layout.cshtml.css          # Layout styles
│   │   ├── _Sidebar.cshtml             # Sidebar navigation
│   │   ├── _TopNavbar.cshtml           # Top navigation bar
│   │   ├── _AuthLayout.cshtml          # Auth-only layout
│   │   ├── Error.cshtml                # Error page
│   │   └── _ValidationScriptsPartial.cshtml  # Validation scripts
│   ├── Auth/
│   │   ├── Login.cshtml                # Login form
│   │   └── Register.cshtml             # Registration form
│   ├── Home/
│   │   ├── Landing.cshtml              # Landing/welcome page
│   │   └── Privacy.cshtml              # Privacy policy
│   ├── Dashboard/
│   │   └── Index.cshtml                # Analytics dashboard
│   ├── Users/
│   │   ├── Index.cshtml                # User list with search/pagination
│   │   ├── Create.cshtml               # Create user form
│   │   ├── Edit.cshtml                 # Edit user form
│   │   └── _CreateUserModal.cshtml     # Modal for user creation
│   ├── Transactions/
│   │   ├── Index.cshtml                # Transaction list
│   │   └── Create.cshtml               # Create transaction form
│   └── ActivityLogs/
│       └── Index.cshtml                # Activity logs audit trail
│
├── wwwroot/                             # Static files (accessible to client)
│   ├── css/
│   │   └── site.css                    # Custom styles
│   ├── js/
│   │   └── site.js                     # Custom scripts
│   └── lib/                             # Third-party libraries
│       ├── bootstrap/                   # Bootstrap CSS framework
│       ├── jquery/                      # jQuery library
│       ├── jquery-validation/           # jQuery validation plugin
│       └── jquery-validation-unobtrusive/  # Unobtrusive validation
│
├── Properties/
│   └── launchSettings.json             # Launch configuration
│
└── bin/ & obj/                          # Build outputs
```

---

## Database Schema

### Entity Relationship Diagram

```
┌─────────────────────┐
│     UserModel       │
├─────────────────────┤
│ Id (PK)             │
│ Name                │
│ Email (Unique)      │
│ Password            │
│ Age                 │
│ Role                │
└─────────────────────┘

┌──────────────────────────┐
│  TransactionModel        │
├──────────────────────────┤
│ TransactionId (PK)       │
│ Date                     │
│ Description              │
│ Amount                   │
│ Category                 │
└──────────────────────────┘

┌──────────────────────────┐
│  ActivityLogModel        │
├──────────────────────────┤
│ Id (PK)                  │
│ Action                   │
│ Details                  │
│ PerformedBy              │
│ CreatedAt                │
└──────────────────────────┘
```

### Database Relationships
- **One-to-Many (Implied)**: One User can have many ActivityLogs and Transactions
- **No foreign keys enforced** at database level (by design, for simplicity)
- All tables use SQLite data types

---

## Core Models

### 1. UserModel
**Purpose**: Represents a user in the system

**Location**: [Models/UserModel.cs](Models/UserModel.cs)

```csharp
public class UserModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age is required.")]
    [Range(18, 60, ErrorMessage = "Age must be between 18 and 60.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [NotMapped]  // Not stored in DB
    [Required(ErrorMessage = "Please confirm your password.")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string Role { get; set; } = "guest";  // "admin" or "guest"
}
```

**Key Features**:
- Primary key: `Id` (auto-increment)
- Email validation and uniqueness enforced in controller
- Password confirmation field (mapped property, not stored in DB)
- Role-based access control: "admin" or "guest"
- Age range validation (18-60)

**Validation Rules**:
| Field | Validation |
|-------|-----------|
| Name | Required, 2-50 characters |
| Email | Required, valid email format |
| Age | Required, 18-60 range |
| Password | Required, minimum 6 characters |
| ConfirmPassword | Must match Password |
| Role | Default: "guest" |

---

### 2. TransactionModel
**Purpose**: Records financial transactions

**Location**: [Models/TransactionModel.cs](Models/TransactionModel.cs)

```csharp
public class TransactionModel
{
    [Key]
    public int TransactionId { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 999999, ErrorMessage = "Amount must be greater than 0.")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [Required]
    public string Category { get; set; } = "General";
}
```

**Key Features**:
- Primary key: `TransactionId` (auto-increment)
- Timestamp: `Date` (defaults to current time)
- Currency support: `Amount` as decimal
- Categorization: `Category` field for filtering
- Description: Free-text field for transaction details

**Validation Rules**:
| Field | Validation |
|-------|-----------|
| Date | Auto-set to current time |
| Description | Required, max 200 characters |
| Amount | Required, 0.01 to 999,999 |
| Category | Required, defaults to "General" |

---

### 3. ActivityLogModel
**Purpose**: Audit trail of all administrative actions

**Location**: [Models/ActivityLogModel.cs](Models/ActivityLogModel.cs)

```csharp
public class ActivityLogModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Details { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string PerformedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
```

**Key Features**:
- Primary key: `Id` (auto-increment)
- Action: Type of operation (e.g., "User Created", "Transaction Deleted")
- Details: Detailed description of what changed
- PerformedBy: Username of the admin who performed the action
- CreatedAt: Timestamp of the action

**Example Log Entries**:
```
Action: "User Created"
Details: "User 'John Doe' (john@school.com) was created."

Action: "Transaction Deleted"
Details: "Transaction #5 (Supplies) amount ₱1,500.50 was deleted."
```

---

### 4. LoginModel
**Purpose**: Form model for user login (not persisted in database)

**Location**: [Models/LoginModel.cs](Models/LoginModel.cs)

```csharp
public class LoginModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
```

**Key Features**:
- No database persistence
- Email validation
- RememberMe checkbox for session extension
- Used only during authentication flow

---

## Controllers & Business Logic

### 1. AuthController
**Purpose**: Handles user authentication (login, register, logout)

**Location**: [Controllers/AuthController.cs](Controllers/AuthController.cs)

**Key Methods**:

#### `Login (GET)`
```csharp
public IActionResult Login()
{
    // If already logged in, redirect to dashboard
    if (User.Identity?.IsAuthenticated == true)
        return RedirectToAction("Index", "Dashboard");
    return View();
}
```
- Displays login form
- Redirects authenticated users to dashboard

#### `Login (POST)`
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginModel model)
{
    // Validate model
    // Check credentials against database
    // Create authentication claims
    // Sign user in with cookie authentication
    // Redirect to dashboard
}
```
- Validates credentials against database
- Creates security claims (ID, Name, Email, Role)
- Sets authentication cookie with expiration
- Supports "Remember Me" for extended sessions (7 days)
- Logs authentication failure to model state

#### `Register (GET/POST)`
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Register(UserModel model)
{
    // Validate model
    // Check for duplicate email
    // Set role to "guest"
    // Add to database
    // Redirect to login
}
```
- New users registered as "guest" role
- Email uniqueness validated
- Password confirmation enforced via model
- Redirects to login after successful registration

#### `Logout`
```csharp
public async Task<IActionResult> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return RedirectToAction("Login");
}
```
- Clears authentication cookie
- Invalidates session
- Redirects to login page

**Authentication Flow**:
```
User Input → POST Login → Validate Credentials → Create Claims 
→ Sign In Cookie → Store in Browser → Redirect to Dashboard
```

---

### 2. HomeController
**Purpose**: Display public-facing pages (landing page)

**Location**: [Controllers/HomeController.cs](Controllers/HomeController.cs)

```csharp
[Authorize]
public class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Landing()
    {
        // Redirect authenticated users to dashboard
        // Display landing page for unauthenticated users
    }
}
```

**Key Features**:
- Controller-level `[Authorize]` attribute (all actions require login by default)
- `Landing` action is `[AllowAnonymous]` for public access
- Auto-redirects authenticated users to dashboard

---

### 3. UsersController
**Purpose**: User management (CRUD operations with role-based access)

**Location**: [Controllers/UsersController.cs](Controllers/UsersController.cs)

**Key Methods**:

#### `Index`
```csharp
public IActionResult Index(
    string search = "", 
    string roleFilter = "", 
    int page = 1, 
    int pageSize = 8)
{
    // Query users with filters
    // Apply search (name/email)
    // Apply role filter
    // Implement pagination (8 per page)
    // Return paginated list
}
```
- **Search**: Filters by user name or email
- **Role Filter**: Filter by admin/guest role
- **Pagination**: Shows 8 users per page
- **Accessible to**: All logged-in users (view-only for guests)

#### `Create`
```csharp
[Authorize(Roles = "admin")]
public IActionResult Create()
{
    // Display user creation form
}

[HttpPost]
[Authorize(Roles = "admin")]
public IActionResult Create(UserModel model)
{
    // Validate model
    // Check for duplicate email
    // Add to database
    // Log activity
    // Redirect to user list
}
```
- **Admin Only**: Requires "admin" role
- Validates email uniqueness
- Logs user creation
- Redirects to user list with success message

#### `Edit`
```csharp
[Authorize(Roles = "admin")]
public IActionResult Edit(int id)
{
    // Load user by ID
    // Display edit form
}

[HttpPost]
[Authorize(Roles = "admin")]
public IActionResult Edit(UserModel model)
{
    // Update user properties (except password)
    // Save changes
    // Log activity
    // Redirect to user list
}
```
- **Admin Only**: Requires "admin" role
- Password fields removed from binding (cannot change via edit)
- Only updates: Name, Email, Age, Role
- Logs user modification

#### `Delete`
```csharp
[HttpPost]
[Authorize(Roles = "admin")]
public IActionResult Delete(int id)
{
    // Find user
    // Remove from database
    // Log activity
    // Redirect to user list
}
```
- **Admin Only**: Requires "admin" role
- Logs user deletion with user ID and name
- Returns to user list after deletion

**Helper Methods**:
```csharp
private string CurrentRole => 
    User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "guest";

private string CurrentUserName => 
    User.Identity?.Name ?? "Unknown";

private void LogActivity(string action, string details)
{
    _db.ActivityLogs.Add(new ActivityLogModel { ... });
    _db.SaveChanges();
}
```

**Access Control**:
| Action | Guest | Admin |
|--------|-------|-------|
| Index | View List | View List |
| Create | ❌ Denied | ✅ Allowed |
| Edit | ❌ Denied | ✅ Allowed |
| Delete | ❌ Denied | ✅ Allowed |

---

### 4. TransactionsController
**Purpose**: Transaction management (CRUD operations)

**Location**: [Controllers/TransactionsController.cs](Controllers/TransactionsController.cs)

**Key Methods**:

#### `Index`
```csharp
public IActionResult Index(
    string search = "", 
    string category = "", 
    string dateFrom = "", 
    int page = 1, 
    int pageSize = 8)
{
    // Query transactions
    // Apply search filter (description)
    // Apply category filter
    // Apply date filter (from date)
    // Implement pagination (8 per page)
    // Return paginated list
}
```
- **Search**: Filters by transaction description
- **Category**: Filter by transaction category
- **Date Filter**: Shows transactions from specified date onward
- **Pagination**: 8 transactions per page
- **Sorting**: Most recent transactions first

#### `Create`
```csharp
[Authorize(Roles = "admin")]
public IActionResult Create()
{
    // Display transaction creation form
}

[HttpPost]
[Authorize(Roles = "admin")]
public IActionResult Create(TransactionModel model)
{
    // Validate model
    // Set current timestamp
    // Add to database
    // Log activity
    // Redirect to transaction list
}
```
- **Admin Only**: Requires "admin" role
- Automatically sets transaction date to current time
- Logs transaction creation with amount and category
- Validates amount and description

#### `Delete`
```csharp
[HttpPost]
[Authorize(Roles = "admin")]
public IActionResult Delete(int id)
{
    // Find transaction
    // Log deleted transaction details
    // Remove from database
    // Redirect to transaction list
}
```
- **Admin Only**: Requires "admin" role
- Logs transaction deletion with full details
- Transaction ID, category, and amount recorded in log

**Access Control**:
| Action | Guest | Admin |
|--------|-------|-------|
| Index | View List | View List |
| Create | ❌ Denied | ✅ Allowed |
| Delete | ❌ Denied | ✅ Allowed |

---

### 5. DashboardController
**Purpose**: Analytics and statistics dashboard

**Location**: [Controllers/DashboardController.cs](Controllers/DashboardController.cs)

```csharp
[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        // Gather statistics
        ViewBag.UserCount = _db.Users.Count();
        ViewBag.TxCount = _db.Transactions.Count();
        ViewBag.TotalAmount = _db.Transactions.Sum(t => (decimal?)t.Amount) ?? 0;
        
        // Weekly revenue chart (last 7 days)
        var weeklyLabels = weekDays.Select(d => d.ToString("ddd"));
        var weeklyData = weekDays.Select(d => 
            transactions on that day summed);
        
        // Category statistics
        var categoryStats = _db.Transactions
            .GroupBy(t => t.Category)
            .Select(g => new { 
                Category = g.Key, 
                Count = g.Count(), 
                TotalAmount = g.Sum(x => x.Amount) 
            });
    }
}
```

**Statistics Displayed**:
1. **Total Users**: Count of all users in system
2. **Total Transactions**: Count of all transactions
3. **Total Amount**: Sum of all transaction amounts
4. **Weekly Revenue Chart**: Transactions aggregated by day for last 7 days
5. **Category Statistics**: 
   - Breakdown by transaction category
   - Count per category
   - Total amount per category

**Data Serialization**:
- JSON serialization for Chart.js compatibility
- Passed to view via ViewBag

---

### 6. ActivityLogsController
**Purpose**: Display audit trail of administrative actions

**Location**: [Controllers/ActivityLogsController.cs](Controllers/ActivityLogsController.cs)

```csharp
[Authorize(Roles = "admin")]
public class ActivityLogsController : Controller
{
    public IActionResult Index()
    {
        var logs = _db.ActivityLogs
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
        return View(logs);
    }
}
```

**Features**:
- **Admin Only**: Only admins can view activity logs
- **Chronological Order**: Most recent logs first
- **Full Audit Trail**: All user and transaction changes logged

**Logged Activities**:
- User Created
- User Edited
- User Deleted
- Transaction Created
- Transaction Deleted

---

## Authentication & Authorization

### Authentication Mechanism: Cookie-Based

**Configuration in Program.cs**:
```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
```

**How It Works**:
1. User logs in with credentials
2. AuthController validates credentials against database
3. Security claims created (ID, Name, Email, Role)
4. Authentication cookie signed and sent to browser
5. Cookie persists across requests
6. Cookie automatically included in subsequent HTTP requests
7. Middleware verifies cookie signature and extracts user info
8. Request context populated with `User` principal

**Session Duration**:
- Default: 8 hours
- With "Remember Me": 7 days
- Can be extended by middleware configuration

### Authorization: Role-Based

**Roles**:
- **admin**: Full access to CRUD operations and activity logs
- **guest**: Read-only access to user and transaction lists

**Authorization Attributes**:
```csharp
[Authorize]                          // Any authenticated user
[Authorize(Roles = "admin")]         // Admins only
[AllowAnonymous]                     // Public access (overrides controller-level auth)
```

**Default Admin Account** (created on first run):
```
Email: admin@school.com
Password: Admin123
Role: admin
```

**Access Matrix**:

| Feature | Anonymous | Guest | Admin |
|---------|-----------|-------|-------|
| Landing Page | ✅ | Redirected | Redirected |
| Login | ✅ | ❌ | ❌ |
| Register | ✅ | ❌ | ❌ |
| View Dashboard | ❌ | ✅ | ✅ |
| View Users | ❌ | ✅ | ✅ |
| Create User | ❌ | ❌ | ✅ |
| Edit User | ❌ | ❌ | ✅ |
| Delete User | ❌ | ❌ | ✅ |
| View Transactions | ❌ | ✅ | ✅ |
| Create Transaction | ❌ | ❌ | ✅ |
| Delete Transaction | ❌ | ❌ | ✅ |
| View Activity Logs | ❌ | ❌ | ✅ |

---

## Views & User Interface

### Layout Structure

#### Master Layout: `_Layout.cshtml`
- Defines overall page structure (header, navigation, footer)
- References Bootstrap CSS framework and custom CSS
- Includes navigation menu and user info
- Used by most pages

#### Auth Layout: `_AuthLayout.cshtml`
- Simplistic layout for login/register pages
- No navigation or sidebar
- Focused, minimalist design

### View Components

#### Shared Components
- **_Sidebar.cshtml**: Left navigation sidebar for authenticated pages
- **_TopNavbar.cshtml**: Top navigation bar with user info and logout
- **_ValidationScriptsPartial.cshtml**: jQuery validation scripts

### Pages Overview

#### Public Pages
- **Landing.cshtml**: Entry point, shows login/register options

#### Authentication Pages
- **Auth/Login.cshtml**: Login form with email/password
- **Auth/Register.cshtml**: Registration form with validation

#### Dashboard
- **Dashboard/Index.cshtml**: 
  - Key statistics (user count, transaction count, total amount)
  - Weekly revenue chart (Chart.js - line/bar chart)
  - Category statistics (pie/doughnut chart)
  - Responsive grid layout

#### User Management
- **Users/Index.cshtml**: 
  - Searchable user list with pagination
  - Search by name or email
  - Filter by role (admin/guest)
  - Edit/Delete buttons (admin only)
  - Modal for creating new user

- **Users/Create.cshtml**: 
  - User creation form
  - Name, email, age, password fields
  - Admin role selection dropdown

- **Users/Edit.cshtml**: 
  - User editing form
  - No password field (can only be changed via database)
  - Edit name, email, age, role

- **Users/_CreateUserModal.cshtml**: 
  - Bootstrap modal for inline user creation
  - Quick add without leaving user list

#### Transaction Management
- **Transactions/Index.cshtml**: 
  - Searchable transaction list with pagination
  - Search by description
  - Filter by category
  - Date range filter
  - Delete button (admin only)

- **Transactions/Create.cshtml**: 
  - Transaction creation form
  - Description, amount, category fields
  - Category dropdown with predefined options

#### Activity Logs
- **ActivityLogs/Index.cshtml**: 
  - Audit trail of all admin actions
  - Table view with Action, Details, Performed By, Created At columns
  - Reverse chronological order

---

## Installation & Setup

### Prerequisites
- **.NET SDK 10.0** or newer
- **Git** (optional, for version control)
- **Code Editor**: Visual Studio, VS Code, or Visual Studio 2022
- **SQLite**: Automatically handled by EF Core

### Step 1: Clone/Extract Project
```bash
cd path/to/SchoolApp
```

### Step 2: Restore NuGet Packages
```bash
dotnet restore
```
This downloads all required dependencies from NuGet.org

### Step 3: Apply Entity Framework Migrations
```bash
dotnet ef database update
```
This creates the SQLite database and applies any pending migrations.

**What happens automatically**:
- Creates `schoolapp.db` file in project root
- Creates `Users`, `Transactions`, `ActivityLogs` tables
- Seeds default admin user (admin@school.com / Admin123)

### Step 4: Configuration (Optional)
- **appsettings.json**: Production configuration
- **appsettings.Development.json**: Development configuration
- Database connection string: `Data Source=schoolapp.db` (SQLite in-project storage)

---

## Running the Application

### Visual Studio
1. Open `SchoolApp.slnx` in Visual Studio
2. Click "Run" or press F5
3. Application launches at `https://localhost:5001` or `http://localhost:5000`

### Visual Studio Code
1. Open project folder in VS Code
2. Open terminal (Ctrl+`)
3. Run: `dotnet run`
4. Navigate to `http://localhost:5000`

### Command Line
```bash
dotnet run
```

**Output**:
```
info: Microsoft.AspNetCore.Hosting.Diagnostics
      Request starting HTTP/1.1 GET https://localhost:5001/
info: Microsoft.Hosting.Lifetime
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime
      Application started. Press Ctrl+C to stop.
```

### First Access
1. Navigate to `http://localhost:5000/Auth/Login`
2. Login with admin credentials:
   - Email: `admin@school.com`
   - Password: `Admin123`
3. Or register a new guest account

---

## Features Overview

### 1. User Management
- **Create Users**: Add new staff/student accounts (admin only)
- **View Users**: Browse all users with search and filtering
- **Edit Users**: Update user information excluding password (admin only)
- **Delete Users**: Remove user accounts (admin only)
- **Role Assignment**: Set user roles (admin/guest)
- **Search**: Find users by name or email
- **Pagination**: Browse users in pages of 8
- **Age Validation**: Users must be 18-60 years old

### 2. Transaction Management
- **Create Transactions**: Record financial transactions (admin only)
- **View Transactions**: Browse all transactions
- **Delete Transactions**: Remove transactions (admin only)
- **Categorization**: Organize transactions by category
- **Date Tracking**: Automatic timestamp for each transaction
- **Amount Validation**: Transactions must have valid amounts
- **Search**: Find transactions by description
- **Category Filter**: Filter transactions by category
- **Date Range Filter**: View transactions from specific date onward
- **Pagination**: Browse transactions in pages of 8

### 3. Dashboard Analytics
- **User Statistics**: Total count of users in system
- **Transaction Statistics**: Total count and sum of all transactions
- **Weekly Revenue Chart**: Visual representation of transaction amounts by day
- **Category Breakdown**: Transactions categorized with counts and totals
- **Responsive Design**: Dashboard adapts to different screen sizes

### 4. Activity Logging & Audit Trail
- **Automatic Logging**: All admin actions logged automatically
- **User Actions**: User creation, edit, deletion tracked
- **Transaction Actions**: Transaction creation and deletion tracked
- **Audit Trail**: View who did what and when
- **Admin Verification**: Verify that sensitive operations were performed

### 5. Authentication & Security
- **Cookie-Based Auth**: Secure session management
- **Session Expiration**: 8 hours default, 7 days with "Remember Me"
- **Password Validation**: Minimum 6 characters
- **Email Validation**: Valid email format required
- **Unique Email**: Prevents duplicate registrations
- **Anti-Forgery Protection**: CSRF token validation on state-changing requests
- **Password Confirmation**: New users must confirm password

### 6. Role-Based Access Control
- **Admin Role**: Full access to all features including CRUD and logs
- **Guest Role**: Read-only access to users and transactions
- **Role-Based Buttons**: UI shows/hides admin-only buttons based on role
- **Server-Side Validation**: Unauthorized actions blocked on server

---

## API Endpoints

### Authentication Endpoints

| Method | Route | Description | Auth Required |
|--------|-------|-------------|---------------|
| GET | `/Auth/Login` | Display login form | No |
| POST | `/Auth/Login` | Process login | No |
| GET | `/Auth/Register` | Display registration form | No |
| POST | `/Auth/Register` | Process registration | No |
| GET | `/Auth/Logout` | Logout user | Yes |
| GET | `/Auth/AccessDenied` | Access denied page | Yes |

### Home Endpoints

| Method | Route | Description | Auth Required |
|--------|-------|-------------|---------------|
| GET | `/Home/Landing` or `/` | Landing page | No |

### Dashboard Endpoints

| Method | Route | Description | Auth Required | Role |
|--------|-------|-------------|----------------|------|
| GET | `/Dashboard/Index` | View dashboard | Yes | Any |

### User Management Endpoints

| Method | Route | Description | Auth Required | Role |
|--------|-------|-------------|----------------|------|
| GET | `/Users/Index` | List users with search/filter | Yes | Any |
| GET | `/Users/Create` | User creation form | Yes | Admin |
| POST | `/Users/Create` | Create new user | Yes | Admin |
| GET | `/Users/Edit/{id}` | User edit form | Yes | Admin |
| POST | `/Users/Edit` | Update user | Yes | Admin |
| POST | `/Users/Delete/{id}` | Delete user | Yes | Admin |

### Transaction Endpoints

| Method | Route | Description | Auth Required | Role |
|--------|-------|-------------|----------------|------|
| GET | `/Transactions/Index` | List transactions | Yes | Any |
| GET | `/Transactions/Create` | Transaction creation form | Yes | Admin |
| POST | `/Transactions/Create` | Create new transaction | Yes | Admin |
| POST | `/Transactions/Delete/{id}` | Delete transaction | Yes | Admin |

### Activity Logs Endpoints

| Method | Route | Description | Auth Required | Role |
|--------|-------|-------------|----------------|------|
| GET | `/ActivityLogs/Index` | View activity logs | Yes | Admin |

---

## Configuration Files

### Program.cs (Application Configuration)

**Purpose**: Application entry point and dependency injection setup

**Key Configurations**:
```csharp
// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=schoolapp.db"));

// Authentication Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

// Authorization setup
builder.Services.AddAuthorization();

// MVC services
builder.Services.AddControllersWithViews();

// Middleware pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();   // Must be before Authorization
app.UseAuthorization();

// Route mapping
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Landing}/{id?}");
```

**Database Auto-initialization**:
```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Users.Any())
    {
        db.Users.Add(new UserModel { ... admin ... });
        db.SaveChanges();
    }
}
```

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Configuration Options**:
- **Logging**: 
  - Default level: Information (logs info, warning, error)
  - AspNetCore level: Warning (suppresses verbose ASP.NET Core logs)
- **AllowedHosts**: `*` allows requests from any host (production: restrict this)

### appsettings.Development.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Development-specific settings**: Enhanced logging for debugging

### SchoolApp.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.5" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.5" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.5" />
  </ItemGroup>

</Project>
```

**Project Settings**:
- **TargetFramework**: .NET 10.0
- **Nullable**: Enabled (null reference warnings)
- **ImplicitUsings**: Enabled (common namespaces auto-imported)

**Dependencies**:
| Package | Version | Purpose |
|---------|---------|---------|
| EF Core Design | 10.0.5 | Database design-time tools |
| EF Core SQLite | 10.0.5 | SQLite provider for EF Core |
| EF Core Tools | 10.0.5 | CLI tools for migrations |

### Properties/launchSettings.json

```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

**Launch Profiles**:
- **http**: Port 5000 (unsecured)
- **https**: Port 5001 (secured with self-signed certificate)
- Auto-launches browser on startup
- ASPNETCORE_ENVIRONMENT: Development (uses appsettings.Development.json)

---

## Database

### SQLite Database: `schoolapp.db`

**Location**: Project root directory

**Auto-created**: Yes, on first application run

**Tables**:

#### Users Table
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Password TEXT NOT NULL,
    Age INTEGER NOT NULL,
    Role TEXT NOT NULL DEFAULT 'guest'
);
```

#### Transactions Table
```sql
CREATE TABLE Transactions (
    TransactionId INTEGER PRIMARY KEY AUTOINCREMENT,
    Date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Description TEXT NOT NULL,
    Amount DECIMAL NOT NULL,
    Category TEXT DEFAULT 'General'
);
```

#### ActivityLogs Table
```sql
CREATE TABLE ActivityLogs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Action TEXT NOT NULL,
    Details TEXT NOT NULL,
    PerformedBy TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);
```

### Data Retention Policy
- All data persisted indefinitely
- No automatic purging of old records
- Activity logs kept for audit purposes

---

## Development Workflow

### Building
```bash
dotnet build
```
Compiles project and checks for syntax errors.

### Cleaning
```bash
dotnet clean
```
Removes compiled output and intermediate files.

### Database Migrations (if schema changes)
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Running Tests (if added)
```bash
dotnet test
```

### Publishing for Production
```bash
dotnet publish -c Release -o ./publish
```

---

## Security Considerations

✅ **Implemented**:
- Cookie-based authentication with HttpOnly flag (default)
- Anti-forgery token validation (ValidateAntiForgeryToken)
- Role-based authorization
- Password validation (minimum 6 characters)
- Email format validation
- Data validation via annotations

⚠️ **Not Implemented** (for production, implement):
- HTTPS enforcement
- Rate limiting on login
- Password hashing (currently plain text - **SECURITY RISK**)
- SQL injection prevention (EF Core parameterized queries mitigate this)
- XSS protection (ASP.NET Core implements by default)
- CORS configuration (if needed)
- Two-factor authentication
- Password reset functionality
- Account lockout on failed attempts

**⚠️ CRITICAL SECURITY ISSUES**:
1. **Passwords stored in plain text** - Must implement hashing (BCrypt, PBKDF2, Argon2)
2. **No HTTPS in development** - Use HTTPS in production
3. **Default admin credentials should be changed** on first deployment

---

## Deployment Considerations

### For Production:
1. Implement password hashing (use `BCrypt.Net-Next` NuGet package)
2. Enable HTTPS enforcement in program configuration
3. Use environment variables for sensitive data (connection strings, API keys)
4. Migrate from SQLite to enterprise database (SQL Server, PostgreSQL, MySQL)
5. Implement backup strategy for database
6. Set up logging to file system or external service
7. Implement error handling and custom error pages
8. Add rate limiting for authentication attempts
9. Configure CORS if API is consumed by external clients
10. Implement Content Security Policy headers
11. Set up health checks for monitoring
12. Configure application insights or similar monitoring

---

## Troubleshooting

### Issue: Database not created
**Solution**: Run `dotnet ef database update`

### Issue: "Admin user already exists" on startup
**Solution**: Database already initialized. Comment out seeding code or delete `schoolapp.db`

### Issue: Login fails with correct credentials
**Solution**: Check `schoolapp.db` for user record. Verify encrypted password or use default admin account.

### Issue: Views not displaying
**Solution**: Ensure `wwwroot` folder exists and files are included in project

### Issue: CSS/JS not loading
**Solution**: Check browser console for 404s. Verify files exist in `wwwroot` and `UseStaticFiles()` is configured

---

## Summary

**SchoolApp** is a role-based school management system featuring:
- ✅ User management with admin controls
- ✅ Transaction tracking with categorization
- ✅ Dashboard analytics and statistics
- ✅ Activity audit logging
- ✅ Cookie-based authentication
- ✅ Role-based authorization
- ✅ Searchable, paginated lists
- ✅ Responsive UI with Bootstrap

**Tech Stack**: .NET 10.0, ASP.NET Core, Entity Framework Core, SQLite, Bootstrap 5

**Next Steps**: Implement password hashing, add more complex analytics, migrate to production database, enhance UI/UX.

---

*Documentation Generated: April 2026*
*Project: SchoolApp v1.0*
