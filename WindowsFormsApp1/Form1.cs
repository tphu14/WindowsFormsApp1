using BUS;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly StudentService studentService = new StudentService();
        private readonly FacultyService facultyService = new FacultyService();


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dataGridView1);
                var listFacultys = facultyService.GetAll();
                var listStudents = studentService.GetAll();
                var listMajors = facultyService.GetAllMajors(); // Lấy danh sách chuyên ngành
                if (listMajors != null)
                    FillMajorCombobox(listMajors);
                FillFalcultyCombobox(listFacultys);
                BindGrid(listStudents);
                dataGridView1.CellClick += dataGridView1_CellClick;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFacultys)
        {
            listFacultys.Insert(0, new Faculty());
            this.cbbKhoa.DataSource = listFacultys;
            this.cbbKhoa.DisplayMember = "FacultyName";
            this.cbbKhoa.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                if (item.Faculty != null)
                    dataGridView1.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells[3].Value = item.AverageScore + "";
                if (item.MajorID != null)
                    dataGridView1.Rows[index].Cells[4].Value = item.Major.Name + "";

                // Hiển thị đường dẫn ảnh vào cột mới
                dataGridView1.Rows[index].Cells[5].Value = item.Avatar; // Cột thứ 6 (index 5) sẽ chứa đường dẫn ảnh

                ShowAvatar(item.Avatar); // Hiển thị avatar vào PictureBox
            }
        }

        private void ShowAvatar(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                pictureBox1.Image = null; // Nếu không có ảnh, đặt ảnh null
            }
            else
            {
                // Lấy đường dẫn thư mục gốc của ứng dụng
                string parentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Tạo đường dẫn đến thư mục Images và tên ảnh
                string imagePath = Path.Combine(parentDirectory, "Images", imageName);

                // Kiểm tra xem file ảnh có tồn tại không
                if (File.Exists(imagePath))
                {
                    pictureBox1.Image = Image.FromFile(imagePath); // Hiển thị ảnh lên PictureBox
                    pictureBox1.Refresh();
                }
                else
                {
                    MessageBox.Show("Ảnh không tồn tại tại: " + imagePath);
                }
            }
        }


        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var listStudents = new List<Student>();
            if (this.checkBox1.Checked)
                listStudents = studentService.GetAllHasNoMajor();
            else
                listStudents = studentService.GetAll();
            BindGrid(listStudents);
        
    }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu nhấp vào dòng hợp lệ (không phải header)
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ các cột trong dòng đã chọn
                var studentID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); // Cột 0 là StudentID
                var fullName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); // Cột 1 là FullName
                var facultyName = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); // Cột 2 là FacultyName
                var averageScore = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(); // Cột 3 là AverageScore
                var majorName = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString(); // Cột 4 là MajorName
                var avatar = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString(); // Cột 5 là Avatar

                // Gán các giá trị vào các ô nhập tương ứng
                txtID.Text = studentID;           // Gán StudentID vào TextBox txtID
                txtName.Text = fullName;          // Gán FullName vào TextBox txtName
                txtDiemTB.Text = averageScore;    // Gán AverageScore vào TextBox txtDiemTB

                // Gán vào ComboBox cbbKhoa
                foreach (var item in cbbKhoa.Items)
                {
                    if (item is Faculty faculty && faculty.FacultyName == facultyName)
                    {
                        cbbKhoa.SelectedItem = item; // Chọn khoa đúng trong ComboBox
                        break;
                    }
                }

                // Gán vào ComboBox hoặc các TextBox khác nếu có
                 cbbMajor.Text = majorName;     // Nếu có TextBox cho MajorName
                ShowAvatar(avatar);              // Hiển thị ảnh vào PictureBox (nếu có)
            }
        }
        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbbKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtDiemTB_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void FillMajorCombobox(List<Major> listMajors)
        {
            listMajors.Insert(0, new Major { MajorID = 0, Name = "" });
            cbbMajor.DataSource = listMajors;
            cbbMajor.DisplayMember = "Name";
            cbbMajor.ValueMember = "MajorID";
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                // Mở hộp thoại để chọn ảnh
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

                // Kiểm tra nếu người dùng chọn ảnh
                string imagePath = null;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Lấy đường dẫn ảnh được chọn
                    imagePath = openFileDialog.FileName;

                    // Lưu ảnh vào thư mục Images
                    string fileName = Path.GetFileName(imagePath); // Lấy tên file ảnh
                    string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName); // Đường dẫn mới
                    File.Copy(imagePath, destinationPath, true); // Sao chép ảnh vào thư mục Images
                }

                // Tạo đối tượng student với thông tin từ giao diện
                var student = new Student
                {
                    StudentID = int.Parse(txtID.Text),
                    FullName = txtName.Text,
                    AverageScore = float.Parse(txtDiemTB.Text),
                    FacultyID = (int)cbbKhoa.SelectedValue,
                    MajorID = (int?)cbbMajor.SelectedValue,
                    Avatar = Path.GetFileName(imagePath) // Lưu tên file ảnh vào Avatar
                };

                // Gọi phương thức thêm sinh viên vào cơ sở dữ liệu
                studentService.Add(student); // Gọi phương thức thêm

                MessageBox.Show("Thêm sinh viên thành công!");

                // Làm mới DataGridView
                BindGrid(studentService.GetAll());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể thêm sinh viên. Lỗi: {ex.Message}");
            }
        }


        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                int studentID = int.Parse(txtID.Text);
                var student = studentService.GetById(studentID);

                if (student != null)
                {
                    student.FullName = txtName.Text;
                    student.AverageScore = float.Parse(txtDiemTB.Text);
                    student.FacultyID = (int)cbbKhoa.SelectedValue;
                    student.MajorID = (int?)cbbMajor.SelectedValue;

                    // Kiểm tra nếu có ảnh mới được chọn
                    if (pictureBox1.Image != null)
                    {
                        // Lưu ảnh vào thư mục Images
                        string fileName = txtID.Text + Path.GetExtension(pictureBox1.ImageLocation); // Tên file mới
                        string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", fileName);
                        pictureBox1.Image.Save(destinationPath); // Lưu ảnh

                        student.Avatar = fileName; // Cập nhật đường dẫn ảnh
                    }

                    studentService.Update(student); // Gọi phương thức cập nhật
                    MessageBox.Show("Cập nhật sinh viên thành công!");

                    // Làm mới DataGridView
                    BindGrid(studentService.GetAll());
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để cập nhật!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật sinh viên. Lỗi: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int studentID = int.Parse(txtID.Text);
                var student = studentService.GetById(studentID);

                if (student != null)
                {
                    // Xóa ảnh nếu tồn tại
                    if (!string.IsNullOrEmpty(student.Avatar))
                    {
                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", student.Avatar);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath); // Xóa file ảnh
                        }
                    }

                    // Xóa sinh viên trong cơ sở dữ liệu
                    studentService.Delete(studentID);
                    MessageBox.Show("Xóa sinh viên thành công!");

                    // Làm mới DataGridView
                    BindGrid(studentService.GetAll());
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để xóa!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnAVT_Click(object sender, EventArgs e)
        {
            // Tạo một đối tượng OpenFileDialog để chọn ảnh
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            // Hiển thị hộp thoại cho phép người dùng chọn ảnh
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Lấy đường dẫn ảnh
                string imagePath = openFileDialog.FileName;

                // Giả sử bạn có một đối tượng sinh viên (Student) mà bạn muốn cập nhật ảnh
                // Ví dụ: lấy sinh viên từ cơ sở dữ liệu theo ID hoặc thông tin khác
                int studentID = 1;  // Thay bằng ID sinh viên thực tế
                using (var context = new MyDbContext())
                {
                    var student = context.Students.FirstOrDefault(s => s.StudentID == studentID);
                    if (student != null)
                    {
                        // Cập nhật đường dẫn ảnh vào thuộc tính Avatar
                        student.Avatar = imagePath;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
