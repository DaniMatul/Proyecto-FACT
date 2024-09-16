using System.Text.Json;

static void crearArchivo(string nombre, string datoAlmacenar){
    string  rutaGeneral  = Directory.GetCurrentDirectory();
    string rutaEvaluar = $"{rutaGeneral}/archivosTrabajados";
    if (!rutaEvaluar.Contains(nombre)){
        // Crea la carpeta ya que no existe
        string rutaCarpeta = Path.Combine(rutaEvaluar, nombre);
        Directory.CreateDirectory(rutaCarpeta);

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

        // SE CREARA LA TABLA FAT DEL ARCHIVO (sera lo ultimo que se haga)
        string direccionTablaFAT = $"{rutaCarpeta}/TablaFat-{nombre}.json";
        TablaFat tablaFat = new TablaFat(nombre,$"{rutaCarpeta}/{nombre}-{1}.json", datoAlmacenar.Length);
        // Serializar el objeto a JSON
        string jsonStringFAT = JsonSerializer.Serialize(tablaFat);
        File.WriteAllText(direccionTablaFAT, jsonStringFAT);
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

static void main(){
    string nombreCarpeta = "archivosTrabajados";
    if (!Directory.Exists(nombreCarpeta)){
        Directory.CreateDirectory(nombreCarpeta);
    }
    
    List<TablaFat>archivos=[];
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
            // Aqui va a estar mi función listar
        }
        else if (opcion == "3"){
            // Aqui va a estar mi función listar
            Console.WriteLine("Ingresa el archivo que desees ver");
            string opcionArchivo = Console.ReadLine()!;
        }

        else if (opcion == "4"){
            // Aqui va a estar mi función listar
            Console.WriteLine("Ingresa el archivo que desees modificar");
            string opcionArchivo = Console.ReadLine()!;

        }

        else if(opcion == "5"){
            // Aqui va a estar mi función listar
            Console.WriteLine("Ingresa el archivo que desees eliminar");
            string opcionArchivo = Console.ReadLine()!;
        }
        else if(opcion == "6"){
            // Aqui va a estar mi función listar papelera
            Console.WriteLine("Ingresa el archivo que desees recuperar");
            string opcionArchivo = Console.ReadLine()!;
        }
        else if(opcion == "7"){
            break;
        }


    }

}

    

main();