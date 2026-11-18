using UnityEngine;
using UnityEngine.UI; //Necesario para trabajar con elementos de interfaz (Imágenes)
using DG.Tweening; //Librería externa "DOTween" para hacer animaciones fluidas por código

public class Carta : MonoBehaviour
{
    [Header("Referencias Visuales")]
    [SerializeField] Image imagenFondo; //Fondo/base de la carta
    [SerializeField] Image imagenIcono; //Icono (hijo). Imagen de la comida en este caso.

    [Header("Sprites de Base")]
    [SerializeField] Sprite baseAnverso; //Imagen de la carta vacía por delante.
    [SerializeField] Sprite reverso; //Imagen de la carta vacía por detrás.
    [SerializeField] Sprite iconoReverso; //Icono de la pila de platos (imagen del reverso)

    [Header("Sonido")]
    [SerializeField] AudioClip sonidoGiro; //Efecto de sonido al girar la carta.
    //Ocultamos en el inspector: los dibujos que le tocan a las cartas, la referencia al controlador del juego y el booleano para saber si la carta está ya boca ariiba.
    [HideInInspector] public Sprite iconoSprite;
    [HideInInspector] public CartasControlador controlador;
    [HideInInspector] public bool esSeleccionado = false;

    private void Start()
    {
        //Al empezar nos aseguramos de que la carta esté boca abajo.
        MostrarReverso();
    }
    //Pone las imágenes correspondientes a la parte trasera.
    void MostrarReverso()
    {
        imagenFondo.sprite = reverso;
        if (imagenIcono != null)
        {
            imagenIcono.sprite = iconoReverso;
            imagenIcono.gameObject.SetActive(true);
        }
    }
    //Pone las imágenes de la parte delantera (la figura a adivinar).
    void MostrarAnverso()
    {
        imagenFondo.sprite = baseAnverso;
        if (imagenIcono != null)
        {
            imagenIcono.sprite = iconoSprite;
            imagenIcono.gameObject.SetActive(true);
        }
    }
    //Método que usa el controlador para decirle a la carta qué dibujo debe tener:
    public void PonerIconoSprite(Sprite spriteRecibido)
    {
        iconoSprite = spriteRecibido;
    }
    //Función para girar la carta hacia adelante (boca arriba).
    public void Mostrar()
    {
        esSeleccionado = true; //Marcamos que ya no se pueda volver a clickar de momento.
        //Hacemos que se reproduzca el sonido de giro.
        controlador.GetComponent<AudioSource>().PlayOneShot(sonidoGiro);
        //Aumenta el tamaño un 10% para dar un efecto de que la carta "salta" hacia el jugador.
        transform.DOScale(1.1f, 0.2f);
        //Giramos la carta sobre el eje Y:
        transform.DORotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            //Mientras el giro sucede, vamos cambiando el icono de la carta.
            MostrarAnverso();
            //Giramos otros 90º para hacer que la carta vuelva a su posición original y modificamos de nuevo el tamaño.
            transform.DORotate(Vector3.zero, 0.2f);
            transform.DOScale(1f, 0.2f);
        });
    }
    //Hacemos una función que vuelva a poner la/s carta/s boca abajo si el usuario falla:
    public void Esconder()
    {
        esSeleccionado = false; //Permitimos que se pueda volver a clickar en el futuro.
        //Giramos a 90 grados (de perfil).
        transform.DORotate(new Vector3(0, 90, 0), 0.2f).OnComplete(() =>
        {
            //Mientras la carta gira, asignamos el dibujo/icono.
            MostrarReverso();
            //Terminamos el giro hacia su posición inicial:
            transform.DORotate(Vector3.zero, 0.2f);
        });
    }
    //Esta función hace que al hacer click sobre la carta (ya que se la asignaremos al componente "Button"), avisa al controlador de que la carta ha sido elegida.
    public void ClickCarta()
    {
        //Se deben cumplir las condiciones: La carta no está ya boca arriba y el controlador nos permite jugar (no estamos en una animación de espera).
        if (!esSeleccionado && controlador.SePuedeInteractuar())
        {
            controlador.EsSeleccionado(this);
        }
    }
}
