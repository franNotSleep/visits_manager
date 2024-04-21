using BussinessLayer;
using EntityLayer;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;
using Npgsql;
using CapaNegocio;
using DataLayer;

namespace FrontendLayer
{
    public partial class Frontend : MaterialForm
    {
        BuildingModel buildingModel;
        UserModel userModel;
        VisitModel visitModel;
        ClassroomModel classroomModel;
        NpgsqlDataSource dataSource;

        User user;
        List<Building> buildings = new List<Building>();
        public Frontend()
        {
            InitializeComponent();
            InitializeMaterialSkin();
            InitializeDatabaseConnection();

            if (this.user == null)
            {
                this.loginCard.Dock = DockStyle.Fill;
                this.loginCard.BringToFront();
                this.card1.Hide();
                this.card2.Hide();
                this.card3.Hide();
            }

            userModel = new UserModel(dataSource);
            classroomModel = new ClassroomModel(dataSource);
            visitModel = new VisitModel();
            buildingModel = new BuildingModel();

            seedComboBoxWithBuilding();

            this.buildingListView.Columns.Add("Id", 100);
            this.buildingListView.Columns.Add("Nombre", 200);

            this.classroomsListView.Columns.Add("Id", 100);
            this.classroomsListView.Columns.Add("Nombre", 200);

            this.visitsViewList.Columns.Add("Id", 50);
            this.visitsViewList.Columns.Add("Nombre", 100);
            this.visitsViewList.Columns.Add("Apellido", 100);
            this.visitsViewList.Columns.Add("Correo", 150);
            this.visitsViewList.Columns.Add("Razon", 150);
            this.visitsViewList.Columns.Add("Llegada", 150);
            this.visitsViewList.Columns.Add("Salida", 150);

            PopulateVisitsListView();
            this.controls.SelectedIndexChanged += Controls_SelectedIndexChanged;
        }

