class Archivo{
    public string datos {get; set;}
    public string rutaSiguiente {get; set;}

    public bool papelera {get; set;}

    public Archivo(string datos, string rutaSiguiente){
        this.datos = datos;
        this.rutaSiguiente=rutaSiguiente;
        this.papelera = false;

    }
}