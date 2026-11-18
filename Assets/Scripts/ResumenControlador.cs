using UnityEngine;
using UnityEngine.SceneManagement; //Nos permite poder cambiar entre escenas.
using TMPro;

public class ResumenControlador : MonoBehaviour
{
    [SerializeField] TMP_Text textoResumen;
    [HideInInspector] public CartasControlador controlador;

    void Start()
    {
        //PlayerPrefs es donde guardamos datos en el disco duro.
        //Aquí sacamos los valores que guardamos al ganar en el script anterior.
        //El número '0' tras la coma es el valor por defecto si no encuentra nada.
        int score = PlayerPrefs.GetInt("ScoreFinal", 0);        
        int moves = PlayerPrefs.GetInt("MovesFinal", 0);
        float time = PlayerPrefs.GetFloat("TimeFinal", 0);

        //El tiempo viene en segundos totales (ej: 75). Queremos pasarlo a formato 01:15.
        //Dividimos entre 60 para sacar los minutos.
        string min = ((int)time / 60).ToString("00");
        //Usamos el símbolo '%' (módulo) para sacar el resto de la división, que son los segundos.
        string seg = (time % 60).ToString("00");

        //Muestra en pantalla con esos tamaños y colores la puntuación final, el nº de movimientos y el tiempo que se ha tardado en completar el juego.
        //Usamos el símbolo '$' antes de las comillas para poder meter variables entre llaves {}.
        //Usamos etiquetas tipo HTML para dar estilo directamente desde el código:
        //<size=150%> hace el texto más grande.
        //<color=yellow> cambia el color de esa palabra específica.        
        textoResumen.text = $"<size=150%>¡PARTIDA FINALIZADA!</size>\n\n" +
                            $"Puntuación: <color=yellow>{score}</color>\n" +
                            $"Movimientos: <color=blue>{moves}</color>\n" +
                            $"Tiempo:  <color=red>{min}:{seg}</color>";
    }

    //Permite jugar de nuevo cargando la escena de la partida (mantiene el nivel elegido: fácil, medio o difícil).
    public void JugarDeNuevo()
    {
        SceneManager.LoadScene("Partida");
    }

    //Hace que cargue la escena del menú principal:
    public void IrAlMenu()
    {
        SceneManager.LoadScene("MenuDeInicio");
    }
}