        private void Controls_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.controls.SelectedIndex == 2)
                this.card2.Hide();
            else if (this.user.Role != "normal")
                this.card2.Show();
        }

        async private void seedComboBoxWithBuilding()
        {
            List<Building> buildings = await buildingModel.GetBuildings();
            this.buildings = buildings;
            this.buildingListView.Clear();

            foreach (Building building in buildings)
            {
                bbuildingComboBox.Items.Add(building.Name);
                cbuildingComboBox.Items.Add(building.Name);

                ListViewItem buildingItem = new ListViewItem();

                buildingItem.SubItems[0].Text = building.Id.ToString();
                buildingItem.SubItems.Add(building.Name);

                this.buildingListView.Items.Add(buildingItem);

                foreach (var classroom  in building.Classrooms)
                {
                    ListViewItem classroomItem = new ListViewItem();
                    classroomItem.SubItems[0].Text = classroom.Id.ToString();
                    classroomItem.SubItems.Add(classroom.Name);
                    classroomsListView.Items.Add(classroomItem);
                }
            }

            bbuildingComboBox.SelectedIndexChanged += bbuildingComboBox_SelectedIndexChanged;
        }


        async private void bbuildingComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bclassroomComboBox.Items.Clear();

            string selectedBuildingName = bbuildingComboBox.SelectedItem.ToString();

            Building selectedBuilding = this.buildings.FirstOrDefault(building => building.Name == selectedBuildingName);

            if (selectedBuilding != null)
            {
                foreach (Classroom classroom in selectedBuilding.Classrooms)
                {
                    bclassroomComboBox.Items.Add(classroom.Name);
                }
            }
        }

        private void InitializeDatabaseConnection()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=droide03;Database=visitsdb";
            NpgsqlDataSource dataSource = NpgsqlDataSource.Create(connectionString);
            this.dataSource = dataSource;
        }
        private void InitializeMaterialSkin()
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo900, Primary.Indigo500, Primary.Blue500, Accent.LightBlue200, TextShade.WHITE);
        }

        async private void materialButton1_Click(object sender, EventArgs e)
        {
            string email = this.emailLoginInput.Text;
            string password = this.pwdLoginInput.Text;

            if (email.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Both fields are required.");
                return;
            }

            this.user = await this.userModel.Login(email, password);

            if (this.user == null)
            {
                MessageBox.Show("Invalid Email Or Password.");
                return;
            }
            this.loginCard.Hide();
            this.card1.Show();
            if (this.user.Role != "normal")
            {
                this.card2.Show();
                this.card3.Show();
            }
        }

        async private void materialButton3_Click(object sender, EventArgs e)
        {
            var building  = await this.buildingModel.Create(this.bname.Text);

            ListViewItem buildingItem = new ListViewItem();

            buildingItem.SubItems[0].Text = building.Id.ToString();
            buildingItem.SubItems.Add(building.Name);

            this.buildingListView.Items.Add(buildingItem);
            this.buildings.Add(building);

            bbuildingComboBox.Items.Add(building.Name);
            cbuildingComboBox.Items.Add(building.Name);
        }

        async private void materialButton4_Click(object sender, EventArgs e)
        {
            string selectedBuildingName = cbuildingComboBox.SelectedItem.ToString();
            Building selectedBuilding = this.buildings.FirstOrDefault(building => building.Name == selectedBuildingName);

            Classroom classroom = await this.classroomModel.CreateClassroom(this.classroomNameInput.Text, selectedBuilding.Id);

            ListViewItem classroomItem = new ListViewItem();
            classroomItem.SubItems[0].Text = classroom.Id.ToString();
            classroomItem.SubItems.Add(classroom.Name);
            classroomsListView.Items.Add(classroomItem);

            selectedBuilding.Classrooms.Add(classroom);
        }

        private void controls_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void visitsListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        async private void materialButton2_Click(object sender, EventArgs e)
        {
            string name = this.vname.Text;
            string lastName = this.vlastName.Text;
            string fieldOfStudy = this.vcareer.Text;
            string email = this.vemail.Text;
            DateTime entry = this.ventrada.Value;
            DateTime exit = this.vsalida.Value;
            string reason = this.vreason.Text;

            string selectedBuildingName = bbuildingComboBox.SelectedItem.ToString();
            Building selectedBuilding = this.buildings.FirstOrDefault(building => building.Name == selectedBuildingName);

            Visit visit = await this.visitModel.CreateVisit(name, lastName, fieldOfStudy, email, selectedBuilding.Id, entry, exit, reason, null);
            ListViewItem visitItem = new ListViewItem();
            visitItem.Text = visit.Id.ToString();
            visitItem.SubItems.Add(name);
            visitItem.SubItems.Add(lastName); 
            visitItem.SubItems.Add(email);
            visitItem.SubItems.Add(reason);
            visitItem.SubItems.Add(entry.ToString());
            visitItem.SubItems.Add(exit.ToString()); 

            this.visitsViewList.Items.Add(visitItem);
        }
        async private void PopulateVisitsListView()
        {
            visitsViewList.Items.Clear();

            List<Visit> visits = await this.visitModel.GetAllVisits();

            foreach (Visit visit in visits)
            {
                ListViewItem visitItem = new ListViewItem();
                visitItem.Text = visit.Id.ToString();
                visitItem.SubItems.Add(visit.FirstName);
                visitItem.SubItems.Add(visit.LastName);
                visitItem.SubItems.Add(visit.Email);
                visitItem.SubItems.Add(visit.Reason);
                visitItem.SubItems.Add(visit.EntryTime.ToString());
                visitItem.SubItems.Add(visit.ExitTime.ToString());

                visitsViewList.Items.Add(visitItem);
            }
        }

        async private void materialButton5_Click(object sender, EventArgs e)
        {
            string username = this.usernameInput.Text;
            string role = this.roleInput.Text;
            string email = this.userEmailInput.Text;
            string password = this.passwordInput.Text;

            await this.userModel.CreateUser(username, email, password, role);

            MessageBox.Show("New user created.");
        }

        private void card1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
