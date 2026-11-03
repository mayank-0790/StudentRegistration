using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentRegistrationSystem
{
    public partial class StudentRegistration : Page
    {
        protected Label lblStatusMessage;
        protected TextBox txtFirstName;
        protected TextBox txtLastName;
        protected TextBox txtEmail;
        protected DropDownList ddlCourse;
        protected Button btnSubmit;
        protected Button btnClear;

        // Phase 5 filter controls
        protected TextBox txtSearch;
        protected DropDownList ddlFilterCourse;
        protected Button btnFilter;
        protected Button btnResetFilter;

        protected GridView gvStudents;

        private readonly StudentRepository _repository = new StudentRepository();

        private static readonly Regex EmailPattern = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase, 
            TimeSpan.FromMilliseconds(250)
        );

        private string SortDirectionState
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }
        }

        private string SortExpressionState
        {
            get { return ViewState["SortExpression"] as string ?? "StudentId"; }
            set { ViewState["SortExpression"] = value; }
        }

        private string ActiveSearchTerm
        {
            get { return ViewState["SearchTerm"] as string ?? string.Empty; }
            set { ViewState["SearchTerm"] = value; }
        }

        private string ActiveCourseFilter
        {
            get { return ViewState["CourseFilter"] as string ?? "ALL"; }
            set { ViewState["CourseFilter"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblStatusMessage.Visible = false;
                BindGrid();
            }
        }

        private void BindGrid()
        {
            string sortOrder = string.Format("{0} {1}", SortExpressionState, SortDirectionState);
            DataTable dt = _repository.GetFilteredStudents(ActiveSearchTerm, ActiveCourseFilter, sortOrder);
            gvStudents.DataSource = dt;
            gvStudents.DataBind();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            ActiveSearchTerm = txtSearch.Text != null ? txtSearch.Text.Trim() : string.Empty;
            ActiveCourseFilter = ddlFilterCourse.SelectedValue ?? "ALL";
            gvStudents.PageIndex = 0;
            BindGrid();
        }

        protected void btnResetFilter_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlFilterCourse.SelectedValue = "ALL";
            ActiveSearchTerm = string.Empty;
            ActiveCourseFilter = "ALL";
            gvStudents.PageIndex = 0;
            BindGrid();
        }

        protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStudents.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortExpressionState == e.SortExpression)
            {
                SortDirectionState = (SortDirectionState == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                SortExpressionState = e.SortExpression;
                SortDirectionState = "ASC";
            }
            BindGrid();
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            BindGrid();

            GridViewRow row = gvStudents.Rows[e.NewEditIndex];
            DropDownList ddlCourseEdit = row.FindControl("ddlGridCourse") as DropDownList;
            if (ddlCourseEdit != null)
            {
                string sortOrder = string.Format("{0} {1}", SortExpressionState, SortDirectionState);
                DataTable dt = _repository.GetFilteredStudents(ActiveSearchTerm, ActiveCourseFilter, sortOrder);
                int actualRowIndex = (gvStudents.PageIndex * gvStudents.PageSize) + e.NewEditIndex;
                if (actualRowIndex < dt.Rows.Count)
                {
                    string currentCourse = dt.Rows[actualRowIndex]["Course"].ToString();
                    ListItem item = ddlCourseEdit.Items.FindByValue(currentCourse);
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            BindGrid();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvStudents.Rows[e.RowIndex];

            TextBox txtFirst = row.FindControl("txtGridFirstName") as TextBox;
            TextBox txtLast = row.FindControl("txtGridLastName") as TextBox;
            TextBox txtMail = row.FindControl("txtGridEmail") as TextBox;
            DropDownList ddlCrs = row.FindControl("ddlGridCourse") as DropDownList;

            string firstName = txtFirst != null ? txtFirst.Text.Trim() : string.Empty;
            string lastName = txtLast != null ? txtLast.Text.Trim() : string.Empty;
            string email = txtMail != null ? txtMail.Text.Trim() : string.Empty;
            string course = ddlCrs != null ? ddlCrs.SelectedValue : string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(course))
            {
                ShowAlert("Fields cannot be empty during update.", isError: true);
                return;
            }

            if (!IsValidEmailFormat(email))
            {
                ShowAlert("Invalid email address provided for update.", isError: true);
                return;
            }

            Student updatedStudent = new Student
            {
                StudentId = studentId,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Course = course
            };

            string errorMsg;
            bool success = _repository.UpdateStudent(updatedStudent, out errorMsg);

            if (success)
            {
                gvStudents.EditIndex = -1;
                ShowAlert("Student record updated successfully!", isError: false);
                BindGrid();
            }
            else
            {
                ShowAlert(errorMsg, isError: true);
            }
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            string errorMsg;
            bool success = _repository.DeleteStudent(studentId, out errorMsg);

            if (success)
            {
                ShowAlert("Student record deleted successfully!", isError: false);
                BindGrid();
            }
            else
            {
                ShowAlert(errorMsg, isError: true);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ShowAlert("Please resolve all input validation errors before continuing.", isError: true);
                return;
            }

            string firstName = txtFirstName.Text != null ? txtFirstName.Text.Trim() : string.Empty;
            string lastName = txtLastName.Text != null ? txtLastName.Text.Trim() : string.Empty;
            string email = txtEmail.Text != null ? txtEmail.Text.Trim() : string.Empty;
            string course = ddlCourse.SelectedValue != null ? ddlCourse.SelectedValue.Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(course))
            {
                ShowAlert("All fields are mandatory.", isError: true);
                return;
            }

            if (!IsValidEmailFormat(email))
            {
                ShowAlert("Please enter a valid email address.", isError: true);
                return;
            }

            Student student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Course = course
            };

            string errorMessage;
            bool isSaved = _repository.InsertStudent(student, out errorMessage);

            if (!isSaved)
            {
                ShowAlert(errorMessage, isError: true);
                return;
            }

            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlCourse.SelectedIndex = 0;

            ShowAlert("Student registered and added to list successfully!", isError: false);
            BindGrid();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlCourse.SelectedIndex = 0;
            lblStatusMessage.Visible = false;
        }

        private bool IsValidEmailFormat(string email)
        {
            try
            {
                return EmailPattern.IsMatch(email);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        private void ShowAlert(string message, bool isError)
        {
            lblStatusMessage.Text = HttpUtility.HtmlEncode(message);
            lblStatusMessage.CssClass = isError ? "alert-box alert-danger" : "alert-box alert-success";
            lblStatusMessage.Visible = true;
        }
    }
}