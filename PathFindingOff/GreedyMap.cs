using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms; 

namespace PathFindingOff
{
    class GreedyMap
    {
        CustomizedRectangle[,] rectangles;
        double[,] graph;

        double[] dist;
        public int[] prev;

        int lines;

        Graphics g;
        Pen pen;

        CustomizedRectangle recStart;
        CustomizedRectangle recEnd;

        int xstart;
        int ystart;
        int xend;
        int yend;

        public GreedyMap(int lines, Graphics g, Pen pen)
        {
            this.g = g;
            this.lines = lines;
            this.pen = pen;
            this.graph = new double[lines * lines, lines * lines];
            this.rectangles = new CustomizedRectangle[lines, lines];
            Random rnd = new Random();
            dist = new double[lines * lines];
            prev = new int[lines * lines];

            for (int x = 0; x < lines; x++)
            {
                for (int y = 0; y < lines; y++)
                {
                    rectangles[x, y] = new CustomizedRectangle(new Rectangle(x * 10, y * 10, 10, 10));

                    if (x == 0)
                    {
                        if (y == 0)
                        {
                            rectangles[x, y].type = VerticeType.EDGE_LEFT_TOP;
                        }
                        else if (y < lines - 1)
                        {
                            rectangles[x, y].type = VerticeType.EDGE_TOP;
                        }
                        else
                        {
                            rectangles[x, y].type = VerticeType.EDGE_RIGHT_TOP;
                        }
                    }
                    else if (x < lines - 1)
                    {

                        if (y == 0)
                        {
                            rectangles[x, y].type = VerticeType.EDGE_LEFT;
                        }
                        else if (y < lines - 1)
                        {
                            rectangles[x, y].type = VerticeType.NORMAL;
                        }
                        else
                        {
                            rectangles[x, y].type = VerticeType.EDGE_RIGHT;
                        }
                    }
                    else
                    {
                        if (y == 0)
                        {
                            rectangles[x, y].type = VerticeType.EDGE_LEFT_BOTTOM;
                        }
                        else if (y < lines - 1)
                        {
                            rectangles[x, y].type = VerticeType.EDGE_BOTTOM;
                        }
                        else
                        {
                            rectangles[x, y].type = VerticeType.EDGE_RIGHT_BOTTOM;
                        }
                    }
                    g.DrawRectangle(pen, rectangles[x, y].rectangle);
                }
            }

            // initialize start and end points randomly 
            xstart = rnd.Next(0, lines - 1);
            ystart = rnd.Next(0, lines - 1);

            xend = rnd.Next(0, lines - 1);
            yend = rnd.Next(0, lines - 1);

            recStart = rectangles[xstart, ystart];
            recEnd = rectangles[xend, yend];

            pen.Color = Color.Green;
            g.FillRectangle(Brushes.Green, recStart.rectangle);

            pen.Color = Color.Red;
            g.FillRectangle(Brushes.Red, recEnd.rectangle);

            recStart.etat = States.STARTPOINT;
            recEnd.etat = States.ENDPOINT;
        }


        public int getStartVertice()
        {
            return getVerticeIndex(xstart, ystart); 
        }

        public int getEndVertice()
        {
            return getVerticeIndex(xend, yend);
        }

        public void drawObstacle(MouseEventArgs e)
        {
            int x = e.Location.X / 10;
            int y = e.Location.Y / 10;

            if (0 <= x && x <=lines -1 && 0 <= y && y <= lines-1)
            {
                CustomizedRectangle rec = rectangles[x, y];
                if (rec.etat != States.ENDPOINT && rec.etat != States.STARTPOINT)
                {
                    pen.Color = Color.Black;
                    g.FillRectangle(Brushes.Black, rec.rectangle);
                    rec.etat = States.OBSTACLE;
                }
            }
            
        }

        public void makeRoad(int x, int y)
        {
            CustomizedRectangle rec = rectangles[x, y];
            if(rec.etat != States.ENDPOINT && rec.etat != States.STARTPOINT)
                g.FillRectangle(Brushes.Blue, rec.rectangle); 
            
        }

        public bool indicesAreValid(int row, int col)
        {
            return (row >= 0) && (row < this.lines) && (col >= 0) && (col < this.lines);
        }



        public static void a_startAlgorithm()
        {

        }

