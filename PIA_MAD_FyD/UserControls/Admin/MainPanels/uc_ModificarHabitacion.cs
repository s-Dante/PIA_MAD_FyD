using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PIA_MAD_FyD.Data.DAO_s;
using PIA_MAD_FyD.Data.Entidades;

namespace PIA_MAD_FyD.UserControls.Admin.MainPanels
{
    public partial class uc_ModificarHabitacion: UserControl
    {
        List<Hotel> hoteles = Hotel_DAO.ObtenerHotelesConUbicacion();

        private Hotel hotelSeleccionado;
        private Habitacion habitacionSeleccionada;
        List<Amenidad> amenidadesSeleccionadas = new List<Amenidad>();
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

        public uc_ModificarHabitacion(Usuario usuarioLogeado)
        {
            InitializeComponent();
            treeView1.AfterSelect += treeView1_AfterSelect;
            this.Load += uc_ModificarHabitacion_Load;
            disableControlls();

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

        private void uc_ModificarHabitacion_Load(object sender, EventArgs e)
        {
            CargarHotelesYHabitaciones();
            treeView1.ExpandAll();
        }

        private void CargarHotelesYHabitaciones()
        {
            treeView1.Nodes.Clear();

            hoteles = Hotel_DAO.ObtenerHotelesConHabitaciones(); // usa el campo de clase, no una variable local

            foreach (var hotel in hoteles)
            {
                TreeNode nodoHotel = new TreeNode(hotel.nombre) { Tag = hotel };

                foreach (var habitacion in hotel.Habitaciones)
                {
                    TreeNode nodoHab = new TreeNode($"Hab. {habitacion.id_Habitacion} - {habitacion.num_Camas} camas")
                    {
                        Tag = habitacion
                    };
                    nodoHotel.Nodes.Add(nodoHab);
                }

                treeView1.Nodes.Add(nodoHotel);
            }
        }


        private void enableControlls()
        {
            numericUpDown1.Enabled = true;
            numericUpDown2.Enabled = true;
            comboBox1.Enabled = true;
            numericUpDown3.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            checkedListBox1.Enabled = true;

            button1.Visible = true;
        }

        private void disableControlls()
        {
            numericUpDown1.Enabled = false;
            numericUpDown2.Enabled = false;
            comboBox1.Enabled = false;
            numericUpDown3.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            checkedListBox1.Enabled = false;
            button1.Visible = false;
        }


        //TreeView para mostrar hoteles con habitaicones
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            e.Node.Expand();

            if (e.Node.Tag is Habitacion habitacion)
            {
                habitacionSeleccionada = habitacion;
                disableControlls();

                numericUpDown1.Value = habitacion.num_Camas;

                numericUpDown2.Minimum = 1;
                numericUpDown2.Maximum = 20;
                numericUpDown2.Value = habitacion.capacidad;
                numericUpDown3.Value = (decimal)habitacion.precio;

                // Establecer el tipo de cama
                comboBox1.SelectedItem = habitacion.tipo_Cama.ToString();

                // Establecer nivel
                switch (habitacion.nivel)
                {
                    case 'E': comboBox2.SelectedItem = "Estandar"; break;
                    case 'D': comboBox2.SelectedItem = "Doble"; break;
                    case 'S': comboBox2.SelectedItem = "Suite"; break;
                    case 'P': comboBox2.SelectedItem = "Presidencial"; break;
                }

                // Establecer vista
                switch (habitacion.vista)
                {
                    case 'M': comboBox3.SelectedItem = "Al mar"; break;
                    case 'A': comboBox3.SelectedItem = "A la Alberca"; break;
                    case 'C': comboBox3.SelectedItem = "A la Ciudad"; break;
                    case 'J': comboBox3.SelectedItem = "Al Jardin"; break;
                    case 'O': comboBox3.SelectedItem = "Otros"; break;
                }

                // Cargar amenidades seleccionadas (si usas relación N:M)
                var amenidades = Hotel_DAO.ObtenerAmenidadesPorHabitacion(habitacion.id_Habitacion);
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    Amenidad a = (Amenidad)checkedListBox1.Items[i];
                    checkedListBox1.SetItemChecked(i, amenidades.Any(am => am.id_Amenidad == a.id_Amenidad));
                }
            }

        }

        //Num Camas
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

        //Capacidad
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        //Tipo de cama
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Precio por noche
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }
        //Nivel
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

        //Vista
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Amenidades
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Modificar
        private void button2_Click(object sender, EventArgs e)
        {
            enableControlls();
        }

        //Guardar Cambios
        private void button1_Click(object sender, EventArgs e)
        {
            if (habitacionSeleccionada == null) return;

            // 1. Obtener nuevos valores
            habitacionSeleccionada.num_Camas = (int)numericUpDown1.Value;
            habitacionSeleccionada.capacidad = (int)numericUpDown2.Value;
            habitacionSeleccionada.precio = (decimal)numericUpDown3.Value;

            // Tipo de cama
            string tipoCamaTexto = comboBox1.SelectedItem.ToString();
            switch (tipoCamaTexto)
            {
                case "Individual": habitacionSeleccionada.tipo_Cama = 'I'; break;
                case "Matrimonial": habitacionSeleccionada.tipo_Cama = 'M'; break;
                case "Queen Size": habitacionSeleccionada.tipo_Cama = 'Q'; break;
                case "King Size": habitacionSeleccionada.tipo_Cama = 'K'; break;
            }

            // Nivel
            string nivelTexto = comboBox2.SelectedItem.ToString();
            habitacionSeleccionada.nivel = nivelTexto[0];

            // Vista
            string vistaTexto = comboBox3.SelectedItem.ToString();
            switch (vistaTexto)
            {
                case "Al mar": habitacionSeleccionada.vista = 'M'; break;
                case "A la Alberca": habitacionSeleccionada.vista = 'A'; break;
                case "A la Ciudad": habitacionSeleccionada.vista = 'C'; break;
                case "Al Jardin": habitacionSeleccionada.vista = 'J'; break;
                case "Otros": habitacionSeleccionada.vista = 'O'; break;
            }

            // 2. Actualizar habitación en base de datos
            bool actualizado = Hotel_DAO.ActualizarHabitacion(habitacionSeleccionada, usuarioLogeado);

            // ✅ Recopilar amenidades seleccionadas
            List<Amenidad> nuevasAmenidades = new List<Amenidad>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                nuevasAmenidades.Add((Amenidad)item);
            }

            // ✅ Actualizar amenidades
            Hotel_DAO.ActualizarAmenidadesDeHabitacion(habitacionSeleccionada.id_Habitacion, nuevasAmenidades);

            MessageBox.Show("Habitación actualizada correctamente.");


            disableControlls();
            CargarHotelesYHabitaciones();
        }
    }
}
