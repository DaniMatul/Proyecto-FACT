class TablaFat{
    public string nombre {get; set;}
    public string ruta {get; set;}
    public bool papelera {get; set;}
    public int caracteres {get; set;}
    public DateTime fechaCreacion {get; set;}
    public DateTime fechaModificacion {get; set;}
    public DateTime fechaEliminacion {get; set;}


    public TablaFat(string nombre, string ruta, int caracteres) {
        this.nombre = nombre;
        this.ruta = ruta;
        this.papelera = false;
        this.caracteres = caracteres;
        this.fechaCreacion = DateTime.Now;
        this.fechaModificacion = DateTime.Now; 
        
    }        
};