        public void generateGraph()
        {
            

            int i = 0;
            int j = 0;

            int vertices = lines * lines;

            for(; i<vertices; i++)
            {
                for (j = 0;  j< vertices; j++)
                {
                    graph[i, j] = 0.0; 
                }
            }
            int comp = 0;
            for (int i2 = 0; i2 < lines; i2++)
            {
                for (int j2 = 0; j2 < lines; j2++)
                {
                    i = comp;  
                    switch (rectangles[i2, j2].type)
                    {
                        case VerticeType.EDGE_LEFT_TOP:
                            
                            graph[i, i + 1] = graph[i, i + lines] = 1;
                            graph[i, i + lines + 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_LEFT:
                            graph[i, i + 1] = graph[i, i + lines] = graph[i, i - lines] = 1;
                            graph[i, i - lines + 1] = graph[i, i + lines + 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_LEFT_BOTTOM:
                            graph[i, i + 1] = graph[i, i - lines] = 1;
                            graph[i, i - lines + 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_BOTTOM:
                            graph[i, i + 1] = graph[i, i - 1] = graph[i, i - lines] = 1;
                            graph[i, i - lines - 1] = graph[i, i - lines + 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_RIGHT_BOTTOM:
                            graph[i, i - 1] = graph[i, i - lines] = 1;
                            graph[i, i - lines - 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_RIGHT:
                            graph[i, i - 1] = graph[i, i + lines] = graph[i, i - lines] = 1;
                            graph[i, i - lines - 1] = graph[i, i + lines - 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_RIGHT_TOP:
                            graph[i, i - 1] = graph[i, i + lines] = 1;
                            graph[i, i + lines - 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.EDGE_TOP:
                            graph[i, i + 1] = graph[i, i - 1] = graph[i, i + lines] = 1;
                            graph[i, i + lines - 1] = graph[i, i + lines + 1] = Math.Sqrt(2);
                            break;
                        case VerticeType.NORMAL:
                            graph[i, i + 1] = graph[i, i - 1] = graph[i, i + lines] = graph[i, i - lines] = 1;
                            graph[i, i - lines - 1] = graph[i, i + lines - 1] = graph[i, i - lines + 1] = graph[i, i + lines + 1] = Math.Sqrt(2);

                            break;
                        default:
                            break;
                    }
                    comp++;
                }

                
            }

            int vert; 
            // delete relation between the vertices and obstacles 
            for(int i3 = 0; i3<lines; i3++)
            {
                for(int j3 = 0; j3<lines; j3++)
                {
                    if (rectangles[i3, j3].etat == States.OBSTACLE)
                    {
                        vert = getVerticeIndex(i3, j3);
                        for (int h = 0; j < vertices; j++)
                        {
                            graph[vert, h] = 0;
                            graph[h, vert] = 0; 
                        }
                    }
                }
            }

        }

        private int getVerticeIndex(int x, int y)
        {
            if (x == 0)
                return y;
            return lines * x + y; 
        }

        private int minDistance(double[] dist, bool[] sp)
        {
            int vertices = lines * lines;
            double min = double.MaxValue; 
            int min_index = -1; 

            for (int v=0; v < vertices; v++)
            {
                if (sp[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v; 
                }
            }

            return min_index; 
        }

        public void dijkstraAlgorithm()
        {
            int target = getVerticeIndex(xend, yend);
            int src = getVerticeIndex(xstart, ystart); 

            int vertices = lines * lines;
            generateGraph();
            HashSet<int> Q;



            bool[] spt = new bool[vertices];

            for (int i = 0; i < vertices; i++)
            {
                dist[i] = double.MaxValue;
                spt[i] = false;
                prev[i] = -1; 
            }

            dist[src] = 0; 

            for(int count = 0; count < vertices-1; count++)
            {
                int u = minDistance(dist, spt);
                spt[u] = true;

                if (u == target)
                    return; 
                for (int v = 0; v < vertices; v++)
                    if (!spt[v] &&
                        graph[u, v] != 0 &&
                        dist[u] != int.MaxValue &&
                        dist[u] + graph[u, v] < dist[v])
                    {
                        dist[v] = dist[u] + graph[u, v];
                        prev[v] = u; 
                    }
            }
        }

        // end point or start point 
        public void mouveEdgePoint()
        {
            

        }

    }
}
