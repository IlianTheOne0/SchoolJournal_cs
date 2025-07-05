# 📚 SchoolJournal\_cs

A desktop application for students, teachers, and school administrators to simplify and improve the management of grades, attendance, and academic progress.

Whether you're assigning final marks, reviewing your academic results, or monitoring a class's performance — SchoolJournal\_cs makes the process faster, safer, and more transparent.

&#x20;

---

## ✨ What You Can Do

* 🧑‍🏫 Teachers can easily enter and edit **grades and attendance**
* 👨‍🎓 Students can track their **own academic progress**
* 🏫 Admins can manage **classes, institutions, and user access**
* 🔐 Secure login and user roles for different types of access
* 🖼 Upload and display **user avatars**

---

## 🔍 Why This Project?

The idea came during the exhausting task of transferring grades from a private platform to a state registry — a manual and error-prone job. SchoolJournal\_cs was built to remove friction and reduce mistakes in the school data workflow.

---

## 🧩 How It Works (Under the Hood)

While users interact through a simple desktop interface, the app behind the scenes is built with a layered architecture to ensure stability and scalability.

* The app is split into layers: interface, logic, and data
* All user actions (e.g., adding a grade) go through clearly defined steps
* The system relies on a secure online backend to store and sync data

> For developers: the app uses WPF and .NET 8, and stores data using Supabase (PostgreSQL + Auth + Storage).

---

## 🏗 Project Overview

```
SchoolJournal_cs/
├── DesktopApplication/     # Interface and interaction logic
├── Database/               # Data connection and repositories
├── Models/                 # Data structures and relationships
└── supabase_keys.json      # Backend connection details
```

### What's Inside:

* **Modern, clean interface**
* **Role-based navigation and access**
* **Offline-friendly design** with smart data syncing
* **Well-structured and testable code** for future improvements

---

## ✅ Highlights

* Organized into layers: interface, logic, data
* Designed for real-life school processes
* Handles multiple institutions, classes, and users
* Flexible access control by role
* Avoids repetitive manual work by automating common tasks

---

## ⚠️ Things to Improve

* Sensitive connection data should be stored securely (not in plain files)
* Some internal codes (e.g. role IDs) are hardcoded — should be replaced by readable constants
* Data refresh can be inconsistent if multiple users edit at once
* Certain background operations could block the interface — optimization needed
* Current error messages could be more specific to help with troubleshooting
* Reuse code more
* Add SQL Lite for offline grading (for example, if the connection to the global network is lost after logging into the account)

---

## 🛠 Getting Started

**For administrators/developers:**
1. Read the license terms
2. Download the application from the link below (in the "Files for download" section) (for administrators), or the project from GitHub (for developers)
3. Create an account at [supabase](https://supabase.com/), if you don't already have one
4. On the home page, click "New Project"
5. Enter the name of your organization, the name of the project (database), and the database password. It is not necessary to specify the region. No changes are required in "Security Options" and "Advanced Configuration".
6. From the main page of your project, copy the following data to the "Database/.env/supabase_keys.json" file: "Project URL" and "API Key" (not private (i.e., for administrators), but public).
  Example of file contents:
```json
{
  "Url": "https://your-url.supabase.co",
  "Key": "your-key",
  "DefaultSchema": "public"
}
```
7. In the "SQL editor" on the supabase website, paste the contents of the file from "Database/Migrations/supabase.sql"
8. Run and test the program



*Additional information for developers:*
To successfully build the solution, replace this code in the **"Database/Repositories/Repositories.Supabase.cs"**:
```cs
public partial class RepositoriesSupabase // ...
{
    // ...
    public RepositoriesSupabase()
    {
        string solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\"));
        string filePath = solutionDirectory + @"Database\.env\supabase_keys.json";
        // ...
```

with this:
```cs
public partial class RepositoriesSupabase // ...
{
    // ...
    public RepositoriesSupabase()
    {
        string solutionDirectory = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\"));
        string filePath = Path.Combine(AppContext.BaseDirectory, @"supabase_keys.json");
        // ...
```

**And add the "supabase_keys.json" file to the built files**



**For regular users:**
1. Download the application from the link below (in the **"Presentations + Files for download"** section).
2. Open the contents of the downloaded file and replace "supabase_keys.json" provided by the administrator
3. Open the application.
4. Accept the license terms if you agree with them.
5. Enjoy using the application.

---

## 🤝 Want to Help?

Contributions are welcome — whether it’s improving the UI, optimizing performance, or fixing bugs.

To get started:

* Fork the repository
* Submit pull requests
* Report bugs and suggest improvements

---

## 🖥💾 Presentations + Files for download

Presentation (EU): [canva](https://www.canva.com/design/DAGsTZcrWQY/yJOi5EOJjdk_0n0a0GdaQQ/edit?utm_content=DAGsTZcrWQY&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)

Presentation (UA): [canva](https://www.canva.com/design/DAGsTI8etQs/yVSgTd8_daY7ilY6R3xyLQ/edit?utm_content=DAGsTI8etQs&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)

Download the application: [google drive](https://drive.google.com/file/d/1zwS0ndkUCUGdtX61venNcPBXV_NDjGkb/view?usp=sharing)

## 📬 Contact

* Email: ilian.dev.ua@gmail.com
* LinkedIn: [Ilian Shchepinskyi](https://www.linkedin.com/in/ilian-shchepinskyi)
* GitHub: [IlianTheOne0](https://github.com/IlianTheOne0)

---

## 📄 License

This project is licensed under the [Creative Commons Attribution 4.0 International (CC BY 4.0)](https://creativecommons.org/licenses/by/4.0/).

You are free to use, share, and adapt this software for any purpose, even commercially, **as long as proper credit is given** to the original author — Ilian Shchepinskyi.
