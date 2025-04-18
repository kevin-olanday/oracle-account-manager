# Oracle Account Manager

The **Oracle Account Manager** is a web-based administrative tool designed to simplify the management of Oracle accounts and related IT infrastructure. It provides a user-friendly interface for performing common administrative tasks, such as changing Oracle passwords and unlocking accounts, while leveraging PowerShell scripts for backend automation.

## Features

### Oracle Account Management
- Change Oracle account passwords.
- Unlock Oracle accounts.

### Email Notifications
- Send email notifications for password changes.
- Notify users about account unlocks.

## Project Structure

### Key Directories
- **`OracleAccountManager/`**: Contains the ASP.NET web application files, including `.aspx` pages, code-behind files, and configuration files.
- **`Scripts/`**: Contains PowerShell scripts for sending email notifications related to Oracle account management tasks.

### Notable Files
- **`Index.aspx`**: The main web page for the application.
- **`Web.config`**: Configuration file for the web application.
- **`OraclePasswordChanger.csproj`**: The project file for the ASP.NET application.
- **`OracleAccountUnlocker-SendEmail.ps1`**: PowerShell script for sending account unlock email notifications.
- **`OraclePasswordChanger-SendEmail.ps1`**: PowerShell script for sending password change email notifications.

## Prerequisites

### Software Requirements
- **.NET Framework**
- **IIS (Internet Information Services)**
- **Oracle Managed Data Access DLLs** for database connectivity
- **PowerShell** for executing email notification scripts

### Configuration
- Ensure the PowerShell scripts are located in the `Scripts` directory.
- Update any hardcoded paths or email server configurations in the scripts or web application files.

## How to Build and Run

1. Open the solution file `Oracle-Account-Manager.sln` in Visual Studio.
2. Build the project to generate the necessary binaries.
3. Deploy the web application to IIS or run it locally using Visual Studio's built-in web server.
4. Access the application via the configured URL (e.g., `http://localhost/OracleAccountManager/`).

## Usage

1. Navigate to the homepage (`Index.aspx`).
2. Use the provided UI to perform administrative tasks:
   - Change Oracle account passwords by filling in the required fields and submitting the form.
   - Unlock Oracle accounts using the appropriate interface.
3. Email notifications will be sent automatically upon successful operations.

## Security Considerations

- Ensure proper authentication and authorization mechanisms are in place to restrict access to the application.
- Validate all user inputs to prevent injection attacks.
- Securely store sensitive information, such as database connection strings and email server credentials, in the `Web.config` file or a secure secrets manager.

## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes and submit a pull request.

## Author

This solution was created by Kevin Olanday to provide a centralized portal for Oracle account management and tailored to meet specific organizational requirements.

## Note on Modernization

This solution is built using older technologies. Consider modernizing the application with the following alternatives:

- **Frontend**: Use modern frontend frameworks like [React](https://reactjs.org/), [Angular](https://angular.io/), or [Vue.js](https://vuejs.org/).
- **Backend**: Upgrade to [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet) for a cross-platform, high-performance backend.
- **Automation**: Leverage modern PowerShell modules or APIs for Oracle account management.
- **Deployment**: Use containerization tools like [Docker](https://www.docker.com/) and orchestration platforms like [Kubernetes](https://kubernetes.io/) for scalable deployments.
- **Authentication**: Implement modern authentication protocols like [OAuth 2.0](https://oauth.net/2/) and [OpenID Connect](https://openid.net/connect/).

## License

This project is proprietary and intended for internal use only. Contact the project owner for licensing details.