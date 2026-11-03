<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentRegistration.aspx.cs" Inherits="StudentRegistrationSystem.StudentRegistration" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Management System</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
        }

        body {
            background-color: #f7f9fc;
            color: #333333;
        }

        .navbar {
            background-color: #2b303a;
            color: #ffffff;
            padding: 14px 28px;
            display: flex;
            align-items: center;
        }

        .navbar-brand {
            font-size: 1.15rem;
            font-weight: 600;
            color: #ffffff;
            text-decoration: none;
        }

        .page-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 30px 16px;
            gap: 30px;
        }

        .card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            width: 100%;
            max-width: 540px;
            padding: 30px 36px;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
        }

        .grid-card {
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 6px;
            width: 100%;
            max-width: 950px;
            padding: 24px 28px;
            box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.05);
            overflow-x: auto;
        }

        .card-title {
            font-size: 1.5rem;
            font-weight: 600;
            text-align: center;
            margin-bottom: 24px;
            color: #1a202c;
        }

        .form-group {
            margin-bottom: 16px;
        }

        .form-label {
            display: block;
            font-size: 0.9rem;
            font-weight: 500;
            color: #4a5568;
            margin-bottom: 6px;
        }

        .form-control {
            width: 100%;
            padding: 10px 12px;
            font-size: 0.95rem;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            outline: none;
            transition: border-color 0.15s ease;
        }

        .form-control:focus {
            border-color: #3182ce;
        }

        .form-control-sm {
            width: 100%;
            padding: 6px 8px;
            font-size: 0.88rem;
            border: 1px solid #cbd5e1;
            border-radius: 4px;
            outline: none;
        }

        .btn-group {
            display: flex;
            gap: 12px;
            margin-top: 20px;
        }

        .btn-primary {
            background-color: #007bff;
            color: #ffffff;
            border: none;
            border-radius: 4px;
            padding: 10px 24px;
            font-size: 0.95rem;
            cursor: pointer;
        }

        .btn-primary:hover {
            background-color: #0069d9;
        }

        .btn-secondary {
            background-color: #6c757d;
            color: #ffffff;
            border: none;
            border-radius: 4px;
            padding: 10px 20px;
            font-size: 0.95rem;
            cursor: pointer;
        }

        .btn-sm {
            padding: 6px 14px;
            font-size: 0.88rem;
            border-radius: 4px;
            cursor: pointer;
            border: none;
        }

        .btn-filter {
            background-color: #0d6efd;
            color: #ffffff;
        }

        .btn-reset {
            background-color: #e2e8f0;
            color: #334155;
        }

        .field-error {
            color: #e53e3e;
            font-size: 0.8rem;
            margin-top: 4px;
            display: block;
        }

        .alert-box {
            padding: 12px 16px;
            border-radius: 4px;
            margin-bottom: 18px;
            font-size: 0.9rem;
            width: 100%;
            max-width: 540px;
        }

        .alert-danger {
            background-color: #fff5f5;
            border: 1px solid #feb2b2;
            color: #c53030;
        }

        .alert-success {
            background-color: #f0fff4;
            border: 1px solid #9ae6b4;
            color: #276749;
        }

        /* Filter Toolbar */
        .filter-toolbar {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 12px;
            background-color: #f8fafc;
            padding: 14px 16px;
            border-radius: 6px;
            border: 1px solid #e2e8f0;
            margin-bottom: 16px;
        }

        .filter-item {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .students-grid {
            width: 100%;
            border-collapse: collapse;
            font-size: 0.92rem;
            margin-top: 8px;
        }

        .students-grid th {
            background-color: #f8fafc;
            color: #475569;
            font-weight: 600;
            padding: 12px;
            border-bottom: 2px solid #e2e8f0;
            text-align: left;
        }

        .students-grid th a {
            color: #1e293b;
            text-decoration: none;
        }

        .students-grid th a:hover {
            text-decoration: underline;
        }

        .students-grid td {
            padding: 10px 12px;
            border-bottom: 1px solid #e2e8f0;
            color: #334155;
            vertical-align: middle;
        }

        .students-grid tr:hover {
            background-color: #f1f5f9;
        }

        .grid-pager td {
            padding: 12px;
            background-color: #f8fafc;
        }

        .grid-pager span {
            background-color: #007bff;
            color: #ffffff;
            padding: 4px 10px;
            border-radius: 4px;
            font-weight: bold;
        }

        .grid-pager a {
            color: #007bff;
            padding: 4px 10px;
            text-decoration: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <header class="navbar">
            <span class="navbar-brand">Student Management System</span>
        </header>

        <main class="page-container">
            <asp:Label ID="lblStatusMessage" runat="server" Visible="false" />

            <!-- Form Card -->
            <section class="card">
                <h1 class="card-title">Register New Student</h1>

                <div class="form-group">
                    <label class="form-label" for="txtFirstName">First Name</label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Enter first name" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="field-error" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label class="form-label" for="txtLastName">Last Name</label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter last name" MaxLength="50" />
                    <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="field-error" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label class="form-label" for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter email" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="field-error" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email." CssClass="field-error" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label class="form-label" for="ddlCourse">Course</label>
                    <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-control">
                        <asp:ListItem Text="-- Select Course --" Value="" />
                        <asp:ListItem Text="B.Tech" Value="B.Tech" />
                        <asp:ListItem Text="BCA" Value="BCA" />
                        <asp:ListItem Text="MCA" Value="MCA" />
                        <asp:ListItem Text="M.Tech" Value="M.Tech" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCourse" runat="server" ControlToValidate="ddlCourse" InitialValue="" ErrorMessage="Please select a course." CssClass="field-error" Display="Dynamic" />
                </div>

                <div class="btn-group">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-primary" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-secondary" CausesValidation="false" OnClick="btnClear_Click" />
                </div>
            </section>

            <!-- GridView Card with Filter Toolbar -->
            <section class="grid-card">
                <h2 style="font-size: 1.3rem; margin-bottom: 12px; color: #1e293b;">Registered Students</h2>
                
                <!-- Filter Toolbar -->
                <div class="filter-toolbar">
                    <div class="filter-item">
                        <label for="txtSearch" style="font-size: 0.88rem; font-weight: 500;">Search:</label>
                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control-sm" placeholder="Name or email..." Style="width: 180px;" />
                    </div>

                    <div class="filter-item">
                        <label for="ddlFilterCourse" style="font-size: 0.88rem; font-weight: 500;">Course:</label>
                        <asp:DropDownList ID="ddlFilterCourse" runat="server" CssClass="form-control-sm" Style="width: 140px;">
                            <asp:ListItem Text="All Courses" Value="ALL" />
                            <asp:ListItem Text="B.Tech" Value="B.Tech" />
                            <asp:ListItem Text="BCA" Value="BCA" />
                            <asp:ListItem Text="MCA" Value="MCA" />
                            <asp:ListItem Text="M.Tech" Value="M.Tech" />
                        </asp:DropDownList>
                    </div>

                    <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn-sm btn-filter" CausesValidation="false" OnClick="btnFilter_Click" />
                    <asp:Button ID="btnResetFilter" runat="server" Text="Reset" CssClass="btn-sm btn-reset" CausesValidation="false" OnClick="btnResetFilter_Click" />
                </div>

                <asp:GridView 
                    ID="gvStudents" 
                    runat="server" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="StudentId"
                    CssClass="students-grid" 
                    AllowPaging="True" 
                    PageSize="5" 
                    AllowSorting="True" 
                    OnPageIndexChanging="gvStudents_PageIndexChanging" 
                    OnSorting="gvStudents_Sorting" 
                    OnRowEditing="gvStudents_RowEditing" 
                    OnRowCancelingEdit="gvStudents_RowCancelingEdit" 
                    OnRowUpdating="gvStudents_RowUpdating" 
                    OnRowDeleting="gvStudents_RowDeleting" 
                    EmptyDataText="No matching student records found.">
                    
                    <PagerStyle CssClass="grid-pager" HorizontalAlign="Center" />
                    
                    <Columns>
                        <asp:BoundField DataField="StudentId" HeaderText="ID" ReadOnly="True" SortExpression="StudentId" />
                        
                        <asp:TemplateField HeaderText="First Name" SortExpression="FirstName">
                            <ItemTemplate>
                                <%# HttpUtility.HtmlEncode(Eval("FirstName")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGridFirstName" runat="server" Text='<%# Bind("FirstName") %>' CssClass="form-control-sm" MaxLength="50" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Last Name" SortExpression="LastName">
                            <ItemTemplate>
                                <%# HttpUtility.HtmlEncode(Eval("LastName")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGridLastName" runat="server" Text='<%# Bind("LastName") %>' CssClass="form-control-sm" MaxLength="50" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email" SortExpression="Email">
                            <ItemTemplate>
                                <%# HttpUtility.HtmlEncode(Eval("Email")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtGridEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control-sm" MaxLength="100" />
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Course" SortExpression="Course">
                            <ItemTemplate>
                                <%# HttpUtility.HtmlEncode(Eval("Course")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="ddlGridCourse" runat="server" CssClass="form-control-sm">
                                    <asp:ListItem Text="B.Tech" Value="B.Tech" />
                                    <asp:ListItem Text="BCA" Value="BCA" />
                                    <asp:ListItem Text="MCA" Value="MCA" />
                                    <asp:ListItem Text="M.Tech" Value="M.Tech" />
                                </asp:DropDownList>
                            </EditItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField DataField="CreatedDate" HeaderText="Enrolled Date" ReadOnly="True" SortExpression="CreatedDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" ForeColor="#007bff" CausesValidation="false" />
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" ForeColor="#dc3545" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this student record?');" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" ForeColor="#28a745" CausesValidation="false" />
                                &nbsp;|&nbsp;
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" ForeColor="#6c757d" CausesValidation="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>
        </main>
    </form>
</body>
</html>