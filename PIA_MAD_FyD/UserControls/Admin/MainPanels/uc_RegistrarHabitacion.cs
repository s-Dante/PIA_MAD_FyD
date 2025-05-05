using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.Entidades;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PIA_MAD_FyD.Data.DAO_s;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_RegistrarHabitacion: UserControl
    {
        private Hotel seleccionado;

        List<Hotel> hoteles = Hotel_DAO.ObtenerHotelesConUbicacion();

        Usuario usuarioLogeado;

        class HabitacionConfig
        {
            public int MaxCamas { get; set; }
            public List<string> TiposCama { get; set; }
            public int MaxCapacidadPorCama { get; set; }
        }


        Dictionary<string, HabitacionConfig> reglas = new Dictionary<string, HabitacionConfig>
        {
            { "E", new HabitacionConfig { MaxCamas = 2, TiposCama = new List<string>{ "Individual", "Matrimonial" }, MaxCapacidadPorCama = 1 } },
            { "D", new HabitacionConfig { MaxCamas = 3, TiposCama = new List<string>{ "Matrimonial", "Queen Size" }, MaxCapacidadPorCama = 2 } },
            { "S", new HabitacionConfig { MaxCamas = 3, TiposCama = new List<string>{ "Queen Size", "King Size" }, MaxCapacidadPorCama = 2 } },
            { "P", new HabitacionConfig { MaxCamas = 5, TiposCama = new List<string>{ "King Size" }, MaxCapacidadPorCama = 3 } }
        };


        public uc_RegistrarHabitacion(Usuario usuarioLogeado)
        {
            InitializeComponent();
            disableControlls();

            listView1.Items.Clear();
            listView1.FullRowSelect = true;

            foreach (var hotel in hoteles)
            {
                var item = new ListViewItem(hotel.nombre);
                string ubication = hotel.ubicacionHotel.pais + ", " + hotel.ubicacionHotel.estado + ", " + hotel.ubicacionHotel.ciudad;
                item.SubItems.Add(ubication);
                string direction = "Colonia: " + hotel.colonia + ", " + hotel.numero + ", Calle:  " + hotel.calle;
                item.SubItems.Add(direction);
                item.Tag = hotel; // Guarda el objeto completo
                listView1.Items.Add(item);
            }

            comboBox2.Items.AddRange(new string[] { "Estandar", "Doble", "Suite", "Presidencial" });
            comboBox3.Items.AddRange(new string[] { "Al mar", "A la Alberca", "A la Ciudad", "Al Jardin", "Otros" }); //-- M = Ve al Mar, A = Vista a la Alberca, C = Vista a la Ciudad, J = Vista al Jardin, O = Otro

            this.usuarioLogeado = usuarioLogeado;

            // Agregar las amenidades al CheckedListBox
            checkedListBox1.Items.Clear();
            List<Amenidad> listaAmenidades = Amenidad_DAO.ObtenerAmenidades();
            foreach (Amenidad a in listaAmenidades)
            {
                checkedListBox1.Items.Add(a); 
            }


        }

        private void uc_RegistrarHabitacion_Load(object sender, EventArgs e)
        {

        }

        private void disableControlls()
        {
            numericUpDown1.Enabled = false; //Numero de camas
            numericUpDown2.Enabled = false; //Capacidad de la habitacion
            comboBox1.Enabled = false; //Tipo de Cama
            numericUpDown3.Enabled = false; //Precio por noche
            comboBox2.Enabled = false; //Nivel de la habitacion
            comboBox3.Enabled = false; //Vista de la habitacion
            checkedListBox1.Enabled = false; //Amenidades
            button2.Enabled = false; //Registrar Habitacion
        }

        private void enableControlls()
        {
            numericUpDown1.Enabled = true; //Numero de camas
            numericUpDown2.Enabled = true; //Capacidad de la habitacion
            comboBox1.Enabled = true; //Tipo de Cama
            numericUpDown3.Enabled = true; //Precio por noche
            comboBox2.Enabled = true; //Nivel de la habitacion
            comboBox3.Enabled = true; //Vista de la habitacion
            checkedListBox1.Enabled = true; //Amenidades
            button2.Enabled = true; //Registrar Habitacion
        }


        //Mostrar la lista de hoteles
        //--> habilitar los controles solo cuando se seleccione un hotel
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableControlls();
        }

        //Nivel de la habitacion
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null) return;

            string nivel = comboBox2.SelectedItem.ToString().Substring(0, 1); // Obtener "E", "D", etc.
            if (reglas.TryGetValue(nivel, out var config))
            {
                // Limitar número de camas
                numericUpDown1.Minimum = 1;
                numericUpDown1.Maximum = config.MaxCamas;
                numericUpDown1.Value = 1;

                // Llenar combo de tipos de cama
                comboBox1.Items.Clear();
                foreach (var tipo in config.TiposCama)
                    comboBox1.Items.Add(tipo);
                comboBox1.SelectedIndex = 0;

                // Capacidad inicial
                numericUpDown2.Minimum = 1;
                numericUpDown2.Maximum = config.MaxCapacidadPorCama * (int)numericUpDown1.Value;
                numericUpDown2.Value = numericUpDown2.Maximum;
            }
        }


        //Numero de camas
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null) return;

            string nivel = comboBox2.SelectedItem.ToString().Substring(0, 1);
            if (reglas.TryGetValue(nivel, out var config))
            {
                int camas = (int)numericUpDown1.Value;
                numericUpDown2.Maximum = camas * config.MaxCapacidadPorCama;

                if (numericUpDown2.Value > numericUpDown2.Maximum)
                    numericUpDown2.Value = numericUpDown2.Maximum;
            }
        }



        //Capacidad de la habitacion
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        //Tipo de Cama
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
             Tipos de cama a tener:
                -- I = Individul, M = Matrimonial, Q = Queen Size, K = King Size
             */
        }

        //Precio por noche
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        //Vista de la habitacion
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Amenidades
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Registrar Habitacion
        private void button2_Click(object sender, EventArgs e)
        {
            seleccionado = (Hotel)listView1.SelectedItems[0].Tag;
            if (seleccionado == null)
            {
                MessageBox.Show("Selecciona un hotel primero.");
                return;
            }

            char nivel;
            switch (comboBox2.SelectedItem.ToString())
            {
                case "Estandar": nivel = 'E'; break;
                case "Doble": nivel = 'D'; break;
                case "Suite": nivel = 'S'; break;
                case "Presidencial": nivel = 'P'; break;
                default: nivel = 'O'; break;
            }

            char vista;
            switch (comboBox3.SelectedItem.ToString())
            {
                case "Al mar": vista = 'M'; break;
                case "A la Alberca": vista = 'A'; break;
                case "A la Ciudad": vista = 'C'; break;
                case "Al Jardin": vista = 'J'; break;
                default: vista = 'O'; break;
            }

            char tipoCama;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Individual": tipoCama = 'I'; break;
                case "Matrimonial": tipoCama = 'M'; break;
                case "Queen Size": tipoCama = 'Q'; break;
                default: tipoCama = 'K'; break;
            }

            Habitacion nuevaHabitacion = new Habitacion();

            nuevaHabitacion.num_Camas = (int)numericUpDown1.Value;
            nuevaHabitacion.tipo_Cama = tipoCama;
            nuevaHabitacion.precio = (decimal)numericUpDown3.Value;
            nuevaHabitacion.capacidad = (int)numericUpDown2.Value;
            nuevaHabitacion.nivel = nivel;
            nuevaHabitacion.vista = vista;
            nuevaHabitacion.usuario_Registrador = usuarioLogeado.num_Nomina;
            nuevaHabitacion.usuario_Modifico = usuarioLogeado.num_Nomina;
            nuevaHabitacion.id_Hotel = seleccionado.id_Hotel;

            List<int> amenidades = new List<int>();

            foreach (object item in checkedListBox1.CheckedItems)
            {
                Amenidad a = (Amenidad)item;
                amenidades.Add(a.id_Amenidad);
            }


            bool exito = Habitacion_DAO.InsertarHabitacion(nuevaHabitacion, amenidades);
            if (exito)
                MessageBox.Show("Habitación registrada exitosamente.");
            else
                MessageBox.Show("Ocurrió un error al registrar la habitación.");

            disableControlls();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 0;
            checkedListBox1.ClearSelected();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
