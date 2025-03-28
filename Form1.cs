using PokemonWinformsConsumir.Service;
using System.Windows.Forms;

namespace PokemonWinformsConsumir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void btnMostrar_Click(object sender, EventArgs e)
        {
            var api = new ApiService();
            var success = await api.LoginAsync("admin", "123");

            if (success)
            {
                MessageBox.Show("Login exitoso!");
                var pokemons = await api.GetPokemonsAsync();
                dgDatos.DataSource = pokemons;
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas");
            }
        }
    }
}
