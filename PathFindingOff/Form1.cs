using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFindingOff
{
    public partial class Form1 : Form
    {
        public const int LINES = 30; 


        GreedyMap greedyMap;
        int vertices; 

        bool cleaking = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (cleaking == true)
            {
                greedyMap.drawObstacle(e); 
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Brushes.Black, 1);

            vertices = LINES * LINES; 
            greedyMap = new GreedyMap(LINES, g, pen); 
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            cleaking = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            cleaking = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x;
            int y;

            int s = greedyMap.getStartVertice(); 

            greedyMap.dijkstraAlgorithm();
            for (int i = 0; i < vertices; i++)
            {
                if (greedyMap.prev[i] != -1)
                {
                    x = greedyMap.prev[i] / LINES;
                    y = greedyMap.prev[i] - LINES * x;

                    greedyMap.makeRoad(x, y);
                }
            }

        }
    }
}
