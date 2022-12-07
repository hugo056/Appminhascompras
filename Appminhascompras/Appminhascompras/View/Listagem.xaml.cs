using Appminhascompras.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Appminhascompras.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Listagem : ContentPage
    {
        ObservableCollection<Produto> lista_produtos = new ObservableCollection<Produto>();

        public Listagem()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked_Novo(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NovoProduto());
            }
            catch(Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
        {
            try
            {
                double soma = lista_produtos.Sum(i => i.preco * i.quantidade);
                DisplayAlert("Total da compra é", soma.ToString("C"), "OK");
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        protected override void OnAppearing()
        {
            if (lista_produtos.Count == 0)
            {
                System.Threading.Tasks.Task.Run(async () =>
                {
                    List<Produto> temp = await App.Database.GetAll();

                    foreach (Produto item in temp)
                    {
                        lista_produtos.Add(item);
                    }

                    ref_carregando.IsRefreshing = false;
                });

                lst_produtos.ItemsSource = lista_produtos;
            }
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem disparador = (MenuItem)sender;

            Produto produto_selecionado = (Produto)disparador.BindingContext;

            bool confirmacao = await DisplayAlert("Tem certeza?", "Remover item?", "Sim", "Não");

            if(confirmacao)
            {
                await App.Database.Delete(produto_selecionado.id);
                lista_produtos.Remove(produto_selecionado);
            }
        }

        private void txt_buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string buscou = e.NewTextValue;

            lista_produtos.Clear();

            Task.Run(async () =>
            {
                List<Produto> temp = await App.Database.Search(buscou);

                foreach (Produto p in temp)
                {
                    lista_produtos.Add(p);
                }
            });
        }

        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            Navigation.PushAsync(new EditarProduto
            {
                BindingContext = (Produto)e.SelectedItem
            });
        }
    }
}