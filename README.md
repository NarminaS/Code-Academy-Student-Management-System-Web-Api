# Code-Academy-Student-Management-System-Web-Api
Asp.NET Core 2. 1 Web-Api Part of CodeAcademy  Student Management System  (continuation of MVC version)

### Used technologies : 
  * ASP.NET Core 2.1 Web Api
  * Entity Framework Core
  * **MailKit** - email sending
  * **Serilog** - logging (MsSqlServer Sink) to database
  * ASP.NET Identity
  * JsonWebToken - for authentication
  * **Cloudinary** - file and images storing
  
### Used design pattern - Generic Repository Pattern (non generic methods also used)  

### BusinessLogicLayer - 
  * Abstract - Interfaces for repository (Database entity models and authentication models)
  * Concrete - Implementations of interfaces  

### Controllers 
  * Administrator - admin panel controllers
  * Editor - editor panel controllers
  * Edu - controllers managing home page, library and books
  * Common - Auth , messaging and etc.    

### DataAccessLayer 
  * AppIdentity - classes for ASP.NET Identity
  * Context - context for database
  * Entities - database models 
    
### Helpers    
  * Attributes
  * Extensions
  * Logging
  * NotifyHelper
  * Services
### DataTransfer Objects    
