using System.Security.Cryptography.X509Certificates;

class Archivo{
    public string nombre {get; set;}
    public string datos {get; set;}
    public string rutaSiguiente {get; set;}

    public bool papelera {get; set;}

    public Archivo(string nombre, string datos, string rutaSiguiente){
        this.nombre = nombre;
        this.datos = datos;
        this.rutaSiguiente=rutaSiguiente;
        this.papelera = false;

    }
}