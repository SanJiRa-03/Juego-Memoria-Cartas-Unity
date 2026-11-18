using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    //Llama a estas funciones desde el OnClick de tus botones en el Inspector
    public void JugarFacil() => IniciarJuego(3); //3 parejas = 6 cartas
    public void JugarMedio() => IniciarJuego(5); //5 parejas = 10 cartas
    public void JugarDificil() => IniciarJuego(9); //9 parejas = 18 cartas

    private void IniciarJuego(int cantidadParejas)
    {
        //Guardamos la cantidad de parejas elegidas
        PlayerPrefs.SetInt("ParejasDificultad", cantidadParejas);
        PlayerPrefs.Save();

        //Cambia "Juego" por el nombre exacto de tu escena de cartas
        SceneManager.LoadScene("Partida");

    }
    //Nos permite cerrar la aplicación si llamamos a esta función:
    public void SalirJuego()
    {
        Application.Quit();
    }
}