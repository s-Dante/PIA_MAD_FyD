using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIA_MAD_FyD.Helpers.FormManager
{
    class FormManager
    {
        private static Dictionary<Type, Form> forms = new Dictionary<Type, Form>();

        // ✅ Mostrar formulario (verifica si ya existe)
        public static Form ShowForm<T>(Form currentForm = null, bool cerrarAppAlCerrar = false, bool ocultarActual = false) where T : Form, new()
        {
            Form form;

            if (forms.ContainsKey(typeof(T)) && !forms[typeof(T)].IsDisposed)
            {
                form = forms[typeof(T)];
                form.BringToFront();
                form.WindowState = FormWindowState.Normal;
            }
            else
            {
                form = new T();
                form.FormClosed += (s, e) => forms.Remove(typeof(T));

                if (cerrarAppAlCerrar)
                    form.FormClosed += (s, e) => Application.Exit();

                forms[typeof(T)] = form;
            }

            if (ocultarActual && currentForm != null)
            {
                currentForm.Hide();
            }

            form.Show();
            return form;
        }


        // ✅ Verificar si el formulario ya existe
        public static bool ContainsForm<T>() where T : Form
        {
            return forms.ContainsKey(typeof(T)) && !forms[typeof(T)].IsDisposed;
        }

        // ✅ Traer formulario al frente si existe
        public static void BringToFront<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].BringToFront();
                forms[typeof(T)].WindowState = FormWindowState.Normal;  // Restaurar si minimizado
            }
        }

        // ✅ Cerrar formulario específico
        public static void CloseForm<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].Close();
            }
        }

        // ✅ Minimizar formulario
        public static void MinimizeForm<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].WindowState = FormWindowState.Minimized;
            }
        }

        // ✅ Maximizar formulario
        public static void MaximizeForm<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].WindowState = FormWindowState.Maximized;
            }
        }

        // ✅ Ocultar formulario (sin cerrarlo)
        public static void HideForm<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].Hide();
            }
        }

        // ✅ Mostrar formulario oculto
        public static void ShowHiddenForm<T>() where T : Form
        {
            if (ContainsForm<T>())
            {
                forms[typeof(T)].Show();
            }
        }

        //Enlistar Forms
        public static void ListForms()
        {
            if (forms.Count == 0)
            {
                Console.WriteLine("No hay formularios activos.");
                return;
            }

            Console.WriteLine("Formularios activos:");
            foreach (var form in forms)
            {
                Console.WriteLine($"- {form.Key.Name}");  // Imprime el nombre de la clase del form
            }
        }
    }
}
