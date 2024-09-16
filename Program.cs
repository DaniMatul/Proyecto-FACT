using System.Runtime.CompilerServices;
using System.Text.Json;

static void crearArchivo(string nombre, string datoAlmacenar){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}/archivosTrabajados";
    if (!rutaEvaluar.Contains(nombre)){
        // Crea la carpeta ya que no existe
        string rutaCarpeta = Path.Combine(rutaEvaluar, nombre);
        Directory.CreateDirectory(rutaCarpeta);
        // SE CREARA LA TABLA FAT DEL ARCHIVO (sera lo ultimo que se haga)
        string direccionTablaFAT = $"{rutaCarpeta}/TablaFat.json";
        TablaFat tablaFat = new TablaFat(nombre,$"{rutaCarpeta}/{nombre}-{1}.json", datoAlmacenar.Length);
        // Serializar el objeto a JSON
        string jsonStringFAT = JsonSerializer.Serialize(tablaFat);
        File.WriteAllText(direccionTablaFAT, jsonStringFAT);

        // Se crearan los archivos del contenido
        string datosIngresar = "";
        int contador = 1;
        for(int i = 0; i < datoAlmacenar.Length; i++){
            datosIngresar += datoAlmacenar[i];
            if (datosIngresar.Length == 20){
                string direccionArch = $"{rutaCarpeta}/{nombre}-{contador}.json";
                // Serializar el objeto a JSON
                string rutaSiguiente = "";
                if (contador == datoAlmacenar.Length / 20){
                    rutaSiguiente = "null";
                }
                else{
                    rutaSiguiente = $"{rutaCarpeta}/{nombre}-{contador + 1}.json";
                }

                Archivo archivoNuevo = new Archivo(datosIngresar, rutaSiguiente);
                string jsonString = JsonSerializer.Serialize(archivoNuevo);
                File.WriteAllText(direccionArch, jsonString);
                contador += 1;
                datosIngresar = "";
            }

            else if(datosIngresar.Length < 20  && datoAlmacenar.Length % 20 != 0 && datoAlmacenar.Length / 20 == contador - 1){
                string direccionArch = $"{rutaCarpeta}/{nombre}-{contador}.json";
                // Serializar el objeto a JSON
                Archivo archivoNuevo = new Archivo(datosIngresar, "null");
                string jsonString = JsonSerializer.Serialize(archivoNuevo);
                File.WriteAllText(direccionArch, jsonString);
            }

        }

        
    }
    
    else{
        Console.WriteLine(@"El nombre del archivo que deseas crear ya existe, 
si quieres modificarlo o eliminarlo entra a las opciones 4 o 5. 
No se creara ningun archivo");
        /*string jsonFromFile = File.ReadAllText(filePath);
        archivos = JsonSerializer.Deserialize<List<TablaFat>>(jsonFromFile)!;
        archivos.Add(archivo);
        // Serializar el objeto a JSON
        string jsonString = JsonSerializer.Serialize(archivos);
        File.WriteAllText(filePath, jsonString);*/
    }
}

static void listarArchivo(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}/archivosTrabajados";
    string[] subcarpetas = Directory.GetDirectories(rutaEvaluar);
    int contador = 0; 
    
    foreach (string subcarpeta in subcarpetas)
        {
            string[] archivosCarpeta = Directory.GetFiles(subcarpeta);
            int contador2 = 0;
            Dictionary<string, Archivo> diccionario = new Dictionary<string, Archivo>();
            foreach (string archivo in archivosCarpeta)

            {
                if(archivo == $"{subcarpeta}\\TablaFat.json"){
                    contador += 1;
                    string jsonFromFile = File.ReadAllText(archivo);
                    TablaFat var = JsonSerializer.Deserialize<TablaFat>(jsonFromFile)!;
                    tablaFats.Add(var);
                    if(!var.papelera){
                        Console.WriteLine($"----Archivo Número {contador}---- \n-Nombre: {var.nombre} \n-Caracteres: {var.caracteres} \n-Fecha creación: {var.fechaCreacion} \n-Fecha modificación: {var.fechaModificacion} \n");

                    }
                }
                else{
                    contador2 += 1;
                    
                    string jsonFromFile = File.ReadAllText(archivo);
                    Archivo var = JsonSerializer.Deserialize<Archivo>(jsonFromFile)!;
                
                    diccionario.Add($"Archivo{contador2}", var);

                }
                

            }
            archivos.Add(diccionario);
            }

}


static void abrirArchivo(List<Dictionary<string, Archivo>> archivos, List<TablaFat> tablaFats, int opcion){
    try{
        Dictionary<string, Archivo> archivoAcceder = archivos[opcion - 1];
        string contenido = "";

        foreach(Archivo arch in archivoAcceder.Values){
            contenido += arch.datos;
        }

        TablaFat tablaAcceder = tablaFats[opcion - 1];

        Console.WriteLine($"----Archivo {tablaAcceder.nombre}---- \n-Caracteres: {tablaAcceder.caracteres} \n-Fecha creación: {tablaAcceder.fechaCreacion} \n-Fecha modificación: {tablaAcceder.fechaModificacion} \n Contenido:");
        Console.WriteLine(contenido);
    }
    catch{
        Console.WriteLine("El número enviado no existe");
    }
    

}

static void main(){
    string nombreCarpeta = "archivosTrabajados";
    if (!Directory.Exists(nombreCarpeta)){
        Directory.CreateDirectory(nombreCarpeta);
    }
    
    List<Dictionary<string, Archivo>>archivos=[];
    List<TablaFat> tablas = [];
    while (true){
        Console.WriteLine(@"----Inicio----
1. Crear un archivo y agregar datos
2. Listar Archivos
3. Abrir un archivo
4. Modificar un archivo
5. Eliminar un Archivo
6. Recuperar un archivo
7. Salir");
        string opcion = Console.ReadLine()!;

        if (opcion == "1"){
            Console.WriteLine("--Aquí crearas un nuevo archivo--");
            Console.WriteLine("Ingresa el nombre de tu archivo:");
            string nombre = Console.ReadLine()!;
            Console.WriteLine("Ingresa el texo que desees almacenar");
            string datoAlmacenar = Console.ReadLine()!;
            crearArchivo(nombre, datoAlmacenar);
        }
        
        else if (opcion == "2"){
            listarArchivo(archivos, tablas);
        }
        else if (opcion == "3"){
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees ver");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));
        }

        else if (opcion == "4"){
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees modificar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));

        }

        else if(opcion == "5"){
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees eliminar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));
        }
        else if(opcion == "6"){
            listarArchivo(archivos, tablas);
            Console.WriteLine("Ingresa el NÚMERO del archivo que desees recuperar");
            string opcionArchivo = Console.ReadLine()!;
            abrirArchivo(archivos, tablas, int.Parse(opcionArchivo));
        }
        else if(opcion == "7"){
            break;
        }


    }

}

    

main();