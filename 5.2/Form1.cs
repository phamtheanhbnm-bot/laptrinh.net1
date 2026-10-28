
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuanLySinhVien
{
    public class Student
    {
        public string StudentId { get; set; } = "";
        public string FullName { get; set; } = "";
        public int Age { get; set; }
        public string Gender { get; set; } = "Nam";
        public string AvatarPath { get; set; } = "";
    }

    public partial class Form1 : Form
    {
        private BindingList<Student> students = new BindingList<Student>();
        private BindingSource source = new BindingSource();

        private TextBox txtId = new TextBox();
        private TextBox txtName = new TextBox();
        private TextBox txtAge = new TextBox();
        private ComboBox cboGender = new ComboBox();
        private PictureBox picAvatar = new PictureBox();
        private DataGridView dgv = new DataGridView();
        private Label lblCount = new Label();

        public Form1()
        {
            InitializeComponent();
            BuildInterface();
            BindData();
        }

        private void BuildInterface()
        {
            Text = "QUAN LY SINH VIEN";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 650);
            MinimumSize = new Size(850, 500);

            TableLayoutPanel main = new TableLayoutPanel();
            main.Dock = DockStyle.Fill;
            main.ColumnCount = 2;
            main.RowCount = 1;
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
            Controls.Add(main);

            Panel left = new Panel();
            left.Dock = DockStyle.Fill;
            left.Padding = new Padding(12);
            main.Controls.Add(left, 0, 0);

            Label title = new Label();
            title.Text = "THONG TIN SINH VIEN";
            title.Font = new Font("Arial", 15, FontStyle.Bold);
            title.Dock = DockStyle.Top;
            title.Height = 42;
            title.TextAlign = ContentAlignment.MiddleCenter;
            left.Controls.Add(title);

            TableLayoutPanel fields = new TableLayoutPanel();
            fields.Dock = DockStyle.Top;
            fields.Height = 175;
            fields.ColumnCount = 2;
            fields.RowCount = 4;
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
            fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));

            for (int i = 0; i < 4; i++)
                fields.RowStyles.Add(new RowStyle(SizeType.Percent, 25));

            AddField(fields, "Ma sinh vien:", txtId, 0);
            AddField(fields, "Ho va ten:", txtName, 1);
            AddField(fields, "Tuoi:", txtAge, 2);

            cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGender.Items.AddRange(new object[] { "Nam", "Nu" });
            AddField(fields, "Gioi tinh:", cboGender, 3);
            left.Controls.Add(fields);

            Label avatarTitle = new Label();
            avatarTitle.Text = "ANH DAI DIEN";
            avatarTitle.Dock = DockStyle.Top;
            avatarTitle.Height = 30;
            avatarTitle.TextAlign = ContentAlignment.MiddleCenter;
            left.Controls.Add(avatarTitle);

            picAvatar.Dock = DockStyle.Top;
            picAvatar.Height = 145;
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            left.Controls.Add(picAvatar);

            Button btnImage = new Button();
            btnImage.Text = "Chon anh";
            btnImage.Dock = DockStyle.Top;
            btnImage.Height = 35;
            btnImage.Click += BtnImage_Click;
            left.Controls.Add(btnImage);

            FlowLayoutPanel buttons = new FlowLayoutPanel();
            buttons.Dock = DockStyle.Top;
            buttons.Height = 100;
            buttons.Padding = new Padding(0, 8, 0, 0);
            left.Controls.Add(buttons);

            Button btnAdd = new Button();
            btnAdd.Text = "Them";
            btnAdd.Width = 90;
            btnAdd.Height = 35;
            btnAdd.Click += BtnAdd_Click;
            buttons.Controls.Add(btnAdd);

            Button btnUpdate = new Button();
            btnUpdate.Text = "Cap nhat";
            btnUpdate.Width = 90;
            btnUpdate.Height = 35;
            btnUpdate.Click += BtnUpdate_Click;
            buttons.Controls.Add(btnUpdate);

            Button btnDelete = new Button();
            btnDelete.Text = "Xoa";
            btnDelete.Width = 90;
            btnDelete.Height = 35;
            btnDelete.Click += BtnDelete_Click;
            buttons.Controls.Add(btnDelete);

            Button btnExport = new Button();
            btnExport.Text = "Xuat CSV";
            btnExport.Width = 110;
            btnExport.Height = 35;
            btnExport.Click += BtnExport_Click;
            buttons.Controls.Add(btnExport);

            Panel right = new Panel();
            right.Dock = DockStyle.Fill;
            right.Padding = new Padding(10);
            main.Controls.Add(right, 1, 0);

            Label listTitle = new Label();
            listTitle.Text = "DANH SACH SINH VIEN";
            listTitle.Font = new Font("Arial", 15, FontStyle.Bold);
            listTitle.Dock = DockStyle.Top;
            listTitle.Height = 45;
            listTitle.TextAlign = ContentAlignment.MiddleCenter;
            right.Controls.Add(listTitle);

            dgv.Dock = DockStyle.Fill;
            dgv.AutoGenerateColumns = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgv.BackgroundColor = Color.White;
            right.Controls.Add(dgv);

            lblCount.Dock = DockStyle.Bottom;
            lblCount.Height = 30;
            lblCount.TextAlign = ContentAlignment.MiddleLeft;
            right.Controls.Add(lblCount);
        }

        private void AddField(TableLayoutPanel panel, string label,
            Control control, int row)
        {
            Label lbl = new Label();
            lbl.Text = label;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleLeft;

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(3, 5, 3, 5);

            panel.Controls.Add(lbl, 0, row);
            panel.Controls.Add(control, 1, row);
        }

        private void BindData()
        {
            source.DataSource = students;
            dgv.DataSource = source;

            txtId.DataBindings.Add("Text", source,
                "StudentId", true, DataSourceUpdateMode.OnPropertyChanged);

            txtName.DataBindings.Add("Text", source,
                "FullName", true, DataSourceUpdateMode.OnPropertyChanged);

            txtAge.DataBindings.Add("Text", source,
                "Age", true, DataSourceUpdateMode.OnPropertyChanged);

            cboGender.DataBindings.Add("SelectedItem", source,
                "Gender", true, DataSourceUpdateMode.OnPropertyChanged);

            dgv.DataBindingComplete += (s, e) =>
            {
                if (dgv.Columns.Contains("StudentId"))
                    dgv.Columns["StudentId"].HeaderText = "Ma SV";

                if (dgv.Columns.Contains("FullName"))
                    dgv.Columns["FullName"].HeaderText = "Ho va ten";

                if (dgv.Columns.Contains("Age"))
                    dgv.Columns["Age"].HeaderText = "Tuoi";

                if (dgv.Columns.Contains("Gender"))
                    dgv.Columns["Gender"].HeaderText = "Gioi tinh";

                if (dgv.Columns.Contains("AvatarPath"))
                    dgv.Columns["AvatarPath"].Visible = false;
            };

            source.CurrentChanged += (s, e) => ShowAvatar();
            students.ListChanged += (s, e) => UpdateCount();

            UpdateCount();
            ShowAvatar();
        }

        private void UpdateCount()
        {
            lblCount.Text = "Tong so sinh vien: " + students.Count;
        }

        private void ShowAvatar()
        {
            picAvatar.Image = null;

            if (source.Current is Student st &&
                !string.IsNullOrWhiteSpace(st.AvatarPath) &&
                File.Exists(st.AvatarPath))
            {
                try
                {
                    using (Image original = Image.FromFile(st.AvatarPath))
                    {
                        picAvatar.Image = new Bitmap(original);
                    }
                }
                catch
                {
                    picAvatar.Image = null;
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            source.EndEdit();

            string id = txtId.Text.Trim();
            string name = txtName.Text.Trim();

            if (id == "" || name == "")
            {
                MessageBox.Show("Vui long nhap ma sinh vien va ho ten!");
                return;
            }

            if (students.Any(s => s.StudentId == id))
            {
                MessageBox.Show("Ma sinh vien da ton tai!");
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 150)
            {
                MessageBox.Show("Tuoi phai la so tu 1 den 150!");
                return;
            }

            Student st = new Student
            {
                StudentId = id,
                FullName = name,
                Age = age,
                Gender = cboGender.SelectedItem?.ToString() ?? "Nam"
            };

            students.Add(st);
            source.Position = students.Count - 1;
            MessageBox.Show("Them sinh vien thanh cong!");
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (source.Current is not Student st)
            {
                MessageBox.Show("Vui long chon sinh vien can cap nhat!");
                return;
            }

            source.EndEdit();

            if (string.IsNullOrWhiteSpace(st.StudentId) ||
                string.IsNullOrWhiteSpace(st.FullName))
            {
                MessageBox.Show("Ma sinh vien va ho ten khong duoc de trong!");
                return;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 1 || age > 150)
            {
                MessageBox.Show("Tuoi phai la so tu 1 den 150!");
                return;
            }

            st.Age = age;
            st.Gender = cboGender.SelectedItem?.ToString() ?? "Nam";

            dgv.Refresh();
            MessageBox.Show("Cap nhat sinh vien thanh cong!");
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (source.Current is not Student st)
            {
                MessageBox.Show("Vui long chon sinh vien can xoa!");
                return;
            }

            if (MessageBox.Show("Ban co chac muon xoa sinh vien nay?",
                "Xac nhan", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                students.Remove(st);
                ShowAvatar();
            }
        }

        private void BtnImage_Click(object sender, EventArgs e)
        {
            if (source.Current is not Student st)
            {
                MessageBox.Show("Hay them hoac chon sinh vien truoc!");
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Chon anh dai dien";
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    st.AvatarPath = dialog.FileName;
                    ShowAvatar();
                    dgv.Refresh();
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            source.EndEdit();

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Title = "Xuat danh sach sinh vien";
                dialog.Filter = "CSV files (*.csv)|*.csv";
                dialog.FileName = "DanhSachSinhVien.csv";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(
                            dialog.FileName, false, new UTF8Encoding(true)))
                        {
                            writer.WriteLine("Ma SV,Ho va ten,Tuoi,Gioi tinh,Duong dan anh");

                            foreach (Student st in students)
                            {
                                writer.WriteLine(string.Join(",",
                                    Csv(st.StudentId),
                                    Csv(st.FullName),
                                    Csv(st.Age.ToString()),
                                    Csv(st.Gender),
                                    Csv(st.AvatarPath)));
                            }
                        }

                        MessageBox.Show("Xuat CSV thanh cong!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Loi khi xuat file: " + ex.Message);
                    }
                }
            }
        }

        private string Csv(string value)
        {
            value ??= "";
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }
    }
}
