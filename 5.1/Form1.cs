
using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace LoginMoi
{
    public partial class Form1 : Form
    {
        private Label lblTitle = new Label();
        private Label lblUsername = new Label();
        private Label lblPassword = new Label();
        private TextBox txtUsername = new TextBox();
        private TextBox txtPassword = new TextBox();
        private CheckBox chkShowPassword = new CheckBox();
        private Button btnLogin = new Button();
        private Button btnExit = new Button();
        private ErrorProvider errorProvider1 = new ErrorProvider();

        private readonly string connectionString =
            @"Server=.;Database=LoginDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public Form1()
        {
            InitializeComponent();
            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            Text = "Đăng nhập hệ thống";
            Size = new Size(450, 330);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(75, 25);

            lblUsername.Text = "Tên đăng nhập:";
            lblUsername.Location = new Point(40, 90);
            lblUsername.Size = new Size(110, 25);

            txtUsername.Name = "txtUsername";
            txtUsername.Location = new Point(155, 87);
            txtUsername.Size = new Size(220, 25);

            lblPassword.Text = "Mật khẩu:";
            lblPassword.Location = new Point(40, 135);
            lblPassword.Size = new Size(110, 25);

            txtPassword.Name = "txtPassword";
            txtPassword.Location = new Point(155, 132);
            txtPassword.Size = new Size(220, 25);
            txtPassword.UseSystemPasswordChar = true;

            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Text = "Hiện mật khẩu";
            chkShowPassword.Location = new Point(155, 170);
            chkShowPassword.AutoSize = true;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;

            btnLogin.Name = "btnLogin";
            btnLogin.Text = "Đăng nhập";
            btnLogin.Location = new Point(95, 220);
            btnLogin.Size = new Size(110, 35);
            btnLogin.Click += BtnLogin_Click;

            btnExit.Name = "btnExit";
            btnExit.Text = "Thoát";
            btnExit.Location = new Point(230, 220);
            btnExit.Size = new Size(110, 35);
            btnExit.Click += (s, e) => Close();

            errorProvider1.ContainerControl = this;

            Controls.Add(lblTitle);
            Controls.Add(lblUsername);
            Controls.Add(txtUsername);
            Controls.Add(lblPassword);
            Controls.Add(txtPassword);
            Controls.Add(chkShowPassword);
            Controls.Add(btnLogin);
            Controls.Add(btnExit);

            AcceptButton = btnLogin;
            CancelButton = btnExit;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            errorProvider1.Clear();

            bool hopLe = true;

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                errorProvider1.SetError(
                    txtUsername, "Vui lòng nhập tên đăng nhập!");
                hopLe = false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                errorProvider1.SetError(
                    txtPassword, "Vui lòng nhập mật khẩu!");
                hopLe = false;
            }

            if (!hopLe)
                return;

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);

                string sql = @"SELECT COUNT(*) FROM TaiKhoan
                               WHERE TenDangNhap = @TenDangNhap
                               AND MatKhau = @MatKhau";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@TenDangNhap", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@MatKhau", txtPassword.Text);

                conn.Open();

                int ketQua = (int)cmd.ExecuteScalar();

                if (ketQua > 0)
                {
                    MessageBox.Show(
                        "Đăng nhập thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Tên đăng nhập hoặc mật khẩu không đúng!",
                        "Lỗi đăng nhập",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Không thể kết nối CSDL. Hãy kiểm tra SQL Server.\n" + ex.Message,
                    "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void chkShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
