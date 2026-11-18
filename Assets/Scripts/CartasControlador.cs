using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CartasControlador : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] Carta cartaPrefab; //Carta que nos va a servir para clonar las demás.
    [SerializeField] Transform cuadriculaTransform; //Cuadrícula donde irán nuestras cartas.
    [SerializeField] Sprite[] sprites; //Lista de todas las imágenes/iconos de cada carta (array).

    [Header("Interfaz")]
    [SerializeField] TMP_Text textoPuntos; //Aquí irán vinculados todos los textos de la interfaz que más adelante se irán modificando.
    [SerializeField] TMP_Text textoMovimientos;
    [SerializeField] TMP_Text textoTiempo;

    [Header("Audio")]
    public AudioSource audioControlador; //Aquí van vinculados el AudioSoruce de la escena y el resto de efectos de sonido.
    [SerializeField] AudioClip sonidoAcierto;
    [SerializeField] AudioClip sonidoFallo;
    [SerializeField] AudioClip sonidoVictoria;

    private List<Sprite> spriteParejas; //Lista de sprites ya duplicados y mezclados.
    private int totalParejasDificultad; //Dependiendo de la dificultad serán 3, 5 o 9 parejas.
    Carta carta1; //Referencias a las 2 cartas que el jugador gira en cada turno.
    Carta carta2;

    private int puntos = 0;
    private int movimientos = 0;
    private int parejasEncontradas = 0;
    private float tiempoInicio;
    private bool puedeJugar = true; //Booleano que permite o no que se pueda seguir clickando. Bloquea el click mientras haya animaciones.

    private void Start()
    {
        //Leemos la dificultad guardada en el menú principal (por defecto 3 - fácil).
        totalParejasDificultad = PlayerPrefs.GetInt("ParejasDificultad", 3);
        //Se ajusta la cuadrícula automáticamente dependiendo del número de cartas.
        ConfigurarCuadricula(totalParejasDificultad);

        tiempoInicio = Time.time;
        PrepararSprites(); //Elige los sprites y los baraja.
        CrearCartas(); //Crea las cartas y estas aparecen en la zona de juego.
        ActualizarUI(); //Pone todos los valores de los textos a 0.
    }

    private void Update()
    {
        //El contenido de este "if", sólo se actualiza si el booleano es verdadero, se detiene cuando el jugador gana o el juego se pausa.
        if (puedeJugar)
        {
            //Nos da los segundos exactos que han pasado desde que comenzó la partida.
            float tiempoActual = Time.time - tiempoInicio;
            //Para mostrar los minutos y segundos que están corriendo en ese momento.
            string min = ((int)tiempoActual / 60).ToString("00");
            string seg = ((int)tiempoActual % 60).ToString("00");

            if (textoTiempo != null)
                textoTiempo.text = $"Tiempo: {min}:{seg}";
        }
    }

    private void PrepararSprites()
    {
        spriteParejas = new List<Sprite>();
        //Creamos una copia de los sprites base para no alterar el array original.
        List<Sprite> spritesDisponibles = new List<Sprite>(sprites);
        barajarSprites(spritesDisponibles); //Mezclamos para que las figuras elegidas varíen en cada partida.

        //Seleccionamos las figuras necesarias según la dificultad e insertamos dos de cada una.
        for (int i = 0; i < totalParejasDificultad; i++)
        {
            spriteParejas.Add(spritesDisponibles[i]);
            spriteParejas.Add(spritesDisponibles[i]);
        }
        barajarSprites(spriteParejas);
    }

    void ConfigurarCuadricula(int parejas)
    {
        GridLayoutGroup grid = cuadriculaTransform.GetComponent<GridLayoutGroup>();
        if (grid == null) return;
        //Cambiamos el número de columnas (constraintCount) según la cantidad de cartas.
        //Esto asegura que el diseño sea simétrico (ej: 6 cartas = 3 columnas).
        if (parejas == 3) grid.constraintCount = 3;
        else if (parejas == 5) grid.constraintCount = 5;
        else if (parejas == 9) grid.constraintCount = 6;
    }

    void CrearCartas()
    {
        //Limpia cartas viejas si existen (útil para el reinicio).
        foreach (Transform child in cuadriculaTransform) Destroy(child.gameObject);
        //Bucle for que crea físicamente cada carta en la interfaz.
        for (int i = 0; i < spriteParejas.Count; i++)
        {
            Carta carta = Instantiate(cartaPrefab, cuadriculaTransform);
            carta.transform.localScale = Vector3.one; //Aseguramos que el tamaño de la carta sea correcto (escala = 1).
            carta.PonerIconoSprite(spriteParejas[i]); //Asigna el dibujo correspondiente.
            carta.controlador = this;
        }
    }

    public bool SePuedeInteractuar() => puedeJugar;

    public void EsSeleccionado(Carta carta)
    {
        //Si el juego está bloqueado por una animación o la carta ya está girada, se ignora el click.
        if (!puedeJugar || carta.esSeleccionado) return;

        carta.Mostrar();
        movimientos++;
        ActualizarUI();

        if (carta1 == null) //Primera carta que se gira en este turno.
        {
            carta1 = carta;
        }
        else if (carta2 == null) //Segunda carta que se gira en este turno.
        {
            carta2 = carta;
            puedeJugar = false; //Bloqueamos el input para evitar que se pueda girar una 3ª carta.
            StartCoroutine(Coincide(carta1, carta2));
        }
    }

    IEnumerator Coincide(Carta a, Carta b)
    {
        //Pausa para que el jugador vea el resultado (acierto/fallo).
        yield return new WaitForSeconds(0.6f);

        if (a.iconoSprite == b.iconoSprite) //Si acierta el jugador la pareja.
        {
            audioControlador.PlayOneShot(sonidoAcierto, 0.5f);
            puntos += 100;
            parejasEncontradas++;
            ActualizarUI();

            if (parejasEncontradas >= totalParejasDificultad)
            {
                FinDelJuego();
            }
            else
            {
                puedeJugar = true; //Permite seguir jugando.
            }
        }
        else //Caso por si el jugador falla.
        {
            audioControlador.PlayOneShot(sonidoFallo, 0.5f);
            //Las cartas vuelven a su posición inicial.
            a.Esconder();
            b.Esconder();
            puedeJugar = true; //Se desbloquea el input.
        }
        //Limpiamos las variables de turno para la siguiente jugada:
        carta1 = null;
        carta2 = null;
    }
    //Esta función va refrescando los textos de la pantalla con los valores actuales.
    void ActualizarUI()
    {
        if (textoPuntos != null) textoPuntos.text = "Puntos: " + puntos;
        if (textoMovimientos != null) textoMovimientos.text = "Movimientos: " + movimientos;
    }

    void FinDelJuego()
    {
        puedeJugar = false; //Lo ponemos en false, para que se bloquee el juego (el jugador ha ganado).
        audioControlador.PlayOneShot(sonidoVictoria); //Se ejecuta el sonido de victoria.
        //Se muestran los vlaores finales en pantalla:
        PlayerPrefs.SetInt("ScoreFinal", puntos);
        PlayerPrefs.SetInt("MovesFinal", movimientos);
        PlayerPrefs.SetFloat("TimeFinal", Time.time - tiempoInicio);
        PlayerPrefs.Save();

        //Esperamos 2 segundos para que se escuche el sonido de victoria antes de cambiar.
        Invoke("CargarEscenaFinal", 2.0f);
    }
    //Sirve para vincularla con un botón y nos lleva a la escena del resumen final.
    void CargarEscenaFinal()
    {
        SceneManager.LoadScene("ResumenFinal");
    }
    //Este algoritmo recorre la lista de atrás hacia adelante intercambiando la posición del elemento actual con uno aleatorio anterior a él.
    void barajarSprites(List<Sprite> lista)
    {
        for (int i = lista.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Sprite temp = lista[i];
            lista[i] = lista[randomIndex];
            lista[randomIndex] = temp;
        }
    }
    //Sirve para cargar la escena actual (se busca por nombre).
    public void ReiniciarPartida()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}