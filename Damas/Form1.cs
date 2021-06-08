using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Damas
{
    public partial class Form1 : Form
    {
        int turno = 0;
        bool movExtra = false;
        PictureBox seleccionado = null;
        Point anterior;

        int moveBlanco = 0;
        int moveRojo = 0;
        int deleteBlanco = 0;
        int deleteRojo = 0;
        int fichaBlanca = 12;
        int fichaRoja = 12;

        List<PictureBox> blancos = new List<PictureBox>();
        List<PictureBox> rojas = new List<PictureBox>();

        /*Esta función es para inicializar la estructura del tablero con las fichas, incluyendo la cantidad de movidas y 
         * saltos para cada color que inicializan en cero.*/
        public Form1() 
        {
            InitializeComponent();
            cargarListas();
            lblMovBlanco.Text = "Blanco tiene: "+moveBlanco+" movidas";
            lblMovRojo.Text = "Rojo tiene: " + moveRojo + " movidas";
            lblDelBlanco.Text = "Blanco ha matado: "+deleteBlanco+" fichas de Rojo";
            lblDelRojo.Text = "Rojo ha matado: "+deleteRojo+" fichas de Blanco";
        }

        /*Esta función es para inicializar la estructura del tablero con las fichas, es decir, validar una ficha 
         *seleccionada*/
        private void cargarListas()
        {
            blancos.Add(blanco1);
            blancos.Add(blanco2);
            blancos.Add(blanco3);
            blancos.Add(blanco4);
            blancos.Add(blanco5);
            blancos.Add(blanco6);
            blancos.Add(blanco7);
            blancos.Add(blanco8);
            blancos.Add(blanco9);
            blancos.Add(blanco10);
            blancos.Add(blanco11);
            blancos.Add(blanco12);
            rojas.Add(roja1);
            rojas.Add(roja2);
            rojas.Add(roja3);
            rojas.Add(roja4);
            rojas.Add(roja5);
            rojas.Add(roja6);
            rojas.Add(roja7);
            rojas.Add(roja8);
            rojas.Add(roja9);
            rojas.Add(roja10);
            rojas.Add(roja11);
            rojas.Add(roja12);
        }

        /*Esta función es para seleccionar la ficha deseada para hacer la jugada, indicando las posiciones posibles
          donde pueda jugar la ficha seleccionada*/
        public void seleccion(object objeto)
        {
            if(!movExtra)
            {
                try
                {
                    seleccionado.BackColor = Color.Black;
                }
                catch
                {

                }
                PictureBox ficha = (PictureBox)objeto;
                seleccionado = ficha;
                seleccionado.BackColor = Color.Lime;
            }
        }

        /*Definir cual ha sido el cuadro en el cual se ha seleccionado o hecho click*/
        private void cuadroClick(object sender, MouseEventArgs e)
        {
            movimiento((PictureBox)sender);
        }

        /*Esta función es para mover la ficha seleccionada desde el cuadro donde está ubicado,
          es decir, la variable seleccionado no sea nula*/
        public void movimiento(PictureBox cuadro)
        {
            if (seleccionado != null)
            {
                string color = seleccionado.Name.ToString().Substring(0, 4);
                
                if (validacion(seleccionado, cuadro, color)) //Validación
                {
                    anterior = seleccionado.Location;
                    seleccionado.Location = cuadro.Location;
                    position(color);
                    int avance = anterior.Y - cuadro.Location.Y;

                    if (!movimientosExtras(color) | Math.Abs(avance) == 50) //Verificar movimientos extras
                    {
                        ifqueen(color);
                        turno++;
                        seleccionado.BackColor = Color.Black;
                        seleccionado = null;
                        movExtra = false;
                        whenMove(color);
                    }
                    else
                    {
                        movExtra = true;
                        whenMove(color);
                    }
                }
            }
        }

        //Esta función para indicar la posición anterior y la posicióna actual de cada ficha para cada color
        private void position(String color)
        {
            if (color == "roja")
            {
                listBox1.Items.Add("Anterior: "+anterior+". Actual: "+seleccionado.Location);
            }
            else if (color == "blan")
            {
                listBox2.Items.Add("Anterior: " +anterior+ ". Actual: " +seleccionado.Location);
            }
        }

        /*Esta función es para verificar si la ficha jugada puede hacer una jugada extra, es decir, 
         *haciendo un segundo salto en plena jugada*/
        private bool movimientosExtras(string color)
        {
            List<PictureBox> bancoContrario = color == "roja" ? blancos : rojas;
            List<Point> posiciones = new List<Point>();
            int sigPosicion = color == "roja" ? -100 : 100;
            
            posiciones.Add(new Point(seleccionado.Location.X + 100, seleccionado.Location.Y + sigPosicion));
            posiciones.Add(new Point(seleccionado.Location.X - 100, seleccionado.Location.Y + sigPosicion));
            if(seleccionado.Tag == "queen")
            {
                posiciones.Add(new Point(seleccionado.Location.X + 100, seleccionado.Location.Y - sigPosicion));
                posiciones.Add(new Point(seleccionado.Location.X - 100, seleccionado.Location.Y - sigPosicion));
            }
            bool resultado = false;
            for(int i=0; i<posiciones.Count; i++)
            {
                if(posiciones[i].X >= 50 && posiciones[i].X <= 400 && posiciones[i].Y >= 50 && posiciones[i].Y <= 400)
                {
                    if(!ocupado(posiciones[i], rojas) && !ocupado(posiciones[i], blancos))
                    {
                        Point puntoMedio = new Point(divide(posiciones[i].X + seleccionado.Location.X, 2), divide(posiciones[i].Y + seleccionado.Location.Y, 2));
                        if (ocupado(puntoMedio, bancoContrario))
                        {
                            resultado = true;
                        }
                    }
                }
            }
            return resultado;
        }

        /*Esta función es para devolver un valor tipo booleano, el punto que se coloca está dentro del mando 
          por el cual también se colocó como parámetro con la ayuda de un ciclo for para buscar un punto dentro
          de una lista de puntos, devuelve verdadero si está ocupado, de lo contrario falso*/
        private bool ocupado(Point punto, List<PictureBox> bando)
        {
            for(int i=0; i<bando.Count; i++)
            {
                if(punto == bando[i].Location)
                {
                    return true;
                }
            }
            return false;
        }

        /*Esta función es para sacar el promedio entre dos números enteros, con el objetivo de sacar 
          el punto medio  */
        private int divide(int a, int b)
        {
            if (a < 0) return -divide(-a, b);
            if (b < 0) return -divide(a, -b);
            if (a < b) return 0;
            return Math.Abs(1 + divide(a - b, b));//Retorna el valor positivo sin importar el orden de los parámatros
        }

        /*Esta función es para validar los saltos hechos con el objetivo de desaparecer las fichas saltadas,
          es decir, validar los movimientos*/
        private bool validacion(PictureBox origen, PictureBox destino, string color)
        {
            Point puntoOrigen = origen.Location;
            Point puntoDestino = destino.Location;
            int avance = puntoOrigen.Y - puntoDestino.Y;
            avance = color == "roja" ? avance : (avance * -1);
            avance = seleccionado.Tag == "queen" ? Math.Abs(avance) : avance;
            if(avance == 50)
            {
                return true;
            }
            else if (avance == 100)
            {
                whenDelete(color);
                Point puntoMedio = new Point(divide(puntoDestino.X + puntoOrigen.X, 2), divide(puntoDestino.Y + puntoOrigen.Y, 2));
                List<PictureBox> bandoContrario = color == "roja" ? blancos : rojas;
                for(int i=0; i<bandoContrario.Count; i++)
                {
                    if(bandoContrario[i].Location == puntoMedio)
                    {
                        bandoContrario[i].Location = new Point(0, 0);
                        bandoContrario[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }

        /*Esta función es para verificar que una ficha de un color haya llegado al otro lado del tablero 
          para convertirse en reina*/
        private void ifqueen(string color)
        {
            if(color == "blan" && seleccionado.Location.Y == 400)
            {
                seleccionado.BackgroundImage = Properties.Resources.Whiteccc;
                seleccionado.Tag = "queen";
            }
            else if (color == "roja" && seleccionado.Location.Y == 50)
            {
                seleccionado.BackgroundImage = Properties.Resources.Redc;
                seleccionado.Tag = "queen";
            }
        }

        /*Esta función es para verificar que cuando la ficha se mueva dependiendo del color, pueda sumar 
          la cantidad de fichas que hayan sido movidas por cada color*/
        private void whenMove(String color)
        {
            if (color == "roja")
            {
                moveRojo++;
                lblMovRojo.Text = "Rojo tiene: " + sumar(moveRojo, 0) + " movidas";
            }
            else if (color == "blan")
            {
                moveBlanco++;
                lblMovBlanco.Text = "Blanco tiene: " + sumar(moveBlanco, 0) + " movidas";
            }
        }

        //Esta función es para sumar la cantidad de fichas que hayan sido movidas por cada color
        private int sumar(int m, int n)
        {
            int y;
            if (n == 0)
            {
                return m;
            }
            y = sumar(m, n - 1) + 1;
            return y;
        }

        /*Esta función es para verificar que cuando la ficha haya sido saltada dependiendo del color, pueda sumar 
          la cantidad de fichas que hayan sido saltadas por cada color*/
        private void whenDelete(String color)
        {
            if (color == "roja")
            {
                deleteRojo++;
                lblDelRojo.Text = "Rojo ha matado: " + sumar(deleteRojo, 0) + " fichas de Blanco";
                fichaBlanca--;
                restar(fichaBlanca, 1);
            }
            else if (color == "blan")
            {
                deleteBlanco++;
                lblDelBlanco.Text = "Blanco ha matado: " + sumar(deleteBlanco, 0) + " fichas de Rojo";
                fichaRoja--;
                restar(fichaRoja, 1);
            }
            whenWin();
        }

        /*Esta función es para restar la cantidad de fichas que hayan sido movidas por cada color*/
        private int restar(int m, int n)
        {
            if (n == 0)
            {
                return m;
            }
            else
            {
                return restar((m - 1), (n - 1));
            }
        }

        /*Esta función es para verificar cuando un color ha saltado todas las fichas del oponente*/
        private void whenWin()
        {
            if (fichaRoja == 0)
            {
                MessageBox.Show("Blanco gana el juego");
                lblWin.Text = "Blanco gana el juego";
                disable();
            }
            else if (fichaBlanca == 0)
            {
                MessageBox.Show("Rojo gana el juego");
                lblWin.Text = "Rojo gana el juego";
                disable();
            }
        }

        /*Esta función es para desactivar los distintos PictureBox o cuadros del tablero cuando termina el juego.*/
        private void disable()
        {
            blanco1.Enabled = false;
            blanco2.Enabled = false;
            blanco3.Enabled = false;
            blanco4.Enabled = false;
            blanco5.Enabled = false;
            blanco6.Enabled = false;
            blanco7.Enabled = false;
            blanco8.Enabled = false;
            blanco9.Enabled = false;
            blanco10.Enabled = false;
            blanco11.Enabled = false;
            blanco12.Enabled = false;
            roja1.Enabled = false;
            roja2.Enabled = false;
            roja3.Enabled = false;
            roja4.Enabled = false;
            roja5.Enabled = false;
            roja6.Enabled = false;
            roja7.Enabled = false;
            roja8.Enabled = false;
            roja9.Enabled = false;
            roja10.Enabled = false;
            roja11.Enabled = false;
            roja12.Enabled = false;
        }

        /*Esta función es para seleccionar las fichas de rojo, y dar una advertencia cuando el color 
          selecciona sus fichas durante el turno del oponente, es decir, condicionar el turno*/
        private void seleccionRoja(object sender, MouseEventArgs e)
        {
            if (turno % 2 == 0)
            {
                seleccion(sender);
            }
            else if (fichaRoja >0)
            {
                MessageBox.Show("Es turno de Blanco");
            }
            else
            {
                whenWin();
            }
        }

        /*Esta función es para seleccionar las fichas de blanco, y dar una advertencia cuando el color 
          selecciona sus fichas durante el turno del oponente, es decir, condicionar el turno*/
        private void seleccionBlanco(object sender, MouseEventArgs e)
        {
            if (turno % 2 == 1)
            {
                seleccion(sender);
            }
            else if (fichaBlanca >0)
            {
                MessageBox.Show("Es turno de Rojo");
            }
            else
            {
                whenWin();
            }
        }

        //Esta función es para verificar si el usuario puede reiniciar el juego
        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea reiniciar el juego?", "Reiniciar Juego",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 reload = new Form1();
                reload.Show();
                this.Dispose(false);
            }
        }

        //Esta función es para verificar si el usuario puede cerrar el juego
        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro que desea cerrar el juego?", "Cerrar Juego",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }
    }
}
